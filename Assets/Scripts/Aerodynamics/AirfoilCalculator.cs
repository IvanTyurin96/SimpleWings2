using Assets.Scripts.Craft.Parts.Modifiers;
using UnityEngine;

namespace Assets.Scripts.Aerodynamics
{
	/// <summary>
	/// Calculator of lift and drag characteristics for different airfoils.
	/// </summary>
	public static class AirfoilCalculator
	{
        private const float _twelveThickness = 12f;

		/// <summary>
		/// Calculate lift characteristics for different airfoils.
		/// </summary>
		/// <param name="airfoilType">Type of airfoil.</param>
		/// <param name="airfoilThickness">Thickness of airfoil in percents. Varying from 1 to 24.</param>
		/// <param name="zeroAngle">Zero AOA. Needs as logical center between positive and negative critical angles.</param>
		/// <param name="zeroCoefficient">Lift coefficient at zero AOA. This value is 0 only for symmetrical airfoils.</param>
		/// 
		/// <param name="negativeCriticalAngle">Negative critical AOA.</param>
		/// <param name="negativeCriticalCoefficient">Lift coefficient at negative critical AOA.</param>
		/// <param name="positiveCriticalAngle">Positive critical AOA.</param>
		/// <param name="positiveCriticalCoefficient">Lift coefficient at positive critical AOA.</param>
		/// 
		/// <param name="negativePostCriticalAngle">Angle between negative critical AOA and -45° AOA.</param>
		/// <param name="negativePostCriticalCoefficient">Lift coefficient at negative post-critical AOA.</param>
		/// <param name="positivePostCriticalAngle">Angle between positive critical AOA and 45° AOA.</param>
		/// <param name="positivePostCriticalCoefficient">Lift coefficient at positive post-critical AOA.</param>
		/// 
		/// <param name="negativeFortyFiveAngle">-45° AOA.</param>
		/// <param name="negativeFortyFiveLiftCoefficient">Lift coefficient at -45° AOA.</param>
		/// <param name="positiveFortyFiveAngle">45° AOA.</param>
		/// <param name="positiveFortyFiveLiftCoefficient">Lift coefficient at 45° AOA.</param>
		/// 
		/// <param name="negativePerpendicularAngle">-90° AOA.</param>
		/// <param name="negativePerpendicularCoefficient">Lift coefficient at -90° AOA.</param>
		/// <param name="positivePerpendicularAngle">90° AOA.</param>
		/// <param name="positivePerpendicularCoefficient">Lift coefficient at 90° AOA.</param>
		/// 
		/// <param name="negativeRevertedFortyFiveAngle">-135° AOA.</param>
		/// <param name="negativeRevertedFortyFiveCoefficient">Lift coefficient at -135° AOA.</param>
		/// <param name="positiveRevertedFortyFiveAngle">135° AOA.</param>
		/// <param name="positiveRevertedFortyFiveCoefficient">Lift coefficient at 135° AOA.</param>
		/// 
		/// <param name="negativeRevertedPostCriticalAngle">Angle between -135° AOA and negative reverted critical AOA.</param>
		/// <param name="negativeRevertedPostCriticalCoefficient">Lift coefficient at negative reverted post-critical AOA.</param>
		/// <param name="positiveRevertedPostCriticalAngle">Angle between 135° AOA and positive reverted critical AOA.</param>
		/// <param name="positiveRevertedPostCriticalCoefficient">Lift coefficient at positive reverted post-critical AOA.</param>
		/// 
		/// <param name="negativeRevertedCriticalAngle">Negative reverted critical AOA.</param>
		/// <param name="negativeRevertedCriticalCoefficient">Lift coefficient at negative reverted critical AOA.</param>
		/// <param name="positiveRevertedCriticalAngle">Positive reverted critical AOA.</param>
		/// <param name="positiveRevertedCriticalCoefficient">Lift coefficient at positive reverted critical AOA.</param>
		/// 
		/// <param name="negativeLastAngle">-180° AOA.</param>
		/// <param name="negativeLastCoefficient">Lift coefficient at -180° AOA.</param>
		/// <param name="positiveLastAngle">180° AOA.</param>
		/// <param name="positiveLastCoefficient">Lift coefficient at 180° AOA.</param>
		/// 
		/// <param name="liftCoefficientPerDegree">Lift coefficient per degree of AOA.</param>
		public static void CalculateLiftCharacteristics(
			AirfoilType airfoilType,
			float airfoilThickness,
			out float zeroAngle,
			out float zeroCoefficient,

			out float negativeCriticalAngle,
			out float negativeCriticalCoefficient,
			out float positiveCriticalAngle,
			out float positiveCriticalCoefficient,

            out float negativePostCriticalAngle,
            out float negativePostCriticalCoefficient,
            out float positivePostCriticalAngle,
            out float positivePostCriticalCoefficient,

            out float negativeFortyFiveAngle,
            out float negativeFortyFiveLiftCoefficient,
            out float positiveFortyFiveAngle,
            out float positiveFortyFiveLiftCoefficient,

            out float negativePerpendicularAngle,
            out float negativePerpendicularCoefficient,
            out float positivePerpendicularAngle,
            out float positivePerpendicularCoefficient,

            out float negativeRevertedFortyFiveAngle,
            out float negativeRevertedFortyFiveCoefficient,
            out float positiveRevertedFortyFiveAngle,
            out float positiveRevertedFortyFiveCoefficient,

            out float negativeRevertedPostCriticalAngle,
            out float negativeRevertedPostCriticalCoefficient,
            out float positiveRevertedPostCriticalAngle,
            out float positiveRevertedPostCriticalCoefficient,

            out float negativeRevertedCriticalAngle,
            out float negativeRevertedCriticalCoefficient,
            out float positiveRevertedCriticalAngle,
            out float positiveRevertedCriticalCoefficient,

            out float negativeLastAngle,
            out float negativeLastCoefficient,
            out float positiveLastAngle,
            out float positiveLastCoefficient,
			
			out float liftCoefficientPerDegree)
		{
			float asymmetryCriticalCoefficient;
            (float minimalThickness, float maximalThickness, float minimalCriticalAngle, float maximalCriticalAngle) data = new();
			float thicknessLerp;
            float negativePostCriticalLength;
            float positivePostCriticalLength;

			switch (airfoilType)
			{
				case AirfoilType.NACA_64_208:
					zeroAngle = 0f;
					zeroCoefficient = 0.2f;
					liftCoefficientPerDegree = 0.1f;
					asymmetryCriticalCoefficient = 0.875f;

					if (airfoilThickness >= 0f && airfoilThickness < 6f)
					{
						data = (1, 6, 3, 6);
					}
					if (airfoilThickness >= 6f && airfoilThickness < 8f)
					{
						data = (6, 8, 6, 8.5f);
					}
					if (airfoilThickness >= 8f && airfoilThickness < 10f)
					{
						data = (8, 10, 8.5f, 12);
					}
					if (airfoilThickness >= 10f && airfoilThickness < 12f)
					{
						data = (10, 12, 12, 13);
					}
					if (airfoilThickness >= 12f && airfoilThickness < 15f)
					{
						data = (12, 15, 13, 16);
					}
					if (airfoilThickness >= 15f && airfoilThickness < 18f)
					{
						data = (15, 18, 16, 17);
					}
					if (airfoilThickness >= 18f && airfoilThickness < 21f)
					{
						data = (18, 21, 17, 19);
					}
					if (airfoilThickness >= 21f && airfoilThickness < (24f + 1f))
					{
						data = (21, 24, 19, 20);
					}

					thicknessLerp = Mathf.InverseLerp(data.minimalThickness, data.maximalThickness, airfoilThickness);
					positiveCriticalAngle = Mathf.Lerp(data.minimalCriticalAngle, data.maximalCriticalAngle, thicknessLerp);
					positiveCriticalCoefficient = positiveCriticalAngle * liftCoefficientPerDegree + zeroCoefficient;
					negativeCriticalAngle = -positiveCriticalAngle * asymmetryCriticalCoefficient;
					negativeCriticalCoefficient = -positiveCriticalAngle * liftCoefficientPerDegree * asymmetryCriticalCoefficient + zeroCoefficient;

					positivePostCriticalLength = 15f;
					negativePostCriticalLength = positivePostCriticalLength * asymmetryCriticalCoefficient;

					break;

				case AirfoilType.T_10_wing:
					zeroAngle = 0f;
					zeroCoefficient = 0.15f;
					liftCoefficientPerDegree = 0.1f;
					asymmetryCriticalCoefficient = 0.25f;

					if (airfoilThickness >= 0f && airfoilThickness < 6f)
					{
						data = (1, 6, 2, 6);
					}
					if (airfoilThickness >= 6f && airfoilThickness < 8f)
					{
						data = (6, 8, 6, 9);
					}
					if (airfoilThickness >= 8f && airfoilThickness < 10f)
					{
						data = (8, 10, 9, 11);
					}
					if (airfoilThickness >= 10f && airfoilThickness < 12f)
					{
						data = (10, 12, 11, 12);
					}
					if (airfoilThickness >= 12f && airfoilThickness < 15f)
					{
						data = (12, 15, 12, 14);
					}
					if (airfoilThickness >= 15f && airfoilThickness < 18f)
					{
						data = (15, 18, 14, 16);
					}
					if (airfoilThickness >= 18f && airfoilThickness < 21f)
					{
						data = (18, 21, 16, 18);
					}
					if (airfoilThickness >= 21f && airfoilThickness < (24f + 1f))
					{
						data = (21, 24, 18, 20);
					}

					thicknessLerp = Mathf.InverseLerp(data.minimalThickness, data.maximalThickness, airfoilThickness);
					positiveCriticalAngle = Mathf.Lerp(data.minimalCriticalAngle, data.maximalCriticalAngle, thicknessLerp);
					positiveCriticalCoefficient = positiveCriticalAngle * liftCoefficientPerDegree + zeroCoefficient;
					negativeCriticalAngle = -positiveCriticalAngle * asymmetryCriticalCoefficient;
					negativeCriticalCoefficient = -positiveCriticalAngle * liftCoefficientPerDegree * asymmetryCriticalCoefficient + zeroCoefficient;

					positivePostCriticalLength = 5f;
					negativePostCriticalLength = positivePostCriticalLength * asymmetryCriticalCoefficient;

					break;

				case AirfoilType.T_10_root:
                    zeroAngle = 0f;
                    zeroCoefficient = 0.1f;
                    liftCoefficientPerDegree = 0.085f;
                    asymmetryCriticalCoefficient = 0.5f;

                    if (airfoilThickness >= 0f && airfoilThickness < 6f)
                    {
                        data = (1, 6, 3, 8);
                    }
                    if (airfoilThickness >= 6f && airfoilThickness < 8f)
                    {
                        data = (6, 8, 8, 11);
                    }
                    if (airfoilThickness >= 8f && airfoilThickness < 10f)
                    {
                        data = (8, 10, 11, 13);
                    }
                    if (airfoilThickness >= 10f && airfoilThickness < 12f)
                    {
                        data = (10, 12, 13, 14);
                    }
                    if (airfoilThickness >= 12f && airfoilThickness < 15f)
                    {
                        data = (12, 15, 14, 17);
                    }
                    if (airfoilThickness >= 15f && airfoilThickness < 18f)
                    {
                        data = (15, 18, 17, 18);
                    }
                    if (airfoilThickness >= 18f && airfoilThickness < 21f)
                    {
                        data = (18, 21, 18, 20);
                    }
                    if (airfoilThickness >= 21f && airfoilThickness < (24f + 1f))
                    {
                        data = (21, 24, 20, 21);
                    }

                    thicknessLerp = Mathf.InverseLerp(data.minimalThickness, data.maximalThickness, airfoilThickness);
                    positiveCriticalAngle = Mathf.Lerp(data.minimalCriticalAngle, data.maximalCriticalAngle, thicknessLerp);
                    positiveCriticalCoefficient = positiveCriticalAngle * liftCoefficientPerDegree + zeroCoefficient;
                    negativeCriticalAngle = -positiveCriticalAngle * asymmetryCriticalCoefficient;
                    negativeCriticalCoefficient = -positiveCriticalAngle * liftCoefficientPerDegree * asymmetryCriticalCoefficient + zeroCoefficient;

                    positivePostCriticalLength = 15f;
                    negativePostCriticalLength = positivePostCriticalLength * asymmetryCriticalCoefficient;

                    break;

                case AirfoilType.Clark_Y:
					zeroAngle = 0f;
					zeroCoefficient = 0.4f;
					liftCoefficientPerDegree = 0.0733f;
					asymmetryCriticalCoefficient = 0.66f;

                    if (airfoilThickness >= 0f && airfoilThickness < 6f)
                    {
                        data = (1, 6, 3, 9);
                    }
                    if (airfoilThickness >= 6f && airfoilThickness < 8f)
                    {
                        data = (6, 8, 9, 13);
                    }
                    if (airfoilThickness >= 8f && airfoilThickness < 10f)
                    {
                        data = (8, 10, 13, 14);
                    }
                    if (airfoilThickness >= 10f && airfoilThickness < 12f)
                    {
                        data = (10, 12, 14, 15);
                    }
                    if (airfoilThickness >= 12f && airfoilThickness < 15f)
                    {
                        data = (12, 15, 15, 18);
                    }
                    if (airfoilThickness >= 15f && airfoilThickness < 18f)
                    {
                        data = (15, 18, 18, 19);
                    }
                    if (airfoilThickness >= 18f && airfoilThickness < 21f)
                    {
                        data = (18, 21, 19, 20.5f);
                    }
                    if (airfoilThickness >= 21f && airfoilThickness < (24f + 1f))
                    {
                        data = (21, 24, 20.5f, 22);
                    }

                    thicknessLerp = Mathf.InverseLerp(data.minimalThickness, data.maximalThickness, airfoilThickness);
                    positiveCriticalAngle = Mathf.Lerp(data.minimalCriticalAngle, data.maximalCriticalAngle, thicknessLerp);
                    positiveCriticalCoefficient = positiveCriticalAngle * liftCoefficientPerDegree + zeroCoefficient;
                    negativeCriticalAngle = -positiveCriticalAngle * asymmetryCriticalCoefficient;
                    negativeCriticalCoefficient = -positiveCriticalAngle * liftCoefficientPerDegree * asymmetryCriticalCoefficient + zeroCoefficient;

                    positivePostCriticalLength = 10f;
                    negativePostCriticalLength = positivePostCriticalLength * asymmetryCriticalCoefficient;

                    break;

				default: /// default = <see cref="AirfoilType.NACA_0012"/>
					zeroAngle = 0f;
					zeroCoefficient = 0f;
                    liftCoefficientPerDegree = 0.1f; // Lift coefficient = 1.0 at 10 degrees of AOA

                    if (airfoilThickness >= 0f && airfoilThickness < 6f)
					{
						data = (1, 6, 3, 7);
					}
					if (airfoilThickness >= 6f && airfoilThickness < 8f)
					{
						data = (6, 8, 7, 12);
					}
					if (airfoilThickness >= 8f && airfoilThickness < 10f)
					{
						data = (8, 10, 12, 14);
					}
					if (airfoilThickness >= 10f && airfoilThickness < 12f)
					{
						data = (10, 12, 14, 15);
					}
					if (airfoilThickness >= 12f && airfoilThickness < 15f)
					{
						data = (12, 15, 15, 17);
					}
					if (airfoilThickness >= 15f && airfoilThickness < 18f)
					{
						data = (15, 18, 17, 18);
					}
					if (airfoilThickness >= 18f && airfoilThickness < 21f)
					{
						data = (18, 21, 18, 19);
					}
					if (airfoilThickness >= 21f && airfoilThickness < (24f + 1f))
					{
						data = (21, 24, 19, 20);
					}

					thicknessLerp = Mathf.InverseLerp(data.minimalThickness, data.maximalThickness, airfoilThickness);
					positiveCriticalAngle = Mathf.Lerp(data.minimalCriticalAngle, data.maximalCriticalAngle, thicknessLerp);
					positiveCriticalCoefficient = positiveCriticalAngle * liftCoefficientPerDegree;
					negativeCriticalAngle = -positiveCriticalAngle;
					negativeCriticalCoefficient = -positiveCriticalCoefficient;

                    positivePostCriticalLength = 10f;
                    negativePostCriticalLength = positivePostCriticalLength;

                    break;
			}

            // Forty-five angle
            float fortyFiveLiftCoefficient = 1.05f;
            negativeFortyFiveAngle = -45f;
            negativeFortyFiveLiftCoefficient = -fortyFiveLiftCoefficient;
            positiveFortyFiveAngle = 45f;
            positiveFortyFiveLiftCoefficient = fortyFiveLiftCoefficient;

            if (positiveCriticalAngle + positivePostCriticalLength > positiveFortyFiveAngle)
            {
                positiveFortyFiveAngle = positiveCriticalAngle + positivePostCriticalLength;
            }
            if (negativeCriticalAngle - negativePostCriticalLength < negativeFortyFiveAngle)
            {
                negativeFortyFiveAngle = negativeCriticalAngle - negativePostCriticalLength;
            }

            // Post-critical angle
            CalculatePostCriticalAngleAndCoefficient(
                zeroAngle,
                zeroCoefficient,

                negativeCriticalAngle,
                positiveCriticalAngle,

                negativePostCriticalLength,
                positivePostCriticalLength,

                negativeFortyFiveAngle,
                negativeFortyFiveLiftCoefficient,
                positiveFortyFiveAngle,
                positiveFortyFiveLiftCoefficient,

                out negativePostCriticalAngle,
                out negativePostCriticalCoefficient,
                out positivePostCriticalAngle,
                out positivePostCriticalCoefficient);

            // Perpendicular angle
            negativePerpendicularAngle = -90f;
            negativePerpendicularCoefficient = 0f;
            positivePerpendicularAngle = 90f;
            positivePerpendicularCoefficient = 0f;

            // Reverted forty-five angle
            float revertedFortyFiveLiftCoefficient = 0.95f;
            negativeRevertedFortyFiveAngle = -135f;
            negativeRevertedFortyFiveCoefficient = revertedFortyFiveLiftCoefficient;
            positiveRevertedFortyFiveAngle = 135f;
            positiveRevertedFortyFiveCoefficient = -revertedFortyFiveLiftCoefficient;

            // Reverted critical angle
            float lastAngleLiftCoefficient = zeroCoefficient / 4f;
            const float revertedCoefficientPerDegree = 0.09f;
            float staticRevertedCriticalAngle = 9.0f + (airfoilThickness / _twelveThickness);
            float staticRevertedCriticalCoefficient = 0.81f + (airfoilThickness / _twelveThickness) * revertedCoefficientPerDegree;
			float dynamicNegativeRevertedCriticalAngle = positiveCriticalAngle * 0.75f;
			float dynamicPositiveRevertedCriticalAngle = Mathf.Abs(negativeCriticalAngle) * 0.75f;

			negativeRevertedCriticalAngle = dynamicNegativeRevertedCriticalAngle > staticRevertedCriticalAngle
				? -180f + staticRevertedCriticalAngle
				: -180f + dynamicNegativeRevertedCriticalAngle;
            negativeRevertedCriticalCoefficient = dynamicNegativeRevertedCriticalAngle > staticRevertedCriticalAngle
				? lastAngleLiftCoefficient + staticRevertedCriticalCoefficient
				: lastAngleLiftCoefficient + dynamicNegativeRevertedCriticalAngle * revertedCoefficientPerDegree;

            positiveRevertedCriticalAngle = dynamicPositiveRevertedCriticalAngle > staticRevertedCriticalAngle
                ? 180f - staticRevertedCriticalAngle
                : 180f - dynamicPositiveRevertedCriticalAngle;
            positiveRevertedCriticalCoefficient = dynamicPositiveRevertedCriticalAngle > staticRevertedCriticalAngle
                ? lastAngleLiftCoefficient - staticRevertedCriticalCoefficient
                : lastAngleLiftCoefficient - dynamicPositiveRevertedCriticalAngle * revertedCoefficientPerDegree;

            // Last angle
            negativeLastAngle = -180f;
            negativeLastCoefficient = lastAngleLiftCoefficient;
            positiveLastAngle = 180f;
            positiveLastCoefficient = lastAngleLiftCoefficient;

            // Reverted post-critical angle
            const float revertedPostCriticalLength = 10f;
            negativeRevertedPostCriticalAngle = Mathf.Clamp(negativeRevertedCriticalAngle + revertedPostCriticalLength, negativeLastAngle, negativeRevertedFortyFiveAngle);
            negativeRevertedPostCriticalCoefficient = Mathf.Lerp(negativeLastCoefficient, negativeRevertedFortyFiveCoefficient,
                Mathf.InverseLerp(negativeLastAngle, negativeRevertedFortyFiveAngle, negativeRevertedPostCriticalAngle));
            positiveRevertedPostCriticalAngle = Mathf.Clamp(positiveRevertedCriticalAngle - revertedPostCriticalLength, positiveRevertedFortyFiveAngle, positiveLastAngle);
            positiveRevertedPostCriticalCoefficient = Mathf.Lerp(positiveLastCoefficient, positiveRevertedFortyFiveCoefficient,
                Mathf.InverseLerp(positiveLastAngle, positiveRevertedFortyFiveAngle, positiveRevertedPostCriticalAngle));
        }

		/// <summary>
		/// Calculate post-critical angle and coefficient. This is angle between critical angle and 45° angle of AOA.
		/// </summary>
		/// <param name="zeroAngle">Zero AOA. Needs as logical center between positive and negative critical angles.</param>
		/// <param name="zeroCoefficient">Lift coefficient at zero AOA. This value is 0 only for symmetrical airfoils.</param>
		/// 
		/// <param name="negativeCriticalAngle">Negative critical AOA.</param>
		/// <param name="positiveCriticalAngle">Positive critical AOA.</param>
		/// 
		/// <param name="negativePostCriticalLength">The length in degrees after negative critical AOA.</param>
		/// <param name="positivePostCriticalLength">The length in degrees after positive critical AOA.</param>
		/// 
		/// <param name="negativeFortyFiveAngle">-45° AOA.</param>
		/// <param name="negativeFortyFiveLiftCoefficient">Lift coefficient at -45° AOA.</param>
		/// <param name="positiveFortyFiveAngle">45° AOA.</param>
		/// <param name="positiveFortyFiveLiftCoefficient">Lift coefficient at 45° AOA.</param>
		/// 
		/// <param name="negativePostCriticalAngle">Angle between negative critical AOA and -45° AOA.</param>
		/// <param name="negativePostCriticalCoefficient">Lift coefficient at negative post-critical AOA.</param>
		/// <param name="positivePostCriticalAngle">Angle between positive critical AOA and 45° AOA.</param>
		/// <param name="positivePostCriticalCoefficient">Lift coefficient at positive post-critical AOA.</param>
		public static void CalculatePostCriticalAngleAndCoefficient(
            float zeroAngle,
            float zeroCoefficient,

            float negativeCriticalAngle,
            float positiveCriticalAngle,

            float negativePostCriticalLength,
            float positivePostCriticalLength,

            float negativeFortyFiveAngle,
            float negativeFortyFiveLiftCoefficient,
            float positiveFortyFiveAngle,
            float positiveFortyFiveLiftCoefficient,

            out float negativePostCriticalAngle,
            out float negativePostCriticalCoefficient,
            out float positivePostCriticalAngle,
            out float positivePostCriticalCoefficient)
        {
            negativePostCriticalAngle = Mathf.Clamp(negativeCriticalAngle - negativePostCriticalLength, negativeFortyFiveAngle, negativeCriticalAngle);
            negativePostCriticalCoefficient = Mathf.Lerp(zeroCoefficient, negativeFortyFiveLiftCoefficient,
                Mathf.InverseLerp(zeroAngle, negativeFortyFiveAngle, negativePostCriticalAngle));

            positivePostCriticalAngle = Mathf.Clamp(positiveCriticalAngle + positivePostCriticalLength, positiveCriticalAngle, positiveFortyFiveAngle);
            positivePostCriticalCoefficient = Mathf.Lerp(zeroCoefficient, positiveFortyFiveLiftCoefficient,
                Mathf.InverseLerp(zeroAngle, positiveFortyFiveAngle, positivePostCriticalAngle));
        }

		/// <summary>
		/// Calculate drag characteristics for different airfoils.
		/// </summary>
		/// <param name="airfoilType">Type of airfoil.</param>
		/// <param name="airfoilThickness">Thickness of airfoil in percents. Varying from 1 to 24.</param>
		/// <param name="minAngle">Minimal drag AOA.</param>
		/// <param name="minCoefficient">Drag coefficient at minimal drag AOA.</param>
		/// 
		/// <param name="negativeCriticalAngle">Negative critical AOA.</param>
		/// <param name="negativeCriticalCoefficient">Drag coefficient at negative critical AOA.</param>
		/// <param name="positiveCriticalAngle">Positive critical AOA.</param>
		/// <param name="positiveCriticalCoefficient">Drag coefficient at positive critical AOA.</param>
		/// 
		/// <param name="negativePerpendicularAngle">-90° AOA.</param>
		/// <param name="negativePerpendicularCoefficient">Drag coefficient at -90° AOA.</param>
		/// <param name="positivePerpendicularAngle">90° AOA.</param>
		/// <param name="positivePerpendicularCoefficient">Drag coefficient at 90° AOA.</param>
		/// 
		/// <param name="negativeRevertedCriticalAngle">Negative reverted critical AOA.</param>
		/// <param name="negativeRevertedCriticalCoefficient">Negative reverted critical drag coefficient.</param>
		/// <param name="positiveRevertedCriticalAngle">Positive reverted critical AOA.</param>
		/// <param name="positiveRevertedCriticalCoefficient">Positive reverted critical drag coefficient.</param>
		/// 
		/// <param name="negativeLastAngle">-180° AOA.</param>
		/// <param name="positiveLastAngle">180° AOA.</param>
		/// <param name="lastCoefficient">Drag coefficient at -/+ 180° AOA.</param>
		public static void CalculateDragCharacteristics(
            AirfoilType airfoilType,
			float airfoilThickness,
			out float minAngle,
			out float minCoefficient,

            float negativeCriticalAngle,
			out float negativeCriticalCoefficient,
            float positiveCriticalAngle,
			out float positiveCriticalCoefficient,

			out float negativePerpendicularAngle,
			out float negativePerpendicularCoefficient,
			out float positivePerpendicularAngle,
			out float positivePerpendicularCoefficient,

		    float negativeRevertedCriticalAngle,
            out float negativeRevertedCriticalCoefficient,
            float positiveRevertedCriticalAngle,
            out float positiveRevertedCriticalCoefficient,

			out float negativeLastAngle,
			out float positiveLastAngle,
			out float lastCoefficient)
		{
			float positiveDragCoefficientPerDegree;
			float negativeDragCoefficientPerDegree;

			switch (airfoilType)
			{
				case AirfoilType.NACA_64_208:
					negativeDragCoefficientPerDegree = 0.002f;
					positiveDragCoefficientPerDegree = 0.0025f;

					minAngle = 0f;
					minCoefficient = 0.006f * airfoilThickness / _twelveThickness;

					negativeCriticalCoefficient = Mathf.Clamp(Mathf.Abs(negativeCriticalAngle) * negativeDragCoefficientPerDegree, minCoefficient, Mathf.Infinity);
					positiveCriticalCoefficient = Mathf.Clamp(Mathf.Abs(positiveCriticalAngle) * positiveDragCoefficientPerDegree, minCoefficient, Mathf.Infinity);

					break;

				case AirfoilType.T_10_wing:
					positiveDragCoefficientPerDegree = 0.002f;
					negativeDragCoefficientPerDegree = positiveDragCoefficientPerDegree;

					minAngle = 1.5f;
					minCoefficient = 0.005f * airfoilThickness / _twelveThickness;

					negativeCriticalCoefficient = Mathf.Clamp(Mathf.Abs(negativeCriticalAngle) * negativeDragCoefficientPerDegree, minCoefficient, Mathf.Infinity);
					positiveCriticalCoefficient = Mathf.Clamp(Mathf.Abs(positiveCriticalAngle) * positiveDragCoefficientPerDegree, minCoefficient, Mathf.Infinity);

					break;

				case AirfoilType.T_10_root:
                    negativeDragCoefficientPerDegree = 0.002f;
                    positiveDragCoefficientPerDegree = 0.00225f;

                    minAngle = 0f;
                    minCoefficient = 0.005f * airfoilThickness / _twelveThickness;

					negativeCriticalCoefficient = Mathf.Clamp(Mathf.Abs(negativeCriticalAngle) * negativeDragCoefficientPerDegree, minCoefficient, Mathf.Infinity);
					positiveCriticalCoefficient = Mathf.Clamp(Mathf.Abs(positiveCriticalAngle) * positiveDragCoefficientPerDegree, minCoefficient, Mathf.Infinity);

                    break;

                case AirfoilType.Clark_Y:
                    negativeDragCoefficientPerDegree = 0.002f;
					positiveDragCoefficientPerDegree = 0.003f;

					minAngle = 1f;
                    minCoefficient = 0.006f * airfoilThickness / _twelveThickness;

					negativeCriticalCoefficient = Mathf.Clamp(Mathf.Abs(negativeCriticalAngle) * negativeDragCoefficientPerDegree, minCoefficient, Mathf.Infinity);
					positiveCriticalCoefficient = Mathf.Clamp(Mathf.Abs(positiveCriticalAngle) * positiveDragCoefficientPerDegree, minCoefficient, Mathf.Infinity);

                    break;

				default: /// default = <see cref="AirfoilType.NACA_0012"/>
					positiveDragCoefficientPerDegree = 0.002f; // at 15° AOA NACA 0012 will have 15 * 0.002 = 0.03 drag coefficient
					negativeDragCoefficientPerDegree = positiveDragCoefficientPerDegree;

					minAngle = 0f;
                    minCoefficient = 0.006f * airfoilThickness / _twelveThickness;

					negativeCriticalCoefficient = Mathf.Clamp(Mathf.Abs(negativeCriticalAngle) * negativeDragCoefficientPerDegree, minCoefficient, Mathf.Infinity);
					positiveCriticalCoefficient = Mathf.Clamp(Mathf.Abs(positiveCriticalAngle) * positiveDragCoefficientPerDegree, minCoefficient, Mathf.Infinity);

					break;
			}

            // Perpendicular angle
            float perpendicularCoefficient = 1.8f;
			negativePerpendicularAngle = -90f;
			negativePerpendicularCoefficient = perpendicularCoefficient;
			positivePerpendicularAngle = 90f;
			positivePerpendicularCoefficient = perpendicularCoefficient;

			// Reverted critical angle
			float revertedDragCoefficientPerDegree = 0.002f;
            lastCoefficient = minCoefficient * 1.25f;
			negativeRevertedCriticalCoefficient = Mathf.Clamp(Mathf.Abs(-180f - negativeRevertedCriticalAngle) * revertedDragCoefficientPerDegree, lastCoefficient, Mathf.Infinity);
			positiveRevertedCriticalCoefficient = Mathf.Clamp(Mathf.Abs(180f - positiveRevertedCriticalAngle) * revertedDragCoefficientPerDegree, lastCoefficient, Mathf.Infinity);

			// Last angle
			negativeLastAngle = -180f;
		    positiveLastAngle = 180f;
		}

        /// <summary>
        /// Get the airfoil critical Mach multiplier.
        /// </summary>
        /// <param name="airfoilType">Type of airfoil.</param>
        /// <returns>Airfoil critical Mach multiplier.</returns>
        public static float GetCriticalMachMultiplier(AirfoilType airfoilType)
        {
            switch (airfoilType)
            {
                case AirfoilType.T_10_wing:
					return 1.5f;

				case AirfoilType.T_10_root:
					return 1.25f;

				default: /// default = <see cref="AirfoilType.NACA_0012"/> and others
					return 1f;
			}
		}

		/// <summary>
		/// Get the airfoil post-critical shake multiplier.
		/// </summary>
		/// <param name="airfoilType">Type of airfoil.</param>
		/// <returns>Airfoil post-critical shake multiplier.</returns>
		public static float GetPostCriticalShakeMultiplier(AirfoilType airfoilType)
        {
            switch (airfoilType)
            {
                case AirfoilType.T_10_wing:
                    return 0.2f;

                case AirfoilType.T_10_root:
					return 0.125f;

				default: /// default = <see cref="AirfoilType.NACA_0012"/> and others
					return 0.1f;
            }
		}

        /// <summary>
        /// Get the weights at critical and post-critical angles. These weights define the post-critical stability.
        /// </summary>
        /// <param name="airfoilType">Type of airfoil.</param>
        /// <returns>Weights.</returns>
        public static (float insideCriticalWeight, float outsideCriticalWeight, float insidePostCriticalWeight) GetCriticalWeights(AirfoilType airfoilType)
        {
			switch (airfoilType)
            {
                case AirfoilType.T_10_wing:
					return (0.3f, 0.25f, 0.75f);

				default: /// default = <see cref="AirfoilType.NACA_0012"/> and others
					return (0.1f, 1f, 0.25f);
			}
		}

		/// <summary>
		/// Get the LERX efficiency asymmetry multiplier for different airfoils.
		/// Symmetry airfoils have 1.0 multiplier: they have same efficiency as at positive as at negative AOA, like <see cref="AirfoilType.NACA_0012"/>.
		/// Asymmetry airfoils can have different LERX efficiency at negative AOA.
		/// Can because asymmetry airfoils like <see cref="AirfoilType.Clark_Y"/> or <see cref="AirfoilType.NACA_64_208"/>
		/// still have same efficiency at negative AOA.
		/// </summary>
		/// <param name="airfoilType">Type of airfoil.</param>
		/// <returns>Positive value, but less or equal 1.0</returns>
		public static float GetLerxEfficiencyAsymmetryMultiplier(AirfoilType airfoilType)
		{
			switch (airfoilType)
			{
				case AirfoilType.T_10_root:
					return 0.5f;

				case AirfoilType.T_10_wing:
					return 0.25f;

				default: /// default = <see cref="AirfoilType.NACA_0012"/> and other airfoils
					return 1f;
			}
		}

        /// <summary>
        /// Get the multiplier of LERX coverage for different airfoils.
		/// Affects the coverage of LERX vortex to other sub-parts of wing.
        /// </summary>
        /// <param name="airfoilType">Type of airfoil.</param>
        /// <returns>LERX coverage multiplier.</returns>
        public static float GetLerxCoverageMultiplier(AirfoilType airfoilType)
		{
			switch (airfoilType)
			{
				case AirfoilType.T_10_wing:
					return 1f;

				case AirfoilType.T_10_root:
					return 0.75f;

				default: /// default = <see cref="AirfoilType.NACA_0012"/> and other airfoils
					return 0.5f;
			}
		}

		/// <summary>
		/// Get the increasing of critical AOA by LERX for different airfoils.
		/// Increases the critical AOA of other sub-parts of wing.
		/// </summary>
		/// <param name="airfoilType">Type of airfoil.</param>
		/// <returns>Increasing of critical AOA in degrees.</returns>
		public static float GetLerxCriticalAngleRaise(AirfoilType airfoilType)
		{
			switch (airfoilType)
			{
				case AirfoilType.T_10_root:
					return 15f;

				case AirfoilType.T_10_wing:
					return 10f;

				default: /// default = <see cref="AirfoilType.NACA_0012"/> and other airfoils
					return 5f;
			}
		}
    }
}
