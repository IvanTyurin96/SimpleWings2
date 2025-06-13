using Assets.Scripts.Aerodynamics;
using Assets.Scripts.Craft.Parts.Modifiers;
using Assets.Tests.Common;
using UnityEngine;

namespace Assets.Tests.Drag
{
    /// <summary>
    /// Tests for <see cref="DragCurveCalculator"/>
    /// </summary>
    public class DragCurveCalculatorTests : BaseCurveCalculatorTests<DragCurveCalculatorTests>
    {
        private readonly KeyComparer _keyComparer = new KeyComparer(0.000002M);

        /// <inheritdoc />
        public override void CalculatedCurveMustBeEqualInverted(
            AirfoilType airfoilType,
            float airfoilThickness,
            int leadingEdgePercentage = 0,
            float leadingEdgeAngle = 0f,
            int controlSurfacePercentage = 0,
            float controlSurfaceAngle = 0f,
            bool isRootAttachedLerxExist = false,
            float negativeRootAttachedLerxEfficiency = 0f,
            float positiveRootAttachedLerxEfficiency = 0f)
        {
            base.CalculatedCurveMustBeEqualInverted(
                airfoilType: airfoilType,
                airfoilThickness: airfoilThickness,
                leadingEdgePercentage: leadingEdgePercentage,
                leadingEdgeAngle: leadingEdgeAngle,
                controlSurfacePercentage: controlSurfacePercentage,
                controlSurfaceAngle: controlSurfaceAngle,
                isRootAttachedLerxExist: isRootAttachedLerxExist,
                negativeRootAttachedLerxEfficiency: negativeRootAttachedLerxEfficiency,
                positiveRootAttachedLerxEfficiency: positiveRootAttachedLerxEfficiency);

            // Arrange
            AnimationCurve Cx = new AnimationCurve();
            AnimationCurve CxInverted = new AnimationCurve();

			// Act
			float rootAttachedLerxCriticalAngleRaise = LerxHelper.CalculateLerxCriticalAngleRaise(airfoilType, airfoilType);

			Cx.keys = DragCurveCalculator.CalculateDragCurve(
                airfoilRootType: airfoilType,
                airfoilTipType: airfoilType,
                airfoilRootThickness: airfoilThickness,
                airfoilTipThickness: airfoilThickness,
                leadingEdgePercentage: leadingEdgePercentage,
                leadingEdgeAngle: leadingEdgeAngle,
                controlSurfacePercentage: controlSurfacePercentage,
                controlSurfaceAngle: controlSurfaceAngle,
                isAirfoilInverted: false,
                washoutAngle: 0f,
                isRootAttachedLerxExist: isRootAttachedLerxExist,
                negativeRootAttachedLerxEfficiency: negativeRootAttachedLerxEfficiency,
                positiveRootAttachedLerxEfficiency: positiveRootAttachedLerxEfficiency,
				rootAttachedLerxCriticalAngleRaise: rootAttachedLerxCriticalAngleRaise, out _, out _);

#pragma warning disable S2234 // Arguments should be passed in the same order as the method parameters
            CxInverted.keys = DragCurveCalculator.CalculateDragCurve(
                airfoilRootType: airfoilType,
                airfoilTipType: airfoilType,
                airfoilRootThickness: airfoilThickness,
                airfoilTipThickness: airfoilThickness,
                leadingEdgePercentage: leadingEdgePercentage,
                leadingEdgeAngle: leadingEdgeAngle * -1f,
                controlSurfacePercentage: controlSurfacePercentage,
                controlSurfaceAngle: controlSurfaceAngle * -1f,
                isAirfoilInverted: true,
                washoutAngle: 0f,
                isRootAttachedLerxExist: isRootAttachedLerxExist,
                negativeRootAttachedLerxEfficiency: positiveRootAttachedLerxEfficiency,
                positiveRootAttachedLerxEfficiency: negativeRootAttachedLerxEfficiency,
				rootAttachedLerxCriticalAngleRaise: rootAttachedLerxCriticalAngleRaise, out _, out _);
#pragma warning restore S2234 // Arguments should be passed in the same order as the method parameters

            // Assert
            if (airfoilType == AirfoilType.NACA_0012
                && controlSurfacePercentage == 0
                && leadingEdgePercentage == 0
                && !isRootAttachedLerxExist)
            {
                CurveComparer.CompareCurvesOfSymmmetricalAirfoil(Cx, CxInverted, _keyComparer);
            }
            else
            {
                CurveComparer.CompareDragCurvesOfAsymmmetricalAirfoil(Cx, CxInverted, _keyComparer);
            }
        }
	}
}
