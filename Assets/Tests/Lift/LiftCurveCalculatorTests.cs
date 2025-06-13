using Assets.Scripts.Aerodynamics;
using Assets.Scripts.Craft.Parts.Modifiers;
using Assets.Tests.Common;
using UnityEngine;

namespace Assets.Tests.Lift
{
    /// <summary>
    /// Tests for <see cref="LiftCurveCalculator"/>
    /// </summary>
    public class LiftCurveCalculatorTests : BaseCurveCalculatorTests<LiftCurveCalculatorTests>
    {
        private readonly KeyComparer _keyComparer = new KeyComparer(0.0001M);

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
            AnimationCurve Cy = new AnimationCurve();
            AnimationCurve CyInverted = new AnimationCurve();

			// Act
			float rootAttachedLerxCriticalAngleRaise = LerxHelper.CalculateLerxCriticalAngleRaise(airfoilType, airfoilType);

			Cy.keys = LiftCurveCalculator.CalculateLiftCurve(
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
                rootAttachedLerxCriticalAngleRaise: rootAttachedLerxCriticalAngleRaise,
                rootAttachedLerxPostCriticalEfficiency: 0f,
                out _,
                out _);

#pragma warning disable S2234 // Arguments should be passed in the same order as the method parameters
            CyInverted.keys = LiftCurveCalculator.CalculateLiftCurve(
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
				rootAttachedLerxCriticalAngleRaise: rootAttachedLerxCriticalAngleRaise,
                rootAttachedLerxPostCriticalEfficiency: 0f,
                out _,
                out _);
#pragma warning restore S2234 // Arguments should be passed in the same order as the method parameters

            // Assert
            if (airfoilType == AirfoilType.NACA_0012
                && controlSurfacePercentage == 0
                && leadingEdgePercentage == 0
                && !isRootAttachedLerxExist)
            {
                CurveComparer.CompareCurvesOfSymmmetricalAirfoil(Cy, CyInverted, _keyComparer);
            }
            else
            {
                CurveComparer.CompareLiftCurvesOfAsymmmetricalAirfoil(Cy, CyInverted, _keyComparer);
            }
        }
	}
}
