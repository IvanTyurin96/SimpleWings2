namespace Assets.Scripts.Craft.Parts.Modifiers
{
	using Assets.Scripts.Aerodynamics;
	using Assets.Scripts.Extensions;
	using Assets.Scripts.Helpers;
	using Assets.Scripts.UI;
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

		// PARTICLE SYSTEM MODIFIERS
		private bool _trailVortexEnabled;
		private bool _trailVortexForOtherSideEnabled;
		private bool _trailVortexInWorldSpaceEnabled;
		private float _trailVortexSizeMultiplier;
		private float _trailVortexLengthMultiplier;
		private float _trailVortexSpeedMultiplier;
		private float _trailVortexEmissionMultiplier;
		private float _trailVortexRandomAngleMultiplier;
		private float _trailVortexRandomLengthMultiplier;
		private int _trailVortexMaxParticles;
		private float _trailVortexOpacityMultiplier;
		private float _trailVortexGrowStartVisibilityAngleOfAttack;
		private float _trailVortexGrowEndVisibilityAngleOfAttack;
		private float _trailVortexFadeStartVisibilityAngleOfAttack;
		private float _trailVortexFadeEndVisibilityAngleOfAttack;
		private float _trailVortexMinVisibilitySpeed;
		private float _trailVortexMaxVisibilitySpeed;

		private bool _lerxVortexEnabled;
		private bool _lerxVortexForOtherSideEnabled;
		private bool _lerxVortexInWorldSpaceEnabled;
		private float _lerxVortexSizeMultiplier;
		private float _lerxVortexRandomSizeMultiplier;
		private float _lerxVortexLengthMultiplier;
		private float _lerxVortexSpeedMultiplier;
		private float _lerxVortexEmissionMultiplier;
		private float _lerxVortexRandomAngleMultiplier;
		private float _lerxVortexAngleOfAttackSensitivity;
		private float _lerxVortexAngleOfSlipSensitivity;
		private float _lerxVortexMaxAngleOfAttack;
		private float _lerxVortexMaxAngleOfSlip;
		private int _lerxVortexMaxParticles;
		private float _lerxVortexOpacityMultiplier;
		private float _lerxVortexGrowStartVisibilityAngleOfAttack;
		private float _lerxVortexGrowEndVisibilityAngleOfAttack;
		private float _lerxVortexFadeStartVisibilityAngleOfAttack;
		private float _lerxVortexFadeEndVisibilityAngleOfAttack;
		private float _lerxVortexMinVisibilitySpeed;
		private float _lerxVortexMaxVisibilitySpeed;

		private bool _wingVortexEnabled;
		private bool _wingVortexForOtherSideEnabled;
		private float _wingVortexSizeMultiplier;
		private float _wingVortexLengthMultiplier;
		private float _wingVortexSpeedMultiplier;
		private float _wingVortexEmissionMultiplier;
		private int _wingVortexMaxParticles;
		private float _wingVortexOpacityMultiplier;
		private float _wingVortexGrowStartVisibilityAngleOfAttack;
		private float _wingVortexGrowEndVisibilityAngleOfAttack;
		private float _wingVortexFadeStartVisibilityAngleOfAttack;
		private float _wingVortexFadeEndVisibilityAngleOfAttack;
		private float _wingVortexMinVisibilitySpeed;
		private float _wingVortexMaxVisibilitySpeed;

		// FLEX MODIFIERS
		private bool _wingFlexEnabled;
		private float _wingFlexRigidityMultiplier;

		// DESIGNER DEFLECT MODIFIERS
		private float _designerDeflectLeadingEdge;
		private float _designerDeflectControlSurface;

        // ADDITIONAL OFFSET MODIFIERS
        private float _additionalRootLeadingOffset;
		private float _additionalRootTrailingOffset;
		private float _additionalTipLeadingOffset;
		private float _additionalTipTrailingOffset;

		public float _additionalRootLeadingRelativePercentage;
		public float _additionalRootTrailingRelativePercentage;
		public float _additionalTipLeadingRelativePercentage;
		public float _additionalTipTrailingRelativePercentage;

		// DECORATIONS
		private bool _actuatorEnabled;
		private bool _isActuatorInverted;
		private float _actuatorScale;
		private float _actuatorLengthPosition;
		private float _actuatorChordPosition;
		private float _actuatorUpOffset;
		private float _actuatorBackLugOffset;
		private float _actuatorShellLength;
		private float _actuatorFrontLugInitialOffset;
		private float _actuatorInitialRotation;
		private float _actuatorBackMountInitialRotation;
		private float _actuatorFrontMountInitialRotation;
		private float _actuatorBackMountLength;
		private float _actuatorFrontMountLength;
		private bool _isActuatorBackMountVisible;
		private bool _isActuatorFrontMountVisible;
		private bool _isActuatorCablesVisible;

		// FUEL MODIFIERS
		private float _fuelAmount;

		// HIDDEN MODIFIERS
		private bool _wingPhysicsEnabled;
		private bool _wingDestructibilityEnabled;
		private bool _transformRelativeRootEnabled;
		private float _lerxSearchSphereRadius;
		private int _lerxPartId;
		private bool _autoAirfoilInvertEnabled;

		// AIRFOIL SPECIAL VALUES
		private const float OneThickness = 1f;
		private const float EightThickness = 8f;
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
		private Transform WingContainer;
		private MeshFilter WingMeshFilter;
		private Transform Airfoil;
		private Transform AirfoilRounding;
		private Vector3 _airfoilRoundingInitialPosition;
		private Vector3 _airfoilRoundingInitialRotation;
		private Transform VelocityVector;
		private Transform VelocityPyramid;
		private Transform LiftVector;
		private Transform DragVector;
		private Transform WaveDragVector;

		// FLEX OBJECTS
		private SimpleWingScript _rootAttachedFlexWing = null;
		private List<SimpleWingScript> _tipAttachedFlexWings = new List<SimpleWingScript>();

		private Transform SkinnedWing;
		private Transform RootWingBone;
		private Vector3 _rootWingBoneInitialPosition;
		private Vector3 _rootWingBoneInitialRotation;
		private Transform TipWingBone;
		private Vector3 _tipWingBoneInitialPosition;
		private Vector3 _tipWingBoneInitialRotation;

		private Transform SkinnedControlSurface;
		private Transform RootControlSurfaceBone;
		private Vector3 _rootControlSurfaceBoneInitialPosition;
		private Vector3 _rootControlSurfaceBoneInitialRotation;
		private Transform TipControlSurfaceBone;
		private Vector3 _tipControlSurfaceBoneInitialPosition;
		private Vector3 _tipControlSurfaceBoneInitialRotation;

		private Transform SkinnedLeadingEdge;
		private Transform RootLeadingEdgeBone;
		private Vector3 _rootLeadingEdgeBoneInitialPosition;
		private Vector3 _rootLeadingEdgeBoneInitialRotation;
		private Transform TipLeadingEdgeBone;
		private Vector3 _tipLeadingEdgeBoneInitialPosition;
		private Vector3 _tipLeadingEdgeBoneInitialRotation;

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

		// PARTICLE SYSTEM OBJECTS
		private ParticleSystem TrailVortex;
		private Vector3 _trailVortexInitialPosition;
		private Vector3 _trailVortexInitialRotation;
		private ParticleSystem LerxVortex;
		private Vector3 _lerxVortexInitialPosition;
		private Vector3 _lerxVortexInitialRotation;
		private ParticleSystem WingVortex;

		// DECORATORS OBJECTS
		private GameObject HydraulicCylinder;
		private Transform HydraulicCylinderBone001;
		private Transform HydraulicCylinderBone002;
		private Transform HydraulicCylinderBone003;
		private Transform HydraulicCylinderBone004;
		private GameObject HydraulicMount001;
		private GameObject HydraulicMount002;
		private Transform HydraulicMount001BoneUp;
		private Transform HydraulicMount001BoneDown;
		private Transform HydraulicMount002BoneUp;
		private Transform HydraulicMount002BoneDown;
		private GameObject HydraulicCables;

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

        // WING CONTINIOUSLY CALCULATING FORCES
        private float _wingLiftForce = 0f;
		private float _wingDragForce = 0f;
		private float _wingWaveDragForce = 0f;
		private float _wingGravityForce = 0f;

		private float _rootWingVisualBendingAngle = 0f;
		private float _wingBendingAngle = 0f;
		private float _wingVisualBendingAngle = 0f;

		// DESTRUCTIBILITY
		private const float DestructionMaximalWingBendingAngle = 6f;
		private float _wingDestructionBendingAngle;
		private const float ControlSurfaceDescreseMaxBendingAnglePerPercent = 0.4f; // 45% control surface will decrease max wing bending ange at 0.4 * 45% = 18%
		private const float LeadingEdgeDescreseMaxBendingAnglePerPercent = 0.3f; // 40% leading edge will decrease max wing bending ange at 0.3 * 40% = 12%
		private bool _wingDestructionExectued = false;

		// ANIMATION CURVES
		private readonly AnimationCurve _Cy = new AnimationCurve();
		private readonly AnimationCurve _Cx = new AnimationCurve();
		private readonly AnimationCurve _aC = new AnimationCurve();

		// INPUT CONTROLLERS
		private IInputController _controlSurfaceInputController;
		private bool _controlSurfaceInputControllerNullMessageSended = false;
        private IInputController _leadingEdgeInputController;

        // ATTACH POINTS
        private Transform _attachPointRoot;
		private Transform _attachPointTip;
		private Transform _attachPointUp;
		private Transform _attachPointDown;

		// SIMPLE WING DATA CONTROLLER
		private float _simpleWingData_NegativeDragPerDegree = 0f;
		private float _simpleWingData_PositiveDragPerDegree = 0f;
		private float _simpleWingData_SweepAngle = 0f;
		private float _simpleWingData_LiftDragEfficiency = 0f;
		private float _simpleWingData_CriticalMachNumber = 0f;
		private float _simpleWingData_LerxCoverage = 0f;
		private float _simpleWingData_ChainLerxCoverage = 0f;

		// FOR DESIGNER UPDATE
		private float _currentLength = 0f;
		private float _lastLength = 0f;
		private float _currentTipOffset = 0f;
		private float _lastTipOffset = 0f;

		// LATE UPDATE
		private bool _lateUpdateExecuted = false;
		private int _lateUpdateExecutionsCount = 0;
		private const int LateUpdateMaxExecutionsCount = 10;

		// RECURSIVE SEARCHING
		private const int MaxRecursiveExecutionsCount = 100;
		private int _recursiveFlexWingsSearchExecutionsCount = 0;
		private int _recursiveLerxSearchExecutionsCount = 0;
		#endregion

		#region MONOBEHAVIOUR FUNCTIONS--------------------------------------------------------------------
		private void Start()
		{
			GetAndClampModifiers();
			DesignerTransformInitialState();

			InvokeAllStartFunctions();
			
		}
		
		internal void InvokeAllStartFunctions()
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
            PrepareWingToFlex();
			TuneAirfoilRounding();

			// PART CENTERING FUNCTIONS
			TransformWingContainer();

			// ATTACH POINTS FUNCTIONS
			GetAttachPoints();
			TuneAttachPoints();

			// PHYSICS FUNCTIONS
			GetVelocityVector();
			TuneVelocityVector();

			CalculateAerodynamicCurves();

			// PARTICLE SYSTEM FUNCTIONS
			TuneTrailVortex();
			TuneLerxVortex();
			TuneWingVortex();

			// DECORATIONS FUNCTIONS
			TuneHydraulicCylinder();

			// FUEL FUNCTIONS
			TuneFuelCapacity();

			TuneCenterOfMass();

			//DestroyUnusedObjects();
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
			CalculateWingBending();

			// FLEX FUNCTIONS
			FlexWing();

			// PARTICLE SYSTEM FUNCTIONS
			TrailVortexControl();
			LerxVortexControl();
			WingVortexControl();

			// DECORATIONS FUNCTIONS
			HydraulicCylinderControl();

			// DESIGNER FUNCTIONS
			TransformPartInDesigner();

			DestructWing();
		}

		private void FixedUpdate()
		{
			CalculateDynamicLeadAngleLiftDragCoefficient();
			CalculateAndApplyAerodynamicForces();
			CalculateGravityForce();
		}

		private void LateUpdate()
		{
			if (!_lateUpdateExecuted)
			{
				FindLerx();
				CalculateChainLerxData();

				FindFlexWings();
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

		#region START FUNCTIONS----------------------------------------------------------------------------
		private void DesignerTransformInitialState()
		{
			_currentLength = _length;
			_lastLength = _currentLength;

			_currentTipOffset = _tipOffset;
			_lastTipOffset = _currentTipOffset;
		}

		private void ClearData()
		{
			_lateUpdateExecuted = false;
			_lateUpdateExecutionsCount = 0;

			_recursiveLerxSearchExecutionsCount = 0;
			_rootAttachedLerx = null;
			_chainLerx.Clear();

			_recursiveFlexWingsSearchExecutionsCount = 0;
			_rootAttachedFlexWing = null;
			_tipAttachedFlexWings.Clear();

			_rootLeadingEdge = null;
		}

		//CORE GEOMETRY FUNCTIONS------------------------------------------------------------------
		/// <summary>
		/// Get <see cref="SimpleWingData"/> modifiers and set to clamped private fields.
		/// </summary>
		private void GetAndClampModifiers()
		{
			_autoAirfoilInvertEnabled = this.Data.autoAirfoilInvertEnabled;

			// WING
			const float WingMinimalValue = 0f;
			const float WingMinimalLength = 0.001f;
			_rootLeadingOffset = Mathf.Clamp(this.Data.rootLeadingOffset, WingMinimalValue, Mathf.Infinity);
			_rootTrailingOffset = Mathf.Clamp(this.Data.rootTrailingOffset, WingMinimalValue, Mathf.Infinity);
			_tipLeadingOffset = Mathf.Clamp(this.Data.tipLeadingOffset, WingMinimalValue, Mathf.Infinity);
			_tipTrailingOffset = Mathf.Clamp(this.Data.tipTrailingOffset, WingMinimalValue, Mathf.Infinity);
			_length = Mathf.Clamp(this.Data.length, WingMinimalLength, Mathf.Infinity);
			_tipOffset = this.Data.tipOffset;

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

			// PARTICLE SYSTEM TRAIL
			_trailVortexEnabled = this.Data.TrVo_enabled;
			_trailVortexForOtherSideEnabled = this.Data.TrVo_forOtherSideEnabled;
			_trailVortexInWorldSpaceEnabled = this.Data.TrVo_inWorldSpaceEnabled;
			_trailVortexSizeMultiplier = Mathf.Clamp(this.Data.TrVo_sizeMultiplier, 0f, Mathf.Infinity);
			_trailVortexLengthMultiplier = Mathf.Clamp(this.Data.TrVo_lengthMultiplier, 0f, Mathf.Infinity);
			_trailVortexSpeedMultiplier = Mathf.Clamp(this.Data.TrVo_speedMultiplier, 0f, Mathf.Infinity);
			_trailVortexEmissionMultiplier = Mathf.Clamp(this.Data.TrVo_emissionMultiplier, 0f, Mathf.Infinity);
			_trailVortexRandomAngleMultiplier = Mathf.Clamp(this.Data.TrVo_randomAngleMultiplier, 0f, Mathf.Infinity);
			_trailVortexRandomLengthMultiplier = Mathf.Clamp(this.Data.TrVo_randomLengthMultiplier, 0f, Mathf.Infinity);
			_trailVortexMaxParticles = Mathf.Clamp(this.Data.TrVo_maxParticles, 0, System.Int32.MaxValue);
			_trailVortexOpacityMultiplier = Mathf.Clamp(this.Data.TrVo_opacityMultiplier, 0f, 1f);
			_trailVortexGrowStartVisibilityAngleOfAttack = Mathf.Clamp(this.Data.TrVo_growStartVisibilityAOA, 0f, 90f);
			_trailVortexGrowEndVisibilityAngleOfAttack = Mathf.Clamp(this.Data.TrVo_growEndVisibilityAOA, 0f, 90f);
			_trailVortexFadeStartVisibilityAngleOfAttack = Mathf.Clamp(this.Data.TrVo_fadeStartVisibilityAOA, 0f, 90f);
			_trailVortexFadeEndVisibilityAngleOfAttack = Mathf.Clamp(this.Data.TrVo_fadeEndVisibilityAOA, 0f, 90f);
			_trailVortexMinVisibilitySpeed = Mathf.Clamp(this.Data.TrVo_minVisibilitySpeed, 0f, Mathf.Infinity);
			_trailVortexMaxVisibilitySpeed = Mathf.Clamp(this.Data.TrVo_maxVisibilitySpeed, 0f, Mathf.Infinity);

			// PARTICLE SYSTEM LERX
			_lerxVortexEnabled = this.Data.LxVo_enabled;
			_lerxVortexForOtherSideEnabled = this.Data.LxVo_forOtherSideEnabled;
			_lerxVortexInWorldSpaceEnabled = this.Data.LxVo_inWorldSpaceEnabled;
			_lerxVortexSizeMultiplier = Mathf.Clamp(this.Data.LxVo_sizeMultiplier, 0f, Mathf.Infinity);
			_lerxVortexRandomSizeMultiplier = Mathf.Clamp(this.Data.LxVo_randomSizeMultiplier, 0f, Mathf.Infinity);
			_lerxVortexLengthMultiplier = Mathf.Clamp(this.Data.LxVo_lengthMultiplier, 0f, Mathf.Infinity);
			_lerxVortexSpeedMultiplier = Mathf.Clamp(this.Data.LxVo_speedMultiplier, 0f, Mathf.Infinity);
			_lerxVortexEmissionMultiplier = Mathf.Clamp(this.Data.LxVo_emissionMultiplier, 0f, Mathf.Infinity);
			_lerxVortexRandomAngleMultiplier = Mathf.Clamp(this.Data.LxVo_randomAngleMultiplier, 0f, Mathf.Infinity);
			_lerxVortexAngleOfAttackSensitivity = Mathf.Clamp(this.Data.LxVo_angleOfAttackSensitivity, 0f, Mathf.Infinity);
			_lerxVortexAngleOfSlipSensitivity = Mathf.Clamp(this.Data.LxVo_angleOfSlipSensitivity, 0f, Mathf.Infinity);
			_lerxVortexMaxAngleOfAttack = Mathf.Clamp(this.Data.LxVo_maxAngleOfAttack, 0f, Mathf.Infinity);
			_lerxVortexMaxAngleOfSlip = Mathf.Clamp(this.Data.LxVo_maxAngleOfSlip, 0f, Mathf.Infinity);
			_lerxVortexMaxParticles = Mathf.Clamp(this.Data.LxVo_maxParticles, 0, System.Int32.MaxValue);
			_lerxVortexOpacityMultiplier = Mathf.Clamp(this.Data.LxVo_opacityMultiplier, 0f, 1f);
			_lerxVortexGrowStartVisibilityAngleOfAttack = Mathf.Clamp(this.Data.LxVo_growStartVisibilityAOA, 0f, 90f);
			_lerxVortexGrowEndVisibilityAngleOfAttack = Mathf.Clamp(this.Data.LxVo_growEndVisibilityAOA, 0f, 90f);
			_lerxVortexFadeStartVisibilityAngleOfAttack = Mathf.Clamp(this.Data.LxVo_fadeStartVisibilityAOA, 0f, 90f);
			_lerxVortexFadeEndVisibilityAngleOfAttack = Mathf.Clamp(this.Data.LxVo_fadeEndVisibilityAOA, 0f, 90f);
			_lerxVortexMinVisibilitySpeed = Mathf.Clamp(this.Data.LxVo_minVisibilitySpeed, 0f, Mathf.Infinity);
			_lerxVortexMaxVisibilitySpeed = Mathf.Clamp(this.Data.LxVo_maxVisibilitySpeed, 0f, Mathf.Infinity);

			// PARTICLE SYSTEM WING VORTEX
			_wingVortexEnabled = this.Data.WiVo_enabled;
			_wingVortexForOtherSideEnabled = this.Data.WiVo_forOtherSideEnabled;
			_wingVortexSizeMultiplier = Mathf.Clamp(this.Data.WiVo_sizeMultiplier, 0f, Mathf.Infinity);
			_wingVortexLengthMultiplier = Mathf.Clamp(this.Data.WiVo_lengthMultiplier, 0f, Mathf.Infinity);
			_wingVortexSpeedMultiplier = Mathf.Clamp(this.Data.WiVo_speedMultiplier, 0f, Mathf.Infinity);
			_wingVortexEmissionMultiplier = Mathf.Clamp(this.Data.WiVo_emissionMultiplier, 0f, Mathf.Infinity);
			_wingVortexMaxParticles = Mathf.Clamp(this.Data.WiVo_maxParticles, 0, System.Int32.MaxValue);
			_wingVortexOpacityMultiplier = Mathf.Clamp(this.Data.WiVo_opacityMultiplier, 0f, 1f);
			_wingVortexGrowStartVisibilityAngleOfAttack = Mathf.Clamp(this.Data.WiVo_growStartVisibilityAOA, 0f, 90f);
			_wingVortexGrowEndVisibilityAngleOfAttack = Mathf.Clamp(this.Data.WiVo_growEndVisibilityAOA, 0f, 90f);
			_wingVortexFadeStartVisibilityAngleOfAttack = Mathf.Clamp(this.Data.WiVo_fadeStartVisibilityAOA, 0f, 90f);
			_wingVortexFadeEndVisibilityAngleOfAttack = Mathf.Clamp(this.Data.WiVo_fadeEndVisibilityAOA, 0f, 90f);
			_wingVortexMinVisibilitySpeed = Mathf.Clamp(this.Data.WiVo_minVisibilitySpeed, 0f, Mathf.Infinity);
			_wingVortexMaxVisibilitySpeed = Mathf.Clamp(this.Data.WiVo_maxVisibilitySpeed, 0f, Mathf.Infinity);

			// FLEX
			_wingFlexEnabled = this.Data.wingFlexEnabled;
			_wingFlexRigidityMultiplier = Mathf.Clamp(this.Data.wingFlexRigidityMultiplier, 0.001f, Mathf.Infinity);

			// DESIGNER DEFLECT
			_designerDeflectLeadingEdge = Mathf.Clamp(this.Data.designerDeflectLeadingEdge, -_leadingEdgeDeflectionAngle, _leadingEdgeDeflectionAngle);
			_designerDeflectControlSurface = Mathf.Clamp(this.Data.designerDeflectControlSurface, -_controlSurfaceDeflectionAngle, _controlSurfaceDeflectionAngle);

			// ADDITIONAL OFFSETS
			const float MinimalOffset = 0.1f;
			_additionalRootLeadingOffset = Mathf.Clamp(this.Data.additionalRootLeadingOffset, 0f, Mathf.Infinity);
			_additionalRootTrailingOffset = Mathf.Clamp(this.Data.additionalRootTrailingOffset, 0f, Mathf.Infinity);
			_additionalTipLeadingOffset = Mathf.Clamp(this.Data.additionalTipLeadingOffset, 0f, Mathf.Infinity);
			_additionalTipTrailingOffset = Mathf.Clamp(this.Data.additionalTipTrailingOffset, 0f, Mathf.Infinity);

			if (_additionalRootLeadingOffset < MinimalOffset)
				_additionalRootLeadingOffset = 0f;

			if (_additionalRootTrailingOffset < MinimalOffset)
				_additionalRootTrailingOffset = 0f;

			if (_additionalTipLeadingOffset < MinimalOffset)
				_additionalTipLeadingOffset = 0f;

			if (_additionalTipTrailingOffset < MinimalOffset)
				_additionalTipTrailingOffset = 0f;

			if (Mathf.Abs(_tipOffset) >= Mathf.Epsilon)
			{
				_additionalRootLeadingOffset = 0f;
				_additionalRootTrailingOffset = 0f;
				_additionalTipLeadingOffset = 0f;
				_additionalTipTrailingOffset = 0f;
			}

			const float MaxRelativePercentage = 50f;
			_additionalRootLeadingRelativePercentage = Mathf.Clamp(this.Data.additionalRootLeadingRelativePercentage, 0f, MaxRelativePercentage);
			_additionalRootTrailingRelativePercentage = Mathf.Clamp(this.Data.additionalRootTrailingRelativePercentage, 0f, MaxRelativePercentage);
			_additionalTipLeadingRelativePercentage = Mathf.Clamp(this.Data.additionalTipLeadingRelativePercentage, 0f, MaxRelativePercentage);
			_additionalTipTrailingRelativePercentage = Mathf.Clamp(this.Data.additionalTipTrailingRelativePercentage, 0f, MaxRelativePercentage);

			// DECORATIONS
			_actuatorEnabled = this.Data.actuatorEnabled;
			_isActuatorInverted = this.Data.isActuatorInverted;
			if (_autoAirfoilInvertEnabled)
			{
				_isActuatorInverted = base.Data.isActuatorInverted ? !IsOnRightSide() : IsOnRightSide();
			}
			_actuatorScale = Mathf.Clamp(this.Data.actuatorScale, 0.001f, Mathf.Infinity);
			_actuatorLengthPosition = Mathf.Clamp(this.Data.actuatorLengthPosition, 0f, 100f);
			_actuatorChordPosition = Mathf.Clamp(this.Data.actuatorChordPosition, 0f, 100f);
			_actuatorUpOffset = this.Data.actuatorUpOffset;
			_actuatorBackLugOffset = Mathf.Clamp(this.Data.actuatorBackLugOffset, 1f, Mathf.Infinity);
			_actuatorShellLength = Mathf.Clamp(this.Data.actuatorShellLength, 0f, Mathf.Infinity);
			_actuatorFrontLugInitialOffset = Mathf.Clamp(this.Data.actuatorFrontLugInitialOffset, 1f, Mathf.Infinity);
			_actuatorInitialRotation = Mathf.Clamp(this.Data.actuatorInitialRotation, -90f, 90f);
			_actuatorBackMountInitialRotation = Mathf.Clamp(this.Data.actuatorBackMountInitialRotation, -90f, 90f);
			_actuatorFrontMountInitialRotation = Mathf.Clamp(this.Data.actuatorFrontMountInitialRotation, -90f, 90f);
			_actuatorBackMountLength = Mathf.Clamp(this.Data.actuatorBackMountLength, 0f, Mathf.Infinity);
			_actuatorFrontMountLength = Mathf.Clamp(this.Data.actuatorFrontMountLength, 0f, Mathf.Infinity);
			_isActuatorBackMountVisible = this.Data.isActuatorBackMountVisible;
			_isActuatorFrontMountVisible = this.Data.isActuatorFrontMountVisible;
			_isActuatorCablesVisible = this.Data.isActuatorCablesVisible;

			// FUEL
			_fuelAmount = Mathf.Clamp(this.Data.fuelAmount, 0f, 100f);

			// HIDDEN
			_wingPhysicsEnabled = this.Data.wingPhysicsEnabled;
			_wingDestructibilityEnabled = this.Data.wingDestructibilityEnabled;
			_transformRelativeRootEnabled = this.Data.transformRelativeRootEnabled;
			_lerxSearchSphereRadius = Mathf.Clamp(this.Data.lerxSearchSphereRadius, 0f, Mathf.Infinity);
			if (this.Data.lerxPartId != "auto")
			{
				_lerxPartId = System.Convert.ToInt32(this.Data.lerxPartId);
			}
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
				_simpleWingData_SweepAngle = _wingLeadAngle * Mathf.Rad2Deg;
				_simpleWingData_LiftDragEfficiency = Mathf.Clamp(Mathf.Cos(_wingLeadAngle), MinimalDynamicLeadAngleLiftDragCoefficient, 1f);

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
				_simpleWingData_CriticalMachNumber = _airfoilCriticalMachNumber / Mathf.Clamp(Mathf.Cos(_wingLeadAngle), MinimalDynamicLeadAngleLiftDragCoefficient, 1f);

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
				_simpleWingData_LerxCoverage = _lerxLeadingEdgeLength * _lerxCoverageMultiplier;

				float controlSurfaceDestructibilityMultiplier = _controlSurfacePercentage * ControlSurfaceDescreseMaxBendingAnglePerPercent;
				float leadingEdgeDestructibilityMultiplier = _leadingEdgePercentage * LeadingEdgeDescreseMaxBendingAnglePerPercent;

				_wingDestructionBendingAngle = DestructionMaximalWingBendingAngle * (1f - (controlSurfaceDestructibilityMultiplier + leadingEdgeDestructibilityMultiplier) / 100f);
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
			WingContainer = this.transform.FindChildByName("WingContainer");
			Airfoil = WingContainer.FindChildByName("Airfoil");
			Transform airfoils = WingContainer.FindChildByName("Airfoils");
			AirfoilRounding = WingContainer.FindChildByName("AirfoilRounding");
			Transform airfoilRoundings = WingContainer.FindChildByName("AirfoilRoundings");
			WingMeshFilter = Airfoil.GetComponent<MeshFilter>();
			TrailVortex = WingContainer.FindChildByName("TrailVortex").GetComponent<ParticleSystem>();
			LerxVortex = WingContainer.FindChildByName("LerxVortex").GetComponent<ParticleSystem>();
			WingVortex = WingContainer.FindChildByName("WingVortex").GetComponent<ParticleSystem>();

			SkinnedWing = WingContainer.FindChildByName("SkinnedWing");
			RootWingBone = WingContainer.FindChildByName("RootWingBone");
            TipWingBone = WingContainer.FindChildByName("TipWingBone");

			SkinnedControlSurface = WingContainer.FindChildByName("SkinnedControlSurface");
			RootControlSurfaceBone = WingContainer.FindChildByName("RootControlSurfaceBone");
			TipControlSurfaceBone = WingContainer.FindChildByName("TipControlSurfaceBone");

			SkinnedLeadingEdge = WingContainer.FindChildByName("SkinnedLeadingEdge");
			RootLeadingEdgeBone = WingContainer.FindChildByName("RootLeadingEdgeBone");
			TipLeadingEdgeBone = WingContainer.FindChildByName("TipLeadingEdgeBone");

			HydraulicCylinder = WingContainer.FindChildByName("HydraulicCylinder").gameObject;
			HydraulicCylinderBone001 = WingContainer.FindChildByName("HydraulicCylinderBone001");
			HydraulicCylinderBone002 = WingContainer.FindChildByName("HydraulicCylinderBone002");
			HydraulicCylinderBone003 = WingContainer.FindChildByName("HydraulicCylinderBone003");
			HydraulicCylinderBone004 = WingContainer.FindChildByName("HydraulicCylinderBone004");
			HydraulicMount001 = WingContainer.FindChildByName("HydraulicMount001").gameObject;
			HydraulicMount002 = WingContainer.FindChildByName("HydraulicMount002").gameObject;
			HydraulicMount001BoneUp = WingContainer.FindChildByName("HydraulicMount001BoneUp");
			HydraulicMount001BoneDown = WingContainer.FindChildByName("HydraulicMount001BoneDown");
			HydraulicMount002BoneUp = WingContainer.FindChildByName("HydraulicMount002BoneUp");
			HydraulicMount002BoneDown = WingContainer.FindChildByName("HydraulicMount002BoneDown");
			HydraulicCables = WingContainer.FindChildByName("HydraulicCables").gameObject;

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

			TrailVortex.gameObject.SetActive(false);
			LerxVortex.gameObject.SetActive(false);

			SkinnedWing.gameObject.SetActive(false);
			SkinnedControlSurface.gameObject.SetActive(false);
			SkinnedLeadingEdge.gameObject.SetActive(false);

			HydraulicCylinder.SetActive(false);
		}

		/// <summary>
		/// Get input controllers for control surface and leading edge.
		/// </summary>
		private void GetInputControllers()
		{
			_controlSurfaceInputController = GetInputController("ControlSurface");
            _leadingEdgeInputController = GetInputController("LeadingEdge");
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

			float rootScaleMultiplier = _rootLeadingOffset + _rootTrailingOffset;
			float rootScaleOffset = (_rootLeadingOffset - _rootTrailingOffset) / 2f;

			float tipScaleMultiplier = _tipLeadingOffset + _tipTrailingOffset;
			float tipScaleOffset = (_tipLeadingOffset - _tipTrailingOffset) / 2f;

			for (int i = 0; i < _verticesPosition.Length; i++)
			{
				bool vertexBelongRoot = _startVerticesPosition[i].y < MiddleWingYPoint;
				if (vertexBelongRoot)
				{
					_verticesPosition[i] = new Vector3(
						_startVerticesPosition[i].x * rootScaleMultiplier * _rootThicknessMultiplier,
						0f,
						_startVerticesPosition[i].z * rootScaleMultiplier + rootScaleOffset);
				}

				bool vertexBelongTip = _startVerticesPosition[i].y > MiddleWingYPoint;
				if (vertexBelongTip)
				{
					_verticesPosition[i] = new Vector3(
						_startVerticesPosition[i].x * tipScaleMultiplier * _tipThicknessMultiplier,
						_length,
						_startVerticesPosition[i].z * tipScaleMultiplier + tipScaleOffset + _tipOffset);
				}
			}

			// ADDITIONAL OFFSETS
			float rootLength = _rootLeadingOffset + _rootTrailingOffset;
			float tipLength = _tipLeadingOffset + _tipTrailingOffset;
			float additionalRootLeadingScale = Mathf.Abs(_rootLeadingOffset) > Mathf.Epsilon
				? (_rootLeadingOffset + _additionalRootLeadingOffset) / _rootLeadingOffset
				: _additionalRootLeadingOffset;
			float additionalRootTrailingScale = Mathf.Abs(_rootTrailingOffset) > Mathf.Epsilon
				? (_rootTrailingOffset + _additionalRootTrailingOffset) / _rootTrailingOffset
				: _additionalRootTrailingOffset;
			float additionalTipLeadingScale = Mathf.Abs(_tipLeadingOffset) > Mathf.Epsilon
				? (_tipLeadingOffset + _additionalTipLeadingOffset) / _tipLeadingOffset
				: _additionalTipLeadingOffset;
			float additionalTipTrailingScale = Mathf.Abs(_tipTrailingOffset) > Mathf.Epsilon
				? (_tipTrailingOffset + _additionalTipTrailingOffset) / _tipTrailingOffset
				: _additionalTipTrailingOffset;

			if (Mathf.Abs(_tipOffset) < Mathf.Epsilon)
			{
				for (int i = 0; i < _verticesPosition.Length; i++)
				{
					// ROOT
					bool vertexBelongRoot = _startVerticesPosition[i].y < MiddleWingYPoint;

					float rootLeadingRelativePoint = _rootLeadingOffset - rootLength * _additionalRootLeadingRelativePercentage / 100f;
					if (vertexBelongRoot && _verticesPosition[i].z > rootLeadingRelativePoint && _verticesPosition[i].z > 0f)
					{
						_verticesPosition[i] = new Vector3(_verticesPosition[i].x, _verticesPosition[i].y, _verticesPosition[i].z * additionalRootLeadingScale);
					}

					float rootTrailingRelativePoint = -_rootTrailingOffset + rootLength * _additionalRootTrailingRelativePercentage / 100f;
					if (vertexBelongRoot && _verticesPosition[i].z < rootTrailingRelativePoint && _verticesPosition[i].z < 0f)
					{
						_verticesPosition[i] = new Vector3(_verticesPosition[i].x, _verticesPosition[i].y, _verticesPosition[i].z * additionalRootTrailingScale);
					}

					// TIP
					bool vertexBelongTip = _startVerticesPosition[i].y > MiddleWingYPoint;

					float tipLeadingRelativePoint = _tipLeadingOffset - tipLength * _additionalTipLeadingRelativePercentage / 100f;
					if (vertexBelongTip && _verticesPosition[i].z > tipLeadingRelativePoint && _verticesPosition[i].z > 0f)
					{
						_verticesPosition[i] = new Vector3(_verticesPosition[i].x, _verticesPosition[i].y, _verticesPosition[i].z * additionalTipLeadingScale);
					}

					float tipTrailingRelativePoint = -_tipTrailingOffset + tipLength * _additionalTipTrailingRelativePercentage / 100f;
					if (vertexBelongTip && _verticesPosition[i].z < tipTrailingRelativePoint && _verticesPosition[i].z < 0f)
					{
						_verticesPosition[i] = new Vector3(_verticesPosition[i].x, _verticesPosition[i].y, _verticesPosition[i].z * additionalTipTrailingScale);
					}
				}
			}

			WingMeshFilter.mesh.vertices = _verticesPosition;
			WingMeshFilter.mesh.RecalculateNormals();
			WingMeshFilter.mesh.RecalculateBounds();
			WingMeshFilter.GetComponent<MeshCollider>().sharedMesh = WingMeshFilter.mesh;
			WingMeshFilter.GetComponent<MeshCollider>().convex = true;
			_unslicedVerticesPosition = WingMeshFilter.mesh.vertices;

			DestroyControlSurface();
			DestroyControlSurfacePoints();
			CreateControlSurface();

			DestroyLeadingEdge();
			DestroyLeadingEdgePoints();
			CreateLeadingEdge();

			float middleYPoint = _length / 2f;
			SliceWingFromControlSurface(middleYPoint, rootScaleMultiplier, rootScaleOffset, tipScaleMultiplier, tipScaleOffset, _tipOffset, _isControlSurfaceBorderRounded);
			SliceControlSurfaceFromWing(middleYPoint, rootScaleMultiplier, rootScaleOffset, tipScaleMultiplier, tipScaleOffset, _tipOffset, _isControlSurfaceBorderRounded);
			SetControlSurfaceParent();

			SliceWingFromLeadingEdge(middleYPoint, rootScaleMultiplier, rootScaleOffset, tipScaleMultiplier, tipScaleOffset, _tipOffset, _isLeadingEdgeBorderRounded);
			SliceLeadingEdgeFromWing(middleYPoint, rootScaleMultiplier, rootScaleOffset, tipScaleMultiplier, tipScaleOffset, _tipOffset, _isLeadingEdgeBorderRounded);
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
				ControlSurface.transform.parent = WingContainer;
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

			_controlSurfaceRootLeadPoint.transform.parent = WingContainer;
			_controlSurfaceRootLeadPoint.transform.localPosition = new Vector3(rootRotatingPoint, 0f, rootSlicePoint);
			_controlSurfaceRootLeadPoint.transform.localEulerAngles = Vector3.zero;
			_controlSurfaceRootLeadPoint.transform.localScale = Vector3.one;

			_controlSurfaceTipLeadPoint.transform.parent = WingContainer;
			_controlSurfaceTipLeadPoint.transform.localPosition = new Vector3(tipRotatingPoint, _length, tipSlicePoint);
			_controlSurfaceTipLeadPoint.transform.localEulerAngles = Vector3.zero;
			_controlSurfaceTipLeadPoint.transform.localScale = Vector3.one;

			_controlSurfaceRotationAxis.transform.parent = WingContainer;
			_controlSurfaceRotationAxis.transform.localPosition = _controlSurfaceRootLeadPoint.transform.localPosition;
			_controlSurfaceRotationAxis.transform.LookAt(_controlSurfaceTipLeadPoint.transform, WingContainer.forward * -1f);
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
				LeadingEdge.transform.parent = WingContainer;
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

			_leadingEdgeRootLeadPoint.transform.parent = WingContainer;
			_leadingEdgeRootLeadPoint.transform.localPosition = new Vector3(rootRotatingPoint, 0f, rootSlicePoint);
			_leadingEdgeRootLeadPoint.transform.localEulerAngles = Vector3.zero;
			_leadingEdgeRootLeadPoint.transform.localScale = Vector3.one;

			_leadingEdgeTipLeadPoint.transform.parent = WingContainer;
			_leadingEdgeTipLeadPoint.transform.localPosition = new Vector3(tipRotatingPoint, _length, tipSlicePoint);
			_leadingEdgeTipLeadPoint.transform.localEulerAngles = Vector3.zero;
			_leadingEdgeTipLeadPoint.transform.localScale = Vector3.one;

			_leadingEdgeRotationAxis.transform.parent = WingContainer;
			_leadingEdgeRotationAxis.transform.localPosition = _leadingEdgeRootLeadPoint.transform.localPosition;
			_leadingEdgeRotationAxis.transform.LookAt(_leadingEdgeTipLeadPoint.transform, WingContainer.forward * -1f);
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

				ControlSurface.transform.parent = WingContainer;
				_controlSurfaceRotationAxis.transform.LookAt(_controlSurfaceTipLeadPoint.transform, WingContainer.forward * -1f);
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

				LeadingEdge.transform.parent = WingContainer;
				_leadingEdgeRotationAxis.transform.LookAt(_leadingEdgeTipLeadPoint.transform, WingContainer.forward * -1f);
				_leadingEdgeRotationAxis.transform.localRotation *= Quaternion.Euler(90f, 0f, 0f);
				LeadingEdge.transform.parent = _leadingEdgeAuxiliaryParent.transform;
			}
		}

		/// <summary>
		/// Prepare wing to flex.
		/// </summary>
		private void PrepareWingToFlex()
		{
			if (_wingPhysicsEnabled && _wingFlexEnabled)
			{
				RootWingBone.localPosition = Vector3.zero;
				_rootWingBoneInitialPosition = RootWingBone.localPosition;
				_rootWingBoneInitialRotation = RootWingBone.localEulerAngles;

				TipWingBone.localPosition = new Vector3(0f, _length, _tipOffset);
				_tipWingBoneInitialPosition = TipWingBone.localPosition;
				_tipWingBoneInitialRotation = TipWingBone.localEulerAngles;

				if (Game.InFlightScene)
				{
					SkinnedWing.gameObject.SetActive(true);

					Skin(WingMeshFilter, SkinnedWing, RootWingBone, TipWingBone);
					Airfoil.GetComponent<MeshRenderer>().enabled = false;

					if (ControlSurface != null)
					{
						SkinnedControlSurface.gameObject.SetActive(true);

						RootControlSurfaceBone.parent = _controlSurfaceRotationAxis.transform;
						RootControlSurfaceBone.position = _controlSurfaceRootLeadPoint.transform.position;
						RootControlSurfaceBone.localEulerAngles = _controlSurfaceParent.transform.localEulerAngles;
						_rootControlSurfaceBoneInitialPosition = RootControlSurfaceBone.localPosition;
						_rootControlSurfaceBoneInitialRotation = RootControlSurfaceBone.localEulerAngles;

						TipControlSurfaceBone.parent = _controlSurfaceRotationAxis.transform;
						TipControlSurfaceBone.position = _controlSurfaceTipLeadPoint.transform.position;
						TipControlSurfaceBone.localEulerAngles = _controlSurfaceParent.transform.localEulerAngles;
						_tipControlSurfaceBoneInitialPosition = TipControlSurfaceBone.localPosition;
						_tipControlSurfaceBoneInitialRotation = TipControlSurfaceBone.localEulerAngles;

						Skin(ControlSurface.GetComponent<MeshFilter>(), SkinnedControlSurface, RootControlSurfaceBone, TipControlSurfaceBone);
						ControlSurface.GetComponent<MeshRenderer>().enabled = false;
					}

					if (LeadingEdge != null)
					{
						SkinnedLeadingEdge.gameObject.SetActive(true);

						RootLeadingEdgeBone.parent = _leadingEdgeRotationAxis.transform;
						RootLeadingEdgeBone.position = _leadingEdgeRootLeadPoint.transform.position;
						RootLeadingEdgeBone.localEulerAngles = _leadingEdgeParent.transform.localEulerAngles;
						_rootLeadingEdgeBoneInitialPosition = RootLeadingEdgeBone.localPosition;
						_rootLeadingEdgeBoneInitialRotation = RootLeadingEdgeBone.localEulerAngles;

						TipLeadingEdgeBone.parent = _leadingEdgeRotationAxis.transform;
						TipLeadingEdgeBone.position = _leadingEdgeTipLeadPoint.transform.position;
						TipLeadingEdgeBone.localEulerAngles = _leadingEdgeParent.transform.localEulerAngles;
						_tipLeadingEdgeBoneInitialPosition = TipLeadingEdgeBone.localPosition;
						_tipLeadingEdgeBoneInitialRotation = TipLeadingEdgeBone.localEulerAngles;

						Skin(LeadingEdge.GetComponent<MeshFilter>(), SkinnedLeadingEdge, RootLeadingEdgeBone, TipLeadingEdgeBone);
						LeadingEdge.GetComponent<MeshRenderer>().enabled = false;
					}
				}
			}
		}

		private void Skin(MeshFilter meshFilter, Transform skinnedPart, Transform rootBone, Transform tipBone)
		{
			Mesh mesh = meshFilter.mesh;
			SkinnedMeshRenderer skinnedMeshRenderer = skinnedPart.GetComponent<SkinnedMeshRenderer>();

			BoneWeight[] boneWeights = new BoneWeight[mesh.vertices.Length];

			float middleYPoint = _length / 2f;
			for (int i = 0; i < boneWeights.Length; i++)
			{
				if (mesh.vertices[i].y < middleYPoint)
				{
					boneWeights[i].boneIndex0 = 0;
					boneWeights[i].weight0 = 1;
				}
				else
				{
					boneWeights[i].boneIndex0 = 1;
					boneWeights[i].weight0 = 1;
				}
			}

			mesh.boneWeights = boneWeights;
			Transform[] bones = new Transform[2];
			Matrix4x4[] bindPoses = new Matrix4x4[2];

			bones[0] = rootBone;
			bindPoses[0] = bones[0].worldToLocalMatrix * WingContainer.localToWorldMatrix;

			bones[1] = tipBone;
			bindPoses[1] = bones[1].worldToLocalMatrix * WingContainer.localToWorldMatrix;

			mesh.bindposes = bindPoses;

			skinnedMeshRenderer.bones = bones;
			skinnedMeshRenderer.sharedMesh = mesh;
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
				WingContainer.FindChildByContainsName($"{_airfoilTipType}_rounding") != null
				? WingContainer.FindChildByContainsName($"{_airfoilTipType}_rounding").GetComponent<MeshFilter>().mesh
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
			_airfoilRoundingInitialPosition = AirfoilRounding.localPosition;
			_airfoilRoundingInitialRotation = AirfoilRounding.localEulerAngles;
		}
		//--------------------------------------------------------------CORE GEOMETRY FUNCTIONS END

		//PART CENTERING FUNCTIONS-----------------------------------------------------------------
		private void TransformWingContainer()
		{
			WingContainer.localPosition = new Vector3(0f, -_length / 2f, -_tipOffset / 2f);
		}
		//-------------------------------------------------------------PART CENTERING FUNCTIONS END

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
				float rotationPoint = _tipOffset + _tipLeadingOffset - tipLength * _washoutRelativePoint;
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
			VelocityVector = WingContainer.FindChildByName("VelocityVector");
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

                if (_controlSurfacePercentage > 0)
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

                Vector3 sideFrom = _rootAttachedLerx.WingContainer.position + _rootAttachedLerx.WingContainer.TransformVector(new Vector3(0f, _rootAttachedLerx._length, 0f));
                Vector3 sideTo = this.WingContainer.position + this.WingContainer.TransformVector(new Vector3(0f, _length, 0f));
                float sideDistance = Vector3.Project(sideTo - sideFrom, this.WingContainer.up).magnitude;
                float sideDot = Vector3.Dot(_rootAttachedLerx.WingContainer.up.normalized, this.WingContainer.up.normalized);

                Vector3 directFrom = _rootAttachedLerx.WingContainer.position + _rootAttachedLerx.WingContainer.TransformVector(new Vector3(0f, _rootAttachedLerx._length, _rootAttachedLerx._tipLeadingOffset + _rootAttachedLerx._tipOffset));
                Vector3 directTo = this.WingContainer.position + this.WingContainer.TransformVector(new Vector3(0f, _meanChordPositionY, _meanChordLeadPoint - _meanChordLength));
                float directDistance = Vector3.Project(directTo - directFrom, this.WingContainer.forward).magnitude;
                float directDot = Vector3.Dot(_rootAttachedLerx.WingContainer.forward.normalized, this.WingContainer.forward.normalized);

				Vector3 antiDirectTo = this.WingContainer.position + this.WingContainer.TransformVector(new Vector3(0f, _meanChordPositionY, _meanChordLeadPoint));
				Vector3 antiDirectProject = Vector3.Project(antiDirectTo - directFrom, this.WingContainer.forward);
				float antiDirectDistance = antiDirectProject.magnitude;
				float antiDirectDot = Vector3.Dot(antiDirectProject.normalized, this.WingContainer.forward.normalized);
				float antiDirectRatio = Mathf.Clamp(1f - antiDirectDistance * Mathf.Clamp(Mathf.Sign(antiDirectDot), 0f, 1f) / Mathf.Clamp(_meanChordLength, 0.001f, Mathf.Infinity), 0f, 1f);

                Vector3 postCriticalFrom = directFrom + _rootAttachedLerx.WingContainer.TransformVector(new Vector3(0f, 0f, -coverageLength));
                Vector3 postCriticalTo = this.WingContainer.position + this.WingContainer.TransformVector(new Vector3(0f, _meanChordPositionY, _meanChordLeadPoint - _meanChordLength));
                float postCriticalDistance = Vector3.Project(postCriticalTo - postCriticalFrom, this.WingContainer.forward).magnitude;

				Vector3 antiPostCriticalTo = this.WingContainer.position + this.WingContainer.TransformVector(new Vector3(0f, _meanChordPositionY, _meanChordLeadPoint));
				Vector3 antiPostCriticalProject = Vector3.Project(antiPostCriticalTo - postCriticalFrom, this.WingContainer.forward);
				float antiPostCriticalDistance = antiPostCriticalProject.magnitude;
				float antiPostCriticalDot = Vector3.Dot(antiPostCriticalProject.normalized, this.WingContainer.forward.normalized);
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

		private void CalculateWingBending()
		{
			if (Game.InFlightScene && _wingPhysicsEnabled)
			{
				float factWingRootThickness = _airfoilRootThickness / 100f * (_rootLeadingOffset + _rootTrailingOffset);
				float factWingRootLength = _rootLeadingOffset + _rootTrailingOffset;
				float wingRigidity = factWingRootThickness * factWingRootLength;
				float forceCoefficient = 0.000005f;

				float aerodynamicForce = _wingLiftForce + _wingDragForce + _wingWaveDragForce;
				float aerodynamicForceMoment = aerodynamicForce * _meanChordPositionY * forceCoefficient / wingRigidity;
				float gravityForceMoment = _wingGravityForce * _length / 2f * forceCoefficient / wingRigidity;
				float totalForceMoment = aerodynamicForceMoment + gravityForceMoment;

				// TIP WING FORCES EFFECT
				foreach (var tipWing in _tipAttachedFlexWings)
				{
					totalForceMoment += (tipWing._wingLiftForce + tipWing._wingDragForce + tipWing._wingWaveDragForce) * _length * forceCoefficient / wingRigidity;
					totalForceMoment += tipWing._wingGravityForce * _length * forceCoefficient / wingRigidity;
				}

				_wingBendingAngle = totalForceMoment;
				_wingVisualBendingAngle = totalForceMoment * (1f /_wingFlexRigidityMultiplier);
			}
		}
		//--------------------------------------------------------------------PHYSICS FUNCTIONS END

		//PARTICLE SYSTEM FUNCTIONS----------------------------------------------------------------
		private void TuneTrailVortex()
		{
			TrailVortex.transform.localPosition = new Vector3(0f, _length, -_tipTrailingOffset - _additionalTipTrailingOffset + _tipOffset);
			TrailVortex.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
			float tipLength = _tipLeadingOffset + _tipTrailingOffset;
			float rotationPoint = _tipOffset + _tipLeadingOffset - tipLength * _washoutRelativePoint;
			TrailVortex.transform.localPosition = MeshHelper.RotatePointAroundPivot(TrailVortex.transform.localPosition, _washoutAngle, new Vector3(0f, 0f, rotationPoint));
			TrailVortex.transform.localEulerAngles += new Vector3(0f, _washoutAngle, 0f);

			_trailVortexInitialPosition = TrailVortex.transform.localPosition;
			_trailVortexInitialRotation = TrailVortex.transform.localEulerAngles;

			if (Game.InFlightScene && _wingPhysicsEnabled && _trailVortexEnabled)
			{
				TrailVortex.gameObject.SetActive(true);
				float trailVortexScaleFactor = _airfoilTipThickness <= EightThickness
					? Mathf.Lerp(1f, 0.25f, Mathf.InverseLerp(EightThickness, OneThickness, _airfoilTipThickness)) * _trailVortexSizeMultiplier
					: Mathf.Lerp(1f, 2f, Mathf.InverseLerp(EightThickness, TwentyFourThickness, _airfoilTipThickness)) * _trailVortexSizeMultiplier;

				ParticleSystem.MainModule main = TrailVortex.main;
				float defaultStartSize = 0.25f;
				main.startSize = defaultStartSize * trailVortexScaleFactor;
				main.maxParticles = _trailVortexMaxParticles;

				ParticleSystem.InheritVelocityModule inheritVelocity = TrailVortex.inheritVelocity;

				if (_trailVortexInWorldSpaceEnabled)
				{
					main.simulationSpace = ParticleSystemSimulationSpace.World;		
					main.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Custom;
					inheritVelocity.enabled = true;
					inheritVelocity.curveMultiplier = 1f;
				}
				else
				{
					main.simulationSpace = ParticleSystemSimulationSpace.Local;
					main.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Rigidbody;
					inheritVelocity.enabled = false;
				}

				ParticleSystem.ShapeModule shape = TrailVortex.shape;
				float defaultRadius = 0.025f;
				shape.radius = defaultRadius * trailVortexScaleFactor;

				ParticleSystem.EmissionModule emission = TrailVortex.emission;
				float defaultRateOverTime = 1280f * _trailVortexEmissionMultiplier;
				emission.rateOverTime = defaultRateOverTime / trailVortexScaleFactor;
			}
		}

		private void TuneLerxVortex()
		{
			LerxVortex.transform.localPosition = new Vector3(0f, 0f, _rootLeadingOffset + _additionalRootLeadingOffset);
			_lerxVortexInitialPosition = LerxVortex.transform.localPosition;
			_lerxVortexInitialRotation = LerxVortex.transform.localEulerAngles;

			if (Game.InFlightScene && _wingPhysicsEnabled && _isLerx && _lerxVortexEnabled)
			{
				LerxVortex.gameObject.SetActive(true);

				ParticleSystem.MainModule main = LerxVortex.main;
				ParticleSystem.MinMaxCurve startSize = main.startSize;
				float defaultStartSize = 1.0f;
				float randomConstantMin = Mathf.Clamp(1f - _lerxVortexRandomSizeMultiplier, 0f, 1f);
				float randomConstantMax = Mathf.Clamp(1f + _lerxVortexRandomSizeMultiplier, 1f, Mathf.Infinity);
				startSize.constantMin = randomConstantMin * defaultStartSize * _lerxVortexSizeMultiplier;
				startSize.constantMax = randomConstantMax * defaultStartSize * _lerxVortexSizeMultiplier;
				main.startSize = startSize;
				main.maxParticles = _lerxVortexMaxParticles;

				ParticleSystem.InheritVelocityModule inheritVelocity = LerxVortex.inheritVelocity;
				if (_lerxVortexInWorldSpaceEnabled)
				{
					main.simulationSpace = ParticleSystemSimulationSpace.World;
					main.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Custom;
					inheritVelocity.enabled = true;
					inheritVelocity.curveMultiplier = 1f;
				}
				else
				{
					main.simulationSpace = ParticleSystemSimulationSpace.Local;
					main.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Rigidbody;
					inheritVelocity.enabled = false;
				}

				ParticleSystem.EmissionModule emission = LerxVortex.emission;
				float defaultRateOverTime = 500f;
				emission.rateOverTime = defaultRateOverTime * _lerxVortexEmissionMultiplier;
			}
		}

		private void TuneWingVortex()
		{
			if (Game.InFlightScene && _wingPhysicsEnabled && _wingVortexEnabled)
			{
				WingVortex.gameObject.SetActive(true);

				ParticleSystem.MainModule main = WingVortex.main;
				float defaultStartSize = 3.0f;
				main.startSize = defaultStartSize * _wingVortexSizeMultiplier;
				main.maxParticles = _wingVortexMaxParticles;

				ParticleSystem.EmissionModule emission = WingVortex.emission;
				float defaultRateOverTime = 3000f;
				emission.rateOverTime = defaultRateOverTime * _wingVortexEmissionMultiplier * (_wingArea / 12f);

				ParticleSystem.ShapeModule shape = WingVortex.shape;
				shape.mesh = WingMeshFilter.mesh;

				if (_wingFlexEnabled)
				{
					WingVortex.transform.parent = TipWingBone;
				}
			}
		}
		//------------------------------------------------------------PARTICLE SYSTEM FUNCTIONS END

		//DECORATIONS FUNCTIONS---------------------------------------------------------------------
		private void TuneHydraulicCylinder()
		{
			if (_controlSurfacePercentage > 0 && _actuatorEnabled)
			{
				HydraulicCylinder.SetActive(true);

				HydraulicCylinderBone001.transform.localScale = new Vector3(_actuatorScale, _actuatorScale, _actuatorScale);
				HydraulicMount001BoneUp.transform.localScale = new Vector3(_actuatorScale, _actuatorScale, _actuatorScale);
				HydraulicMount002BoneUp.transform.localScale = new Vector3(_actuatorScale, _actuatorScale, _actuatorScale);

				float rootLength = _rootLeadingOffset + _rootTrailingOffset;
				float tipLength = _tipLeadingOffset + _tipTrailingOffset;
				float rootPosition = -_rootLeadingOffset + rootLength * _actuatorChordPosition / 100f;
				float tipPosition = -_tipLeadingOffset - _tipOffset + tipLength * _actuatorChordPosition / 100f;
				float positionScaleFactor = (_rootLeadingOffset + _rootTrailingOffset + _tipLeadingOffset + _tipTrailingOffset) / 2f * (_airfoilRootThickness + _airfoilTipThickness) / 12f / 2f;
				float hydraulicCylinderStartPositionY = 0.0625f * positionScaleFactor;
				HydraulicCylinderBone001.transform.localPosition = new Vector3(Mathf.Lerp(0f, _length, _actuatorLengthPosition / 100f), hydraulicCylinderStartPositionY + _actuatorUpOffset / 100f, Mathf.Lerp(rootPosition, tipPosition, _actuatorLengthPosition / 100f));
				HydraulicCylinderBone002.transform.localPosition = new Vector3(-_actuatorBackLugOffset / 100f, 0f, 0f);
				HydraulicCylinderBone003.transform.localPosition = new Vector3(-_actuatorShellLength / 100f, 0f, 0f);
				HydraulicCylinderBone004.transform.localPosition = new Vector3(-_actuatorFrontLugInitialOffset / 100f, 0f, 0f);

				HydraulicCylinderBone001.rotation = _controlSurfaceRotationAxis.transform.rotation * Quaternion.Euler(-270f, -90f, 0f);
				if (_isActuatorInverted)
				{
					HydraulicCylinderBone001.transform.localPosition = new Vector3(HydraulicCylinderBone001.transform.localPosition.x, HydraulicCylinderBone001.transform.localPosition.y * -1f, HydraulicCylinderBone001.transform.localPosition.z);
					HydraulicCylinderBone001.rotation *= Quaternion.Euler(180f, 0f, 0f);
				}
				HydraulicCylinderBone001.rotation *= Quaternion.Euler(0f, 0f, _actuatorInitialRotation);

				HydraulicMount001BoneUp.position = HydraulicCylinderBone001.position;
				HydraulicMount002BoneUp.position = HydraulicCylinderBone004.position;
				HydraulicMount001BoneUp.rotation = HydraulicCylinderBone001.rotation * Quaternion.Euler(0f, 0f, -_actuatorInitialRotation + _actuatorBackMountInitialRotation);
				HydraulicMount002BoneUp.rotation = HydraulicCylinderBone004.rotation * Quaternion.Euler(0f, 0f, -_actuatorInitialRotation + _actuatorFrontMountInitialRotation);

				HydraulicMount001BoneDown.localPosition = new Vector3(0f, _actuatorBackMountLength / 100f, 0f);
				HydraulicMount002BoneDown.localPosition = new Vector3(0f, _actuatorFrontMountLength / 100f, 0f);

				if (_isActuatorBackMountVisible)
				{
					HydraulicMount001.SetActive(true);
				}
				else
				{
					HydraulicMount001.SetActive(false);
				}

				if (_isActuatorFrontMountVisible)
				{
					HydraulicMount002.SetActive(true);
				}
				else
				{
					HydraulicMount002.SetActive(false);
				}

				if (_isActuatorCablesVisible)
				{
					HydraulicCables.SetActive(true);
				}
				else
				{
					HydraulicCables.SetActive(false);
				}

				if (Game.InFlightScene)
				{
					if (!_wingFlexEnabled)
					{
						HydraulicCylinderBone004.parent = _controlSurfaceParent.transform;
						HydraulicMount002BoneUp.parent = _controlSurfaceParent.transform;
					}
					else
					{
						HydraulicCylinderBone001.parent = TipWingBone;
						HydraulicMount001BoneUp.parent = TipWingBone;
						HydraulicCylinderBone004.parent = TipControlSurfaceBone;
						HydraulicMount002BoneUp.parent = TipControlSurfaceBone;
					}
				}
			}
			else
			{
				HydraulicCylinder.SetActive(false);
			}
		}
		//-----------------------------------------------------------------DECORATORS FUNCTIONS END

		private void TuneFuelCapacity()
		{
			FuelTankScript fuelTankScript = this.transform.GetComponent<FuelTankScript>();

			if (fuelTankScript != null)
			{
				float factWingRootLength = _rootLeadingOffset + _rootTrailingOffset;
				float factWingTipLength = _tipLeadingOffset + _tipTrailingOffset;
				float factAirfoilRootThickness = _airfoilRootThickness / 100f * factWingRootLength;
				float factAirfoilTipThickness = _airfoilTipThickness / 100f * factWingTipLength;
				float factMidThickness = (factAirfoilRootThickness + factAirfoilTipThickness) / 2f;

				float airfoilCapacityMultiplier = 0.66f;
				float fuelCapacity = _wingArea * factMidThickness * 1000f * airfoilCapacityMultiplier * (1f - (float)_controlSurfacePercentage / 100f - (float)_leadingEdgePercentage / 100f);
				fuelTankScript.Data.Capacity = fuelCapacity;
				fuelTankScript.Data.Fuel = fuelCapacity * _fuelAmount / 100f;
			}
		}

		private void TuneCenterOfMass()
		{
			float rootLength = _rootLeadingOffset + _rootTrailingOffset;
			float tipLength = _tipLeadingOffset + _tipTrailingOffset;

			float chordOfMassPosition = 0.5f;
			float rootCenterOfMass = _rootLeadingOffset - rootLength * chordOfMassPosition;
			float tipCenterOfMass = _tipLeadingOffset - tipLength * chordOfMassPosition;
			float lengthCenterOfMassPosition = 0.5f;
			float centerOfMass = Mathf.Lerp(rootCenterOfMass, tipCenterOfMass, lengthCenterOfMassPosition);

			this.Data.Part.Config.CenterOfMass = new Vector3(0f, 0f, centerOfMass);
		}

		private void DestroyUnusedObjects()
		{
			if (Game.InFlightScene)
			{
				DestroyImmediate(WingContainer.FindChildByName("Airfoils").gameObject);
				DestroyImmediate(WingContainer.FindChildByName("AirfoilRoundings").gameObject);

				if (!_isAirfoilRounded)
				{
					DestroyImmediate(AirfoilRounding.gameObject);
				}

				if (!_trailVortexEnabled || !_wingPhysicsEnabled)
				{
					DestroyImmediate(TrailVortex.gameObject);
				}

				if (!_lerxVortexEnabled || !_wingPhysicsEnabled)
				{
					DestroyImmediate(LerxVortex.gameObject);
				}

				if (!_wingVortexEnabled || !_wingPhysicsEnabled)
				{
					DestroyImmediate(WingVortex.gameObject);
				}


				if (!_wingFlexEnabled || !_wingPhysicsEnabled)
				{
					DestroyImmediate(SkinnedWing.gameObject);
					DestroyImmediate(SkinnedControlSurface.gameObject);
					DestroyImmediate(SkinnedLeadingEdge.gameObject);

					DestroyImmediate(RootWingBone.gameObject);
					DestroyImmediate(TipWingBone.gameObject);

					DestroyImmediate(RootControlSurfaceBone.gameObject);
					DestroyImmediate(TipControlSurfaceBone.gameObject);

					DestroyImmediate(RootLeadingEdgeBone.gameObject);
					DestroyImmediate(TipLeadingEdgeBone.gameObject);
				}
				DestroyImmediate(WingContainer.FindChildByName("Bone001").gameObject);
				DestroyImmediate(WingContainer.FindChildByName("Bone002").gameObject);
				DestroyImmediate(WingContainer.FindChildByName("Bone003").gameObject);

				if (!_actuatorEnabled)
				{
					DestroyImmediate(HydraulicCylinder.gameObject);
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
				VelocityVector.rotation = Quaternion.LookRotation(velocity, WingContainer.up);

				// Vector scaling
				VelocityPyramid.localScale = new Vector3(0.05f * 0.5f, 0.05f * 0.5f, ModSettings.Instance.VelocityVectorLengthScale);
			}
		}

		private void RotateControlSurface()
		{
			if (_controlSurfacePercentage > 0 && _controlSurfaceInputController != null)
			{
				float headingAngle = 0f;

				if (Game.InFlightScene)
				{
					float limitedSurfaceInputValue = Mathf.Clamp(_controlSurfaceInputController.Value, -1f, 1f);
					headingAngle = 180f + limitedSurfaceInputValue * _controlSurfaceDeflectionAngle;
				}
					
				float currentAngle = _controlSurfaceParent.transform.localEulerAngles.y;
				currentAngle = AngleTracker.TrackAngle(headingAngle, currentAngle, _controlSurfaceRotationSpeed, 90f, 270f);
				if (Game.InDesignerScene)
				{
					currentAngle = 180f + _designerDeflectControlSurface;
				}
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
			if (_leadingEdgePercentage > 0)
			{
				float angleOfAttack = 0f;

				if (Game.InFlightScene)
				{
					angleOfAttack = GetAngleOfAttack() * _leadingEdgeAngleOfAttackSensitivity;
				}

				float limitedLeadingEdgeInputValue = _isAirfoilInverted
					? Mathf.Clamp(angleOfAttack, -_leadingEdgeDeflectionAngle, 0f)
					: Mathf.Clamp(angleOfAttack, 0f, _leadingEdgeDeflectionAngle);

				if (Game.InFlightScene && this.PartScript.BodyScript.VelocityMagnitude < 1f)
				{
					limitedLeadingEdgeInputValue = 0f;
				}

				if (Game.InFlightScene && this.PartScript.CommandPod.GetActivationGroupState(_leadingEdgeFullDeflectActivationGroup))
				{
					limitedLeadingEdgeInputValue = _isAirfoilInverted ? -_leadingEdgeDeflectionAngle : _leadingEdgeDeflectionAngle;
				}

				float headingAngle = 180f - limitedLeadingEdgeInputValue;
				float currentAngle = _leadingEdgeParent.transform.localEulerAngles.y;
				currentAngle = AngleTracker.TrackAngle(headingAngle, currentAngle, _leadingEdgeRotationSpeed, 90f, 270f);
				if (Game.InFlightScene && _isLeadingEdgeAttachedToRoot && _rootLeadingEdge != null)
				{
					currentAngle = 180f - _rootLeadingEdge._leadingEdgeAngle;
				}
				if (Game.InDesignerScene)
				{
					float clampedDesignerDeflectLeadingEdge = _isAirfoilInverted
						? Mathf.Clamp(_designerDeflectLeadingEdge, -_leadingEdgeDeflectionAngle, 0f)
						: Mathf.Clamp(_designerDeflectLeadingEdge, 0f, _leadingEdgeDeflectionAngle);

					currentAngle = 180f - clampedDesignerDeflectLeadingEdge;
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

		//FLEX FUNCTIONS---------------------------------------------------------------------------
		private void FlexWing()
		{
			if (Game.InFlightScene && _wingPhysicsEnabled && _wingFlexEnabled && _lateUpdateExecuted)
			{
				float rootDeltaX = 0f;
				float rootDeltaY = 0f;
				float rootDeltaZ = 0f;
				float angleBetweenRootBones = 0f;

				// ROOT FLEX WING EFFECT
				if (_rootAttachedFlexWing != null && _rootAttachedFlexWing._wingFlexEnabled)
				{
					angleBetweenRootBones = Vector3.SignedAngle(_rootAttachedFlexWing.WingContainer.forward, WingContainer.forward, WingContainer.up) * Mathf.Deg2Rad;

					rootDeltaX = (_rootAttachedFlexWing.TipWingBone.localPosition.x - _rootAttachedFlexWing._tipWingBoneInitialPosition.x) * Mathf.Cos(angleBetweenRootBones);
					rootDeltaY = _rootAttachedFlexWing.TipWingBone.localPosition.y - _rootAttachedFlexWing._tipWingBoneInitialPosition.y;
					rootDeltaZ = (_rootAttachedFlexWing.TipWingBone.localPosition.x - _rootAttachedFlexWing._tipWingBoneInitialPosition.x) * Mathf.Sin(angleBetweenRootBones)
						+ (_rootAttachedFlexWing.TipWingBone.localPosition.z - _rootAttachedFlexWing._tipWingBoneInitialPosition.z);

					_rootWingVisualBendingAngle = (_rootAttachedFlexWing._wingVisualBendingAngle + _rootAttachedFlexWing._rootWingVisualBendingAngle) * Mathf.Cos(angleBetweenRootBones);
				}

				float angleInRadians = (_rootWingVisualBendingAngle + _wingVisualBendingAngle) * Mathf.Deg2Rad;
				float tipDeltaX = Mathf.Sin(angleInRadians) * _length;
				float tipDeltaY = Mathf.Cos(angleInRadians) * _length - _length;

				// WING
				RootWingBone.localPosition = _rootWingBoneInitialPosition + new Vector3(rootDeltaX, rootDeltaY, rootDeltaZ);
				TipWingBone.localPosition = _tipWingBoneInitialPosition + new Vector3(tipDeltaX + rootDeltaX, tipDeltaY + rootDeltaY, rootDeltaZ);

				if (_isAirfoilRounded)
				{
					AirfoilRounding.localPosition = _airfoilRoundingInitialPosition + new Vector3(tipDeltaX + rootDeltaX, tipDeltaY + rootDeltaY, rootDeltaZ);
				}
				if (_trailVortexEnabled)
				{
					TrailVortex.transform.localPosition = _trailVortexInitialPosition + new Vector3(tipDeltaX + rootDeltaX, tipDeltaY + rootDeltaY, rootDeltaZ);
				}
				if (_lerxVortexEnabled)
				{
					LerxVortex.transform.localPosition = _lerxVortexInitialPosition + new Vector3(rootDeltaX, rootDeltaY, rootDeltaZ);
				}

				float rootRotation = -_rootWingVisualBendingAngle;
				float tipRotation = -_rootWingVisualBendingAngle - _wingVisualBendingAngle;

				RootWingBone.localEulerAngles = _rootWingBoneInitialRotation;
				TipWingBone.localEulerAngles = _tipWingBoneInitialRotation;
				RootWingBone.RotateAround(RootWingBone.transform.position, WingContainer.forward, rootRotation);
				TipWingBone.RotateAround(TipWingBone.transform.position, WingContainer.forward, tipRotation);

				if (_isAirfoilRounded)
				{
					AirfoilRounding.localEulerAngles = _airfoilRoundingInitialRotation;
					AirfoilRounding.RotateAround(TipWingBone.transform.position, WingContainer.forward, tipRotation);
				}
				if (_lerxVortexEnabled)
				{
					LerxVortex.transform.localEulerAngles = _lerxVortexInitialRotation;
					LerxVortex.transform.RotateAround(RootWingBone.transform.position, WingContainer.forward, rootRotation);
				}

				// CONTROL SURFACE
				if (ControlSurface != null)
				{
					RootControlSurfaceBone.position = _controlSurfaceRotationAxis.transform.TransformPoint(_rootControlSurfaceBoneInitialPosition)
						+ (rootDeltaX) * WingContainer.right
						+ (rootDeltaY) * WingContainer.up
						+ (rootDeltaZ) * WingContainer.forward;
					TipControlSurfaceBone.position = _controlSurfaceRotationAxis.transform.TransformPoint(_tipControlSurfaceBoneInitialPosition)
						+ (tipDeltaX + rootDeltaX) * WingContainer.right
						+ (tipDeltaY + rootDeltaY) * WingContainer.up
						+ (rootDeltaZ) * WingContainer.forward;

					RootControlSurfaceBone.localEulerAngles = new Vector3(0f, _controlSurfaceParent.transform.localEulerAngles.y, 0f);
					TipControlSurfaceBone.localEulerAngles = new Vector3(0f, _controlSurfaceParent.transform.localEulerAngles.y, 0f);

					RootControlSurfaceBone.RotateAround(RootWingBone.transform.position, WingContainer.forward, rootRotation);
					TipControlSurfaceBone.RotateAround(TipWingBone.transform.position, WingContainer.forward, tipRotation);
				}

				// LEADING EDGE
				if (LeadingEdge != null)
				{
					RootLeadingEdgeBone.position = _leadingEdgeRotationAxis.transform.TransformPoint(_rootLeadingEdgeBoneInitialPosition)
						+ (rootDeltaX) * WingContainer.right
						+ (rootDeltaY) * WingContainer.up
						+ (rootDeltaZ) * WingContainer.forward;
					TipLeadingEdgeBone.position = _leadingEdgeRotationAxis.transform.TransformPoint(_tipLeadingEdgeBoneInitialPosition)
						+ (tipDeltaX + rootDeltaX) * WingContainer.right
						+ (tipDeltaY + rootDeltaY) * WingContainer.up
						+ (rootDeltaZ) * WingContainer.forward;

					RootLeadingEdgeBone.localEulerAngles = new Vector3(0f, _leadingEdgeParent.transform.localEulerAngles.y + _leadingEdgeAuxiliaryParent.transform.localEulerAngles.y, 0f);
					TipLeadingEdgeBone.localEulerAngles = new Vector3(0f, _leadingEdgeParent.transform.localEulerAngles.y + _leadingEdgeAuxiliaryParent.transform.localEulerAngles.y, 0f);

					RootLeadingEdgeBone.RotateAround(RootWingBone.transform.position, WingContainer.forward, rootRotation);
					TipLeadingEdgeBone.RotateAround(TipWingBone.transform.position, WingContainer.forward, tipRotation);
				}
			}
		}
		//-----------------------------------------------------------------------FLEX FUNCTIONS END

		//PARTICLE SYSTEM FUNCTIONS----------------------------------------------------------------
		private void TrailVortexControl()
		{
			if (Game.InFlightScene && _wingPhysicsEnabled && _trailVortexEnabled)
			{
				const float MaximalAngleOfAttack = 60f;
				float angleOfAttack = Mathf.Clamp(GetAngleOfAttack(), -MaximalAngleOfAttack, MaximalAngleOfAttack);
				float angleOfSlip = GetAngleOfSlip(WingContainer);

				ParticleSystem.MainModule main = TrailVortex.main;
				if (_trailVortexInWorldSpaceEnabled)
				{
					main.emitterVelocity = this.PartScript.BodyScript.CraftScript.FrameVelocity;
				}
				float startSpeed = 40f * _trailVortexSpeedMultiplier;
				float randomLifetimeMultiplier = 0.1f * Random.Range(-1f, 1f) * _trailVortexRandomLengthMultiplier;
				float startLifetime = 10f / startSpeed * _meanChordLength * _trailVortexLengthMultiplier + randomLifetimeMultiplier;
				float multiplier = startSpeed / startLifetime;
				main.startLifetime = startLifetime;
				main.startSpeed = startSpeed;

				float speedVisibilityMultiplier = Mathf.InverseLerp(_trailVortexMinVisibilitySpeed, _trailVortexMaxVisibilitySpeed, Mathf.Abs(this.PartScript.BodyScript.VelocityMagnitude));
				float airDensityMultiplier = Mathf.Clamp(this.PartScript.BodyScript.FluidDensity, 0f, 1f);
				float visibilityMultiplier = airDensityMultiplier * speedVisibilityMultiplier * _trailVortexOpacityMultiplier;
				if ((!_trailVortexForOtherSideEnabled && angleOfAttack < 0f && !_isAirfoilInverted)
					|| (!_trailVortexForOtherSideEnabled && angleOfAttack >= 0f && _isAirfoilInverted))
				{
					visibilityMultiplier = 0f;
				}
				main.startColor = angleOfAttack >= 0f
					? new Color(1f, 1f, 1f, visibilityMultiplier * Mathf.Lerp(0f, 1f, Mathf.InverseLerp(_trailVortexGrowStartVisibilityAngleOfAttack, _trailVortexGrowEndVisibilityAngleOfAttack, angleOfAttack)))
					: new Color(1f, 1f, 1f, visibilityMultiplier * Mathf.Lerp(0f, 1f, Mathf.InverseLerp(-_trailVortexGrowStartVisibilityAngleOfAttack, -_trailVortexGrowEndVisibilityAngleOfAttack, angleOfAttack)));
				if (angleOfAttack > _trailVortexFadeStartVisibilityAngleOfAttack || angleOfAttack < -_trailVortexFadeStartVisibilityAngleOfAttack)
				{
					main.startColor = angleOfAttack >= 0f
						? new Color(1f, 1f, 1f, visibilityMultiplier * Mathf.Lerp(1f, 0f, Mathf.InverseLerp(_trailVortexFadeStartVisibilityAngleOfAttack, _trailVortexFadeEndVisibilityAngleOfAttack, angleOfAttack)))
						: new Color(1f, 1f, 1f, visibilityMultiplier * Mathf.Lerp(1f, 0f, Mathf.InverseLerp(-_trailVortexFadeStartVisibilityAngleOfAttack, -_trailVortexFadeEndVisibilityAngleOfAttack, angleOfAttack)));
				}

				ParticleSystem.ForceOverLifetimeModule forceOverLifetime = TrailVortex.forceOverLifetime;
				float randomAngle = 10f * _trailVortexRandomAngleMultiplier;
				float randomAngleX = Random.Range(-randomAngle, randomAngle);
				float randomAngleY = Random.Range(-randomAngle, randomAngle);
				float forceX = -Mathf.Tan((angleOfAttack + _washoutAngle + randomAngleX) * Mathf.Deg2Rad) * multiplier;
				float forceY = Mathf.Tan((angleOfSlip + randomAngleY) * Mathf.Deg2Rad) * multiplier;
				forceOverLifetime.x = new(forceX, forceX);
				forceOverLifetime.y = new(forceY, forceY);
			}
		}

		private void LerxVortexControl()
		{
			if (Game.InFlightScene && _wingPhysicsEnabled && _isLerx && _lerxVortexEnabled)
			{
				const float AngleLimit = 60f;

				float maximalAngleOfAttack = Mathf.Clamp(_lerxVortexMaxAngleOfAttack, 0f, AngleLimit);
				float factAngleOfAttack = GetAngleOfAttack();
				float angleOfAttack = Mathf.Clamp(factAngleOfAttack * _lerxVortexAngleOfAttackSensitivity, -maximalAngleOfAttack, maximalAngleOfAttack);
				float maximalAngleOfSlip = Mathf.Clamp(_lerxVortexMaxAngleOfSlip, 0f, AngleLimit);
				float angleOfSlip = Mathf.Clamp(GetAngleOfSlip(WingContainer) * _lerxVortexAngleOfSlipSensitivity, -maximalAngleOfSlip, maximalAngleOfSlip);
				float wingLeadAngle = _wingLeadAngle * Mathf.Rad2Deg;
				LerxVortex.transform.localEulerAngles = new Vector3(wingLeadAngle - 90f, 180f - angleOfAttack, -_rootWingVisualBendingAngle);

				ParticleSystem.MainModule main = LerxVortex.main;
				if (_lerxVortexInWorldSpaceEnabled)
				{
					main.emitterVelocity = this.PartScript.BodyScript.CraftScript.FrameVelocity;
				}
				float startSpeed = 40f * _lerxVortexSpeedMultiplier;
				float lerxLeadingEdgeLength = _isChainLerx ? _chainLerx.Sum(x => x._lerxLeadingEdgeLength) : _lerxLeadingEdgeLength;
				float startLifetime = lerxLeadingEdgeLength / Mathf.Clamp(startSpeed, 0.001f, Mathf.Infinity) * 2f * _lerxVortexLengthMultiplier;
				float multiplier = startSpeed / Mathf.Clamp(startLifetime, 0.000001f, Mathf.Infinity);
				main.startLifetime = startLifetime;
				main.startSpeed = startSpeed;

				float speedVisibilityMultiplier = Mathf.InverseLerp(_lerxVortexMinVisibilitySpeed, _lerxVortexMaxVisibilitySpeed, Mathf.Abs(this.PartScript.BodyScript.VelocityMagnitude));
				float airDensityMultiplier = Mathf.Clamp(this.PartScript.BodyScript.FluidDensity, 0f, 1f);
				float visibilityMultiplier = airDensityMultiplier * speedVisibilityMultiplier * _lerxVortexOpacityMultiplier;
				if ((!_lerxVortexForOtherSideEnabled && angleOfAttack < 0f && !_isAirfoilInverted)
					|| (!_lerxVortexForOtherSideEnabled && angleOfAttack >= 0f && _isAirfoilInverted))
				{
					visibilityMultiplier = 0f;
				}
				main.startColor = factAngleOfAttack >= 0f
					? new Color(1f, 1f, 1f, visibilityMultiplier * Mathf.Lerp(0f, 1f, Mathf.InverseLerp(_lerxVortexGrowStartVisibilityAngleOfAttack, _lerxVortexGrowEndVisibilityAngleOfAttack, factAngleOfAttack)))
					: new Color(1f, 1f, 1f, visibilityMultiplier * Mathf.Lerp(0f, 1f, Mathf.InverseLerp(-_lerxVortexGrowStartVisibilityAngleOfAttack, -_lerxVortexGrowEndVisibilityAngleOfAttack, factAngleOfAttack)));
				if (factAngleOfAttack > _lerxVortexFadeStartVisibilityAngleOfAttack || factAngleOfAttack < -_lerxVortexFadeStartVisibilityAngleOfAttack)
				{
					main.startColor = factAngleOfAttack >= 0f
						? new Color(1f, 1f, 1f, visibilityMultiplier * Mathf.Lerp(1f, 0f, Mathf.InverseLerp(_lerxVortexFadeStartVisibilityAngleOfAttack, _lerxVortexFadeEndVisibilityAngleOfAttack, factAngleOfAttack)))
						: new Color(1f, 1f, 1f, visibilityMultiplier * Mathf.Lerp(1f, 0f, Mathf.InverseLerp(-_lerxVortexFadeStartVisibilityAngleOfAttack, -_lerxVortexFadeEndVisibilityAngleOfAttack, factAngleOfAttack)));
				}

				ParticleSystem.ForceOverLifetimeModule forceOverLifetime = LerxVortex.forceOverLifetime;
				float randomAngle = 5f * _lerxVortexRandomAngleMultiplier;
				float randomAngleX = Random.Range(-randomAngle, randomAngle);
				float randomAngleY = Random.Range(-randomAngle, randomAngle);
				float forceX = Mathf.Tan((angleOfAttack + randomAngleX) * Mathf.Deg2Rad) * multiplier;
				float forceY = Mathf.Tan((-angleOfSlip + randomAngleY) * Mathf.Deg2Rad + 90f * Mathf.Deg2Rad - _wingLeadAngle) * multiplier * -1f;
				forceOverLifetime.x = new(forceX, forceX);
				forceOverLifetime.y = new(forceY, forceY);
			}
		}

		private void WingVortexControl()
		{
			if (Game.InFlightScene && _wingPhysicsEnabled && _wingVortexEnabled)
			{
				Vector3 rigidbodyVelocity = this.PartScript.BodyScript.RigidBody.GetPointVelocity(VelocityVector.position) + GetVelocityDifference();

				float startSpeed = 20f * _wingVortexSpeedMultiplier;
				ParticleSystem.VelocityOverLifetimeModule velocity = WingVortex.velocityOverLifetime;
				velocity.x = rigidbodyVelocity.normalized.x * -1f * startSpeed;
				velocity.y = rigidbodyVelocity.normalized.y * -1f * startSpeed;
				velocity.z = rigidbodyVelocity.normalized.z * -1f * startSpeed;

				float startLifetime = 2f / Mathf.Clamp(startSpeed, 0.001f, Mathf.Infinity) * _wingVortexLengthMultiplier;
				ParticleSystem.MainModule main = WingVortex.main;
				main.startLifetime = startLifetime;

				float angleOfAttack = GetAngleOfAttack();
				float speedVisibilityMultiplier = Mathf.InverseLerp(_wingVortexMinVisibilitySpeed, _wingVortexMaxVisibilitySpeed, Mathf.Abs(this.PartScript.BodyScript.VelocityMagnitude));
				float airDensityMultiplier = Mathf.Clamp(this.PartScript.BodyScript.FluidDensity, 0f, 1f);
				float visibilityMultiplier = airDensityMultiplier * speedVisibilityMultiplier * _wingVortexOpacityMultiplier * 0.1f;
				if ((!_wingVortexForOtherSideEnabled && angleOfAttack < 0f && !_isAirfoilInverted)
					|| (!_wingVortexForOtherSideEnabled && angleOfAttack >= 0f && _isAirfoilInverted))
				{
					visibilityMultiplier = 0f;
				}
				main.startColor = angleOfAttack >= 0f
					? new Color(1f, 1f, 1f, visibilityMultiplier * Mathf.Lerp(0f, 1f, Mathf.InverseLerp(_wingVortexGrowStartVisibilityAngleOfAttack, _wingVortexGrowEndVisibilityAngleOfAttack, angleOfAttack)))
					: new Color(1f, 1f, 1f, visibilityMultiplier * Mathf.Lerp(0f, 1f, Mathf.InverseLerp(-_wingVortexGrowStartVisibilityAngleOfAttack, -_wingVortexGrowEndVisibilityAngleOfAttack, angleOfAttack)));
				if (angleOfAttack > _wingVortexFadeStartVisibilityAngleOfAttack || angleOfAttack < -_wingVortexFadeStartVisibilityAngleOfAttack)
				{
					main.startColor = angleOfAttack >= 0f
						? new Color(1f, 1f, 1f, visibilityMultiplier * Mathf.Lerp(1f, 0f, Mathf.InverseLerp(_wingVortexFadeStartVisibilityAngleOfAttack, _wingVortexFadeEndVisibilityAngleOfAttack, angleOfAttack)))
						: new Color(1f, 1f, 1f, visibilityMultiplier * Mathf.Lerp(1f, 0f, Mathf.InverseLerp(-_wingVortexFadeStartVisibilityAngleOfAttack, -_wingVortexFadeEndVisibilityAngleOfAttack, angleOfAttack)));
				}
			}
		}
		//------------------------------------------------------------PARTICLE SYSTEM FUNCTIONS END

		//DECORATIONS FUNCTIONS--------------------------------------------------------------------
		private void HydraulicCylinderControl()
		{
			if (Game.InFlightScene && _controlSurfacePercentage > 0 && _actuatorEnabled)
			{
				HydraulicCylinderBone001.LookAt(HydraulicCylinderBone004, _controlSurfaceRotationAxis.transform.right);
				HydraulicCylinderBone004.LookAt(HydraulicCylinderBone001, _controlSurfaceRotationAxis.transform.right);

				if (_wingFlexEnabled)
				{
					HydraulicCylinderBone001.LookAt(HydraulicCylinderBone004, TipControlSurfaceBone.transform.right * -1f);
					HydraulicCylinderBone004.LookAt(HydraulicCylinderBone001, TipControlSurfaceBone.transform.right * -1f);
				}

				HydraulicCylinderBone001.rotation *= Quaternion.Euler(0f, -90f, 180f);
				HydraulicCylinderBone004.rotation *= Quaternion.Euler(0f, -270f, 180f);

				if (_isActuatorInverted)
				{
					HydraulicCylinderBone001.rotation *= Quaternion.Euler(180f, 0f, 0f);
					HydraulicCylinderBone004.rotation *= Quaternion.Euler(180f, 0f, 0f);
				}
			}
		}
		//----------------------------------------------------------------DECORATIONS FUNCTIONS END

		//DESIGNER FUNCTIONS-----------------------------------------------------------------------
		internal void TransformPartInDesigner()
		{
			if (Game.InDesignerScene && _transformRelativeRootEnabled)
			{
				_currentLength = _length;
				if (Mathf.Abs(_currentLength - _lastLength) > Mathf.Epsilon)
				{
					float lengthDelta = _currentLength - _lastLength;
					this.PartScript.Transform.position += this.transform.TransformVector(new Vector3(0f, lengthDelta / 2f, 0f));
				}
				_lastLength = _currentLength;

				_currentTipOffset = _tipOffset;
				if (Mathf.Abs(_currentTipOffset - _lastTipOffset) > Mathf.Epsilon)
				{
					float tipOffsetDelta = _currentTipOffset - _lastTipOffset;
					this.PartScript.Transform.position += this.transform.TransformVector(new Vector3(0f, 0f, tipOffsetDelta / 2f));
				}
				_lastTipOffset = _currentTipOffset;
			}
		}
		//-------------------------------------------------------------------DESIGNER FUNCTIONS END

		private void DestructWing()
		{
			if (!_wingDestructionExectued && Game.InFlightScene && _wingPhysicsEnabled && _wingDestructibilityEnabled
				&& Mathf.Abs(_wingBendingAngle) > _wingDestructionBendingAngle)
			{
				_wingDestructionExectued = true;
				this.PartScript.TakeDamage(System.Int32.MaxValue);
			}
		}
		#endregion

		#region FIXED UPDATE FUNCTIONS---------------------------------------------------------------------
		private void CalculateDynamicLeadAngleLiftDragCoefficient()
		{
			if (Game.InFlightScene && _wingPhysicsEnabled)
			{
				float angleOfSlip = GetAngleOfSlip(WingContainer);
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
                float airflow = this.PartScript.BodyScript.FluidDensity * this.PartScript.BodyScript.VelocitySquared / 2f;
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
			LiftVector.localScale = new Vector3(Cy * ModSettings.Instance.ForceVectorsLengthScale * shakeMultiplier * _dynamicLeadAngleLiftDragCoefficient, 0.5f, 0.5f);

			// Data for wing flexing
			_wingLiftForce = force * shakeMultiplier * Vector3.Project(VelocityVector.right.normalized, WingContainer.right.normalized).magnitude;

			if (angleOfAttack > 90f || angleOfAttack < -90f)
			{
				_wingLiftForce = _wingLiftForce * -1f;
			}
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
			DragVector.localScale = new Vector3(0.5f, 0.5f, Cx * ModSettings.Instance.ForceVectorsLengthScale * shakeMultiplier * currentLeadAngleLiftDragCoefficient);

			// Data for wing flexing
			_wingDragForce = force * shakeMultiplier * Vector3.Project(VelocityVector.forward.normalized * -1f, WingContainer.right.normalized).magnitude * Mathf.Sign(_wingLiftForce);
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
			WaveDragVector.localScale = new Vector3(0.5f, 0.5f, CxWave * ModSettings.Instance.ForceVectorsLengthScale);

			// Data for wing flexing
			_wingWaveDragForce = force * Vector3.Project(VelocityVector.forward.normalized * -1f, WingContainer.right.normalized).magnitude * Mathf.Sign(_wingLiftForce);
		}

		private void CalculateAndApplyControlSurfaceTorque(float airflow, float angleOfAttack)
		{
			if (_controlSurfacePercentage > 0)
			{
				float controlSurfaceLiftCoefficient = _controlSurfaceAngle * _controlSurfaceSlipAngleEfficiencyCoefficient * _liftCoefficientPerDegree;
				float controlSurfaceArea = _wingArea * _controlSurfacePercentage / 100f;
				float maximalControlSurfaceForceArm = _meanChordLength * (1f - (float)_controlSurfacePercentage * 0.5f / 100f);

				Vector3 from = this.WingContainer.position + this.WingContainer.TransformVector(new Vector3(0f, _meanChordPositionY, _meanChordLeadPoint - maximalControlSurfaceForceArm));
				Vector3 to = this.PartScript.BodyScript.RigidBody.position;
				float distanceFromControlSurfaceToRigidbody = Vector3.Project(from - to, this.WingContainer.forward).magnitude;
				float controlSurfaceForceArm = Mathf.Clamp(distanceFromControlSurfaceToRigidbody, 0f, maximalControlSurfaceForceArm);

				float distanceFromVelocityVectorToRigidbody = Vector3.Project(VelocityVector.position - to, this.WingContainer.forward).magnitude;
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

				this.PartScript.BodyScript.RigidBody.AddTorque(this.WingContainer.up * -1f * torque * Constants.MassScale, ForceMode.Force);
			}
		}

		private void CalculateGravityForce()
		{
			if (Game.InFlightScene && _wingPhysicsEnabled)
			{
				float gravityProject = Vector3.Project(Physics.gravity.normalized, WingContainer.right.normalized).magnitude;
				float gravityAngle = Vector3.Angle(Physics.gravity.normalized, WingContainer.right.normalized);
				if (gravityAngle > 90f)
				{
					gravityProject *= -1f;
				}
				_wingGravityForce = this.Data.Part.Mass / Constants.MassScale * Physics.gravity.magnitude * gravityProject;
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
					_rootAttachedLerx = Physics.OverlapSphere(WingContainer.position, _lerxSearchSphereRadius)
						.FirstOrDefault(x => x.transform != this.Airfoil
							&& x.transform.name == "Airfoil"
							&& x.transform.GetComponentInParent<SimpleWingScript>() != null)
						?.GetComponentInParent<SimpleWingScript>();

					bool lerxAutoSearch = _lerxPartId == 0;
					if (!lerxAutoSearch)
					{
						_rootAttachedLerx = Physics.OverlapSphere(WingContainer.position, _lerxSearchSphereRadius)
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
				_simpleWingData_ChainLerxCoverage = _chainLerxLeadingEdgeLength * _chainLerxCoverageMultiplier;
			}
		}

		private void FindFlexWings()
		{
			if (_wingPhysicsEnabled && _wingFlexEnabled)
			{
				_rootAttachedFlexWing = Physics.OverlapSphere(WingContainer.position, 0.001f)
					.FirstOrDefault(x => x.transform != this.Airfoil
						&& x.transform.name == "Airfoil"
						&& x.transform.GetComponentInParent<SimpleWingScript>() != null)?.GetComponentInParent<SimpleWingScript>();

				RecursiveFlexWingsSearch(this, _tipAttachedFlexWings);
				_tipAttachedFlexWings = _tipAttachedFlexWings.Distinct().ToList();
			}
		}

		private void FindRootLeadingEdge()
		{
			_rootLeadingEdge = Physics.OverlapSphere(WingContainer.position, 0.001f)
				.FirstOrDefault(x => x.transform != this.Airfoil
					&& x.transform.name == "Airfoil"
					&& x.transform.GetComponentInParent<SimpleWingScript>() != null)?.GetComponentInParent<SimpleWingScript>();
		}

		private void RecursiveFlexWingsSearch(SimpleWingScript simpleWing, List<SimpleWingScript> tipAttachedFlexWings)
		{
			if (_recursiveFlexWingsSearchExecutionsCount < MaxRecursiveExecutionsCount)
			{
				_recursiveFlexWingsSearchExecutionsCount++;

				SimpleWingScript tipAttachedFlexWing = Physics.OverlapSphere(simpleWing.TipWingBone.position, 0.001f)
					.FirstOrDefault(x => x.transform != simpleWing.Airfoil
						&& x.transform.name == "Airfoil"
						&& x.transform.GetComponentInParent<SimpleWingScript>() != null)?.GetComponentInParent<SimpleWingScript>();

				if (tipAttachedFlexWing != null)
				{
					tipAttachedFlexWings.Add(tipAttachedFlexWing);
	
					RecursiveFlexWingsSearch(tipAttachedFlexWing, tipAttachedFlexWings);
				}
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields", Justification = "<Pending>")]
		public void SimpleWingDataRedraw()
		{
			if (Game.InDesignerScene && _wingPhysicsEnabled)
			{
				SimpleWingDataController.PartId = this.Data.Part.Id.ToString();

				// Main page, drag data
				SimpleWingDataController.PositiveDragPerDegree = _simpleWingData_PositiveDragPerDegree;
				SimpleWingDataController.NegativeDragPerDegree = _simpleWingData_NegativeDragPerDegree;

				// Main page, wing data
				SimpleWingDataController.Cy = _Cy;
				SimpleWingDataController.Cx = _Cx;
				SimpleWingDataController.WingArea = _wingArea;
				SimpleWingDataController.SweepAngle = _simpleWingData_SweepAngle;
				SimpleWingDataController.LiftDragEfficiency = _simpleWingData_LiftDragEfficiency;
				SimpleWingDataController.CriticalMachNumber = _simpleWingData_CriticalMachNumber;
				SimpleWingDataController.PostCriticalShake = _airfoilPostCriticalShakeMultiplier;

				SimpleWingDataController.LerxArea = _lerxArea;
				SimpleWingDataController.LerxPositiveCriticalAngleRaise = _lerxCriticalAngleRaise * _positiveLerxAngleOfAttackEfficiency;
				SimpleWingDataController.LerxNegativeCriticalAngleRaise = _lerxCriticalAngleRaise * _negativeLerxAngleOfAttackEfficiency;
				SimpleWingDataController.LerxCoverage = _simpleWingData_LerxCoverage;

				SimpleWingDataController.ChainLerxCount = _chainLerx.Count;
				SimpleWingDataController.ChainLerxArea = _chainLerxArea;
				SimpleWingDataController.ChainLerxPositiveCriticalAngleRaise = _chainLerxCriticalAngleRaise * _positiveLerxAngleOfAttackEfficiency;
				SimpleWingDataController.ChainLerxNegativeCriticalAngleRaise = _chainLerxCriticalAngleRaise * _negativeLerxAngleOfAttackEfficiency;
				SimpleWingDataController.ChainLerxCoverage = _simpleWingData_ChainLerxCoverage;

				// Main page, LERX influence
				SimpleWingDataController.LerxPartId = (!_isLerx && _rootAttachedLerx != null && _rootAttachedLerx._isLerx) ? _rootAttachedLerx.Data.Part.Id.ToString() : "not found";
				CalculateRootAttachedLerxEfficiency(out bool _, out float positiveRootAttachedLerxEfficiency, out float negativeRootAttachedLerxEfficiency, out float rootAttachedLerxCriticalAngleRaise, out float rootAttachedLerxPostCriticalEfficiency, out float areaEfficiency, out float wingspanCoverageEfficiency, out float meanChordCoverageEfficiency);
				SimpleWingDataController.PositiveRootAttachedLerxEfficiency = positiveRootAttachedLerxEfficiency;
				SimpleWingDataController.NegativeRootAttachedLerxEfficiency = negativeRootAttachedLerxEfficiency;
				SimpleWingDataController.PositiveRootAttachedLerxCriticalAngleRaise = rootAttachedLerxCriticalAngleRaise * positiveRootAttachedLerxEfficiency;
				SimpleWingDataController.NegativeRootAttachedLerxCriticalAngleRaise = rootAttachedLerxCriticalAngleRaise * negativeRootAttachedLerxEfficiency;
				SimpleWingDataController.AreaCoverage = areaEfficiency;
				SimpleWingDataController.WingspanCoverage = wingspanCoverageEfficiency;
				SimpleWingDataController.MeanChordCoverage = meanChordCoverageEfficiency;
				SimpleWingDataController.RootAttachedLerxPostCriticalEfficiency = rootAttachedLerxPostCriticalEfficiency;
				SimpleWingDataController.RootAttachedLerxPostCriticalAngleRaise = rootAttachedLerxPostCriticalEfficiency * rootAttachedLerxCriticalAngleRaise;

				SimpleWingDataController.Redraw.Invoke();
			}
		}
		#endregion

		#region COMMON FUNCTIONS
		private float GetAngleOfAttack()
		{
			if (Game.InFlightScene)
			{
				Vector3 velocity = this.PartScript.BodyScript.RigidBody.GetPointVelocity(VelocityVector.position) + GetVelocityDifference();
				
				float forwardAngle = Vector3.Angle(WingContainer.forward, velocity);
				float angleOfAttack = Vector3.Angle(WingContainer.right, velocity) - 90f;
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
			InvokeAllStartFunctions();
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