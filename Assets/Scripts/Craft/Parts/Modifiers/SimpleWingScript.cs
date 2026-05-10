namespace Assets.Scripts.Craft.Parts.Modifiers
{
	using Assets.Scripts.Aerodynamics;
	using Assets.Scripts.Craft.Parts.Modifiers.Wing;
	using Assets.Scripts.Extensions;
	using Assets.Scripts.Helpers;
	using ModApi;
	using ModApi.Common.Extensions;
	using ModApi.Craft.Parts;
	using ModApi.Craft.Parts.Input;
	using ModApi.Design;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEngine;

	public class SimpleWingScript : PartModifierScript<SimpleWingData>
	{
		#region FIELDS-------------------------------------------------------------------------------------
		
		// WING MODIFIERS
		private float _rootLeadingOffset;
		private float _rootTrailingOffset;
		private float _tipLeadingOffset;
		private float _tipTrailingOffset;
		private float _length;
		private float _tipOffset;

		// AIRFOIL MODIFIERS
		private AirfoilType _airfoilRootType;
		private AirfoilType _airfoilTipType;
		private float _airfoilRootThickness;
		private float _airfoilTipThickness;
		private bool _isAirfoilInverted;

		// CONTROL SURFACE MODIFIERS
		private int _controlSurfacePercentage;
		private float _controlSurfaceDeflectionAngle;
		private float _controlSurfaceRotationSpeed;
		private float _controlSurfaceSpacing;
		private bool _isControlSurfaceBorderRounded;

		// LEADING EDGE MODIFIERS
		private int _leadingEdgePercentage;
		private float _leadingEdgeDeflectionAngle;
		private float _leadingEdgeRotationSpeed;
		private float _leadingEdgeAngleOfAttackSensitivity;
		private float _leadingEdgeSpacing;
		private bool _isLeadingEdgeBorderRounded;
		private int _leadingEdgeFullDeflectActivationGroup;
		private bool _isLeadingEdgeAttachedToRoot;

		// WASHOUT MODIFIERS
		private float _washoutAngle;
		private float _washoutRelativePoint;

		// AIRFOIL ROUNDING MODIFIERS
		private bool _isAirfoilRounded;
		private float _airfoilRoundingLength;

		// RESHAPE MODIFIERS
		private float _rootReshape;
		private float _tipReshape;

		// HIDDEN MODIFIERS
		private bool _wingPhysicsEnabled;
		private float _lerxSearchSphereRadius;
		private int _lerxPartId;
		private bool _autoAirfoilInvertEnabled;
		private bool _overrideStockMeshInDesignerEnabled;

		// AIRFOIL SPECIAL VALUES
		private const float OneThickness = 1f;
        private const float FifteenThickness = 15f;
        private const float TwentyFourThickness = 24f;
		private const float NACA_0012_native_thickness = 12.0179f;
		private const float Clark_Y_native_thickness = 12.2446f;
		private const float T_10_root_native_thickness = 7.8225f;
		private const float T_10_wing_native_thickness = 5.3049f;
		private const float NACA_64_208_native_thickness = 7.9033f;
		private float _rootThicknessMultiplier;
		private float _tipThicknessMultiplier;

		// WING OBJECTS
		private Transform SimpleWingContainer;
		private MeshFilter WingMeshFilter;
		private Transform Airfoil;
		private Transform AirfoilRounding;
		private Transform VelocityVector;
		private Transform VelocityPyramid;
		private Transform LiftVector;
		private Transform DragVector;
		private Transform WaveDragVector;
		private Mesh MeshForStockWing = null;

		// CONTROL SURFACE OBJECTS
		private GameObject ControlSurface;
		private GameObject _controlSurfaceRootLeadPoint;
		private GameObject _controlSurfaceTipLeadPoint;
		private GameObject _controlSurfaceRotationAxis;
		private GameObject _controlSurfaceParent;
		private float _controlSurfaceAngle = 0f;

		// LEADING EDGE OBJECTS
		private GameObject LeadingEdge;
		private GameObject _leadingEdgeRootLeadPoint;
		private GameObject _leadingEdgeTipLeadPoint;
		private GameObject _leadingEdgeRotationAxis;
		private GameObject _leadingEdgeParent;
		private GameObject _leadingEdgeAuxiliaryParent;
		private float _leadingEdgeAngle = 0f;

		// ROOT LEADING EDGE OBJECTS
		private SimpleWingScript _rootLeadingEdge = null;

		// VECTORS
		private Vector3[] _startVerticesPosition;
		private Vector3[] _verticesPosition;
		private Vector3[] _unslicedVerticesPosition;
		private Vector3[] _controlSurfaceVerticesPosition;
		private Vector3[] _leadingEdgeVerticesPosition;
		private Vector3[] _tipAirfoilVerticesPosition;

		// WING CALCULATED CHARACTERISTICS
		private float _wingArea = 0f;
		private float _wingLeadAngle = 0f; // in radians
		private float _leadingEdgeRotationAxisAngle = 0f; // in radians
		private float _meanChordLeadPoint = 0f;
		private float _meanChordLength = 0f;
        private float _meanChordPositionY = 0f;
		private float _airfoilCriticalMachNumber = 1f;
		private float _airfoilPostCriticalShakeMultiplier = 0f;

		// WING CONTINIOUSLY CALCULATED CHARACTERISTICS
		private float _dynamicLeadAngleLiftDragCoefficient = 1f;
		private float _controlSurfaceSlipAngleEfficiencyCoefficient = 1f;
		private const float MinimalDynamicLeadAngleLiftDragCoefficient = 0.5f;

		// AERODYNAMIC CURVES CHARACTERISTICS
		private float _liftCoefficientPerDegree = 0f;
		private float _controlSurfaceRotationAngleLiftEfficiencyCoefficient = 0f;

		// LERX CHARACTERISTICS
		private bool _isLerx = false;
		private float _lerxArea = 0f;
		private float _lerxLeadingEdgeLength = 0f;
		private float _lerxCoverageMultiplier = 0f;
		private float _lerxCriticalAngleRaise = 0f;
		private float _positiveLerxAngleOfAttackEfficiency = 0f;
		private float _negativeLerxAngleOfAttackEfficiency = 0f;
		private float _lerxEfficiencyAsymmetryMultiplier = 0f;
		private SimpleWingScript _rootAttachedLerx = null;
		private const float MaximalShakeReduction = 0.75f;

		// CHAIN LERX CHARACTERISTICS
		private List<SimpleWingScript> _chainLerx = new List<SimpleWingScript>();
		private bool _isChainLerx = false;
		private float _chainLerxArea = 0f;
		private float _chainLerxLeadingEdgeLength = 0f;
        private float _chainLerxCoverageMultiplier = 0f;
		private float _chainLerxCriticalAngleRaise = 0f;

		// ANIMATION CURVES
		private readonly AnimationCurve _Cy = new AnimationCurve();
		private readonly AnimationCurve _Cx = new AnimationCurve();
		private readonly AnimationCurve _aC = new AnimationCurve();

		// INPUT CONTROLLERS
		private IInputController _controlSurfaceInputController;
		private bool _controlSurfaceInputControllerNullMessageSended = false;

        // ATTACH POINTS
        private Transform _attachPointRoot;
		private Transform _attachPointTip;
		private Transform _attachPointUp;
		private Transform _attachPointDown;

		// SIMPLE WING DATA INFORMATION OUTPUT
		private float _simpleWingData_NegativeDragPerDegree = 0f;
		private float _simpleWingData_PositiveDragPerDegree = 0f;

		// LATE UPDATE
		private bool _lateUpdateExecuted = false;
		private int _lateUpdateExecutionsCount = 0;
		private const int LateUpdateMaxExecutionsCount = 10;

		// RECURSIVE SEARCHING
		private const int MaxRecursiveExecutionsCount = 100;
		private int _recursiveLerxSearchExecutionsCount = 0;

		// DESIGNER UPDATE
		private float _currentModifiersSumValue = 0f;
		private float _lastModifiersSumValue = 0f;

		#endregion

		#region MONOBEHAVIOUR FUNCTIONS--------------------------------------------------------------------
		private void Awake()
		{
			DestroySimpleWingContainer();
			InstantiateSimpleWingContainer();
		}

		internal void Start()
		{
			ClearData();

			// CORE GEOMETRY FUNCTIONS. ORDER IS IMPORTANT.
			GetAndClampModifiers();
			CalculateWingCharacteristics();
			GetInputControllers();
			GetWingObjects();
			CalculateAirfoilThicknessMultiplier();
			GetWingStartVerticesPosition();
			MoveWingVertices();
			TuneAirfoilRounding();
			ApplyStockMaterialToAirfoil();

			//// ATTACH POINTS FUNCTIONS
			//GetAttachPoints();
			//TuneAttachPoints();

			// PHYSICS FUNCTIONS
			GetVelocityVector();
			TuneVelocityVector();

			CalculateAerodynamicCurves();

			UpdateStockMesh();
			DisableStockMesh();

			DestroyUnusedObjects();
		}

		private void Update()
		{
			// VISIBLE FUNCTIONS
			RotateVelocityVector();
			RotateControlSurface();
			RotateLeadingEdge();
			SetPyramidVisibility();

			// PHYSICS FUNCTIONS
			if (_leadingEdgePercentage > 0
				|| _controlSurfacePercentage > 0
				|| (!_isLerx && _rootAttachedLerx != null && _rootAttachedLerx._isLerx))
			{
				CalculateAerodynamicCurves(); // For wing without leading edge
											  // and without control curface
											  // and without LERX affect
											  // curves calculates only once at start, else - every frame.
			}
			GetLerxAngleOfAttackEfficiency();
			MoveAerodynamicCenter();

			// DESIGNER UPDATE FUNCTIONS
			DesignerUpdate();
			DesignerOverrideStockMesh();

		}

		private void FixedUpdate()
		{
			CalculateDynamicLeadAngleLiftDragCoefficient();
			CalculateAndApplyAerodynamicForces();
		}

		private void LateUpdate()
		{
			if (!_lateUpdateExecuted)
			{
				FindLerx();
				CalculateChainLerxData();

				FindRootLeadingEdge();

				_lateUpdateExecutionsCount++;

				if (_lateUpdateExecutionsCount >= LateUpdateMaxExecutionsCount)
				{
					_lateUpdateExecuted = true;
					SimpleWingDataRedraw();
				}
			}
		}

		#endregion

		#region AWAKE FUNCTIONS----------------------------------------------------------------------------

		private void DestroySimpleWingContainer()
		{
			if (SimpleWingContainer != null)
			{
				Destroy(SimpleWingContainer.gameObject);
			}
		}

		private void InstantiateSimpleWingContainer()
		{
			SimpleWingContainer = Instantiate(Mod.Instance.Mod.AssetBundle.LoadAsset<GameObject>("SimpleWingContainer")).transform;
			SimpleWingContainer.transform.parent = this.transform;
			SimpleWingContainer.gameObject.SetActive(true);
			SimpleWingContainer.localPosition = Vector3.zero;// + new Vector3(-1f, 0f, 0f);
			SimpleWingContainer.localEulerAngles = Vector3.zero;
			SimpleWingContainer.localScale = Vector3.one;
		}

		#endregion

		#region START FUNCTIONS----------------------------------------------------------------------------

		private void ClearData()
		{
			_lateUpdateExecuted = false;
			_lateUpdateExecutionsCount = 0;

			_recursiveLerxSearchExecutionsCount = 0;
			_rootAttachedLerx = null;
			_chainLerx.Clear();

			_rootLeadingEdge = null;
		}

		//CORE GEOMETRY FUNCTIONS------------------------------------------------------------------
		/// <summary>
		/// Get <see cref="SimpleWingData"/> modifiers and set to clamped private fields.
		/// </summary>
		private void GetAndClampModifiers()
		{
			_autoAirfoilInvertEnabled = this.Data.autoAirfoilInvertEnabled;

			// STOCK WING
			WingScript wingScript = this.transform.GetComponent<WingScript>();

			if (wingScript != null)
			{
				wingScript.Data.WingPhysicsEnabled = false;

				const float LeadingMinimalOffset = 0.000001f;
				const float WingMinimalLength = 0.000001f;
				_rootLeadingOffset = Mathf.Clamp(wingScript.Data.RootLeadingOffset, LeadingMinimalOffset, Mathf.Infinity);
				_rootTrailingOffset = Mathf.Clamp(wingScript.Data.RootTrailingOffset, 0f, Mathf.Infinity);
				_tipLeadingOffset = Mathf.Clamp(wingScript.Data.TipLeadingOffset, LeadingMinimalOffset, Mathf.Infinity);
				_tipTrailingOffset = Mathf.Clamp(wingScript.Data.TipTrailingOffset, 0f, Mathf.Infinity);
				_length = Mathf.Clamp(wingScript.Data.TipPosition.y, WingMinimalLength, Mathf.Infinity);
				_tipOffset = wingScript.Data.TipPosition.z;
			}
			else
			{
				Game.Instance.DevConsole.LogWarning($"{nameof(SimpleWingScript)}: {nameof(WingScript)} not found.");
			}

			// AIRFOIL
			const float AirfoilMinimalThickness = 1f;
			const float AirfoilMaximalThickness = 24f;
			_airfoilRootType = GetAirfoil(this.Data.airfoilRootType);
			_airfoilTipType = GetAirfoil(this.Data.airfoilTipType);
			_airfoilRootThickness = Mathf.Clamp(this.Data.airfoilRootThickness, AirfoilMinimalThickness, AirfoilMaximalThickness);
			_airfoilTipThickness = Mathf.Clamp(this.Data.airfoilTipThickness, AirfoilMinimalThickness, AirfoilMaximalThickness);
			_isAirfoilInverted = this.Data.isAirfoilInverted;
			if (_autoAirfoilInvertEnabled)
			{
				_isAirfoilInverted = base.Data.isAirfoilInverted ? !IsOnRightSide() : IsOnRightSide();
			}
			if (wingScript != null)
			{
				wingScript.Data.Thickness = _airfoilRootThickness / 100f * wingScript.Data.BaseChord;
				wingScript.Data.InvertAirfoil = !_isAirfoilInverted;
				wingScript.Data.Airfoil = "Symmetric";
			}

			// CONTROL SURFACE
			const int ControlSurfaceMinimalPercentage = 7;
			const int ControlSurfaceMaximalPercentage = 45;
			_controlSurfacePercentage = Mathf.Clamp(this.Data.CoSu_percentage, 0, ControlSurfaceMaximalPercentage);
			if (_controlSurfacePercentage < ControlSurfaceMinimalPercentage)
			{
				_controlSurfacePercentage = 0;
			}
			const float ControlSurfaceMaximalDeflectionAngle = 90f;
			_controlSurfaceDeflectionAngle = Mathf.Clamp(this.Data.CoSu_deflectionAngle, 0f, ControlSurfaceMaximalDeflectionAngle);
			_controlSurfaceRotationSpeed = Mathf.Clamp(this.Data.CoSu_rotationSpeed, 0f, Mathf.Infinity);
			_controlSurfaceSpacing = Mathf.Clamp(this.Data.CoSu_spacing, 0f, ControlSurfaceMaximalPercentage - _controlSurfacePercentage);
			_isControlSurfaceBorderRounded = this.Data.CoSu_isBorderRounded;

			// LEADING EDGE
			const int LeadingEdgeMaximalPercentage = 40;
			_leadingEdgePercentage = Mathf.Clamp(this.Data.LeEd_percentage, 0, LeadingEdgeMaximalPercentage);
			const float LeadingEdgeMaximalDeflectionAngle = 30f;
			_leadingEdgeDeflectionAngle = Mathf.Clamp(this.Data.LeEd_deflectionAngle, 0f, LeadingEdgeMaximalDeflectionAngle);
			_leadingEdgeRotationSpeed = Mathf.Clamp(this.Data.LeEd_rotationSpeed, 0f, Mathf.Infinity);
			_leadingEdgeAngleOfAttackSensitivity = Mathf.Clamp(this.Data.LeEd_angleOfAttackSensitivity, 0f, Mathf.Infinity);
			_leadingEdgeSpacing = Mathf.Clamp(this.Data.LeEd_spacing, 0f, LeadingEdgeMaximalPercentage - _leadingEdgePercentage);
			_isLeadingEdgeBorderRounded = this.Data.LeEd_isBorderRounded;
			_leadingEdgeFullDeflectActivationGroup = this.Data.LeEd_fullDeflectActivationGroup;
			_isLeadingEdgeAttachedToRoot = this.Data.LeEd_isAttachedToRoot;

			// WASHOUT
			const float MaximalWashoutAngle = 15f;
			_washoutAngle = Mathf.Clamp(this.Data.washoutAngle, -MaximalWashoutAngle, MaximalWashoutAngle);
			_washoutAngle *= _isAirfoilInverted ? -1f : 1f;
			_washoutRelativePoint = Mathf.Clamp(this.Data.washoutRelativePoint, 0f, 1f);

			// AIRFOIL ROUNDING
			_isAirfoilRounded = this.Data.isAirfoilRounded;
			_airfoilRoundingLength = Mathf.Clamp(this.Data.airfoilRoundingLength, 0f, Mathf.Infinity);

			// RESHAPE
			const float MinReshape = -0.5f;
			const float MaxReshape = 0.5f;
			_rootReshape = Mathf.Clamp(this.Data.rootReshape, MinReshape, MaxReshape);
			_tipReshape = Mathf.Clamp(this.Data.tipReshape, MinReshape, MaxReshape);

			// HIDDEN
			_wingPhysicsEnabled = this.Data.wingPhysicsEnabled;
			_lerxSearchSphereRadius = Mathf.Clamp(this.Data.lerxSearchSphereRadius, 0f, Mathf.Infinity);
			if (this.Data.lerxPartId != "auto")
			{
				_lerxPartId = System.Convert.ToInt32(this.Data.lerxPartId);
			}
			_overrideStockMeshInDesignerEnabled = this.Data.overrideStockMeshInDesignerEnabled;
		}

		private void CalculateWingCharacteristics()
		{
			if (_wingPhysicsEnabled)
			{
				_wingArea = (_rootLeadingOffset + _rootTrailingOffset + _tipLeadingOffset + _tipTrailingOffset) * _length / 2f;

				if (Mathf.Abs(_length) <= Mathf.Epsilon)
				{
					_wingLeadAngle = 0f;
				}
				else
				{
					float tanAlpha = (_rootLeadingOffset - _tipOffset - _tipLeadingOffset) / _length;
					_wingLeadAngle = Mathf.Atan(tanAlpha); // in radians
				}

				if (_leadingEdgePercentage > 0)
				{
					float rootLength = _rootLeadingOffset + _rootTrailingOffset;
					float tipLength = _tipLeadingOffset + _tipTrailingOffset;
					float leadingEdgeMultiplier = (float)_leadingEdgePercentage / 100f;
					float tanAlpha = (_rootLeadingOffset - (rootLength * leadingEdgeMultiplier) - _tipOffset - _tipLeadingOffset + (tipLength * leadingEdgeMultiplier)) / _length;
					_leadingEdgeRotationAxisAngle = Mathf.Atan(tanAlpha); // in radians
				}

				float middleAirfoilThickness = (_airfoilRootThickness + _airfoilTipThickness) / 2f;
				const float OneThicknessCriticalMachNumber = 1f;
				const float FifteenThicknessCriticalMachNumber = 0.62f;
				const float TwentyFourCriticalMachNumber = 0.47f;
				float airfoilCriticalMachMultiplier = (AirfoilCalculator.GetCriticalMachMultiplier(_airfoilRootType) + AirfoilCalculator.GetCriticalMachMultiplier(_airfoilTipType)) / 2f;
				_airfoilCriticalMachNumber = middleAirfoilThickness <= FifteenThickness
					? Mathf.Lerp(OneThicknessCriticalMachNumber, FifteenThicknessCriticalMachNumber, Mathf.InverseLerp(OneThickness, FifteenThickness, middleAirfoilThickness)) * airfoilCriticalMachMultiplier
					: Mathf.Lerp(FifteenThicknessCriticalMachNumber, TwentyFourCriticalMachNumber, Mathf.InverseLerp(FifteenThickness, TwentyFourThickness, middleAirfoilThickness)) * airfoilCriticalMachMultiplier;

				float rootAirfoilPostCriticalShakeMultiplier = AirfoilCalculator.GetPostCriticalShakeMultiplier(_airfoilRootType);
				float tipAirfoilPostCriticalShakeMultiplier = AirfoilCalculator.GetPostCriticalShakeMultiplier(_airfoilTipType);
				_airfoilPostCriticalShakeMultiplier = (rootAirfoilPostCriticalShakeMultiplier + tipAirfoilPostCriticalShakeMultiplier) / 2f;

				const float BecomeLerxAngle = 61f;
				if (_wingLeadAngle * Mathf.Rad2Deg > BecomeLerxAngle)
				{
					_isLerx = true;
					_lerxArea = Mathf.Clamp(_rootLeadingOffset - _tipLeadingOffset - _tipOffset, 0f, Mathf.Infinity) * _length / 2f;
					_lerxArea = Mathf.Clamp(_lerxArea, 0f, _wingArea);
					_lerxLeadingEdgeLength = Mathf.Sqrt(Mathf.Pow(Mathf.Clamp(_rootLeadingOffset - _tipLeadingOffset - _tipOffset, 0f, Mathf.Infinity), 2f) + Mathf.Pow(_length, 2f));
					_lerxCoverageMultiplier = LerxHelper.CalculateLerxCoverageMultiplier(_airfoilRootType, _airfoilTipType);
					_lerxCriticalAngleRaise = LerxHelper.CalculateLerxCriticalAngleRaise(_airfoilRootType, _airfoilTipType);
					_lerxEfficiencyAsymmetryMultiplier = LerxHelper.CalculateLerxEfficiencyAsymmetryMultiplier(_airfoilRootType, _airfoilTipType);

					_chainLerx.Add(this);

					_controlSurfacePercentage = 0; // LERX can't have control surface or leading edge
					_leadingEdgePercentage = 0;
				}
				else
				{
					_isLerx = false;
					_lerxArea = 0f;
					_lerxLeadingEdgeLength = 0f;
					_lerxCoverageMultiplier = 0f;
					_lerxCriticalAngleRaise = 0f;
					_lerxEfficiencyAsymmetryMultiplier = 0f;
				}
			}
		}

		/// <summary>
		/// Gets the airfoil enum depended from airfoil name.
		/// </summary>
		/// <param name="airfoilName">Airfoil name.</param>
		/// <returns>Airfoil type.</returns>
		private static AirfoilType GetAirfoil(string airfoilName)
		{
			switch (airfoilName)
			{
				case "NACA 0012":
					return AirfoilType.NACA_0012;

				case "Clark Y":
					return AirfoilType.Clark_Y;

				case "T-10 root":
					return AirfoilType.T_10_root;

				case "T-10 wing":
					return AirfoilType.T_10_wing;

				case "NACA 64-208":
					return AirfoilType.NACA_64_208;

				default:
					return AirfoilType.NACA_0012;
			}
		}

		/// <summary>
		/// Calculate thickness for different airfoils with different native thickness (3d-model thickness).
		/// </summary>
		private void CalculateAirfoilThicknessMultiplier()
		{
			switch (_airfoilRootType)
			{
				case AirfoilType.NACA_0012:
					_rootThicknessMultiplier = _airfoilRootThickness / NACA_0012_native_thickness;
					break;

				case AirfoilType.Clark_Y:
					_rootThicknessMultiplier = _airfoilRootThickness / Clark_Y_native_thickness;
					break;

				case AirfoilType.T_10_root:
					_rootThicknessMultiplier = _airfoilRootThickness / T_10_root_native_thickness;
					break;

				case AirfoilType.T_10_wing:
					_rootThicknessMultiplier = _airfoilRootThickness / T_10_wing_native_thickness;
					break;

				case AirfoilType.NACA_64_208:
					_rootThicknessMultiplier = _airfoilRootThickness / NACA_64_208_native_thickness;
					break;
			}

			switch (_airfoilTipType)
			{
				case AirfoilType.NACA_0012:
					_tipThicknessMultiplier = _airfoilTipThickness / NACA_0012_native_thickness;
					break;

				case AirfoilType.Clark_Y:
					_tipThicknessMultiplier = _airfoilTipThickness / Clark_Y_native_thickness;
					break;

				case AirfoilType.T_10_root:
					_tipThicknessMultiplier = _airfoilTipThickness / T_10_root_native_thickness;
					break;

				case AirfoilType.T_10_wing:
					_tipThicknessMultiplier = _airfoilTipThickness / T_10_wing_native_thickness;
					break;

				case AirfoilType.NACA_64_208:
					_tipThicknessMultiplier = _airfoilTipThickness / NACA_64_208_native_thickness;
					break;
			}
		}

		/// <summary>
		/// Get the wing objects.
		/// </summary>
		private void GetWingObjects()
		{
			Airfoil = SimpleWingContainer.FindChildByName("Airfoil");
			Transform airfoils = SimpleWingContainer.FindChildByName("Airfoils");
			AirfoilRounding = SimpleWingContainer.FindChildByName("AirfoilRounding");
			Transform airfoilRoundings = SimpleWingContainer.FindChildByName("AirfoilRoundings");
			WingMeshFilter = Airfoil.GetComponent<MeshFilter>();

			switch (_airfoilRootType)
			{
				case AirfoilType.Clark_Y:
					WingMeshFilter.mesh = airfoils.FindChildByName(AirfoilType.Clark_Y.ToString()).GetComponent<MeshFilter>().mesh;
					break;

				case AirfoilType.T_10_root:
					WingMeshFilter.mesh = airfoils.FindChildByName(AirfoilType.T_10_root.ToString()).GetComponent<MeshFilter>().mesh;
					break;

				case AirfoilType.T_10_wing:
					WingMeshFilter.mesh = airfoils.FindChildByName(AirfoilType.T_10_wing.ToString()).GetComponent<MeshFilter>().mesh;
					break;

				case AirfoilType.NACA_64_208:
					WingMeshFilter.mesh = airfoils.FindChildByName(AirfoilType.NACA_64_208.ToString()).GetComponent<MeshFilter>().mesh;
					break;

				default: /// default = <see cref="AirfoilType.NACA_0012"/>
					WingMeshFilter.mesh = airfoils.FindChildByName(AirfoilType.NACA_0012.ToString()).GetComponent<MeshFilter>().mesh;
					break;
			}

			switch (_airfoilTipType)
			{
				case AirfoilType.Clark_Y:
					_tipAirfoilVerticesPosition = airfoils.FindChildByName(AirfoilType.Clark_Y.ToString()).GetComponent<MeshFilter>().mesh.vertices;
					break;

				case AirfoilType.T_10_root:
					_tipAirfoilVerticesPosition = airfoils.FindChildByName(AirfoilType.T_10_root.ToString()).GetComponent<MeshFilter>().mesh.vertices;
					break;

				case AirfoilType.T_10_wing:
					_tipAirfoilVerticesPosition = airfoils.FindChildByName(AirfoilType.T_10_wing.ToString()).GetComponent<MeshFilter>().mesh.vertices;
					break;

				case AirfoilType.NACA_64_208:
					_tipAirfoilVerticesPosition = airfoils.FindChildByName(AirfoilType.NACA_64_208.ToString()).GetComponent<MeshFilter>().mesh.vertices;
					break;

				default: /// default = <see cref="AirfoilType.NACA_0012"/>
					_tipAirfoilVerticesPosition = airfoils.FindChildByName(AirfoilType.NACA_0012.ToString()).GetComponent<MeshFilter>().mesh.vertices;
					break;
			}

			airfoils.gameObject.SetActive(false);

			AirfoilRounding.gameObject.SetActive(false);
			airfoilRoundings.gameObject.SetActive(false);
		}

		/// <summary>
		/// Get input controllers for control surface.
		/// </summary>
		private void GetInputControllers()
		{
			_controlSurfaceInputController = GetInputController("ControlSurface");
        }

		/// <summary>
		/// Get start position of vertices of the wing mesh.
		/// </summary>
		private void GetWingStartVerticesPosition()
		{
			_startVerticesPosition = WingMeshFilter.mesh.vertices;
			_verticesPosition = WingMeshFilter.mesh.vertices;
		}

		/// <summary>
		/// Move wing vertices.
		/// </summary>
		private void MoveWingVertices()
		{
			if (_airfoilRootType != _airfoilTipType)
			{
				_startVerticesPosition = MeshHelper.TransformTipVertices(_startVerticesPosition, _tipAirfoilVerticesPosition);
			}

			const float MiddleWingYPoint = 0.5f;

			if (_isAirfoilInverted)
			{
				_startVerticesPosition = MeshHelper.InvertAirfoilVertices(_startVerticesPosition);
			}

			float rootLength = _rootLeadingOffset + _rootTrailingOffset;
			float rootMiddle = (_rootLeadingOffset - _rootTrailingOffset) / 2f;

			float tipLength = _tipLeadingOffset + _tipTrailingOffset;
			float tipMiddle = (_tipLeadingOffset - _tipTrailingOffset) / 2f;

			for (int i = 0; i < _verticesPosition.Length; i++)
			{
				bool vertexBelongRoot = _startVerticesPosition[i].y < MiddleWingYPoint;
				if (vertexBelongRoot)
				{
					_verticesPosition[i] = new Vector3(
						_startVerticesPosition[i].x * rootLength * _rootThicknessMultiplier,
						0f,
						_startVerticesPosition[i].z * rootLength + rootMiddle);
				}

				bool vertexBelongTip = _startVerticesPosition[i].y > MiddleWingYPoint;
				if (vertexBelongTip)
				{
					_verticesPosition[i] = new Vector3(
						_startVerticesPosition[i].x * tipLength * _tipThicknessMultiplier,
						_length,
						_startVerticesPosition[i].z * tipLength + tipMiddle + _tipOffset);
				}
			}

			// RESHAPE
			float rootReshape = 0.5f + _rootReshape;
			float tipReshape = 0.5f + _tipReshape;
			for (int i = 0; i < _verticesPosition.Length; i++)
			{
				bool vertexBelongRoot = _startVerticesPosition[i].y < MiddleWingYPoint;
				if (vertexBelongRoot)
				{
					float point = (_verticesPosition[i].z + _rootTrailingOffset) / rootLength;
					point = 2 * point * (1f - point) * rootReshape + Mathf.Pow(point, 2);
					point = point * rootLength - _rootTrailingOffset;

					_verticesPosition[i] = new Vector3(_verticesPosition[i].x, _verticesPosition[i].y, point);
				}

				bool vertexBelongTip = _startVerticesPosition[i].y > MiddleWingYPoint;
				if (vertexBelongTip)
				{
					float point = (_verticesPosition[i].z + _tipTrailingOffset - _tipOffset) / tipLength;
					point = 2 * point * (1f - point) * tipReshape + Mathf.Pow(point, 2);
					point = point * tipLength - _tipTrailingOffset + _tipOffset;

					_verticesPosition[i] = new Vector3(_verticesPosition[i].x, _verticesPosition[i].y, point);
				}
			}

			WingMeshFilter.mesh.vertices = _verticesPosition;
			WingMeshFilter.mesh.RecalculateNormals();
			WingMeshFilter.mesh.RecalculateBounds();
			WingMeshFilter.GetComponent<MeshCollider>().sharedMesh = WingMeshFilter.mesh;
			WingMeshFilter.GetComponent<MeshCollider>().convex = true;
			if (Game.InDesignerScene)
			{
				MeshForStockWing = Instantiate(WingMeshFilter.mesh);
			}
			_unslicedVerticesPosition = WingMeshFilter.mesh.vertices;

			DestroyControlSurface();
			DestroyControlSurfacePoints();
			CreateControlSurface();

			DestroyLeadingEdge();
			DestroyLeadingEdgePoints();
			CreateLeadingEdge();

			float middleYPoint = _length / 2f;
			SliceWingFromControlSurface(middleYPoint, rootLength, rootMiddle, tipLength, tipMiddle, _tipOffset, _isControlSurfaceBorderRounded);
			SliceControlSurfaceFromWing(middleYPoint, rootLength, rootMiddle, tipLength, tipMiddle, _tipOffset, _isControlSurfaceBorderRounded);
			SetControlSurfaceParent();

			SliceWingFromLeadingEdge(middleYPoint, rootLength, rootMiddle, tipLength, tipMiddle, _tipOffset, _isLeadingEdgeBorderRounded);
			SliceLeadingEdgeFromWing(middleYPoint, rootLength, rootMiddle, tipLength, tipMiddle, _tipOffset, _isLeadingEdgeBorderRounded);
			SetLeadingEdgeParent();

			WashoutTip();
		}

		/// <summary>
		/// For sure that <see cref="ControlSurface"/> not exist.
		/// </summary>
		private void DestroyControlSurface()
		{
			if (ControlSurface != null)
			{
				DestroyImmediate(ControlSurface);
			}
		}

		/// <summary>
		/// For sure that surface points not exist.
		/// </summary>
		private void DestroyControlSurfacePoints()
		{
			if (_controlSurfaceRootLeadPoint != null)
				DestroyImmediate(_controlSurfaceRootLeadPoint);

			if (_controlSurfaceTipLeadPoint != null)
				DestroyImmediate(_controlSurfaceTipLeadPoint);

			if (_controlSurfaceRotationAxis != null)
				DestroyImmediate(_controlSurfaceRotationAxis);

			if (_controlSurfaceParent != null)
				DestroyImmediate(_controlSurfaceParent);
		}

		/// <summary>
		/// Creates the control surface if control surface percentage > 0 and tune it.
		/// </summary>
		private void CreateControlSurface()
		{
			if (_controlSurfacePercentage > 0)
			{
				ControlSurface = new GameObject();
				ControlSurface.name = "ControlSurface";
				ControlSurface.transform.parent = SimpleWingContainer;
				ControlSurface.transform.localPosition = Vector3.zero;
				ControlSurface.transform.localEulerAngles = Vector3.zero;
				ControlSurface.transform.localScale = Vector3.one;
				ControlSurface.SetActive(true);

				ControlSurface.AddComponent<MeshFilter>().mesh = WingMeshFilter.mesh;
				ControlSurface.AddComponent<MeshRenderer>().material = WingMeshFilter.GetComponent<MeshRenderer>().material;
				_controlSurfaceVerticesPosition = ControlSurface.GetComponent<MeshFilter>().mesh.vertices;
			}
		}

		private void SliceWingFromControlSurface(float middleWingYPoint, float rootScaleMultiplier, float rootScaleOffset, float tipScaleMultiplier, float tipScaleOffset, float tipOffset, bool isControlSurfaceBorderRounded)
		{
			if (_controlSurfacePercentage > 0)
			{
				_verticesPosition = MeshHelper.SliceWingFromControlSurface(_verticesPosition, (float)_controlSurfacePercentage + _controlSurfaceSpacing, middleWingYPoint, rootScaleMultiplier, rootScaleOffset, tipScaleMultiplier, tipScaleOffset, tipOffset, isControlSurfaceBorderRounded);

				WingMeshFilter.mesh.vertices = _verticesPosition;
				WingMeshFilter.mesh.RecalculateNormals();
				WingMeshFilter.mesh.RecalculateBounds();
			}
		}

		private void SliceControlSurfaceFromWing(float middleWingYPoint, float rootScaleMultiplier, float rootScaleOffset, float tipScaleMultiplier, float tipScaleOffset, float tipOffset, bool controlSurfaceRoundBorder)
		{
			if (_controlSurfacePercentage > 0)
			{
				const float SliceFromPoint = -0.5f;
				(float _, float _, float _, float _, float rootSlicePoint, float tipSlicePoint) =
					MeshHelper.CalculateSlicePoint(_unslicedVerticesPosition, SliceFromPoint, (float)_controlSurfacePercentage, middleWingYPoint, rootScaleMultiplier, rootScaleOffset, tipScaleMultiplier, tipScaleOffset, tipOffset);

				_controlSurfaceVerticesPosition = MeshHelper.SliceControlSurfaceFromWing(_controlSurfaceVerticesPosition, _unslicedVerticesPosition, (float)_controlSurfacePercentage, middleWingYPoint, rootScaleMultiplier, rootScaleOffset, tipScaleMultiplier, tipScaleOffset, tipOffset, controlSurfaceRoundBorder);
				ControlSurface.GetComponent<MeshFilter>().mesh.vertices = _controlSurfaceVerticesPosition;
				ControlSurface.GetComponent<MeshFilter>().mesh.RecalculateNormals();
				ControlSurface.GetComponent<MeshFilter>().mesh.RecalculateBounds();

				// Calculate rotating point
				float rootMaxPositiveX = 0f;
				float rootMaxNegativeX = 0f;
				float tipMaxPositiveX = 0f;
				float tipMaxNegativeX = 0f;

				for (int i = 0; i < _controlSurfaceVerticesPosition.Length; i++)
				{
					bool vertexBelongRoot = _controlSurfaceVerticesPosition[i].y < middleWingYPoint;
					bool vertexBelongTip = _controlSurfaceVerticesPosition[i].y > middleWingYPoint;
					bool vertexXPositive = _controlSurfaceVerticesPosition[i].x > 0f;
					bool vertexXNegative = _controlSurfaceVerticesPosition[i].x < 0f;

					if (vertexBelongRoot && vertexXPositive && _controlSurfaceVerticesPosition[i].x > rootMaxPositiveX)
						rootMaxPositiveX = _controlSurfaceVerticesPosition[i].x;

					if (vertexBelongRoot && vertexXNegative && _controlSurfaceVerticesPosition[i].x < rootMaxNegativeX)
						rootMaxNegativeX = _controlSurfaceVerticesPosition[i].x;

					if (vertexBelongTip && vertexXPositive && _controlSurfaceVerticesPosition[i].x > tipMaxPositiveX)
						tipMaxPositiveX = _controlSurfaceVerticesPosition[i].x;

					if (vertexBelongTip && vertexXNegative && _controlSurfaceVerticesPosition[i].x < tipMaxNegativeX)
						tipMaxNegativeX = _controlSurfaceVerticesPosition[i].x;
				}

				float rootRotatingPoint = rootMaxPositiveX - (rootMaxPositiveX - rootMaxNegativeX) / 2f;
				float tipRotatingPoint = tipMaxPositiveX - (tipMaxPositiveX - tipMaxNegativeX) / 2f;

				CreateControlSurfacePoints(rootSlicePoint, tipSlicePoint, rootRotatingPoint, tipRotatingPoint);
			}
		}

		/// <summary>
		/// Create control surface points.
		/// </summary>
		/// <param name="rootSlicePoint">The Z-coordinate in space of 3d-model of wing of root slice point.</param>
		/// <param name="tipSlicePoint">The Z-coordinate in space of 3d-model of wing of tip slice point.</param>
		/// <param name="rootRotatingPoint">The X-coordinate in space of 3d-model of wing of root slice point.</param>
		/// <param name="tipRotatingPoint">The X-coordinate in space of 3d-model of wing of tip slice point.</param>
		private void CreateControlSurfacePoints(float rootSlicePoint, float tipSlicePoint, float rootRotatingPoint, float tipRotatingPoint)
		{
			_controlSurfaceRootLeadPoint = new GameObject();
			_controlSurfaceRootLeadPoint.name = "ControlSurfaceRootLeadPoint";

			_controlSurfaceTipLeadPoint = new GameObject();
			_controlSurfaceTipLeadPoint.name = "ControlSurfaceTipLeadPoint";

			_controlSurfaceRotationAxis = new GameObject();
			_controlSurfaceRotationAxis.name = "ControlSurfaceRotationAxis";

			_controlSurfaceParent = new GameObject();
			_controlSurfaceParent.name = "ControlSurfaceParent";

			_controlSurfaceRootLeadPoint.transform.parent = SimpleWingContainer;
			_controlSurfaceRootLeadPoint.transform.localPosition = new Vector3(rootRotatingPoint, 0f, rootSlicePoint);
			_controlSurfaceRootLeadPoint.transform.localEulerAngles = Vector3.zero;
			_controlSurfaceRootLeadPoint.transform.localScale = Vector3.one;

			_controlSurfaceTipLeadPoint.transform.parent = SimpleWingContainer;
			_controlSurfaceTipLeadPoint.transform.localPosition = new Vector3(tipRotatingPoint, _length, tipSlicePoint);
			_controlSurfaceTipLeadPoint.transform.localEulerAngles = Vector3.zero;
			_controlSurfaceTipLeadPoint.transform.localScale = Vector3.one;

			_controlSurfaceRotationAxis.transform.parent = SimpleWingContainer;
			_controlSurfaceRotationAxis.transform.localPosition = _controlSurfaceRootLeadPoint.transform.localPosition;
			_controlSurfaceRotationAxis.transform.LookAt(_controlSurfaceTipLeadPoint.transform, SimpleWingContainer.forward * -1f);
			_controlSurfaceRotationAxis.transform.localRotation *= Quaternion.Euler(90f, 0f, 0f);
			_controlSurfaceRotationAxis.transform.localScale = Vector3.one;

			_controlSurfaceParent.transform.parent = _controlSurfaceRotationAxis.transform;
			_controlSurfaceParent.transform.localPosition = Vector3.zero;
			_controlSurfaceParent.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
			_controlSurfaceParent.transform.localScale = Vector3.one;
		}

		/// <summary>
		/// Set control surface as child of control surface parent object.
		/// </summary>
		private void SetControlSurfaceParent()
		{
			if (_controlSurfacePercentage > 0)
			{
				ControlSurface.transform.parent = _controlSurfaceParent.transform;
			}
		}

		/// <summary>
		/// For sure that <see cref="LeadingEdge"/> not exist.
		/// </summary>
		private void DestroyLeadingEdge()
		{
			if (LeadingEdge != null)
			{
				DestroyImmediate(LeadingEdge);
			}
		}

		/// <summary>
		/// For sure that leading edge points not exist
		/// </summary>
		private void DestroyLeadingEdgePoints()
		{
			if (_leadingEdgeRootLeadPoint != null)
				DestroyImmediate(_leadingEdgeRootLeadPoint);

			if (_leadingEdgeTipLeadPoint != null)
				DestroyImmediate(_leadingEdgeTipLeadPoint);

			if (_leadingEdgeRotationAxis != null)
				DestroyImmediate(_leadingEdgeRotationAxis);

			if (_leadingEdgeParent != null)
				DestroyImmediate(_leadingEdgeParent);

			if (_leadingEdgeAuxiliaryParent != null)
				DestroyImmediate(_leadingEdgeAuxiliaryParent);
		}

		/// <summary>
		/// Creates the leading edge if leading edge percentage > 0 and tune it
		/// </summary>
		private void CreateLeadingEdge()
		{
			if (_leadingEdgePercentage > 0)
			{
				LeadingEdge = new GameObject();
				LeadingEdge.name = "LeadingEdge";
				LeadingEdge.transform.parent = SimpleWingContainer;
				LeadingEdge.transform.localPosition = Vector3.zero;
				LeadingEdge.transform.localEulerAngles = Vector3.zero;
				LeadingEdge.transform.localScale = Vector3.one;
				LeadingEdge.SetActive(true);

				LeadingEdge.AddComponent<MeshFilter>().mesh = WingMeshFilter.mesh;
				LeadingEdge.AddComponent<MeshRenderer>().material = WingMeshFilter.GetComponent<MeshRenderer>().material;
				_leadingEdgeVerticesPosition = LeadingEdge.GetComponent<MeshFilter>().mesh.vertices;
			}
		}

		private void SliceWingFromLeadingEdge(float middleWingYPoint, float rootScaleMultiplier, float rootScaleOffset, float tipScaleMultiplier, float tipScaleOffset, float tipOffset, bool leadingEdgeWingRoundBorder)
		{
			if (_leadingEdgePercentage > 0)
			{
				_verticesPosition = MeshHelper.SliceWingFromLeadingEdge(_verticesPosition, (float)_leadingEdgePercentage + _leadingEdgeSpacing, middleWingYPoint, rootScaleMultiplier, rootScaleOffset, tipScaleMultiplier, tipScaleOffset, tipOffset, leadingEdgeWingRoundBorder);

				WingMeshFilter.mesh.vertices = _verticesPosition;
				WingMeshFilter.mesh.RecalculateNormals();
				WingMeshFilter.mesh.RecalculateBounds();
			}
		}

		private void SliceLeadingEdgeFromWing(float middleWingYPoint, float rootScaleMultiplier, float rootScaleOffset, float tipScaleMultiplier, float tipScaleOffset, float tipOffset, bool isLeadingEdgeBorderRounded)
		{
			if (_leadingEdgePercentage > 0)
			{
				const float SliceFromPoint = 0.5f;
				(float _, float _, float _, float _, float rootSlicePoint, float tipSlicePoint) =
					MeshHelper.CalculateSlicePoint(_unslicedVerticesPosition, SliceFromPoint, (float)_leadingEdgePercentage + _leadingEdgeSpacing, middleWingYPoint, rootScaleMultiplier, rootScaleOffset, tipScaleMultiplier, tipScaleOffset, tipOffset);

				_leadingEdgeVerticesPosition = MeshHelper.SliceLeadingEdgeFromWing(_leadingEdgeVerticesPosition, _unslicedVerticesPosition, (float)_leadingEdgePercentage, middleWingYPoint, rootScaleMultiplier, rootScaleOffset, tipScaleMultiplier, tipScaleOffset, tipOffset, isLeadingEdgeBorderRounded);
				LeadingEdge.GetComponent<MeshFilter>().mesh.vertices = _leadingEdgeVerticesPosition;
				LeadingEdge.GetComponent<MeshFilter>().mesh.RecalculateNormals();
				LeadingEdge.GetComponent<MeshFilter>().mesh.RecalculateBounds();

				// Calculate rotating point
				float rootMaxPositiveX = 0f;
				float rootMaxNegativeX = 0f;
				float tipMaxPositiveX = 0f;
				float tipMaxNegativeX = 0f;

				for (int i = 0; i < _leadingEdgeVerticesPosition.Length; i++)
				{
					bool vertexBelongRoot = _leadingEdgeVerticesPosition[i].y < middleWingYPoint;
					bool vertexBelongTip = _leadingEdgeVerticesPosition[i].y > middleWingYPoint;
					bool vertexXPositive = _leadingEdgeVerticesPosition[i].x > 0f;
					bool vertexXNegative = _leadingEdgeVerticesPosition[i].x < 0f;

					if (vertexBelongRoot && vertexXPositive && _leadingEdgeVerticesPosition[i].x > rootMaxPositiveX)
						rootMaxPositiveX = _leadingEdgeVerticesPosition[i].x;

					if (vertexBelongRoot && vertexXNegative && _leadingEdgeVerticesPosition[i].x < rootMaxNegativeX)
						rootMaxNegativeX = _leadingEdgeVerticesPosition[i].x;

					if (vertexBelongTip && vertexXPositive && _leadingEdgeVerticesPosition[i].x > tipMaxPositiveX)
						tipMaxPositiveX = _leadingEdgeVerticesPosition[i].x;

					if (vertexBelongTip && vertexXNegative && _leadingEdgeVerticesPosition[i].x < tipMaxNegativeX)
						tipMaxNegativeX = _leadingEdgeVerticesPosition[i].x;
				}

				float rootRotatingPoint = rootMaxPositiveX - (rootMaxPositiveX - rootMaxNegativeX) / 2f;
				float tipRotatingPoint = tipMaxPositiveX - (tipMaxPositiveX - tipMaxNegativeX) / 2f;

				CreateLeadingEdgePoints(rootSlicePoint, tipSlicePoint, rootRotatingPoint, tipRotatingPoint);
			}
		}

		/// <summary>
		/// Create leading edge points.
		/// </summary>
		/// <param name="rootSlicePoint">The Z-coordinate in space of 3d-model of wing of root slice point.</param>
		/// <param name="tipSlicePoint">The Z-coordinate in space of 3d-model of wing of tip slice point.</param>
		/// <param name="rootRotatingPoint">The X-coordinate in space of 3d-model of wing of root slice point.</param>
		/// <param name="tipRotatingPoint">The X-coordinate in space of 3d-model of wing of tip slice point.</param>
		private void CreateLeadingEdgePoints(float rootSlicePoint, float tipSlicePoint, float rootRotatingPoint, float tipRotatingPoint)
		{
			_leadingEdgeRootLeadPoint = new GameObject();
			_leadingEdgeRootLeadPoint.name = "LeadingEdgeRootLeadPoint";

			_leadingEdgeTipLeadPoint = new GameObject();
			_leadingEdgeTipLeadPoint.name = "LeadingEdgeTipLeadPoint";

			_leadingEdgeRotationAxis = new GameObject();
			_leadingEdgeRotationAxis.name = "LeadingEdgeRotationAxis";

			_leadingEdgeParent = new GameObject();
			_leadingEdgeParent.name = "LeadingEdgeParent";

			_leadingEdgeRootLeadPoint.transform.parent = SimpleWingContainer;
			_leadingEdgeRootLeadPoint.transform.localPosition = new Vector3(rootRotatingPoint, 0f, rootSlicePoint);
			_leadingEdgeRootLeadPoint.transform.localEulerAngles = Vector3.zero;
			_leadingEdgeRootLeadPoint.transform.localScale = Vector3.one;

			_leadingEdgeTipLeadPoint.transform.parent = SimpleWingContainer;
			_leadingEdgeTipLeadPoint.transform.localPosition = new Vector3(tipRotatingPoint, _length, tipSlicePoint);
			_leadingEdgeTipLeadPoint.transform.localEulerAngles = Vector3.zero;
			_leadingEdgeTipLeadPoint.transform.localScale = Vector3.one;

			_leadingEdgeRotationAxis.transform.parent = SimpleWingContainer;
			_leadingEdgeRotationAxis.transform.localPosition = _leadingEdgeRootLeadPoint.transform.localPosition;
			_leadingEdgeRotationAxis.transform.LookAt(_leadingEdgeTipLeadPoint.transform, SimpleWingContainer.forward * -1f);
			_leadingEdgeRotationAxis.transform.localRotation *= Quaternion.Euler(90f, 0f, 0f);
			_leadingEdgeRotationAxis.transform.localScale = Vector3.one;

			_leadingEdgeParent.transform.parent = _leadingEdgeRotationAxis.transform;
			_leadingEdgeParent.transform.localPosition = Vector3.zero;
			_leadingEdgeParent.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
			_leadingEdgeParent.transform.localScale = Vector3.one;

			_leadingEdgeAuxiliaryParent = new GameObject();
			_leadingEdgeAuxiliaryParent.name = "LeadingEdgeAuxiliaryParent";
			_leadingEdgeAuxiliaryParent.transform.parent = _leadingEdgeParent.transform;
			_leadingEdgeAuxiliaryParent.transform.localPosition = Vector3.zero;
			_leadingEdgeAuxiliaryParent.transform.localEulerAngles = Vector3.zero;
			_leadingEdgeAuxiliaryParent.transform.localScale = Vector3.one;
		}

		/// <summary>
		/// Set leading edge as child of leading edge auxiliary parent object.
		/// </summary>
		private void SetLeadingEdgeParent()
		{
			if (_leadingEdgePercentage > 0)
			{
				LeadingEdge.transform.parent = _leadingEdgeAuxiliaryParent.transform;
			}
		}

		/// <summary>
		/// Washout tip of the wing.
		/// </summary>
		private void WashoutTip()
		{
			float tipLength = _tipLeadingOffset + _tipTrailingOffset;
			float rotationPoint = _tipOffset + _tipLeadingOffset - tipLength * _washoutRelativePoint;

			_verticesPosition = MeshHelper.RotateTipVerticesAroundPivot(_verticesPosition, _washoutAngle, new Vector3(0f, 0f, rotationPoint));
			WingMeshFilter.mesh.vertices = _verticesPosition;
			WingMeshFilter.mesh.RecalculateNormals();
			WingMeshFilter.mesh.RecalculateBounds();

			if (_controlSurfacePercentage > 0)
			{
				_controlSurfaceVerticesPosition = MeshHelper.RotateTipVerticesAroundPivot(_controlSurfaceVerticesPosition, _washoutAngle, new Vector3(0f, 0f, rotationPoint));
				ControlSurface.GetComponent<MeshFilter>().mesh.vertices = _controlSurfaceVerticesPosition;
				ControlSurface.GetComponent<MeshFilter>().mesh.RecalculateNormals();
				ControlSurface.GetComponent<MeshFilter>().mesh.RecalculateBounds();

				_controlSurfaceTipLeadPoint.transform.localPosition = MeshHelper.RotatePointAroundPivot(_controlSurfaceTipLeadPoint.transform.localPosition, _washoutAngle, new Vector3(0f, 0f, rotationPoint));

				ControlSurface.transform.parent = SimpleWingContainer;
				_controlSurfaceRotationAxis.transform.LookAt(_controlSurfaceTipLeadPoint.transform, SimpleWingContainer.forward * -1f);
				_controlSurfaceRotationAxis.transform.localRotation *= Quaternion.Euler(90f, 0f, 0f);
				ControlSurface.transform.parent = _controlSurfaceParent.transform;
			}

			if (_leadingEdgePercentage > 0)
			{
				_leadingEdgeVerticesPosition = MeshHelper.RotateTipVerticesAroundPivot(_leadingEdgeVerticesPosition, _washoutAngle, new Vector3(0f, 0f, rotationPoint));
				LeadingEdge.GetComponent<MeshFilter>().mesh.vertices = _leadingEdgeVerticesPosition;
				LeadingEdge.GetComponent<MeshFilter>().mesh.RecalculateNormals();
				LeadingEdge.GetComponent<MeshFilter>().mesh.RecalculateBounds();

				_leadingEdgeTipLeadPoint.transform.localPosition = MeshHelper.RotatePointAroundPivot(_leadingEdgeTipLeadPoint.transform.localPosition, _washoutAngle, new Vector3(0f, 0f, rotationPoint));

				LeadingEdge.transform.parent = SimpleWingContainer;
				_leadingEdgeRotationAxis.transform.LookAt(_leadingEdgeTipLeadPoint.transform, SimpleWingContainer.forward * -1f);
				_leadingEdgeRotationAxis.transform.localRotation *= Quaternion.Euler(90f, 0f, 0f);
				LeadingEdge.transform.parent = _leadingEdgeAuxiliaryParent.transform;
			}
		}

		/// <summary>
		/// Tune the scale and position of airfoil rounding.
		/// </summary>
		private void TuneAirfoilRounding()
		{
			AirfoilRounding.localPosition = Vector3.zero;
			AirfoilRounding.localEulerAngles = Vector3.zero;
			AirfoilRounding.localScale = Vector3.one;
			AirfoilRounding.gameObject.SetActive(false);

			if (!_isAirfoilRounded)
				return;

			AirfoilRounding.GetComponent<MeshFilter>().mesh =
				SimpleWingContainer.FindChildByContainsName($"{_airfoilTipType}_rounding") != null
				? SimpleWingContainer.FindChildByContainsName($"{_airfoilTipType}_rounding").GetComponent<MeshFilter>().mesh
				: null;

			if (AirfoilRounding.GetComponent<MeshFilter>().mesh == null)
				return;

			float tipScaleMultiplier = _tipLeadingOffset + _tipTrailingOffset;
			float tipScaleOffset = (_tipLeadingOffset - _tipTrailingOffset) / 2f;

			AirfoilRounding.localPosition = new Vector3(0f, _length, tipScaleOffset + _tipOffset);
			float invertAirfoilTip = _isAirfoilInverted ? -1f : 1f;
			AirfoilRounding.localScale =
				new Vector3(tipScaleMultiplier * _tipThicknessMultiplier * invertAirfoilTip, tipScaleMultiplier * _tipThicknessMultiplier * _airfoilRoundingLength, tipScaleMultiplier);

			// Washout
			float tipLength = _tipLeadingOffset + _tipTrailingOffset;
			float rotationPoint = _tipOffset + _tipLeadingOffset - tipLength * _washoutRelativePoint;
			AirfoilRounding.localPosition = MeshHelper.RotatePointAroundPivot(AirfoilRounding.localPosition, _washoutAngle, new Vector3(0f, 0f, rotationPoint));
			AirfoilRounding.localEulerAngles = new Vector3(0f, _washoutAngle, 0f);

			AirfoilRounding.gameObject.SetActive(true);
		}

		internal void ApplyStockMaterialToAirfoil()
		{
			PartMaterial partMaterial = this.PartScript.PartMaterialScript.GetPartMaterial(0);
			Airfoil.GetComponent<MeshRenderer>().material.color = partMaterial.Color;
			Airfoil.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", partMaterial.Metallic);
			Airfoil.GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", partMaterial.Smoothness);

			AirfoilRounding.GetComponent<MeshRenderer>().material.color = partMaterial.Color;
			AirfoilRounding.GetComponent<MeshRenderer>().material.SetFloat("_Metallic", partMaterial.Metallic);
			AirfoilRounding.GetComponent<MeshRenderer>().material.SetFloat("_Glossiness", partMaterial.Smoothness);
		}
		//--------------------------------------------------------------CORE GEOMETRY FUNCTIONS END


		//ATTACH POINTS FUNCTIONS------------------------------------------------------------------
		private void GetAttachPoints()
		{
			if (Game.InDesignerScene)
			{
				_attachPointRoot = this.transform.FindChildByName("AttachPoints").GetChild(0);
				_attachPointTip = this.transform.FindChildByName("AttachPoints").GetChild(1);
				_attachPointUp = this.transform.FindChildByName("AttachPoints").GetChild(2);
				_attachPointDown = this.transform.FindChildByName("AttachPoints").GetChild(3);
			}
		}

		private void TuneAttachPoints()
		{
			if (Game.InDesignerScene)
			{
				_attachPointRoot.localPosition = new Vector3(0f, -_length / 2f, -_tipOffset / 2f);
				_attachPointTip.localPosition = new Vector3(0f, _length / 2f, _tipOffset / 2f);

				float tipLength = _tipLeadingOffset + _tipTrailingOffset;
				float rotationPoint = _tipOffset / 2f + _tipLeadingOffset - tipLength * _washoutRelativePoint;
				_attachPointTip.localPosition = MeshHelper.RotatePointAroundPivot(_attachPointTip.localPosition, _washoutAngle, new Vector3(0f, 0f, rotationPoint));
				_attachPointTip.localEulerAngles = new Vector3(-90f, _washoutAngle, 180f);

				float factWingRootLength = _rootLeadingOffset + _rootTrailingOffset;
				float factWingTipLength = _tipLeadingOffset + _tipTrailingOffset;
				float factAirfoilRootThickness = _airfoilRootThickness / 100f * factWingRootLength;
				float factAirfoilTipThickness = _airfoilTipThickness / 100f * factWingTipLength;
				float factMidThickness = (factAirfoilRootThickness + factAirfoilTipThickness) / 2f;
				_attachPointUp.localPosition = new Vector3(factMidThickness, 0f, 0f);
				_attachPointDown.localPosition = new Vector3(-factMidThickness, 0f, 0f);
			}
		}
		//--------------------------------------------------------------ATTACH POINTS FUNCTIONS END

		//PHYSICS FUNCTIONS------------------------------------------------------------------------
		private void GetVelocityVector()
		{
			VelocityVector = SimpleWingContainer.FindChildByName("VelocityVector");
			VelocityPyramid = VelocityVector.FindChildByName("VelocityPyramid");
			LiftVector = VelocityVector.FindChildByName("LiftVector");
			DragVector = VelocityVector.FindChildByName("DragVector");
			WaveDragVector = VelocityVector.FindChildByName("WaveDragVector");
		}

		private void TuneVelocityVector()
		{
			float rootChordLength = _rootLeadingOffset + _rootTrailingOffset;
			float tipChordLength = _tipLeadingOffset + _tipTrailingOffset;

			const float MiddleChordPosition = 0.5f;

			// Line 1
			float x1 = _rootLeadingOffset - rootChordLength * MiddleChordPosition;
			float y1 = 0f;
			float x2 = _tipLeadingOffset + _tipOffset - tipChordLength * MiddleChordPosition;
			float y2 = _length;

			// Line 2
			float x3 = _rootLeadingOffset - rootChordLength - tipChordLength;
			float y3 = 0f;
			float x4 = _tipLeadingOffset + _tipOffset + rootChordLength;
			float y4 = _length;

			// Intersection
			float topY = (x1 * y2 - x2 * y1) * (y3 - y4) - (x3 * y4 - x4 * y3) * (y1 - y2);
			float bottom = (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);
			float intersectY = topY / bottom;
			
            float rootLeadPoint = _rootLeadingOffset;
			float rootTrailPoint = -_rootTrailingOffset;
			float tipLeadPoint = _tipLeadingOffset + _tipOffset;
			float tipTrailPoint = -_tipTrailingOffset + _tipOffset;

			float meanChordLeadPoint = Mathf.Lerp(rootLeadPoint, tipLeadPoint, intersectY / _length);
			float meanChordTrailPoint = Mathf.Lerp(rootTrailPoint, tipTrailPoint, intersectY / _length);
			float meanChordLength = meanChordLeadPoint - meanChordTrailPoint;

			const float MeanChordAerodynamicFocus = 0.25f; // 25% of mean chord

			/// Position of VelocityVector changes in <see cref="MoveAerodynamicCenter"/>, instruction below is initial position.
			VelocityVector.localPosition = new Vector3(0f, intersectY, meanChordLeadPoint - meanChordLength * MeanChordAerodynamicFocus);

			_meanChordLeadPoint = meanChordLeadPoint;
			_meanChordPositionY = intersectY;
			_meanChordLength = meanChordLength;
		}

		private void CalculateAerodynamicCurves()
        {
			if (_wingPhysicsEnabled)
			{
				// LERX DATA
				CalculateRootAttachedLerxEfficiency(out bool isRootAttachedLerxExist, out float positiveRootAttachedLerxEfficiency, out float negativeRootAttachedLerxEfficiency, out float rootAttachedLerxCriticalAngleRaise, out float rootAttachedLerxPostCriticalEfficiency, out float _, out float _, out float _);

				if (ControlSurface != null && _controlSurfacePercentage > 0)
				{
					_controlSurfaceSlipAngleEfficiencyCoefficient = Mathf.Abs(Mathf.Cos(GetAngleOfSlip(_controlSurfaceRotationAxis.transform) * Mathf.Deg2Rad));
				}

				// LIFT
				_Cy.preWrapMode = WrapMode.Loop;
				_Cy.postWrapMode = WrapMode.Loop;
				_Cy.keys = LiftCurveCalculator.CalculateLiftCurve(_airfoilRootType, _airfoilTipType, _airfoilRootThickness, _airfoilTipThickness, _leadingEdgePercentage, _leadingEdgeAngle, _controlSurfacePercentage, _controlSurfaceAngle * _controlSurfaceSlipAngleEfficiencyCoefficient, _isAirfoilInverted, _washoutAngle, isRootAttachedLerxExist, negativeRootAttachedLerxEfficiency, positiveRootAttachedLerxEfficiency, rootAttachedLerxCriticalAngleRaise, rootAttachedLerxPostCriticalEfficiency, out _liftCoefficientPerDegree, out _controlSurfaceRotationAngleLiftEfficiencyCoefficient);

				// DRAG
				_Cx.preWrapMode = WrapMode.Loop;
				_Cx.postWrapMode = WrapMode.Loop;
				_Cx.keys = DragCurveCalculator.CalculateDragCurve(_airfoilRootType, _airfoilTipType, _airfoilRootThickness, _airfoilTipThickness, _leadingEdgePercentage, _leadingEdgeAngle, _controlSurfacePercentage, _controlSurfaceAngle * _controlSurfaceSlipAngleEfficiencyCoefficient, _isAirfoilInverted, _washoutAngle, isRootAttachedLerxExist, negativeRootAttachedLerxEfficiency, positiveRootAttachedLerxEfficiency, rootAttachedLerxCriticalAngleRaise, out _simpleWingData_NegativeDragPerDegree, out _simpleWingData_PositiveDragPerDegree);

				// AERODYNAMIC CENTER
				_aC.preWrapMode = WrapMode.Clamp;
				_aC.postWrapMode = WrapMode.Clamp;
				_aC.keys = AerodynamicCenterCurveCalculator.CalculateAerodynamicCenterCurve(_Cy.keys[7].time, _Cy.keys[9].time);
			}
		}

		private void GetLerxAngleOfAttackEfficiency()
		{
			if (_wingPhysicsEnabled)
			{
				if (_isLerx && _Cy.length == 17)
				{
					const float PostFortyFiveLength = 15f;

					float angleOfAttack = GetAngleOfAttack();
					const float fullEfficiencyLength = 4f;

					_positiveLerxAngleOfAttackEfficiency = Mathf.InverseLerp(_Cy.keys[9].time, _Cy.keys[9].time + fullEfficiencyLength, angleOfAttack);
					_negativeLerxAngleOfAttackEfficiency = Mathf.InverseLerp(_Cy.keys[7].time, _Cy.keys[7].time - fullEfficiencyLength, angleOfAttack);

					if (angleOfAttack > _Cy.keys[11].time)
					{
						_positiveLerxAngleOfAttackEfficiency = Mathf.Lerp(1f, 0f, Mathf.InverseLerp(_Cy.keys[11].time, _Cy.keys[11].time + PostFortyFiveLength, angleOfAttack));
					}

					if (angleOfAttack < _Cy.keys[5].time)
					{
						_negativeLerxAngleOfAttackEfficiency = Mathf.Lerp(1f, 0f, Mathf.InverseLerp(_Cy.keys[5].time, _Cy.keys[5].time - PostFortyFiveLength, angleOfAttack));
					}

					if (Game.InDesignerScene)
					{
						_positiveLerxAngleOfAttackEfficiency = 1f;
						_negativeLerxAngleOfAttackEfficiency = 1f;
					}

					if (!_isAirfoilInverted)
					{
						_negativeLerxAngleOfAttackEfficiency *= _lerxEfficiencyAsymmetryMultiplier;
					}
					else
					{
						_positiveLerxAngleOfAttackEfficiency *= _lerxEfficiencyAsymmetryMultiplier;
					}
				}
				else
				{
					_positiveLerxAngleOfAttackEfficiency = 0f;
					_negativeLerxAngleOfAttackEfficiency = 0f;
				}
			}
		}

		private void CalculateRootAttachedLerxEfficiency(
			out bool isRootAttachedLerxExist,
			out float positiveRootAttachedLerxEfficiency,
			out float negativeRootAttachedLerxEfficiency,
			out float rootAttachedLerxCriticalAngleRaise,
            out float rootAttachedLerxPostCriticalEfficiency,
            out float areaEfficiency,
			out float wingspanCoverageEfficiency,
			out float meanChordCoverageEfficiency)
		{
			isRootAttachedLerxExist = false;
			positiveRootAttachedLerxEfficiency = 0f;
			negativeRootAttachedLerxEfficiency = 0f;
			rootAttachedLerxCriticalAngleRaise = 0f;
            rootAttachedLerxPostCriticalEfficiency = 0f;
            areaEfficiency = 0f;
			wingspanCoverageEfficiency = 0f;
			meanChordCoverageEfficiency = 0f;

            // Root attached LERX can affect only non-LERX wing.
            if (!_isLerx && _rootAttachedLerx != null && _rootAttachedLerx._isLerx)
			{
				isRootAttachedLerxExist = true;

                bool isRootAttachedChainLerx = _rootAttachedLerx._isChainLerx;

				float rootAttachedLerxArea = isRootAttachedChainLerx
					? _rootAttachedLerx._chainLerxArea
					: _rootAttachedLerx._lerxArea;

				float positiveRootAttachedLerxAngleOfAttackEfficiency = isRootAttachedChainLerx
					? _rootAttachedLerx._chainLerx.Sum(x => x._positiveLerxAngleOfAttackEfficiency) / _rootAttachedLerx._chainLerx.Count
					: _rootAttachedLerx._positiveLerxAngleOfAttackEfficiency;

				float negativeRootAttachedLerxAngleOfAttackEfficiency = isRootAttachedChainLerx
					? _rootAttachedLerx._chainLerx.Sum(x => x._negativeLerxAngleOfAttackEfficiency) / _rootAttachedLerx._chainLerx.Count
					: _rootAttachedLerx._negativeLerxAngleOfAttackEfficiency;

                float rootAttachedLerxLeadingEdgeLength = isRootAttachedChainLerx
					? _rootAttachedLerx._chainLerxLeadingEdgeLength
					: _rootAttachedLerx._lerxLeadingEdgeLength;

                float rootAttachedLerxCoverageMultiplier = isRootAttachedChainLerx
					? _rootAttachedLerx._chainLerxCoverageMultiplier
                    : _rootAttachedLerx._lerxCoverageMultiplier;

				rootAttachedLerxCriticalAngleRaise = isRootAttachedChainLerx
					? _rootAttachedLerx._chainLerxCriticalAngleRaise
					: _rootAttachedLerx._lerxCriticalAngleRaise;

				const float FullEfficiencyAreaRatio = 0.25f;
				float areaRatio = rootAttachedLerxArea / _wingArea;
				areaEfficiency = Mathf.Clamp(areaRatio / FullEfficiencyAreaRatio, 0f, 1f);
				float coverageLength = rootAttachedLerxLeadingEdgeLength * rootAttachedLerxCoverageMultiplier;

                Vector3 sideFrom = _rootAttachedLerx.SimpleWingContainer.position + _rootAttachedLerx.SimpleWingContainer.TransformVector(new Vector3(0f, _rootAttachedLerx._length, 0f));
                Vector3 sideTo = this.SimpleWingContainer.position + this.SimpleWingContainer.TransformVector(new Vector3(0f, _length, 0f));
                float sideDistance = Vector3.Project(sideTo - sideFrom, this.SimpleWingContainer.up).magnitude;
                float sideDot = Vector3.Dot(_rootAttachedLerx.SimpleWingContainer.up.normalized, this.SimpleWingContainer.up.normalized);

                Vector3 directFrom = _rootAttachedLerx.SimpleWingContainer.position + _rootAttachedLerx.SimpleWingContainer.TransformVector(new Vector3(0f, _rootAttachedLerx._length, _rootAttachedLerx._tipLeadingOffset + _rootAttachedLerx._tipOffset));
                Vector3 directTo = this.SimpleWingContainer.position + this.SimpleWingContainer.TransformVector(new Vector3(0f, _meanChordPositionY, _meanChordLeadPoint - _meanChordLength));
                float directDistance = Vector3.Project(directTo - directFrom, this.SimpleWingContainer.forward).magnitude;
                float directDot = Vector3.Dot(_rootAttachedLerx.SimpleWingContainer.forward.normalized, this.SimpleWingContainer.forward.normalized);

				Vector3 antiDirectTo = this.SimpleWingContainer.position + this.SimpleWingContainer.TransformVector(new Vector3(0f, _meanChordPositionY, _meanChordLeadPoint));
				Vector3 antiDirectProject = Vector3.Project(antiDirectTo - directFrom, this.SimpleWingContainer.forward);
				float antiDirectDistance = antiDirectProject.magnitude;
				float antiDirectDot = Vector3.Dot(antiDirectProject.normalized, this.SimpleWingContainer.forward.normalized);
				float antiDirectRatio = Mathf.Clamp(1f - antiDirectDistance * Mathf.Clamp(Mathf.Sign(antiDirectDot), 0f, 1f) / Mathf.Clamp(_meanChordLength, 0.001f, Mathf.Infinity), 0f, 1f);

                Vector3 postCriticalFrom = directFrom + _rootAttachedLerx.SimpleWingContainer.TransformVector(new Vector3(0f, 0f, -coverageLength));
                Vector3 postCriticalTo = this.SimpleWingContainer.position + this.SimpleWingContainer.TransformVector(new Vector3(0f, _meanChordPositionY, _meanChordLeadPoint - _meanChordLength));
                float postCriticalDistance = Vector3.Project(postCriticalTo - postCriticalFrom, this.SimpleWingContainer.forward).magnitude;

				Vector3 antiPostCriticalTo = this.SimpleWingContainer.position + this.SimpleWingContainer.TransformVector(new Vector3(0f, _meanChordPositionY, _meanChordLeadPoint));
				Vector3 antiPostCriticalProject = Vector3.Project(antiPostCriticalTo - postCriticalFrom, this.SimpleWingContainer.forward);
				float antiPostCriticalDistance = antiPostCriticalProject.magnitude;
				float antiPostCriticalDot = Vector3.Dot(antiPostCriticalProject.normalized, this.SimpleWingContainer.forward.normalized);
				float antiPostCriticalRatio = Mathf.Clamp(1f - antiPostCriticalDistance * Mathf.Clamp(Mathf.Sign(antiPostCriticalDot), 0f, 1f) / Mathf.Clamp(_meanChordLength, 0.001f, Mathf.Infinity), 0f, 1f);

                wingspanCoverageEfficiency = Mathf.InverseLerp(sideDistance - _length, sideDistance, coverageLength);

				meanChordCoverageEfficiency = Mathf.InverseLerp(directDistance - _meanChordLength, directDistance, coverageLength)
					* Mathf.Clamp(sideDot, 0f, 1f)
					* Mathf.Clamp(Mathf.Sign(directDot), 0f, 1f)
					* antiDirectRatio;

				float postCriticalWingspanCoverageEfficiency = Mathf.InverseLerp(sideDistance - _length, sideDistance, coverageLength / 2f);
				rootAttachedLerxPostCriticalEfficiency = Mathf.InverseLerp(postCriticalDistance - _meanChordLength, postCriticalDistance, coverageLength)
                    * Mathf.Clamp(sideDot, 0f, 1f)
                    * Mathf.Clamp(Mathf.Sign(directDot), 0f, 1f)
					* antiPostCriticalRatio
					* areaEfficiency
					* postCriticalWingspanCoverageEfficiency;

				positiveRootAttachedLerxEfficiency = areaEfficiency * wingspanCoverageEfficiency * meanChordCoverageEfficiency * positiveRootAttachedLerxAngleOfAttackEfficiency;
				negativeRootAttachedLerxEfficiency = areaEfficiency * wingspanCoverageEfficiency * meanChordCoverageEfficiency * negativeRootAttachedLerxAngleOfAttackEfficiency;
			}
		}

        private void MoveAerodynamicCenter()
		{
			if (Game.InFlightScene && _wingPhysicsEnabled && _aC.keys.Length == 4)
			{
				// Calculating position of aerodynamic center from angle of attack
				float angleOfAttack = GetAngleOfAttack();
				float aerodynamicCenterFromAOA = _aC.Evaluate(angleOfAttack);

				// Calculating position of aerodynamic center from speed
				float machCritical = _airfoilCriticalMachNumber / _dynamicLeadAngleLiftDragCoefficient;
				float machNumber = this.PartScript.BodyScript.MachNumber;
				float deltaMach = Mathf.Clamp(machNumber - machCritical, 0f, Mathf.Infinity);
				const float MaximalMovingBackFromSpeed = 0.25f;
				const float MaximalDeltaMach = 0.1f;
				float aerodynamicCenterFromSpeed = Mathf.Lerp(0f, MaximalMovingBackFromSpeed, Mathf.InverseLerp(0f, MaximalDeltaMach, deltaMach));

				// Calculating position of aerodynamic center
				const float MinimalAerodynamicCenter = 0.25f; // 25% of mean chord
				const float MaximalAerodynamicCenter = 0.5f; // 50% of mean chord
				float meanChordAerodynamicCenter = Mathf.Clamp(aerodynamicCenterFromAOA + aerodynamicCenterFromSpeed, MinimalAerodynamicCenter, MaximalAerodynamicCenter);

				VelocityVector.localPosition = new Vector3(0f, _meanChordPositionY, _meanChordLeadPoint - _meanChordLength * meanChordAerodynamicCenter);
			}
		}
		//--------------------------------------------------------------------PHYSICS FUNCTIONS END

		private void UpdateStockMesh()
		{
			var wingScript = this.transform.GetComponent<WingScript>();
			if (wingScript != null)
			{
				wingScript.UpdateWingShape();
				wingScript.UpdateAirfoil("Symmetric");
			}
		}
		
		private void DisableStockMesh()
		{
			var wingRoot = this.transform.FindChildByName("WingRoot");
			if (wingRoot != null)
			{
				foreach (var meshRenderer in wingRoot.GetComponentsInChildren<MeshRenderer>())
				{
					if (Game.InFlightScene)
					{
						meshRenderer.gameObject.SetActive(false);
					}
					else
					{
						meshRenderer.enabled = false;
					}
				}
			}
		}

		private void DestroyUnusedObjects()
		{
			if (Game.InFlightScene)
			{
				DestroyImmediate(SimpleWingContainer.FindChildByName("Airfoils").gameObject);
				DestroyImmediate(SimpleWingContainer.FindChildByName("AirfoilRoundings").gameObject);

				if (!_isAirfoilRounded)
				{
					DestroyImmediate(AirfoilRounding.gameObject);
				}

				if (!_wingPhysicsEnabled)
				{
					DestroyImmediate(VelocityVector.gameObject);
				}
			}
		}

		#endregion

		#region UPDATE FUNCTIONS---------------------------------------------------------------------------
		
		//VISIBLE FUNCTIONS------------------------------------------------------------------------
		private void RotateVelocityVector()
		{
			if (Game.InFlightScene && _wingPhysicsEnabled)
			{
				Vector3 velocity = this.PartScript.BodyScript.RigidBody.GetPointVelocity(VelocityVector.position) + GetVelocityDifference();
				VelocityVector.rotation = Quaternion.LookRotation(velocity, SimpleWingContainer.up);

				// Vector scaling
				VelocityPyramid.localScale = new Vector3(0.05f * 0.5f, 0.05f * 0.5f, ModSettings.Instance.VelocityVectorLengthScale);
			}
		}

		private void RotateControlSurface()
		{
			if (Game.InFlightScene && _controlSurfacePercentage > 0 && _controlSurfaceInputController != null)
			{
				float headingAngle = 0f;

				float limitedSurfaceInputValue = Mathf.Clamp(_controlSurfaceInputController.Value, -1f, 1f);
				headingAngle = 180f + limitedSurfaceInputValue * _controlSurfaceDeflectionAngle;

				float currentAngle = _controlSurfaceParent.transform.localEulerAngles.y;
				currentAngle = AngleTracker.TrackAngle(headingAngle, currentAngle, _controlSurfaceRotationSpeed, 90f, 270f);

				_controlSurfaceParent.transform.localEulerAngles = new Vector3(0f, currentAngle, 0f);
				_controlSurfaceAngle = currentAngle - 180f;
			}
			else if (!_controlSurfaceInputControllerNullMessageSended && _controlSurfacePercentage > 0 && _controlSurfaceInputController == null)
			{
				_controlSurfaceInputControllerNullMessageSended = true;

				string message = $"{nameof(SimpleWingScript)}: Part id = {this.Data.Part.Id}, InputController with inputId = \"ControlSurface\" not found. Unpossible to rotate control surface.";
				
				if (Game.InDesignerScene)
				{
					Game.Instance.Designer.DesignerUi.ShowMessage(message);
				}
					
				Game.Instance.DevConsole.LogWarning(message);
			}
		}

		private void RotateLeadingEdge()
		{
			if (Game.InFlightScene && _leadingEdgePercentage > 0)
			{
				float angleOfAttack = GetAngleOfAttack() * _leadingEdgeAngleOfAttackSensitivity;

				float limitedLeadingEdgeInputValue = _isAirfoilInverted
					? Mathf.Clamp(angleOfAttack, -_leadingEdgeDeflectionAngle, 0f)
					: Mathf.Clamp(angleOfAttack, 0f, _leadingEdgeDeflectionAngle);

				if (this.PartScript.BodyScript.VelocityMagnitude < 1f)
				{
					limitedLeadingEdgeInputValue = 0f;
				}

				if (this.PartScript.CommandPod != null && this.PartScript.CommandPod.GetActivationGroupState(_leadingEdgeFullDeflectActivationGroup))
				{
					limitedLeadingEdgeInputValue = _isAirfoilInverted ? -_leadingEdgeDeflectionAngle : _leadingEdgeDeflectionAngle;
				}

				float headingAngle = 180f - limitedLeadingEdgeInputValue;
				float currentAngle = _leadingEdgeParent.transform.localEulerAngles.y;
				currentAngle = AngleTracker.TrackAngle(headingAngle, currentAngle, _leadingEdgeRotationSpeed, 90f, 270f);
				if (_isLeadingEdgeAttachedToRoot && _rootLeadingEdge != null)
				{
					currentAngle = 180f - _rootLeadingEdge._leadingEdgeAngle;
				}
				_leadingEdgeParent.transform.localEulerAngles = new Vector3(0f, currentAngle, 0f);
				_leadingEdgeAngle = 180f - currentAngle;

				float auxiliaryRotation = _leadingEdgeAngle - (_leadingEdgeAngle * SweepWingCalculator.GetLeadingEdgeRotationCoefficient(_leadingEdgeRotationAxisAngle * Mathf.Rad2Deg));
				_leadingEdgeAuxiliaryParent.transform.localEulerAngles = new Vector3(0f, auxiliaryRotation, 0f);
			}
		}

		private void SetPyramidVisibility()
		{
			if (Game.InFlightScene && _wingPhysicsEnabled && ModSettings.Instance.Debug && this.PartScript.BodyScript.SurfaceVelocity.magnitude > 1f)
			{
				VelocityPyramid.gameObject.SetActive(true);
				LiftVector.gameObject.SetActive(true);
				DragVector.gameObject.SetActive(true);
				WaveDragVector.gameObject.SetActive(true);
			}
			else if (Game.InFlightScene && _wingPhysicsEnabled)
			{
				VelocityPyramid.gameObject.SetActive(false);
				LiftVector.gameObject.SetActive(false);
				DragVector.gameObject.SetActive(false);
				WaveDragVector.gameObject.SetActive(false);
			}
		}
		//--------------------------------------------------------------------VISIBLE FUNCTIONS END

		//DESIGNER UPDATE FUNCTIONS----------------------------------------------------------------
		private void DesignerUpdate()
		{
			if (Game.InDesignerScene)
			{
				GetAndClampModifiers();

				_lastModifiersSumValue = _currentModifiersSumValue;
				_currentModifiersSumValue = _rootLeadingOffset +
											_rootTrailingOffset +
											_tipLeadingOffset +
											_tipTrailingOffset +
											_length +
											_tipOffset;

				if (Mathf.Abs(_currentModifiersSumValue - _lastModifiersSumValue) > Mathf.Epsilon)
				{
					Start();
				}
			}
		}

		private void DesignerOverrideStockMesh()
		{
			if (Game.InDesignerScene && _overrideStockMeshInDesignerEnabled)
			{
				float rootMiddle = (_rootLeadingOffset - _rootTrailingOffset) / 2f;
				var wingRoot = this.transform.FindChildByName("WingRoot");
				if (wingRoot != null)
				{
					var meshFilter = wingRoot.GetComponentInChildren<MeshFilter>();
					if (meshFilter != null && MeshForStockWing != null)
					{
						meshFilter.transform.localPosition = new Vector3(0f, 0f, -rootMiddle);
						meshFilter.mesh = MeshForStockWing;
					}
				}
			}
		}
		//------------------------------------------------------------DESIGNER UPDATE FUNCTIONS END

		#endregion

		#region FIXED UPDATE FUNCTIONS---------------------------------------------------------------------
		
		private void CalculateDynamicLeadAngleLiftDragCoefficient()
		{
			if (Game.InFlightScene && _wingPhysicsEnabled)
			{
				float angleOfSlip = GetAngleOfSlip(SimpleWingContainer);
				_dynamicLeadAngleLiftDragCoefficient = Mathf.Abs(Mathf.Cos(angleOfSlip * Mathf.Deg2Rad + _wingLeadAngle));
				_dynamicLeadAngleLiftDragCoefficient = Mathf.Clamp(_dynamicLeadAngleLiftDragCoefficient, MinimalDynamicLeadAngleLiftDragCoefficient, 1f);
			}
		}

		private void CalculateAndApplyAerodynamicForces()
		{
            if (Game.InFlightScene && _wingPhysicsEnabled)
			{
                // Calculation
                float angleOfAttack = GetAngleOfAttack();
				float fluidDensity = Mathf.Lerp(Mathf.Clamp(this.PartScript.BodyScript.FluidDensity, 0f, 5f), 10f, base.PartScript.WaterPhysics.UnderWaterAmount);
				float airflow = fluidDensity * this.PartScript.BodyScript.VelocitySquared / 2f;
                CalculateRootAttachedLerxEfficiency(out bool _, out float positiveRootAttachedLerxEfficiency, out float negativeRootAttachedLerxEfficiency, out float _, out float _, out float _, out float _, out float _);

				CalculateAndApplyLiftForce(angleOfAttack, airflow, positiveRootAttachedLerxEfficiency, negativeRootAttachedLerxEfficiency);
                CalculateAndApplyDragForce(angleOfAttack, airflow, positiveRootAttachedLerxEfficiency, negativeRootAttachedLerxEfficiency);
                CalculateAndApplyWaveDragForce(airflow);

				CalculateAndApplyControlSurfaceTorque(airflow, angleOfAttack);
			}
        }

        private void CalculateAndApplyLiftForce(float angleOfAttack, float airflow, float positiveRootAttachedLerxEfficiency, float negativeRootAttachedLerxEfficiency)
		{
			// Calculation
			float Cy = _Cy.Evaluate(angleOfAttack);
			float force = Cy * airflow * _wingArea * _dynamicLeadAngleLiftDragCoefficient;

			// Post-critical shake
			float randomShakeValue = 0f;
			if (_Cy.length == 17 && (angleOfAttack > _Cy[9].time || angleOfAttack < _Cy[7].time))
			{
				randomShakeValue = UnityEngine.Random.Range(-1f, 1f) * _airfoilPostCriticalShakeMultiplier;

				if (angleOfAttack > _Cy[9].time)
				{
					randomShakeValue *= 1f - positiveRootAttachedLerxEfficiency * MaximalShakeReduction;
				}
				if (angleOfAttack < _Cy[7].time)
				{
					randomShakeValue *= 1f - negativeRootAttachedLerxEfficiency * MaximalShakeReduction;
				}
			}
			float shakeMultiplier = 1f + randomShakeValue;

			// Force applying
			this.PartScript.BodyScript.RigidBody.AddForceAtPosition(VelocityVector.right * force * shakeMultiplier * Constants.MassScale, VelocityVector.position, ForceMode.Force);

			// Vector scaling
			LiftVector.localScale = new Vector3(Cy * ModSettings.Instance.ForceVectorsLengthScale * shakeMultiplier * _dynamicLeadAngleLiftDragCoefficient, 0.5f * ModSettings.Instance.ForceVectorsWidthScale, 0.5f * ModSettings.Instance.ForceVectorsWidthScale);
		}

		private void CalculateAndApplyDragForce(float angleOfAttack, float airflow, float positiveRootAttachedLerxEfficiency, float negativeRootAttachedLerxEfficiency)
		{
			// Calculation
			float Cx = _Cx.Evaluate(angleOfAttack);
			float currentLeadAngleLiftDragCoefficient = _dynamicLeadAngleLiftDragCoefficient;
			if (_Cx.length == 9 && (angleOfAttack > _Cx[5].time || angleOfAttack < _Cx[3].time)) // Before critical AOA sweeped wing have less drag that straight wing
			{
				if (angleOfAttack > _Cx[5].time)
				{
					currentLeadAngleLiftDragCoefficient = Mathf.Lerp(_dynamicLeadAngleLiftDragCoefficient, 1f, Mathf.InverseLerp(_Cx[5].time, _Cx[6].time, angleOfAttack));
				}
				if (angleOfAttack < _Cx[3].time)
				{
					currentLeadAngleLiftDragCoefficient = Mathf.Lerp(_dynamicLeadAngleLiftDragCoefficient, 1f, Mathf.InverseLerp(_Cx[3].time, _Cx[2].time, angleOfAttack));
				}
			}
			float force = Cx * airflow * _wingArea * currentLeadAngleLiftDragCoefficient;

			// Post-critical shake
			float randomShakeValue = 0f;
			if (_Cx.length == 9 && (angleOfAttack > _Cx[5].time || angleOfAttack < _Cx[3].time))
			{
				randomShakeValue = UnityEngine.Random.Range(0f, 1f) * _airfoilPostCriticalShakeMultiplier; // only 0 or positive

				if (angleOfAttack > _Cx[5].time)
				{
					randomShakeValue *= 1f - positiveRootAttachedLerxEfficiency * MaximalShakeReduction;
				}
				if (angleOfAttack < _Cx[3].time)
				{
					randomShakeValue *= 1f - negativeRootAttachedLerxEfficiency * MaximalShakeReduction;
				}
			}
			float shakeMultiplier = 1f + randomShakeValue;

			// Force applying
			this.PartScript.BodyScript.RigidBody.AddForceAtPosition(VelocityVector.forward * -1f * force * shakeMultiplier * Constants.MassScale, VelocityVector.position, ForceMode.Force);

			// Vector scaling
			DragVector.localScale = new Vector3(0.5f * ModSettings.Instance.ForceVectorsWidthScale, 0.5f * ModSettings.Instance.ForceVectorsWidthScale, Cx * ModSettings.Instance.ForceVectorsLengthScale * shakeMultiplier * currentLeadAngleLiftDragCoefficient);
		}

		private void CalculateAndApplyWaveDragForce(float airflow)
		{
			// Calculation
			float machCritical = _airfoilCriticalMachNumber / _dynamicLeadAngleLiftDragCoefficient;
			float machNumber = this.PartScript.BodyScript.MachNumber;
			float deltaMach = Mathf.Clamp(machNumber - machCritical, 0f, Mathf.Infinity);
			float CxWave = 0.002f * Mathf.Pow(1f + 2.5f * (deltaMach / (0.06f + deltaMach)), 3f) - 0.002f;
            float force = CxWave * airflow * _wingArea;

			// Force applying
			this.PartScript.BodyScript.RigidBody.AddForceAtPosition(VelocityVector.forward * -1f * force * Constants.MassScale, VelocityVector.position, ForceMode.Force);

			// Vector scaling
			WaveDragVector.localScale = new Vector3(0.5f * ModSettings.Instance.ForceVectorsWidthScale, 0.5f * ModSettings.Instance.ForceVectorsWidthScale, CxWave * ModSettings.Instance.ForceVectorsLengthScale);
		}

		private void CalculateAndApplyControlSurfaceTorque(float airflow, float angleOfAttack)
		{
			if (_controlSurfacePercentage > 0)
			{
				float controlSurfaceLiftCoefficient = _controlSurfaceAngle * _controlSurfaceSlipAngleEfficiencyCoefficient * _liftCoefficientPerDegree;
				float controlSurfaceArea = _wingArea * _controlSurfacePercentage / 100f;
				float maximalControlSurfaceForceArm = _meanChordLength * (1f - (float)_controlSurfacePercentage * 0.5f / 100f);

				Vector3 from = this.SimpleWingContainer.position + this.SimpleWingContainer.TransformVector(new Vector3(0f, _meanChordPositionY, _meanChordLeadPoint - maximalControlSurfaceForceArm));
				Vector3 to = this.PartScript.BodyScript.RigidBody.position;
				float distanceFromControlSurfaceToRigidbody = Vector3.Project(from - to, this.SimpleWingContainer.forward).magnitude;
				float controlSurfaceForceArm = Mathf.Clamp(distanceFromControlSurfaceToRigidbody, 0f, maximalControlSurfaceForceArm);

				float distanceFromVelocityVectorToRigidbody = Vector3.Project(VelocityVector.position - to, this.SimpleWingContainer.forward).magnitude;
				float armCoefficient = Mathf.Clamp(1f - distanceFromVelocityVectorToRigidbody / Mathf.Clamp(maximalControlSurfaceForceArm, 0.001f, Mathf.Infinity), 0f, 1f);

				float postCriticalEfficiency = 1f;
				if (_Cy.length == 17 && (angleOfAttack > _Cy[9].time || angleOfAttack < _Cy[7].time))
				{
					if (angleOfAttack > _Cy[9].time)
					{
						postCriticalEfficiency = Mathf.Lerp(1f, 0f, Mathf.InverseLerp(_Cy[9].time, _Cy[10].time, angleOfAttack));
					}
					if (angleOfAttack < _Cy[7].time)
					{
						postCriticalEfficiency = Mathf.Lerp(1f, 0f, Mathf.InverseLerp(_Cy[7].time, _Cy[6].time, angleOfAttack));
					}
				}

				float torque = controlSurfaceLiftCoefficient * controlSurfaceArea * controlSurfaceForceArm * armCoefficient * airflow
					* _controlSurfaceRotationAngleLiftEfficiencyCoefficient
					* _dynamicLeadAngleLiftDragCoefficient
					* postCriticalEfficiency;

				this.PartScript.BodyScript.RigidBody.AddTorque(this.SimpleWingContainer.up * -1f * torque * Constants.MassScale, ForceMode.Force);
			}
		}
        
		#endregion

        #region LATE UPDATE FUNCTIONS----------------------------------------------------------------------
        
		private void FindLerx()
		{
			if (_wingPhysicsEnabled)
			{
				if (_rootAttachedLerx == null)
				{
					_rootAttachedLerx = Physics.OverlapSphere(SimpleWingContainer.position, _lerxSearchSphereRadius)
						.FirstOrDefault(x => x.transform != this.Airfoil
							&& x.transform.name == "Airfoil"
							&& x.transform.GetComponentInParent<SimpleWingScript>() != null)
						?.GetComponentInParent<SimpleWingScript>();

					bool lerxAutoSearch = _lerxPartId == 0;
					if (!lerxAutoSearch)
					{
						_rootAttachedLerx = Physics.OverlapSphere(SimpleWingContainer.position, _lerxSearchSphereRadius)
						.FirstOrDefault(x => x.transform != this.Airfoil
							&& x.transform.name == "Airfoil"
							&& x.transform.GetComponentInParent<SimpleWingScript>() != null
							&& x.transform.GetComponentInParent<SimpleWingScript>().Data.Part.Id == _lerxPartId)
						?.GetComponentInParent<SimpleWingScript>();
					}
				}

				// ORDER IS IMPORTANT. This block for creating LERX chain.
				if (_isLerx && _rootAttachedLerx != null && _rootAttachedLerx._isLerx)
				{
					_chainLerx.AddRange(_rootAttachedLerx._chainLerx.ToList());
				}

				// ORDER IS IMPORTANT. This block for finding LERX for wing.
				RecursiveLerxSearch();
			}
		}

		private void RecursiveLerxSearch()
		{
			if (_recursiveLerxSearchExecutionsCount < MaxRecursiveExecutionsCount)
			{
				_recursiveLerxSearchExecutionsCount++;

				if (!_isLerx && _rootAttachedLerx != null && !_rootAttachedLerx._isLerx)
				{
					_rootAttachedLerx = _rootAttachedLerx._rootAttachedLerx;
					RecursiveLerxSearch();
				}
			}
		}

		private void CalculateChainLerxData()
		{
			if (_wingPhysicsEnabled)
			{
				_chainLerx = _chainLerx.Distinct().ToList();
				if (_chainLerx.Count > 1)
				{
					_isChainLerx = true;

					_chainLerxLeadingEdgeLength = _chainLerx.Sum(x => x._lerxLeadingEdgeLength);

					float chainLerxAverageLeadAngle = _chainLerx.Sum(x => Mathf.Abs(x._wingLeadAngle)) / _chainLerx.Count;
					_chainLerxArea = Mathf.Pow(_chainLerxLeadingEdgeLength, 2f) / 4f * Mathf.Sin(2f * chainLerxAverageLeadAngle);

					_chainLerxCoverageMultiplier = _chainLerx.Sum(x => LerxHelper.CalculateLerxCoverageMultiplier(x._airfoilRootType, x._airfoilTipType)) / _chainLerx.Count;
					_chainLerxCriticalAngleRaise = _chainLerx.Sum(x => LerxHelper.CalculateLerxCriticalAngleRaise(x._airfoilRootType, x._airfoilTipType)) / _chainLerx.Count;
				}
				else
				{
					_isChainLerx = false;
					_chainLerxArea = 0f;
					_chainLerxLeadingEdgeLength = 0f;
					_chainLerxCoverageMultiplier = 0f;
					_chainLerxCriticalAngleRaise = 0f;
				}
			}
		}

		private void FindRootLeadingEdge()
		{
			_rootLeadingEdge = Physics.OverlapSphere(SimpleWingContainer.position, 0.01f)
				.FirstOrDefault(x => x.transform != this.Airfoil
					&& x.transform.name == "Airfoil"
					&& x.transform.GetComponentInParent<SimpleWingScript>() != null)?.GetComponentInParent<SimpleWingScript>();
		}

		public void SimpleWingDataRedraw()
		{
			if (Game.InDesignerScene && _wingPhysicsEnabled)
			{
				// LIFT
				string zeroAngle = $"Zero angle = {_Cy.keys[8].time.ToString("0.00")}°";
				string zeroCoefficient = $"Zero coefficient = {_Cy.keys[8].value.ToString("0.00")}";

				string positiveCriticalLiftAngle = $"Positive critical angle = {_Cy.keys[9].time.ToString("0.00")}°";
				string positiveCriticalLiftCoefficient = $"Positive critial coefficient = {_Cy.keys[9].value.ToString("0.00")}";

				string negativeCriticalLiftAngle = $"Negative critical angle = {_Cy.keys[7].time.ToString("0.00")}°";
				string negativeCriticalLiftCoefficient = $"Negative critical coefficient = {_Cy.keys[7].value.ToString("0.00")}";

				float positiveLiftPerDegree = Mathf.Abs(_Cy.keys[9].time - _Cy.keys[8].time) > Mathf.Epsilon ?
					(_Cy.keys[9].value - _Cy.keys[8].value) / (_Cy.keys[9].time - _Cy.keys[8].time)
					: 0f;
				float negativeLiftPerDegree = Mathf.Abs(_Cy.keys[8].time - _Cy.keys[7].time) > Mathf.Epsilon ?
					(_Cy.keys[8].value - _Cy.keys[7].value) / (_Cy.keys[8].time - _Cy.keys[7].time)
					: 0f;
				
				string positiveLiftPerDegreeText = $"Lift coeff. per degree of +AoA = {positiveLiftPerDegree.ToString("0.000")}";
				string negativeLiftPerDegreeText = $"Lift coeff. per degree of -AoA = {(-negativeLiftPerDegree).ToString("0.000")}";

				float positivePostCriticalLength = _Cy.keys[10].time - _Cy.keys[9].time;
				float negativePostCriticalLength = _Cy.keys[7].time - _Cy.keys[6].time;

				string positivePostCriticalLengthText = $"Positive post-critical length = {positivePostCriticalLength.ToString("0.00")}°";
				string negativePostCriticalLengthText = $"Negative post-critical length = {negativePostCriticalLength.ToString("0.00")}°";

				// DRAG
				string minimalDragAngle = $"Minimal drag angle = {_Cx.keys[4].time.ToString("0.00")}°";
				string minimalDragCoefficient = $"Minimal drag coefficient = {_Cx.keys[4].value.ToString("0.00000")}";

				string positiveCriticalDragAngle = $"Positive critical angle = {_Cx.keys[5].time.ToString("0.00")}°";
				string positiveCriticalDragCoefficient = $"Positive critical coefficient = {_Cx.keys[5].value.ToString("0.00000")}";

				string negativeCriticalDragAngle = $"Negative critical angle = {_Cx.keys[3].time.ToString("0.00")}°";
				string negativeCriticalDragCoefficient = $"Negative critical coefficient = {_Cx.keys[3].value.ToString("0.00000")}";

				string positiveDragPerDegree = $"Drag coeff. per degree of +AoA = {_simpleWingData_PositiveDragPerDegree.ToString("0.00000")}";
				string negativeDragPerDegree = $"Drag coeff. per degree of -AoA = {_simpleWingData_NegativeDragPerDegree.ToString("0.00000")}";

				// WING
				string wingArea = $"Wing area, m^2 = {_wingArea.ToString("0.00")}";
				string sweepAngle = $"Sweep angle = {(_wingLeadAngle * Mathf.Rad2Deg).ToString("0.00")}°";
				string liftDragEfficiency = $"Lift and drag efficiency = {((Mathf.Clamp(Mathf.Cos(_wingLeadAngle), MinimalDynamicLeadAngleLiftDragCoefficient, 1f)) * 100f).ToString("0.0")}%";
				string criticalMachNumber = $"Critical Mach number = {(_airfoilCriticalMachNumber / Mathf.Clamp(Mathf.Cos(_wingLeadAngle), MinimalDynamicLeadAngleLiftDragCoefficient, 1f)).ToString("0.00")}";
				string postCriticalShake = $"Post-critical shake = {(_airfoilPostCriticalShakeMultiplier * 100f).ToString("0.0")}%";

				string lerxArea = $"LERX area, m^2 = {_lerxArea.ToString("0.00")}";
				string lerxPositiveCriticalAngleRaise = $"LERX critical angle raise, +AoA = {(_lerxCriticalAngleRaise * _positiveLerxAngleOfAttackEfficiency).ToString("0.00")}°";
				string lerxNegativeCriticalAngleRaise = $"LERX critical angle raise, -AoA = {(_lerxCriticalAngleRaise * _negativeLerxAngleOfAttackEfficiency).ToString("0.00")}°";
				string lerxCoverage = $"LERX coverage, m = {(_lerxLeadingEdgeLength * _lerxCoverageMultiplier).ToString("0.00")}";
				
				string ChainLerxCount = $"Chain LERX count = {_chainLerx.Count}";
				string ChainLerxArea = $"Chain LERX area, m^2 = {_chainLerxArea.ToString("0.00")}";
				string ChainLerxPositiveCriticalAngleRaise = $"Chain LERX critical angle raise, +AoA = {(_chainLerxCriticalAngleRaise * _positiveLerxAngleOfAttackEfficiency).ToString("0.00")}°";
				string ChainLerxNegativeCriticalAngleRaise = $"Chain LERX critical angle raise, -AoA = {(_chainLerxCriticalAngleRaise * _negativeLerxAngleOfAttackEfficiency).ToString("0.00")}°";
				string ChainLerxCoverage = $"Chain LERX coverage, m = {(_chainLerxLeadingEdgeLength * _chainLerxCoverageMultiplier).ToString("0.00")}";

				// LERX INFLUENCE
				string lerxPartId = (!_isLerx && _rootAttachedLerx != null && _rootAttachedLerx._isLerx) ? _rootAttachedLerx.Data.Part.Id.ToString() : "not found";
				string lerxPartIdText = $"LERX part id = {lerxPartId}";
				CalculateRootAttachedLerxEfficiency(out bool _, out float positiveRootAttachedLerxEfficiency, out float negativeRootAttachedLerxEfficiency, out float rootAttachedLerxCriticalAngleRaise, out float rootAttachedLerxPostCriticalEfficiency, out float areaEfficiency, out float wingspanCoverageEfficiency, out float meanChordCoverageEfficiency);
				string positiveRootAttachedLerxEfficiencyText = $"Full efficiency at +AoA = {(positiveRootAttachedLerxEfficiency * 100f).ToString("0.0")}%";
				string negativeRootAttachedLerxEfficiencyText = $"Full efficiency at -AoA = {(negativeRootAttachedLerxEfficiency * 100f).ToString("0.0")}%";
				string positiveRootAttachedLerxCriticalAngleRaise = $"Critical angle raise at +AoA = {(rootAttachedLerxCriticalAngleRaise * positiveRootAttachedLerxEfficiency).ToString("0.00")}°";
				string negativeRootAttachedLerxCriticalAngleRaise = $"Critical angle raise at -AoA = {(rootAttachedLerxCriticalAngleRaise * negativeRootAttachedLerxEfficiency).ToString("0.00")}°";
				string areaCoverage = $"Area coverage = {(areaEfficiency * 100f).ToString("0.0")}%";
				string wingspanCoverage = $"Wingspan coverage = {(wingspanCoverageEfficiency * 100f).ToString("0.0")}%";
				string meanChordCoverage = $"Mean chord coverage = {(meanChordCoverageEfficiency * 100f).ToString("0.0")}%";
				string rootAttachedLerxPostCriticalEfficiencyText = $"Post-critical efficiency = {(rootAttachedLerxPostCriticalEfficiency * 100f).ToString("0.0")}%";
				string rootAttachedLerxPostCriticalAngleRaiseText = $"Post-critical angle raise = {(rootAttachedLerxPostCriticalEfficiency * rootAttachedLerxCriticalAngleRaise).ToString("0.00")}°";

				float messageTime = 50f;

				string message = string.Empty;

				switch(ModSettings.Instance.SimpleWingDataInformationOutput)
				{
					case 0:
						break;
					case 1:
						message = $"" +
							$"Part id = {this.Data.Part.Id.ToString()}\n" +
							$"INFORMATION OUTPUT: LIFT DATA\n" +
							$"\n" +
							$"{zeroAngle}\n" +
							$"{zeroCoefficient}\n" +
							$"{positiveCriticalLiftAngle}\n" +
							$"{positiveCriticalLiftCoefficient}\n" +
							$"{negativeCriticalLiftAngle}\n" +
							$"{negativeCriticalLiftCoefficient}\n" +
							$"{positiveLiftPerDegreeText}\n" +
							$"{negativeLiftPerDegreeText}\n" +
							$"{positivePostCriticalLengthText}\n" +
							$"{negativePostCriticalLengthText}\n";

						Game.Instance.Designer.ShowMessage(message, messageTime);
						break;
					case 2:
						message = $"" +
							$"Part id = {this.Data.Part.Id.ToString()}\n" +
							$"INFORMATION OUTPUT: DRAG DATA\n" +
							$"\n" +
							$"{minimalDragAngle}\n" +
							$"{minimalDragCoefficient}\n" +
							$"{positiveCriticalDragAngle}\n" +
							$"{positiveCriticalDragCoefficient}\n" +
							$"{negativeCriticalDragAngle}\n" +
							$"{negativeCriticalDragCoefficient}\n" +
							$"{positiveDragPerDegree}\n" +
							$"{negativeDragPerDegree}\n" +
							$"{positivePostCriticalLengthText}\n" +
							$"{negativePostCriticalLengthText}\n";

						Game.Instance.Designer.ShowMessage(message, messageTime);
						break;
					case 3:
						message = $"" +
							$"Part id = {this.Data.Part.Id.ToString()}\n" +
							$"INFORMATION OUTPUT: WING DATA\n" +
							$"\n" +
							$"{wingArea}\n" +
							$"{sweepAngle}\n" +
							$"{liftDragEfficiency}\n" +
							$"{criticalMachNumber}\n" +
							$"{postCriticalShake}\n" +
							$"{lerxArea}\n" +
							$"{lerxPositiveCriticalAngleRaise}\n" +
							$"{lerxNegativeCriticalAngleRaise}\n" +
							$"{lerxCoverage}\n" +
							$"{ChainLerxCount}\n" +
							$"{ChainLerxArea}\n" +
							$"{ChainLerxPositiveCriticalAngleRaise}\n" +
							$"{ChainLerxNegativeCriticalAngleRaise}\n" +
							$"{ChainLerxCoverage}\n";

						Game.Instance.Designer.ShowMessage(message, messageTime);
						break;
					case 4:
						message = $"" +
							$"Part id = {this.Data.Part.Id.ToString()}\n" +
							$"INFORMATION OUTPUT: LERX INFLUENCE\n" +
							$"\n" +
							$"{lerxPartIdText}\n" +
							$"{positiveRootAttachedLerxEfficiencyText}\n" +
							$"{negativeRootAttachedLerxEfficiencyText}\n" +
							$"{positiveRootAttachedLerxCriticalAngleRaise}\n" +
							$"{negativeRootAttachedLerxCriticalAngleRaise}\n" +
							$"{areaCoverage}\n" +
							$"{wingspanCoverage}\n" +
							$"{meanChordCoverage}\n" +
							$"{rootAttachedLerxPostCriticalEfficiencyText}\n" +
							$"{rootAttachedLerxPostCriticalAngleRaiseText}\n" +
							$"{ChainLerxArea}\n" +
							$"{ChainLerxPositiveCriticalAngleRaise}\n" +
							$"{ChainLerxNegativeCriticalAngleRaise}\n" +
							$"{ChainLerxCoverage}\n";

						Game.Instance.Designer.ShowMessage(message, messageTime);
						break;
					case 5:
						message = $"" +
							$"Part id = {this.Data.Part.Id.ToString()}\n" +
							$"INFORMATION OUTPUT: LIFT AND DRAG DATA\n" +
							$"\n" +
							$"{zeroAngle}                    {minimalDragAngle}\n" +
							$"{zeroCoefficient}                    {minimalDragCoefficient}\n" +
							$"{positiveCriticalLiftAngle}                    {positiveCriticalDragAngle}\n" +
							$"{positiveCriticalLiftCoefficient}                    {positiveCriticalDragCoefficient}\n" +
							$"{negativeCriticalLiftAngle}                    {negativeCriticalDragAngle}\n" +
							$"{negativeCriticalLiftCoefficient}                    {negativeCriticalDragCoefficient}\n" +
							$"{positiveLiftPerDegreeText}                    {positiveDragPerDegree}\n" +
							$"{negativeLiftPerDegreeText}                    {negativeDragPerDegree}\n" +
							$"{positivePostCriticalLengthText}\n" +
							$"{negativePostCriticalLengthText}\n";

						Game.Instance.Designer.ShowMessage(message, messageTime);
						break;
					case 6:
						message = $"" +
							$"Part id = {this.Data.Part.Id.ToString()}\n" +
							$"INFORMATION OUTPUT: WING DATA AND LERX INFLUENCE\n" +
							$"\n" +
							$"{wingArea}                    {lerxPartIdText}\n" +
							$"{sweepAngle}                    {positiveRootAttachedLerxEfficiencyText}\n" +
							$"{liftDragEfficiency}                    {negativeRootAttachedLerxEfficiencyText}\n" +
							$"{criticalMachNumber}                    {positiveRootAttachedLerxCriticalAngleRaise}\n" +
							$"{postCriticalShake}                    {negativeRootAttachedLerxCriticalAngleRaise}\n" +
							$"{lerxArea}                    {areaCoverage}\n" +
							$"{lerxPositiveCriticalAngleRaise}                    {wingspanCoverage}\n" +
							$"{lerxNegativeCriticalAngleRaise}                    {meanChordCoverage}\n" +
							$"{lerxCoverage}                    {rootAttachedLerxPostCriticalEfficiencyText}\n" +
							$"{ChainLerxCount}                    {rootAttachedLerxPostCriticalAngleRaiseText}\n" +
							$"{ChainLerxArea}\n" +
							$"{ChainLerxPositiveCriticalAngleRaise}\n" +
							$"{ChainLerxNegativeCriticalAngleRaise}\n" +
							$"{ChainLerxCoverage}\n";

						Game.Instance.Designer.ShowMessage(message, messageTime);
						break;
					case 7:
						message = $"" +
							$"Part id = {this.Data.Part.Id.ToString()}\n" +
							$"INFORMATION OUTPUT: ALL\n" +
							$"\n" +
							$"LIFT DATA                    DRAG DATA\n" +
							$"{zeroAngle}                    {minimalDragAngle}\n" +
							$"{zeroCoefficient}                    {minimalDragCoefficient}\n" +
							$"{positiveCriticalLiftAngle}                    {positiveCriticalDragAngle}\n" +
							$"{positiveCriticalLiftCoefficient}                    {positiveCriticalDragCoefficient}\n" +
							$"{negativeCriticalLiftAngle}                    {negativeCriticalDragAngle}\n" +
							$"{negativeCriticalLiftCoefficient}                    {negativeCriticalDragCoefficient}\n" +
							$"{positiveLiftPerDegreeText}                    {positiveDragPerDegree}\n" +
							$"{negativeLiftPerDegreeText}                    {negativeDragPerDegree}\n" +
							$"{positivePostCriticalLengthText}\n" +
							$"{negativePostCriticalLengthText}\n" +
							$"\n" +
							$"WING DATA                    LERX INFLUENCE\n" +
							$"{wingArea}                    {lerxPartIdText}\n" +
							$"{sweepAngle}                    {positiveRootAttachedLerxEfficiencyText}\n" +
							$"{liftDragEfficiency}                    {negativeRootAttachedLerxEfficiencyText}\n" +
							$"{criticalMachNumber}                    {positiveRootAttachedLerxCriticalAngleRaise}\n" +
							$"{postCriticalShake}                    {negativeRootAttachedLerxCriticalAngleRaise}\n" +
							$"{lerxArea}                    {areaCoverage}\n" +
							$"{lerxPositiveCriticalAngleRaise}                    {wingspanCoverage}\n" +
							$"{lerxNegativeCriticalAngleRaise}                    {meanChordCoverage}\n" +
							$"{lerxCoverage}                    {rootAttachedLerxPostCriticalEfficiencyText}\n" +
							$"{ChainLerxCount}                    {rootAttachedLerxPostCriticalAngleRaiseText}\n" +
							$"{ChainLerxArea}\n" +
							$"{ChainLerxPositiveCriticalAngleRaise}\n" +
							$"{ChainLerxNegativeCriticalAngleRaise}\n" +
							$"{ChainLerxCoverage}\n";

						Game.Instance.Designer.ShowMessage(message, messageTime);
						break;
				}
			}
		}
		
		#endregion

		#region COMMON FUNCTIONS
		
		private float GetAngleOfAttack()
		{
			if (Game.InFlightScene)
			{
				Vector3 velocity = this.PartScript.BodyScript.RigidBody.GetPointVelocity(VelocityVector.position) + GetVelocityDifference();
				
				float forwardAngle = Vector3.Angle(SimpleWingContainer.forward, velocity);
				float angleOfAttack = Vector3.Angle(SimpleWingContainer.right, velocity) - 90f;
				if (forwardAngle > 90f)
				{
					angleOfAttack = 180f * Mathf.Sign(angleOfAttack) - angleOfAttack;
				}

				return angleOfAttack;
			}

			return 0f;
		}

		private float GetAngleOfSlip(Transform wingContainer)
		{
			if (Game.InFlightScene)
			{
				Vector3 velocity = this.PartScript.BodyScript.RigidBody.GetPointVelocity(VelocityVector.position) + GetVelocityDifference();

				float forwardAngle = Vector3.Angle(wingContainer.forward, velocity);
				float angleOfSlip = Vector3.Angle(wingContainer.up, velocity) - 90f;
				if (forwardAngle > 90f)
				{
					angleOfSlip = 180f * Mathf.Sign(angleOfSlip) - angleOfSlip;
				}

				return angleOfSlip;
			}

			return 0f;
		}


		private Vector3 GetVelocityDifference()
		{
			if (Game.InFlightScene)
			{
				Vector3 surfaceVelocity = this.PartScript.BodyScript.SurfaceVelocity;
				Vector3 frameVelocity = this.PartScript.BodyScript.CraftScript.FrameVelocity;
				Vector3 difference = surfaceVelocity - frameVelocity;

				return difference;
			}

			return Vector3.zero;
		}
		
		#endregion

		public override void OnSymmetry(SymmetryMode mode, IPartScript originalPart, bool created)
		{
			Start();
		}


		private bool IsOnRightSide()
		{
			if (base.PartScript.CraftScript?.PrimaryCommandPod != null)
			{
				float num = Vector3.Dot(this.transform.right, base.PartScript.CraftScript.PrimaryCommandPod.PilotSeatOrientation.up);
				return num < -0.01f;
			}

			return true;
		}
	}

	public enum AirfoilType
	{
		NACA_0012 = 1,
		Clark_Y = 2,
		T_10_root = 3,
		T_10_wing = 4,
		NACA_64_208 = 5
	}
}