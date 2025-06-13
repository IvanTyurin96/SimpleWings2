using Assets.Scripts.Aerodynamics;
using Assets.Scripts.Craft.Parts.Modifiers;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Demonstrator of curves. Should be a component of any object in scene. Allow to investigate calculated curves in Unity editor.
    /// </summary>
    public class CurveDemonstrator : MonoBehaviour
    {
		// CURVES
        public AnimationCurve _Cy = new AnimationCurve();
        public AnimationCurve _Cx = new AnimationCurve();
		public AnimationCurve _aC = new AnimationCurve();

		// AIRFOIL
		private float _thickness = 12f;
		private readonly AirfoilType _airfoilRootType = AirfoilType.NACA_0012;
		private readonly AirfoilType _airfoilTipType = AirfoilType.NACA_0012;
		private readonly bool isAirfoilInverted = false;
		private readonly float _washoutAngle = 0f;

		// LEADING EDGE
		private readonly float _leadingEdgePercentage = 0f;
        private float _leadingEdgeAngle = 0f;

		// CONTROL SURFACE
        private readonly float _controlSurfacePercentage = 0f;
        private float _controlSurfaceAngle = 0f;

		// LERX DATA
		private readonly bool _isRootAttachedLerxExist = false;
		private float _positiveRootAttachedLerxEfficiency = 0.0f;
		private float _negativeRootAttachedLerxEfficiency = 0.0f;
		private float rootAttachedLerxPostCriticalEfficiency = 0f;


		private void Update()
        {
            CalculateAerodynamicCurves();
        }

        private void CalculateAerodynamicCurves()
        {
			//_thickness += 1f * Time.deltaTime * 2f;
			//if (_thickness > 24f)
			//{
			//	_thickness = 12f;
			//}
			//Debug.Log($"thickness = {_thickness}");

			//_thickness -= 1f * Time.deltaTime * 0.5f;
			//if (_thickness < 1f)
			//{
			//	_thickness = 1f;
			//}
			//Debug.Log($"Thickness = {_thickness}");

			//_leadingEdgeAngle += 1f * Time.deltaTime * 1f;
			//if (_leadingEdgeAngle > 30f)
			//{
			//	_leadingEdgeAngle = 30f;
			//}
			//Debug.Log($"_leadingEdgeAngle = {_leadingEdgeAngle}");

			//_controlSurfaceAngle += 1f * Time.deltaTime * 1f;
			//if (_controlSurfaceAngle > 90f)
			//{
			//	_controlSurfaceAngle = 0f;
			//}
			//Debug.Log($"_controlSurfaceAngle = {_controlSurfaceAngle}");

			//_negativeRootAttachedLerxEfficiency += Time.deltaTime / 12f;
			//if (_negativeRootAttachedLerxEfficiency > 1f)
			//{
			//	_negativeRootAttachedLerxEfficiency = 0f;
			//}
			//_positiveRootAttachedLerxEfficiency += Time.deltaTime / 12f;
			//if (_positiveRootAttachedLerxEfficiency > 1f)
			//{
			//	_positiveRootAttachedLerxEfficiency = 0f;
			//}
			//Debug.Log($"_negativeRootAttachedLerxEfficiency = {_negativeRootAttachedLerxEfficiency} _positiveRootAttachedLerxEfficiency = {_positiveRootAttachedLerxEfficiency}");

			//rootAttachedLerxPostCriticalEfficiency += 1f * Time.deltaTime / 4f;
			//if (rootAttachedLerxPostCriticalEfficiency > 1f)
			//{
			//	rootAttachedLerxPostCriticalEfficiency = 0f;
			//}
			//Debug.Log($"rootAttachedLerxPostCriticalEfficiency = {rootAttachedLerxPostCriticalEfficiency}");

			float rootAttachedLerxCriticalAngleRaise = LerxHelper.CalculateLerxCriticalAngleRaise(AirfoilType.T_10_root, AirfoilType.T_10_root);

			// LIFT
			_Cy.keys = LiftCurveCalculator.CalculateLiftCurve(_airfoilRootType, _airfoilTipType, _thickness, _thickness, (int)_leadingEdgePercentage, _leadingEdgeAngle, (int)_controlSurfacePercentage, _controlSurfaceAngle, isAirfoilInverted, _washoutAngle, _isRootAttachedLerxExist, _negativeRootAttachedLerxEfficiency, _positiveRootAttachedLerxEfficiency, rootAttachedLerxCriticalAngleRaise, rootAttachedLerxPostCriticalEfficiency, out _, out _);

            // DRAG
            _Cx.keys = DragCurveCalculator.CalculateDragCurve(_airfoilRootType, _airfoilTipType, _thickness, _thickness, (int)_leadingEdgePercentage, _leadingEdgeAngle, (int)_controlSurfacePercentage, _controlSurfaceAngle, isAirfoilInverted, _washoutAngle, _isRootAttachedLerxExist, _negativeRootAttachedLerxEfficiency, _positiveRootAttachedLerxEfficiency, rootAttachedLerxCriticalAngleRaise, out _, out _);

			// AERODYNAMIC CENTER
			_aC.keys = AerodynamicCenterCurveCalculator.CalculateAerodynamicCenterCurve(_Cy.keys[7].time, _Cy.keys[9].time);
		}
    }
}