using System;
using Assets.Scripts.Aerodynamics;
using Assets.Scripts.Craft.Parts.Modifiers;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SimpleWingDataController : MonoBehaviour
	{
		// MOD SETTINGS
		private static ModSettings _modSettings;

		// SIMPLEWINGDATA OBJECTS
		[SerializeField]
		private GameObject _canvas;
		[SerializeField]
		private RectTransform _simpleWingDataUI;
		[SerializeField]
		private GameObject _mainPage;
		[SerializeField]
		private GameObject _liftDragPage;
		[SerializeField]
		private GameObject _liftThicknessPage;
		[SerializeField]
		private GameObject _polarPage;

		// COMMON
		private const float _graphsCanvasWidth = 360f;
		private const float _graphsCanvasHeight = 300f;

		[SerializeField]
		private TextMeshProUGUI _partId;
		internal static string PartId { get; set; } = string.Empty;

		#region MAIN PAGE
		// LIFT
		[SerializeField]
		private TextMeshProUGUI _mainPage_LiftData_ZeroAngle;
		[SerializeField]
		private TextMeshProUGUI _mainPage_LiftData_ZeroCoefficient;

		[SerializeField]
		private TextMeshProUGUI _mainPage_LiftData_PositiveCriticalLiftAngle;
		[SerializeField]
		private TextMeshProUGUI _mainPage_LiftData_PositiveCriticalLiftCoefficient;

		[SerializeField]
		private TextMeshProUGUI _mainPage_LiftData_NegativeCriticalLiftAngle;
		[SerializeField]
		private TextMeshProUGUI _mainPage_LiftData_NegativeCriticalLiftCoefficient;

        [SerializeField]
        private TextMeshProUGUI _mainPage_LiftData_PositiveLiftPerDegree;
        [SerializeField]
        private TextMeshProUGUI _mainPage_LiftData_NegativeLiftPerDegree;

		[SerializeField]
		private TextMeshProUGUI _mainPage_LiftData_PositivePostCriticalLength;
		[SerializeField]
		private TextMeshProUGUI _mainPage_LiftData_NegativePostCriticalLength;

		// DRAG
		[SerializeField]
        private TextMeshProUGUI _mainPage_DragData_MinDragAngle;
        [SerializeField]
        private TextMeshProUGUI _mainPage_DragData_MinDragCoefficient;

        [SerializeField]
        private TextMeshProUGUI _mainPage_DragData_PositiveCriticalDragAngle;
        [SerializeField]
        private TextMeshProUGUI _mainPage_DragData_PositiveCriticalDragCoefficient;

        [SerializeField]
        private TextMeshProUGUI _mainPage_DragData_NegativeCriticalDragAngle;
        [SerializeField]
        private TextMeshProUGUI _mainPage_DragData_NegativeCriticalDragCoefficient;

        [SerializeField]
        private TextMeshProUGUI _mainPage_DragData_PositiveDragPerDegree;
        [SerializeField]
        private TextMeshProUGUI _mainPage_DragData_NegativeDragPerDegree;
		internal static float PositiveDragPerDegree { get; set; } = 0f;
		internal static float NegativeDragPerDegree { get; set; } = 0f;

		// WING
		[SerializeField]
		private TextMeshProUGUI _mainPage_WingData_WingArea;
		[SerializeField]
		private TextMeshProUGUI _mainPage_WingData_SweepAngle;
		[SerializeField]
		private TextMeshProUGUI _mainPage_WingData_LiftDragEfficiency;
		[SerializeField]
		private TextMeshProUGUI _mainPage_WingData_CriticalMachNumber;
		[SerializeField]
		private TextMeshProUGUI _mainPage_WingData_PostCriticalShake;

		[SerializeField]
		private TextMeshProUGUI _mainPage_WingData_LerxArea;
		[SerializeField]
		private TextMeshProUGUI _mainPage_WingData_LerxPositiveCriticalAngleRaise;
		[SerializeField]
		private TextMeshProUGUI _mainPage_WingData_LerxNegativeCriticalAngleRaise;
		[SerializeField]
		private TextMeshProUGUI _mainPage_WingData_LerxCoverage;

		[SerializeField]
		private TextMeshProUGUI _mainPage_WingData_ChainLerxCount;
		[SerializeField]
		private TextMeshProUGUI _mainPage_WingData_ChainLerxArea;
		[SerializeField]
		private TextMeshProUGUI _mainPage_WingData_ChainLerxPositiveCriticalAngleRaise;
		[SerializeField]
		private TextMeshProUGUI _mainPage_WingData_ChainLerxNegativeCriticalAngleRaise;
		[SerializeField]
		private TextMeshProUGUI _mainPage_WingData_ChainLerxCoverage;

		internal static float WingArea { get; set; } = 0f;
		internal static float SweepAngle { get; set; } = 0f;
		internal static float LiftDragEfficiency { get; set; } = 1f;
		internal static float CriticalMachNumber { get; set; } = 1f;
		internal static float PostCriticalShake { get; set; } = 0f;

		internal static float LerxArea { get; set; } = 0f;
		internal static float LerxPositiveCriticalAngleRaise { get; set; } = 0f;
		internal static float LerxNegativeCriticalAngleRaise { get; set; } = 0f;
		internal static float LerxCoverage { get; set; } = 0f;

		internal static int ChainLerxCount { get; set; } = 0;
		internal static float ChainLerxArea { get; set; } = 0f;
		internal static float ChainLerxPositiveCriticalAngleRaise { get; set; } = 0f;
		internal static float ChainLerxNegativeCriticalAngleRaise { get; set; } = 0f;
		internal static float ChainLerxCoverage { get; set; } = 0f;

		// LERX INFLUENCE
		[SerializeField]
		private TextMeshProUGUI _mainPage_LerxInfluenceData_LerxPartId;
		[SerializeField]
		private TextMeshProUGUI _mainPage_LerxInfluenceData_PositiveRootAttachedLerxEfficiency;
		[SerializeField]
		private TextMeshProUGUI _mainPage_LerxInfluenceData_NegativeRootAttachedLerxEfficiency;
		[SerializeField]
		private TextMeshProUGUI _mainPage_LerxInfluenceData_PositiveRootAttachedLerxCriticalAngleRaise;
		[SerializeField]
		private TextMeshProUGUI _mainPage_LerxInfluenceData_NegativeRootAttachedLerxCriticalAngleRaise;
		[SerializeField]
		private TextMeshProUGUI _mainPage_LerxInfluenceData_AreaCoverage;
		[SerializeField]
		private TextMeshProUGUI _mainPage_LerxInfluenceData_WingspanCoverage;
		[SerializeField]
		private TextMeshProUGUI _mainPage_LerxInfluenceData_MeanChordCoverage;
		[SerializeField]
		private TextMeshProUGUI _mainPage_LerxInfluenceData_RootAttachedLerxPostCriticalEfficiency;
		[SerializeField]
		private TextMeshProUGUI _mainPage_LerxInfluenceData_RootAttachedLerxPostCriticalAngleRaise;
		internal static string LerxPartId { get; set; } = string.Empty;
		internal static float PositiveRootAttachedLerxEfficiency { get; set; } = 0f;
		internal static float NegativeRootAttachedLerxEfficiency { get; set; } = 0f;
		internal static float PositiveRootAttachedLerxCriticalAngleRaise { get; set; } = 0f;
		internal static float NegativeRootAttachedLerxCriticalAngleRaise { get; set; } = 0f;
		internal static float AreaCoverage { get; set; } = 0f;
		internal static float WingspanCoverage { get; set; } = 0f;
		internal static float MeanChordCoverage { get; set; } = 0f;
		internal static float RootAttachedLerxPostCriticalEfficiency { get; set; } = 0f;
		internal static float RootAttachedLerxPostCriticalAngleRaise { get; set; } = 0f;
		#endregion

		#region LIFTDRAG PAGE
		// COMMON
		[SerializeField]
		private GameObject _LD_verticalLineInstance;
		[SerializeField]
		private Transform _LD_verticalLinesParent;
		private static Transform[] _LD_verticalLinesArray = new Transform[37];

		[SerializeField]
		private GameObject _LD_horizontalLineInstance;
		[SerializeField]
		private Transform _LD_horizontalLinesParent;
		private static Transform[] _LD_horizontalLinesArray = new Transform[31];

		private static float _LD_displayAngleRange = 360f;
		private static float _LD_displayCoefficientRange = 6f;

		// LIFT
		[SerializeField]
		private GameObject _LD_liftPointInstance;
		[SerializeField]
		private Transform _LD_liftPointsParent;
		private static Transform[] _LD_liftPointsArray;
		internal static AnimationCurve Cy = new AnimationCurve();

		// DRAG
		[SerializeField]
		private GameObject _LD_dragPointInstance;
		[SerializeField]
		private Transform _LD_dragPointsParent;
		private static Transform[] _LD_dragPointsArray;
		internal static AnimationCurve Cx = new AnimationCurve();
		#endregion

		#region ALPHA THICKNESS PAGE
		[SerializeField]
		private GameObject _AT_verticalLineInstance;
		[SerializeField]
		private Transform _AT_verticalLinesParent;
		private static Transform[] _AT_verticalLinesArray = new Transform[25];

		[SerializeField]
		private GameObject _AT_horizontalLineInstance;
		[SerializeField]
		private Transform _AT_horizontalLinesParent;
		private static Transform[] _AT_horizontalLinesArray = new Transform[23];

		[SerializeField]
		private GameObject _AT_NACA_0012_pointInstance;
		[SerializeField]
		private Transform _AT_NACA_0012_pointsParent;
		private static Transform[] _AT_NACA_0012_pointsArray;

		[SerializeField]
		private GameObject _AT_Clark_Y_pointInstance;
		[SerializeField]
		private Transform _AT_Clark_Y_pointsParent;
		private static Transform[] _AT_Clark_Y_pointsArray;

		[SerializeField]
		private GameObject _AT_T_10_root_pointInstance;
		[SerializeField]
		private Transform _AT_T_10_root_pointsParent;
		private static Transform[] _AT_T_10_root_pointsArray;

		[SerializeField]
		private GameObject _AT_T_10_wing_pointInstance;
		[SerializeField]
		private Transform _AT_T_10_wing_pointsParent;
		private static Transform[] _AT_T_10_wing_pointsArray;

		[SerializeField]
		private GameObject _AT_NACA_64_208_pointInstance;
		[SerializeField]
		private Transform _AT_NACA_64_208_pointsParent;
		private static Transform[] _AT_NACA_64_208_pointsArray;
		#endregion

		#region POLAR PAGE
		private static float _P_displayDragCoefficientRange = 0.5f;
		private static float _P_displayLiftCoefficientRange = 6f;
		private static float _P_angleOfAttackRange = 60f;

		[SerializeField]
		private GameObject _P_verticalLineInstance;
		[SerializeField]
		private Transform _P_verticalLinesParent;
		private static Transform[] _P_verticalLinesArray = new Transform[21];

		[SerializeField]
		private GameObject _P_horizontalLineInstance;
		[SerializeField]
		private Transform _P_horizontalLinesParent;
		private static Transform[] _P_horizontalLinesArray = new Transform[31];

		[SerializeField]
		private GameObject _P_pointInstance;
		[SerializeField]
		private Transform _P_pointsParent;
		private static Transform[] _P_pointsArray;
		#endregion

		// EXTERNAL INVOKE
		public delegate void RedrawSimpleWingData();
		public static RedrawSimpleWingData Redraw;

		#region MONOBEHAVIOUR FUNCTIONS--------------------------------------------------------------------
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S2696:Instance members should not write to \"static\" fields", Justification = "<Pending>")]
        private void Start()
		{
			GetModSettings();
			InitizalizeArrays();
			SetUIPosition();
            SetState();

			// LIFT DRAG PAGE
			LD_CreateVerticalLines();
			LD_CreateHorizontalLines();
			LD_CreateLiftPoints();
			LD_CreateDragPoints();

			// ALPHA THICKNESS PAGE
			AT_CreateVerticalLines();
			AT_CreateHorizontalLines();
			AT_CreatePoints(_AT_NACA_0012_pointsParent, _AT_NACA_0012_pointsArray, _AT_NACA_0012_pointInstance);
			AT_CreatePoints(_AT_Clark_Y_pointsParent, _AT_Clark_Y_pointsArray, _AT_Clark_Y_pointInstance);
			AT_CreatePoints(_AT_T_10_root_pointsParent, _AT_T_10_root_pointsArray, _AT_T_10_root_pointInstance);
			AT_CreatePoints(_AT_T_10_wing_pointsParent, _AT_T_10_wing_pointsArray, _AT_T_10_wing_pointInstance);
			AT_CreatePoints(_AT_NACA_64_208_pointsParent, _AT_NACA_64_208_pointsArray, _AT_NACA_64_208_pointInstance);
			UpdateAlphaThicknessPage();

			// POLAR PAGE
			P_CreateVerticalLines();
			P_CreateHorizontalLines();
			P_CreatePoints();

			Redraw = Draw;
		}

		private void Update()
		{
			KeyboardControl();
			SetUIPosition();
			SetState();
			SetPage();
		}
		#endregion

		#region START FUNCTIONS----------------------------------------------------------------------------
		private static void GetModSettings()
		{
			_modSettings = ModSettings.Instance;
		}

		private static void InitizalizeArrays()
		{
			int minimalPointCount = 10;
			int maximalPointCount = 10000;

			_LD_liftPointsArray = new Transform[Mathf.Clamp(_modSettings.SimpleWingDataCurvePointCount, minimalPointCount, maximalPointCount)];
			_LD_dragPointsArray = new Transform[Mathf.Clamp(_modSettings.SimpleWingDataCurvePointCount, minimalPointCount, maximalPointCount)];

			_AT_NACA_0012_pointsArray = new Transform[Mathf.Clamp(_modSettings.SimpleWingDataCurvePointCount, minimalPointCount, maximalPointCount)];
			_AT_Clark_Y_pointsArray = new Transform[Mathf.Clamp(_modSettings.SimpleWingDataCurvePointCount, minimalPointCount, maximalPointCount)];
			_AT_T_10_root_pointsArray = new Transform[Mathf.Clamp(_modSettings.SimpleWingDataCurvePointCount, minimalPointCount, maximalPointCount)];
			_AT_T_10_wing_pointsArray = new Transform[Mathf.Clamp(_modSettings.SimpleWingDataCurvePointCount, minimalPointCount, maximalPointCount)];
			_AT_NACA_64_208_pointsArray = new Transform[Mathf.Clamp(_modSettings.SimpleWingDataCurvePointCount, minimalPointCount, maximalPointCount)];

			_P_pointsArray = new Transform[Mathf.Clamp(_modSettings.SimpleWingDataCurvePointCount, minimalPointCount, maximalPointCount)];
		}

		private void SetUIPosition()
		{
			if (Game.InDesignerScene)
			{
				switch (_modSettings.SimpleWingDataPosition)
				{
					case 2:
						_simpleWingDataUI.pivot = new Vector2(0f, 1.111f);
						_simpleWingDataUI.transform.localPosition = new Vector2(-Screen.width / 2f, Screen.height / 2f);
						break;

					case 3:
						_simpleWingDataUI.pivot = new Vector2(0f, 0f);
						_simpleWingDataUI.transform.localPosition = new Vector2(-Screen.width / 2f, -Screen.height / 2f);
						break;

					case 4:
						_simpleWingDataUI.pivot = new Vector2(1f, 0f);
						_simpleWingDataUI.transform.localPosition = new Vector2(Screen.width / 2f, -Screen.height / 2f);
						break;

					case 5:
						_simpleWingDataUI.pivot = new Vector2(0.5f, 0.5f);
						_simpleWingDataUI.transform.localPosition = new Vector2(0f, 0f);
						break;

					default: // = 1
						_simpleWingDataUI.pivot = new Vector2(1f, 1.111f);
						_simpleWingDataUI.transform.localPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
						break;
				}
			}
			
		}

		private void SetState()
		{
			if (Game.InDesignerScene && _modSettings.SimpleWingDataVisible)
			{
				_canvas.SetActive(true);
			}
			else
			{
				_canvas.SetActive(false);
			}

			_simpleWingDataUI.transform.localScale = new Vector2(_modSettings.SimpleWingDataSize, _modSettings.SimpleWingDataSize);

			// LIFT DRAG PAGE
			_LD_verticalLineInstance.SetActive(false);
			_LD_horizontalLineInstance.SetActive(false);

			// ALPHA THICKNESS PAGE
			_AT_verticalLineInstance.SetActive(false);
			_AT_horizontalLineInstance.SetActive(false);

			// POLAR PAGE
			_P_verticalLineInstance.SetActive(false);
			_P_horizontalLineInstance.SetActive(false);
			_P_pointInstance.SetActive(false);
		}

		// LIFT DRAG PAGE--------------------------------------------------------------------------
		private void LD_CreateVerticalLines()
		{
			for (int i = 0; i < _LD_verticalLinesArray.Length; i++)
			{
				GameObject line = GameObject.Instantiate(_LD_verticalLineInstance);
				line.SetActive(true);
				line.transform.SetParent(_LD_verticalLinesParent);
				line.transform.localPosition = Vector3.zero;
				line.transform.localScale = Vector3.one;
				_LD_verticalLinesArray[i] = line.transform;
			}
		}

		private void LD_CreateHorizontalLines()
		{
			for (int i = 0; i < _LD_horizontalLinesArray.Length; i++)
			{
				GameObject line = GameObject.Instantiate(_LD_horizontalLineInstance);
				line.SetActive(true);
				line.transform.SetParent(_LD_horizontalLinesParent);
				line.transform.localPosition = Vector3.zero;
				line.transform.localScale = Vector3.one;
				_LD_horizontalLinesArray[i] = line.transform;
			}
		}

		private void LD_CreateLiftPoints()
		{
			for (int i = 0; i < _LD_liftPointsArray.Length; i++)
			{
				GameObject point = GameObject.Instantiate(_LD_liftPointInstance);
				point.SetActive(true);
				point.transform.SetParent(_LD_liftPointsParent);
				point.transform.localPosition = Vector3.zero;
				point.transform.localScale = Vector3.one;
				point.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
				point.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
				_LD_liftPointsArray[i] = point.transform;
			}
		}

		private void LD_CreateDragPoints()
		{
			for (int i = 0; i < _LD_dragPointsArray.Length; i++)
			{
				GameObject point = GameObject.Instantiate(_LD_dragPointInstance);
				point.SetActive(true);
				point.transform.SetParent(_LD_dragPointsParent);
				point.transform.localPosition = Vector3.zero;
				point.transform.localScale = Vector3.one;
				point.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
				point.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
				_LD_dragPointsArray[i] = point.transform;
			}
		}
		//-----------------------------------------------------------------------------------------

		// ALPHA THICKNESS PAGE---------------------------------------------------------------------
		private void AT_CreateVerticalLines()
		{
			for (int i = 0; i < _AT_verticalLinesArray.Length; i++)
			{
				GameObject line = GameObject.Instantiate(_AT_verticalLineInstance);
				line.SetActive(true);
				line.transform.SetParent(_AT_verticalLinesParent);
				line.transform.localPosition = Vector3.zero;
				line.transform.localScale = Vector3.one;
				_AT_verticalLinesArray[i] = line.transform;
			}
		}

		private void AT_CreateHorizontalLines()
		{
			for (int i = 0; i < _AT_horizontalLinesArray.Length; i++)
			{
				GameObject line = GameObject.Instantiate(_AT_horizontalLineInstance);
				line.SetActive(true);
				line.transform.SetParent(_AT_horizontalLinesParent);
				line.transform.localPosition = Vector3.zero;
				line.transform.localScale = Vector3.one;
				_AT_horizontalLinesArray[i] = line.transform;
			}
		}

		private void AT_CreatePoints(Transform pointsParent, Transform[] pointsArray, GameObject pointInstance)
		{
			for (int i = 0; i < pointsArray.Length; i++)
			{
				GameObject point = GameObject.Instantiate(pointInstance);
				point.SetActive(true);
				point.transform.SetParent(pointsParent);
				point.transform.localPosition = Vector3.zero;
				point.transform.localScale = Vector3.one;
				point.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
				point.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
				pointsArray[i] = point.transform;
			}
		}

		private static void UpdateAlphaThicknessPage()
		{
			AT_DrawVerticalLines();
			AT_DrawHorizontalLines();
			AT_DrawCurve(AirfoilType.NACA_0012, _AT_NACA_0012_pointsArray);
			AT_DrawCurve(AirfoilType.Clark_Y, _AT_Clark_Y_pointsArray);
			AT_DrawCurve(AirfoilType.T_10_root, _AT_T_10_root_pointsArray);
			AT_DrawCurve(AirfoilType.T_10_wing, _AT_T_10_wing_pointsArray);
			AT_DrawCurve(AirfoilType.NACA_64_208, _AT_NACA_64_208_pointsArray);
		}

		private static void AT_DrawVerticalLines()
		{
			float realRange = _graphsCanvasWidth;
			float minimalReal = -realRange / 2f;
			float realStep = realRange / (_AT_verticalLinesArray.Length - 1);

			float displayRange = 24f;
			float displayStep = displayRange / (_AT_verticalLinesArray.Length - 1);

			for (int i = 0; i < _AT_verticalLinesArray.Length; i++)
			{
				_AT_verticalLinesArray[i].gameObject.SetActive(true);
				_AT_verticalLinesArray[i].localPosition = new Vector3(minimalReal + i * realStep, 0f, 0f);
				_AT_verticalLinesArray[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{i * displayStep}";
			}
			_AT_verticalLinesArray[0].gameObject.SetActive(false);

			_AT_verticalLinesArray[_AT_verticalLinesArray.Length - 1].GetChild(0).gameObject.SetActive(false);
			_AT_verticalLinesArray[_AT_verticalLinesArray.Length - 2].GetChild(0).gameObject.SetActive(false);
		}

		private static void AT_DrawHorizontalLines()
		{
			float realRange = _graphsCanvasHeight;
			float minimalReal = -realRange / 2f;
			float realStep = realRange / (_AT_horizontalLinesArray.Length - 1);

			float displayRange = 44f;
			float minimalDisplay = -displayRange / 2f;
			float displayStep = displayRange / (_AT_horizontalLinesArray.Length - 1);

			for (int i = 0; i < _AT_horizontalLinesArray.Length; i++)
			{
				_AT_horizontalLinesArray[i].localPosition = new Vector3(-180f, minimalReal + i * realStep, 0f);
				_AT_horizontalLinesArray[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{minimalDisplay + i * displayStep}°";
			}
			int middleIndex = _AT_horizontalLinesArray.Length / 2;
			_AT_horizontalLinesArray[middleIndex].gameObject.SetActive(false);

			_AT_horizontalLinesArray[0].GetChild(0).gameObject.SetActive(false);
			_AT_horizontalLinesArray[_AT_horizontalLinesArray.Length - 1].GetChild(0).gameObject.SetActive(false);
		}

		private static void AT_DrawCurve(AirfoilType airfoilType, Transform[] pointsArray)
		{
			float horizontalRealRange = _graphsCanvasWidth;
			float horizontalMinimalReal = -horizontalRealRange / 2f;

			float horizontalDisplayRange = 24f;
			float horizontalMinimalDisplay = 1f;
			float horizontalScale = horizontalRealRange / horizontalDisplayRange;

			float verticalRealRange = _graphsCanvasHeight;
			float verticalDisplayRange = 44f;
			float verticalScale = verticalRealRange / verticalDisplayRange;

			Span<Transform> points = new Span<Transform>(pointsArray);
			Span<Transform> positivePoints = points.Slice(0, points.Length / 2);
			Span<Transform> negativePoints = points.Slice(points.Length / 2, points.Length / 2);

			if (positivePoints.Length + negativePoints.Length != pointsArray.Length)
			{
				pointsArray[pointsArray.Length - 1].gameObject.SetActive(false);
			}

			for (int i = 0; i < positivePoints.Length; i++)
			{
				float thickness = horizontalMinimalDisplay + (horizontalDisplayRange - horizontalMinimalDisplay) * i / positivePoints.Length;

				AirfoilCalculator.CalculateLiftCharacteristics(
					airfoilType,
					thickness,
					out float _,
					out float _,
					out float negativeCriticalAngle,
					out float _,
					out float positiveCriticalAngle,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _,
					out float _);

				float x = horizontalMinimalReal + thickness * horizontalScale;
				float positiveY = positiveCriticalAngle * verticalScale;
				float negativeY = negativeCriticalAngle * verticalScale;
				positivePoints[i].localPosition = new Vector3(x, positiveY, 0f);
				negativePoints[i].localPosition = new Vector3(x, negativeY, 0f);
			}
		}
		//-----------------------------------------------------------------------------------------

		// POLAR PAGE------------------------------------------------------------------------------
		private void P_CreateVerticalLines()
		{
			for (int i = 0; i < _P_verticalLinesArray.Length; i++)
			{
				GameObject line = GameObject.Instantiate(_P_verticalLineInstance);
				line.SetActive(true);
				line.transform.SetParent(_P_verticalLinesParent);
				line.transform.localPosition = Vector3.zero;
				line.transform.localScale = Vector3.one;
				_P_verticalLinesArray[i] = line.transform;
			}
		}

		private void P_CreateHorizontalLines()
		{
			for (int i = 0; i < _P_horizontalLinesArray.Length; i++)
			{
				GameObject line = GameObject.Instantiate(_P_horizontalLineInstance);
				line.SetActive(true);
				line.transform.SetParent(_P_horizontalLinesParent);
				line.transform.localPosition = Vector3.zero;
				line.transform.localScale = Vector3.one;
				_P_horizontalLinesArray[i] = line.transform;
			}
		}

		private void P_CreatePoints()
		{
			for (int i = 0; i < _P_pointsArray.Length; i++)
			{
				GameObject point = GameObject.Instantiate(_P_pointInstance);
				point.SetActive(true);
				point.transform.SetParent(_P_pointsParent);
				point.transform.localPosition = Vector3.zero;
				point.transform.localScale = Vector3.one;
				point.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1f);
				point.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 1f);
				_P_pointsArray[i] = point.transform;
			}
		}
		//-----------------------------------------------------------------------------------------
		#endregion

		#region UPDATE FUNCTIONS---------------------------------------------------------------------------
		private static void KeyboardControl()
		{
			if (Game.InDesignerScene)
			{
				if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
				{
					_modSettings.SimpleWingDataVisible.Value = !_modSettings.SimpleWingDataVisible.Value;
				}

				if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
				{
					_modSettings.SimpleWingDataPage.Value += _modSettings.SimpleWingDataPage.Step;

					if (_modSettings.SimpleWingDataPage.Value > _modSettings.SimpleWingDataPage.Max)
					{
						_modSettings.SimpleWingDataPage.Value = _modSettings.SimpleWingDataPage.Min;
					}
				}

				if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
				{
					_modSettings.SimpleWingDataSize.Value += _modSettings.SimpleWingDataSize.Step;

					if (_modSettings.SimpleWingDataSize.Value > _modSettings.SimpleWingDataSize.Max)
					{
						_modSettings.SimpleWingDataSize.Value = _modSettings.SimpleWingDataSize.Min;
					}
				}

				if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.Alpha4))
				{
					_modSettings.SimpleWingDataPosition.Value += _modSettings.SimpleWingDataPosition.Step;

					if (_modSettings.SimpleWingDataPosition.Value > _modSettings.SimpleWingDataPosition.Max)
					{
						_modSettings.SimpleWingDataPosition.Value = _modSettings.SimpleWingDataPosition.Min;
					}
				}

				if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.Alpha5))
				{
					_modSettings.SimpleWingData_LiftDragPage_AngleRange.Value -= _modSettings.SimpleWingData_LiftDragPage_AngleRange.Step;

					if (_modSettings.SimpleWingData_LiftDragPage_AngleRange.Value < _modSettings.SimpleWingData_LiftDragPage_AngleRange.Min)
					{
						_modSettings.SimpleWingData_LiftDragPage_AngleRange.Value = _modSettings.SimpleWingData_LiftDragPage_AngleRange.Max;
					}

					SimpleWingDataController.Redraw.Invoke();
				}

				if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKey(KeyCode.Alpha6))
				{
					float speed = Time.deltaTime * 1f;
					_modSettings.SimpleWingData_LiftDragPage_CoefficientRange.Value -= _modSettings.SimpleWingData_LiftDragPage_CoefficientRange.Value * speed;

					if (_modSettings.SimpleWingData_LiftDragPage_CoefficientRange.Value < _modSettings.SimpleWingData_LiftDragPage_CoefficientRange.Min)
					{
						_modSettings.SimpleWingData_LiftDragPage_CoefficientRange.Value = _modSettings.SimpleWingData_LiftDragPage_CoefficientRange.Max;
					}

					SimpleWingDataController.Redraw.Invoke();
				}

				if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKey(KeyCode.Alpha7))
				{
					float speed = Time.deltaTime * 1f;
					_modSettings.SimpleWingData_PolarPage_DragCoefficientRange.Value -= _modSettings.SimpleWingData_PolarPage_DragCoefficientRange.Value * speed;

					if (_modSettings.SimpleWingData_PolarPage_DragCoefficientRange.Value < _modSettings.SimpleWingData_PolarPage_DragCoefficientRange.Min)
					{
						_modSettings.SimpleWingData_PolarPage_DragCoefficientRange.Value = _modSettings.SimpleWingData_PolarPage_DragCoefficientRange.Max;
					}

					SimpleWingDataController.Redraw.Invoke();
				}

				if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKey(KeyCode.Alpha8))
				{
					float speed = Time.deltaTime * 1f;
					_modSettings.SimpleWingData_PolarPage_LiftCoefficientRange.Value -= _modSettings.SimpleWingData_PolarPage_LiftCoefficientRange.Value * speed;

					if (_modSettings.SimpleWingData_PolarPage_LiftCoefficientRange.Value < _modSettings.SimpleWingData_PolarPage_LiftCoefficientRange.Min)
					{
						_modSettings.SimpleWingData_PolarPage_LiftCoefficientRange.Value = _modSettings.SimpleWingData_PolarPage_LiftCoefficientRange.Max;
					}

					SimpleWingDataController.Redraw.Invoke();
				}

				if (UnityEngine.Input.GetKey(KeyCode.LeftControl) && UnityEngine.Input.GetKeyDown(KeyCode.Alpha0))
				{
					_modSettings.SimpleWingDataVisible.Value = true;
					_modSettings.SimpleWingDataPage.Value = 1;
					_modSettings.SimpleWingDataSize.Value = 2f;
					_modSettings.SimpleWingDataPosition.Value = 4;

					_modSettings.SimpleWingData_LiftDragPage_AngleRange.Value = 360;
					_modSettings.SimpleWingData_LiftDragPage_CoefficientRange.Value = 6f;

					_modSettings.SimpleWingData_PolarPage_DragCoefficientRange.Value = 0.5f;
					_modSettings.SimpleWingData_PolarPage_LiftCoefficientRange.Value = 6f;
					_modSettings.SimpleWingData_PolarPage_AngleOfAttackRange.Value = 60f;

					SimpleWingDataController.Redraw.Invoke();
				}
			}
		}

		private void SetPage()
		{
			// Using switch for this case - bad idea.
			if (_modSettings.SimpleWingDataPage == 1 && !_mainPage.activeSelf)
			{
				_mainPage.SetActive(true);
				_liftDragPage.SetActive(false);
				_liftThicknessPage.SetActive(false);
				_polarPage.SetActive(false);
			}
			if (_modSettings.SimpleWingDataPage == 2 && !_liftDragPage.activeSelf)
			{
				_liftDragPage.SetActive(true);
				_mainPage.SetActive(false);
				_liftThicknessPage.SetActive(false);
				_polarPage.SetActive(false);
			}
			if (_modSettings.SimpleWingDataPage == 3 && !_liftThicknessPage.activeSelf)
			{
				_liftThicknessPage.SetActive(true);
				_mainPage.SetActive(false);
				_liftDragPage.SetActive(false);
				_polarPage.SetActive(false);
			}
			if (_modSettings.SimpleWingDataPage == 4 && !_polarPage.activeSelf)
			{
				_polarPage.SetActive(true);
				_mainPage.SetActive(false);
				_liftDragPage.SetActive(false);
				_liftThicknessPage.SetActive(false);
			}
			if (_mainPage.activeSelf && _liftDragPage.activeSelf && _liftThicknessPage.activeSelf && _polarPage.activeSelf)
			{
				_mainPage.SetActive(false);
				_liftDragPage.SetActive(false);
				_liftThicknessPage.SetActive(false);
				_polarPage.SetActive(false);
			}
		}
		#endregion

		#region EXTERNAL INVOKE FUNCTIONS------------------------------------------------------------------
		private void Draw()
		{
			UpdatePartId();
			UpdateMainPage();
			UpdateLiftDragPage();
			UpdatePolarPage();
		}

		// MAIN PAGE-------------------------------------------------------------------------------
		private void UpdatePartId()
		{
			_partId.text = $"Part id = {PartId}";
		}

		private void UpdateMainPage()
		{
			// LIFT
			_mainPage_LiftData_ZeroAngle.text = $"{Cy.keys[8].time.ToString("0.00")}°";
			_mainPage_LiftData_ZeroCoefficient.text = $"{Cy.keys[8].value.ToString("0.00")}";

			_mainPage_LiftData_PositiveCriticalLiftAngle.text = $"{Cy.keys[9].time.ToString("0.00")}°";
			_mainPage_LiftData_PositiveCriticalLiftCoefficient.text = $"{Cy.keys[9].value.ToString("0.00")}";
			
			_mainPage_LiftData_NegativeCriticalLiftAngle.text = $"{Cy.keys[7].time.ToString("0.00")}°";
			_mainPage_LiftData_NegativeCriticalLiftCoefficient.text = $"{Cy.keys[7].value.ToString("0.00")}";
			
			float positiveLiftPerDegree = Mathf.Abs(Cy.keys[9].time - Cy.keys[8].time) > Mathf.Epsilon ?
				(Cy.keys[9].value - Cy.keys[8].value) / (Cy.keys[9].time - Cy.keys[8].time)
				: 0f;
			float negativeLiftPerDegree = Mathf.Abs(Cy.keys[8].time - Cy.keys[7].time) > Mathf.Epsilon ?
				(Cy.keys[8].value - Cy.keys[7].value) / (Cy.keys[8].time - Cy.keys[7].time)
				: 0f;
            _mainPage_LiftData_PositiveLiftPerDegree.text = $"{positiveLiftPerDegree.ToString("0.000")}";
            _mainPage_LiftData_NegativeLiftPerDegree.text = $"{(-negativeLiftPerDegree).ToString("0.000")}";

			float positivePostCriticalLength = Cy.keys[10].time - Cy.keys[9].time;
			float negativePostCriticalLength = Cy.keys[7].time - Cy.keys[6].time;
			_mainPage_LiftData_PositivePostCriticalLength.text = $"{positivePostCriticalLength.ToString("0.00")}°";
			_mainPage_LiftData_NegativePostCriticalLength.text = $"{negativePostCriticalLength.ToString("0.00")}°";

			// DRAG
			_mainPage_DragData_MinDragAngle.text = $"{Cx.keys[4].time.ToString("0.00")}°";
			_mainPage_DragData_MinDragCoefficient.text = $"{Cx.keys[4].value.ToString("0.00000")}";

			_mainPage_DragData_PositiveCriticalDragAngle.text = $"{Cx.keys[5].time.ToString("0.00")}°";
            _mainPage_DragData_PositiveCriticalDragCoefficient.text = $"{Cx.keys[5].value.ToString("0.00000")}";
			
			_mainPage_DragData_NegativeCriticalDragAngle.text = $"{Cx.keys[3].time.ToString("0.00")}°";
            _mainPage_DragData_NegativeCriticalDragCoefficient.text = $"{Cx.keys[3].value.ToString("0.00000")}";
			
            _mainPage_DragData_PositiveDragPerDegree.text = $"{PositiveDragPerDegree.ToString("0.00000")}";
            _mainPage_DragData_NegativeDragPerDegree.text = $"{NegativeDragPerDegree.ToString("0.00000")}";

			// WING
			_mainPage_WingData_WingArea.text = $"{WingArea.ToString("0.00")}";
			_mainPage_WingData_SweepAngle.text = $"{SweepAngle.ToString("0.00")}°";
			_mainPage_WingData_CriticalMachNumber.text = $"{CriticalMachNumber.ToString("0.00")}";
			_mainPage_WingData_PostCriticalShake.text = $"{(PostCriticalShake * 100f).ToString("0.0")}%";
			_mainPage_WingData_LiftDragEfficiency.text = $"{(LiftDragEfficiency * 100f).ToString("0.0")}%";

			_mainPage_WingData_LerxArea.text = $"{LerxArea.ToString("0.00")}";
			_mainPage_WingData_LerxPositiveCriticalAngleRaise.text = $"{LerxPositiveCriticalAngleRaise.ToString("0.00")}°";
			_mainPage_WingData_LerxNegativeCriticalAngleRaise.text = $"{LerxNegativeCriticalAngleRaise.ToString("0.00")}°";
			_mainPage_WingData_LerxCoverage.text = $"{LerxCoverage.ToString("0.00")}";

			_mainPage_WingData_ChainLerxCount.text = $"{ChainLerxCount}";
			_mainPage_WingData_ChainLerxArea.text = $"{ChainLerxArea.ToString("0.00")}";
			_mainPage_WingData_ChainLerxPositiveCriticalAngleRaise.text = $"{ChainLerxPositiveCriticalAngleRaise.ToString("0.00")}°";
			_mainPage_WingData_ChainLerxNegativeCriticalAngleRaise.text = $"{ChainLerxNegativeCriticalAngleRaise.ToString("0.00")}°";
			_mainPage_WingData_ChainLerxCoverage.text = $"{ChainLerxCoverage.ToString("0.00")}";

			// LERX INFLUENCE
			_mainPage_LerxInfluenceData_LerxPartId.text = $"{LerxPartId}";
			_mainPage_LerxInfluenceData_PositiveRootAttachedLerxEfficiency.text = $"{(PositiveRootAttachedLerxEfficiency * 100f).ToString("0.0")}%";
			_mainPage_LerxInfluenceData_NegativeRootAttachedLerxEfficiency.text = $"{(NegativeRootAttachedLerxEfficiency * 100f).ToString("0.0")}%";
			_mainPage_LerxInfluenceData_PositiveRootAttachedLerxCriticalAngleRaise.text = $"{PositiveRootAttachedLerxCriticalAngleRaise.ToString("0.00")}°";
			_mainPage_LerxInfluenceData_NegativeRootAttachedLerxCriticalAngleRaise.text = $"{NegativeRootAttachedLerxCriticalAngleRaise.ToString("0.00")}°";
			_mainPage_LerxInfluenceData_AreaCoverage.text = $"{(AreaCoverage * 100f).ToString("0.0")}%";
			_mainPage_LerxInfluenceData_WingspanCoverage.text = $"{(WingspanCoverage * 100f).ToString("0.0")}%";
			_mainPage_LerxInfluenceData_MeanChordCoverage.text = $"{(MeanChordCoverage * 100f).ToString("0.0")}%";
			_mainPage_LerxInfluenceData_RootAttachedLerxPostCriticalEfficiency.text = $"{(RootAttachedLerxPostCriticalEfficiency * 100f).ToString("0.0")}%";
			_mainPage_LerxInfluenceData_RootAttachedLerxPostCriticalAngleRaise.text = $"{(RootAttachedLerxPostCriticalAngleRaise).ToString("0.00")}°";

			if (_modSettings.SimpleWingDataMainPagePaint)
			{
				PaintMainPage(positiveLiftPerDegree, negativeLiftPerDegree, positivePostCriticalLength, negativePostCriticalLength);
			}
			else
			{
				PaintMainPageDefault();
			}
		}

		private void PaintMainPageDefault()
		{
			for (int i = 0; i < _mainPage.transform.childCount; i++)
			{
				Transform child = _mainPage.transform.GetChild(i);
				TextPainter.PaintChildDefaultColor(child);

				if (child != null)
				{
					for (int j = 0; j < child.childCount; j++)
					{
						Transform child2 = child.GetChild(j);
						TextPainter.PaintChildDefaultColor(child2);
					}
				}
			}
		}

		private void PaintMainPage(
			float positiveLiftPerDegree,
			float negativeLiftPerDegree,
			float positivePostCriticalLength,
			float negativePostCriticalLength)
		{
			// LIFT
			TextPainter.PaintRedYellowGrayGreen(_mainPage_LiftData_ZeroCoefficient, Cy.keys[8].value, -0.1f, 0.2f, 0.4f);

			TextPainter.PaintRedYellowGrayGreen(_mainPage_LiftData_PositiveCriticalLiftAngle, Cy.keys[9].time, 7f, 12f, 18f);
			TextPainter.PaintRedYellowGrayGreen2(_mainPage_LiftData_NegativeCriticalLiftAngle, Cy.keys[7].time, -7f, -12f, -18f);

			TextPainter.PaintRedYellowGrayGreen(_mainPage_LiftData_PositiveLiftPerDegree, positiveLiftPerDegree, 0.075f, 0.090f, 0.125f);
			TextPainter.PaintRedYellowGrayGreen2(_mainPage_LiftData_NegativeLiftPerDegree, -negativeLiftPerDegree, -0.075f, -0.090f, -0.125f);

			TextPainter.PaintGreenGrayYellowRed(_mainPage_WingData_PostCriticalShake, PostCriticalShake, 0.05f, 0.15f, 0.2f);

			TextPainter.PaintRedYellowGrayGreen(_mainPage_LiftData_PositivePostCriticalLength, positivePostCriticalLength, 5f, 7.5f, 15f);
			TextPainter.PaintRedYellowGrayGreen(_mainPage_LiftData_NegativePostCriticalLength, negativePostCriticalLength, 5f, 7.5f, 15f);

			// DRAG
			TextPainter.PaintGreenGrayYellowRed(_mainPage_DragData_MinDragCoefficient, Cx.keys[4].value, 0.003f, 0.008f, 0.010f);

			TextPainter.PaintRedYellowGrayGreen(_mainPage_DragData_PositiveCriticalDragAngle, Cx.keys[5].time, 7f, 12f, 18f);
			TextPainter.PaintRedYellowGrayGreen2(_mainPage_DragData_NegativeCriticalDragAngle, Cx.keys[3].time, -7f, -12f, -18f);

			TextPainter.PaintGreenGrayYellowRed(_mainPage_DragData_PositiveDragPerDegree, PositiveDragPerDegree, 0.0015f, 0.0025f, 0.003f);
			TextPainter.PaintGreenGrayYellowRed(_mainPage_DragData_NegativeDragPerDegree, NegativeDragPerDegree, 0.0015f, 0.0025f, 0.003f);

			// WING
			TextPainter.PaintRedYellowGreen(_mainPage_WingData_LiftDragEfficiency, LiftDragEfficiency, 0.6f, 0.9f);

			TextPainter.PaintRedYellowGreen(_mainPage_WingData_LerxPositiveCriticalAngleRaise, LerxPositiveCriticalAngleRaise, 5f, 10f);
			TextPainter.PaintRedYellowGreen(_mainPage_WingData_LerxNegativeCriticalAngleRaise, LerxNegativeCriticalAngleRaise, 5f, 10f);

			// LERX INFLUENCE
			TextPainter.PaintRedYellowGreen(_mainPage_LerxInfluenceData_PositiveRootAttachedLerxEfficiency, PositiveRootAttachedLerxEfficiency, 0.25f, 0.8f);
			TextPainter.PaintRedYellowGreen(_mainPage_LerxInfluenceData_NegativeRootAttachedLerxEfficiency, NegativeRootAttachedLerxEfficiency, 0.25f, 0.8f);
			TextPainter.PaintRedYellowGreen(_mainPage_LerxInfluenceData_PositiveRootAttachedLerxCriticalAngleRaise, PositiveRootAttachedLerxCriticalAngleRaise, 5f, 10f);
			TextPainter.PaintRedYellowGreen(_mainPage_LerxInfluenceData_NegativeRootAttachedLerxCriticalAngleRaise, NegativeRootAttachedLerxCriticalAngleRaise, 5f, 10f);
			TextPainter.PaintRedYellowGreen(_mainPage_LerxInfluenceData_AreaCoverage, AreaCoverage, 0.25f, 0.8f);
			TextPainter.PaintRedYellowGreen(_mainPage_LerxInfluenceData_WingspanCoverage, WingspanCoverage, 0.25f, 0.8f);
			TextPainter.PaintRedYellowGreen(_mainPage_LerxInfluenceData_MeanChordCoverage, MeanChordCoverage, 0.25f, 0.8f);
			TextPainter.PaintRedYellowGreen(_mainPage_LerxInfluenceData_RootAttachedLerxPostCriticalEfficiency, RootAttachedLerxPostCriticalEfficiency, 0.25f, 0.8f);
			TextPainter.PaintRedYellowGreen(_mainPage_LerxInfluenceData_RootAttachedLerxPostCriticalAngleRaise, RootAttachedLerxPostCriticalAngleRaise, 5f, 10f);
		}
		//-----------------------------------------------------------------------------------------

		// LIFT DRAG PAGE--------------------------------------------------------------------------
		private static void UpdateLiftDragPage()
		{
			LD_UpdateAngleCoefficientRange();
			LD_DrawVerticalLines();
			LD_DrawHorizontalLines();
			LD_DrawCurves();
		}

		private static void LD_UpdateAngleCoefficientRange()
		{
			float minimalAngleRange = 1f;
			float maximalAngleRange = 360f;
			_LD_displayAngleRange = Mathf.Clamp((float)_modSettings.SimpleWingData_LiftDragPage_AngleRange, minimalAngleRange, maximalAngleRange);

			float minimalCoefficientRange = 0.001f;
			float maximalCoefficientRange = 6f;
			_LD_displayCoefficientRange =  Mathf.Clamp(_modSettings.SimpleWingData_LiftDragPage_CoefficientRange, minimalCoefficientRange, maximalCoefficientRange);
		}
	
		private static void LD_DrawVerticalLines()
		{
			float realRange = _graphsCanvasWidth;
			float minimalReal = -realRange / 2f;
			float realStep = realRange / (_LD_verticalLinesArray.Length - 1);

			float displayRange = _LD_displayAngleRange;
			float minimalDisplay = -displayRange / 2f;
			float displayStep = displayRange / (_LD_verticalLinesArray.Length - 1);

			for (int i = 0; i < _LD_verticalLinesArray.Length; i++)
			{
				_LD_verticalLinesArray[i].localPosition = new Vector3(minimalReal + i * realStep, 0f, 0f);
				_LD_verticalLinesArray[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{(minimalDisplay + i * displayStep).ToString("0")}°";
			}
			int middleIndex = _LD_verticalLinesArray.Length / 2;
			_LD_verticalLinesArray[middleIndex].gameObject.SetActive(false);
			_LD_verticalLinesArray[_LD_verticalLinesArray.Length - 1].GetChild(0).gameObject.SetActive(false);
			_LD_verticalLinesArray[_LD_verticalLinesArray.Length - 2].GetChild(0).gameObject.SetActive(false);
		}

		private static void LD_DrawHorizontalLines()
		{
			float realRange = _graphsCanvasHeight;
			float minimalReal = -realRange / 2f;
			float realStep = realRange / (_LD_horizontalLinesArray.Length - 1);

			float displayRange = _LD_displayCoefficientRange;
			float minimalDisplay = -displayRange / 2f;
			float displayStep = displayRange / (_LD_horizontalLinesArray.Length - 1);

			for (int i = 0; i < _LD_horizontalLinesArray.Length; i++)
			{
				_LD_horizontalLinesArray[i].localPosition = new Vector3(0f, minimalReal + i * realStep, 0f);
				float value = minimalDisplay + i * displayStep;
				string text = value.ToString("0.0");
				if (_LD_displayCoefficientRange <= 3.0f)
					text = value.ToString("0.00");
				if (_LD_displayCoefficientRange <= 1.0f)
					text = value.ToString("0.000");
				if (_LD_displayCoefficientRange <= 0.2f)
					text = value.ToString("0.0000");
				_LD_horizontalLinesArray[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{text}";
			}
			int middleIndex = _LD_horizontalLinesArray.Length / 2;
			_LD_horizontalLinesArray[middleIndex].gameObject.SetActive(false);
		}

		private static void LD_DrawCurves()
		{
			float horizontalRealRange = _graphsCanvasWidth;
			float horizontalDisplayRange = _LD_displayAngleRange;
			float horizontalScale = horizontalRealRange / horizontalDisplayRange;

			float verticalRealRange = _graphsCanvasHeight;
			float verticalDisplayRange = _LD_displayCoefficientRange;
			float verticalScale = verticalRealRange / verticalDisplayRange;

			for (int i = 0; i < _LD_liftPointsArray.Length; i++)
			{
				float x = -horizontalDisplayRange / 2f + i / (float)_LD_liftPointsArray.Length * horizontalDisplayRange;
				float y = Cy.Evaluate(x);
				_LD_liftPointsArray[i].localPosition = new Vector3(x * horizontalScale, y * verticalScale, 0f);
			}

			for (int i = 0; i < _LD_dragPointsArray.Length; i++)
			{
				float x = -horizontalDisplayRange / 2f + i / (float)_LD_dragPointsArray.Length * horizontalDisplayRange;
				float y = Cx.Evaluate(x);
				_LD_dragPointsArray[i].localPosition = new Vector3(x * horizontalScale, y * verticalScale, 0f);
			}
		}
		//-----------------------------------------------------------------------------------------

		// POLAR PAGE------------------------------------------------------------------------------
		private static void UpdatePolarPage()
		{
			P_UpdateCoefficientsRange();
			P_DrawVerticalLines();
			P_DrawHorizontalLines();
			P_DrawCurve();
		}

		private static void P_UpdateCoefficientsRange()
		{
			float minimalDragCoefficientRange = 0.001f;
			float maximalDragCoefficientRange = 1.8f;
			_P_displayDragCoefficientRange = Mathf.Clamp(_modSettings.SimpleWingData_PolarPage_DragCoefficientRange, minimalDragCoefficientRange, maximalDragCoefficientRange);

			float minimalLiftCoefficientRange = 0.001f;
			float maximalLiftCoefficientRange = 6f;
			_P_displayLiftCoefficientRange = Mathf.Clamp(_modSettings.SimpleWingData_PolarPage_LiftCoefficientRange, minimalLiftCoefficientRange, maximalLiftCoefficientRange);

			float minimalAngleOfAttackRange = 1f;
			float maximalAngleOfAttackRange = 180f;
			_P_angleOfAttackRange = Mathf.Clamp(_modSettings.SimpleWingData_PolarPage_AngleOfAttackRange, minimalAngleOfAttackRange, maximalAngleOfAttackRange);
		}

		private static void P_DrawVerticalLines()
		{
			float realRange = _graphsCanvasWidth;
			float minimalReal = -realRange / 2f;
			float realStep = realRange / (_P_verticalLinesArray.Length - 1);

			float displayRange = _P_displayDragCoefficientRange;
			float displayStep = displayRange / (_P_verticalLinesArray.Length - 1);
			
			for (int i = 0; i < _P_verticalLinesArray.Length; i++)
			{
				_P_verticalLinesArray[i].gameObject.SetActive(true);
				_P_verticalLinesArray[i].localPosition = new Vector3(minimalReal + i * realStep, 0f, 0f);
				float value = i * displayStep;
				string text = value.ToString("0.0");
				if (_P_displayDragCoefficientRange <= 1.0f)
					text = value.ToString("0.00");
				if (_P_displayDragCoefficientRange <= 0.5f)
					text = value.ToString("0.000");
				if (_P_displayDragCoefficientRange <= 0.1f)
					text = value.ToString("0.0000");
				_P_verticalLinesArray[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{text}";
			}
			_P_verticalLinesArray[0].gameObject.SetActive(false);
			_P_verticalLinesArray[_P_verticalLinesArray.Length - 1].GetChild(0).gameObject.SetActive(false);
			_P_verticalLinesArray[_P_verticalLinesArray.Length - 2].GetChild(0).gameObject.SetActive(false);
		}

		private static void P_DrawHorizontalLines()
		{
			float realRange = _graphsCanvasHeight;
			float minimalReal = -realRange / 2f;
			float realStep = realRange / (_P_horizontalLinesArray.Length - 1);

			float displayRange = _P_displayLiftCoefficientRange;
			float minimalDisplay = -displayRange / 2f;
			float displayStep = displayRange / (_P_horizontalLinesArray.Length - 1);

			for (int i = 0; i < _P_horizontalLinesArray.Length; i++)
			{
				_P_horizontalLinesArray[i].localPosition = new Vector3(-180f, minimalReal + i * realStep, 0f);
				float value = minimalDisplay + i * displayStep;
				string text = value.ToString("0.0");
				if (_P_displayLiftCoefficientRange <= 3.0f)
					text = value.ToString("0.00");
				if (_P_displayLiftCoefficientRange <= 1.0f)
					text = value.ToString("0.000");
				if (_P_displayLiftCoefficientRange <= 0.2f)
					text = value.ToString("0.0000");
				_P_horizontalLinesArray[i].GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{text}";
			}
			_P_horizontalLinesArray[_P_horizontalLinesArray.Length - 1].GetChild(0).gameObject.SetActive(false);
		}

		private static void P_DrawCurve()
		{
			float horizontalRealRange = _graphsCanvasWidth;
			float horizontalMinimalReal = -horizontalRealRange / 2f;

			float horizontalDisplayRange = _P_displayDragCoefficientRange;
			float horizontalScale = horizontalRealRange / horizontalDisplayRange;

			float verticalRealRange = _graphsCanvasHeight;
			float verticalDisplayRange = _P_displayLiftCoefficientRange;
			float verticalScale = verticalRealRange / verticalDisplayRange;

			float angleOfAttackRange = _P_angleOfAttackRange;

			for (int i = 0; i < _P_pointsArray.Length; i++)
			{
				float angleOfAttack = -angleOfAttackRange / 2f + i / (float)_P_pointsArray.Length * angleOfAttackRange;
				float drag = Cx.Evaluate(angleOfAttack);
				float lift = Cy.Evaluate(angleOfAttack);

				float x = horizontalMinimalReal + drag * horizontalScale;
				float y = lift * verticalScale;

				_P_pointsArray[i].localPosition = new Vector3(x, y, 0f);
			}
		}
		//-----------------------------------------------------------------------------------------
		#endregion
	}
}
