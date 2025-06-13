using System;
using System.Text;
using Assets.Scripts.Craft.Parts.Modifiers;
using Assets.Tests.Drag;
using Assets.Tests.Lift;
using UnityEngine;

namespace Assets.Tests.Common
{
    /// <summary>
    /// Base class for <see cref="LiftCurveCalculatorTests"/> and <see cref="DragCurveCalculatorTests"/>.
    /// </summary>
    /// <typeparam name="CurveCalculatorTestsType">Type of tests: <see cref="LiftCurveCalculatorTests"/> or <see cref="DragCurveCalculatorTests"/></typeparam>
    public abstract class BaseCurveCalculatorTests<CurveCalculatorTestsType>
    {
        public void RunTests()
        {
            // Clean airfoil
            foreach (AirfoilType airfoilType in Enum.GetValues(typeof(AirfoilType)))
            {
                foreach (AirfoilThickness airfoilThickness in Enum.GetValues(typeof(AirfoilThickness)))
                {
                    CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness);
                }   
            }

            // Airfoil with leading edge
            const int LeadingEdgePercentage = 40;
            foreach (AirfoilType airfoilType in Enum.GetValues(typeof(AirfoilType)))
            {
                foreach (AirfoilThickness airfoilThickness in Enum.GetValues(typeof(AirfoilThickness)))
                {
                    foreach (LeadingEdgeAngle leadingEdgeAngle in Enum.GetValues(typeof(LeadingEdgeAngle)))
                    {
                        CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness,
                            leadingEdgePercentage: LeadingEdgePercentage,
                            leadingEdgeAngle: (float)leadingEdgeAngle);
                    }
                }
            }

            // Airfoil with control surface
            const int ControlSurfacePercentage = 50;
            foreach (AirfoilType airfoilType in Enum.GetValues(typeof(AirfoilType)))
            {
                foreach (AirfoilThickness airfoilThickness in Enum.GetValues(typeof(AirfoilThickness)))
                {
                    foreach (ControlSurfaceAngle controlSurfaceAngle in Enum.GetValues(typeof(ControlSurfaceAngle)))
                    {
                        CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness,
                            controlSurfacePercentage: ControlSurfacePercentage,
                            controlSurfaceAngle: (float)controlSurfaceAngle);
                    }
                }
            }

            // Airfoil with leading edge and control surface
            foreach (AirfoilType airfoilType in Enum.GetValues(typeof(AirfoilType)))
            {
                foreach (AirfoilThickness airfoilThickness in Enum.GetValues(typeof(AirfoilThickness)))
                {
                    foreach (LeadingEdgeAngle leadingEdgeAngle in Enum.GetValues(typeof(LeadingEdgeAngle)))
                    {
                        foreach (ControlSurfaceAngle controlSurfaceAngle in Enum.GetValues(typeof(ControlSurfaceAngle)))
                        {
                            CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness,
                                leadingEdgePercentage: LeadingEdgePercentage,
                                leadingEdgeAngle: (float)leadingEdgeAngle,
                                controlSurfacePercentage: ControlSurfacePercentage,
                                controlSurfaceAngle: (float)controlSurfaceAngle);
                        }
                    }
                }
            }

            // Airfoil with LERX
            const float NegativeRootAttachedLerxEfficiency = 1f;
            const float PositiveRootAttachedLerxEfficiency = 1f;
            foreach (AirfoilType airfoilType in Enum.GetValues(typeof(AirfoilType)))
            {
                foreach (AirfoilThickness airfoilThickness in Enum.GetValues(typeof(AirfoilThickness)))
                {
                    CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness,
                        isRootAttachedLerxExist: true,
                        positiveRootAttachedLerxEfficiency: PositiveRootAttachedLerxEfficiency);

                    CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness,
						isRootAttachedLerxExist: true,
						negativeRootAttachedLerxEfficiency: NegativeRootAttachedLerxEfficiency);

                    CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness,
						isRootAttachedLerxExist: true,
						negativeRootAttachedLerxEfficiency: NegativeRootAttachedLerxEfficiency,
                        positiveRootAttachedLerxEfficiency: PositiveRootAttachedLerxEfficiency);
                }
            }

            // Airfoil with LERX and control surface
            foreach (AirfoilType airfoilType in Enum.GetValues(typeof(AirfoilType)))
            {
                foreach (AirfoilThickness airfoilThickness in Enum.GetValues(typeof(AirfoilThickness)))
                {
                    foreach (ControlSurfaceAngle controlSurfaceAngle in Enum.GetValues(typeof(ControlSurfaceAngle)))
                    {
                        CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness,
                            controlSurfacePercentage: ControlSurfacePercentage,
                            controlSurfaceAngle: (float)controlSurfaceAngle,
							isRootAttachedLerxExist: true,
							positiveRootAttachedLerxEfficiency: PositiveRootAttachedLerxEfficiency);

                        CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness,
                            controlSurfacePercentage: ControlSurfacePercentage,
                            controlSurfaceAngle: (float)controlSurfaceAngle,
							isRootAttachedLerxExist: true,
							negativeRootAttachedLerxEfficiency: NegativeRootAttachedLerxEfficiency);

                        CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness,
                            controlSurfacePercentage: ControlSurfacePercentage,
                            controlSurfaceAngle: (float)controlSurfaceAngle,
							isRootAttachedLerxExist: true,
							negativeRootAttachedLerxEfficiency: NegativeRootAttachedLerxEfficiency,
                            positiveRootAttachedLerxEfficiency: PositiveRootAttachedLerxEfficiency);
                    }
                }
            }

            // Airfoil with LERX and leading edge
            foreach (AirfoilType airfoilType in Enum.GetValues(typeof(AirfoilType)))
            {
                foreach (AirfoilThickness airfoilThickness in Enum.GetValues(typeof(AirfoilThickness)))
                {
                    foreach (LeadingEdgeAngle leadingEdgeAngle in Enum.GetValues(typeof(LeadingEdgeAngle)))
                    {
                        CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness,
                            leadingEdgePercentage: LeadingEdgePercentage,
                            leadingEdgeAngle: (float)leadingEdgeAngle,
							isRootAttachedLerxExist: true,
							positiveRootAttachedLerxEfficiency: PositiveRootAttachedLerxEfficiency);

                        CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness,
                            leadingEdgePercentage: LeadingEdgePercentage,
                            leadingEdgeAngle: (float)leadingEdgeAngle,
							isRootAttachedLerxExist: true,
							negativeRootAttachedLerxEfficiency: NegativeRootAttachedLerxEfficiency);

                        CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness,
                            leadingEdgePercentage: LeadingEdgePercentage,
                            leadingEdgeAngle: (float)leadingEdgeAngle,
							isRootAttachedLerxExist: true,
							negativeRootAttachedLerxEfficiency: NegativeRootAttachedLerxEfficiency,
                            positiveRootAttachedLerxEfficiency: PositiveRootAttachedLerxEfficiency);
                    }
                }
            }

            // Airfoil with LERX and leading edge and control surface
            foreach (AirfoilType airfoilType in Enum.GetValues(typeof(AirfoilType)))
            {
                foreach (AirfoilThickness airfoilThickness in Enum.GetValues(typeof(AirfoilThickness)))
                {
                    foreach (LeadingEdgeAngle leadingEdgeAngle in Enum.GetValues(typeof(LeadingEdgeAngle)))
                    {
                        foreach (ControlSurfaceAngle controlSurfaceAngle in Enum.GetValues(typeof(ControlSurfaceAngle)))
                        {
                            CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness,
                                leadingEdgePercentage: LeadingEdgePercentage,
                                leadingEdgeAngle: (float)leadingEdgeAngle,
                                controlSurfacePercentage: ControlSurfacePercentage,
                                controlSurfaceAngle: (float)controlSurfaceAngle,
								isRootAttachedLerxExist: true,
								positiveRootAttachedLerxEfficiency: PositiveRootAttachedLerxEfficiency);

                            CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness,
                                leadingEdgePercentage: LeadingEdgePercentage,
                                leadingEdgeAngle: (float)leadingEdgeAngle,
                                controlSurfacePercentage: ControlSurfacePercentage,
                                controlSurfaceAngle: (float)controlSurfaceAngle,
								isRootAttachedLerxExist: true,
								negativeRootAttachedLerxEfficiency: NegativeRootAttachedLerxEfficiency);

                            CalculatedCurveMustBeEqualInverted(airfoilType, (int)airfoilThickness,
                                leadingEdgePercentage: LeadingEdgePercentage,
                                leadingEdgeAngle: (float)leadingEdgeAngle,
                                controlSurfacePercentage: ControlSurfacePercentage,
                                controlSurfaceAngle: (float)controlSurfaceAngle,
								isRootAttachedLerxExist: true,
								negativeRootAttachedLerxEfficiency: NegativeRootAttachedLerxEfficiency,
                                positiveRootAttachedLerxEfficiency: PositiveRootAttachedLerxEfficiency);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculated lift/drag curve of not-inverted airfoil at positive AOA must be equal to lift/drag curve of inverted airfoil at negative AOA.
        /// </summary>
        /// <param name="airfoilType">Type of airfoil.</param>
        /// <param name="airfoilThickness">Thickness of airfoil in percents. Varying from 1 to 24.</param>
		/// <param name="leadingEdgePercentage">Percentage of leading edge.</param>
		/// <param name="leadingEdgeAngle">Current deflect angle of leading edge.</param>
		/// <param name="controlSurfacePercentage">Percentage of control surface.</param>
		/// <param name="controlSurfaceAngle">Current deflect angle of control surface.</param>
		/// <param name="isRootAttachedLerxExist">Is root attached LERX exist?</param>
		/// <param name="negativeRootAttachedLerxEfficiency">Root attached LERX efficiency at negative AOA.</param>
		/// <param name="positiveRootAttachedLerxEfficiency">Root attached LERX efficiency at positive AOA.</param>
        public virtual void CalculatedCurveMustBeEqualInverted(
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
            StringBuilder debugMessage = new StringBuilder();

            string baseInfo = $"Running test {typeof(CurveCalculatorTestsType)}: {nameof(CalculatedCurveMustBeEqualInverted)}, {airfoilType}, {airfoilThickness}% thickness";
            debugMessage.Append(baseInfo);

            string leadingEdgeInfo = leadingEdgePercentage > 0
                ? $", leading edge = {leadingEdgePercentage}%, leading edge angle = {leadingEdgeAngle}°"
                : string.Empty;
            debugMessage.Append(leadingEdgeInfo);

            string controlSurfaceInfo = controlSurfacePercentage > 0
                ? $", control surface = {controlSurfacePercentage}%, control surface angle = {controlSurfaceAngle}°"
                : string.Empty;
            debugMessage.Append(controlSurfaceInfo);

            string negativeLerxEfficiencyInfo = isRootAttachedLerxExist && negativeRootAttachedLerxEfficiency > 0f
                ? $", negative lerx efficiency = {negativeRootAttachedLerxEfficiency}"
                : string.Empty;
            debugMessage.Append(negativeLerxEfficiencyInfo);

            string positiveLerxEfficiencyInfo = isRootAttachedLerxExist && positiveRootAttachedLerxEfficiency > 0f
                ? $", positive lerx efficiency = {positiveRootAttachedLerxEfficiency}"
                : string.Empty;
            debugMessage.Append(positiveLerxEfficiencyInfo);

            Debug.Log(debugMessage);
        }

        private enum AirfoilThickness
        {
            One = 1,
            Twelve = 12,
            TwentyFour = 24
        }

        private enum ControlSurfaceAngle
        {
            Fifteen = 15,
            Thirty = 30,
            FortyFive = 45,
            Sixty = 60,
            SeventyFive = 75,
            Ninety = 90,
            FifteenNegative = -15,
            ThirtyNegative = -30,
            FortyFiveNegative = -45,
            SixtyNegative = -60,
            SeventyFiveNegative = -75,
            NinetyNegative = -90,
        }

        private enum LeadingEdgeAngle
        {
            Ten = 10,
            Twenty = 20,
            Thirty = 30,
            TenNegative = -10,
            TwentyNegative = -20,
            ThirtyNegative = -30,
        }
    }
}
