using Assets.Scripts.Craft.Parts.Modifiers;
using UnityEngine;

namespace Assets.Scripts.Aerodynamics
{
	/// <summary>
	/// Calculator of lift curve.
	/// </summary>
	public static class LiftCurveCalculator
	{
		/// <summary>
		/// Calculate the keys of lift curve.
		/// </summary>
		/// <param name="airfoilRootType">Type of root airfoil.</param>
		/// <param name="airfoilTipType">Type of tip airfoil.</param>
		/// <param name="airfoilRootThickness">Thickness of root airfoil in percents. Varying from 1 to 24.</param>
		/// <param name="airfoilTipThickness">Thickness of tip airfoil in percents. Varying from 1 to 24.</param>
		/// <param name="leadingEdgePercentage">Percentage of leading edge.</param>
		/// <param name="leadingEdgeAngle">Current deflect angle of leading edge.</param>
		/// <param name="controlSurfacePercentage">Percentage of control surface.</param>
		/// <param name="controlSurfaceAngle">Current deflect angle of control surface.</param>
		/// <param name="isAirfoilInverted">Is airfoil inverted?</param>
		/// <param name="washoutAngle">Washout angle.</param>
		/// <param name="isRootAttachedLerxExist">Is root attached LERX exist?</param>
		/// <param name="negativeRootAttachedLerxEfficiency">Root attached LERX efficiency at negative AOA.</param>
		/// <param name="positiveRootAttachedLerxEfficiency">Root attached LERX efficiency at positive AOA.</param>
		/// <param name="rootAttachedLerxCriticalAngleRaise">Root attached LERX critical AOA raise.</param>
		/// <param name="rootAttachedLerxPostCriticalEfficiency">Post-critical efficiency of root attached LERX. Varying from 0 to 1.</param>
		/// <param name="liftCoefficientPerDegree">Lift coefficient per one degree of AOA. Depends from airfoil. For <see cref="AirfoilType.NACA_0012"/> this value = 0.1.</param>
		/// <param name="controlSurfaceRotationAngleLiftEfficiencyCoefficient">Coefficient of control surface lift, depends from current control surface angle. Raise from 0° to +-45°, fade from +-45° to +-90°. Varying from 0 to 1. Needs for control surface torque calculation.</param>
		/// <returns>Keys of lift curve.</returns>
		public static Keyframe[] CalculateLiftCurve(AirfoilType airfoilRootType, AirfoilType airfoilTipType, float airfoilRootThickness, float airfoilTipThickness, int leadingEdgePercentage, float leadingEdgeAngle, int controlSurfacePercentage, float controlSurfaceAngle, bool isAirfoilInverted, float washoutAngle, bool isRootAttachedLerxExist, float negativeRootAttachedLerxEfficiency, float positiveRootAttachedLerxEfficiency, float rootAttachedLerxCriticalAngleRaise, float rootAttachedLerxPostCriticalEfficiency, out float liftCoefficientPerDegree, out float controlSurfaceRotationAngleLiftEfficiencyCoefficient)
		{
			Keyframe[] rootCyKeys = LiftCurveCalculator.CalculateKeys(airfoilRootType, airfoilRootThickness, leadingEdgePercentage, leadingEdgeAngle, controlSurfacePercentage, controlSurfaceAngle, isAirfoilInverted, 0f, isRootAttachedLerxExist, negativeRootAttachedLerxEfficiency, positiveRootAttachedLerxEfficiency, rootAttachedLerxCriticalAngleRaise, rootAttachedLerxPostCriticalEfficiency, out float rootLiftCoefficientPerDegree, out controlSurfaceRotationAngleLiftEfficiencyCoefficient);
            Keyframe[] tipCyKeys = LiftCurveCalculator.CalculateKeys(airfoilTipType, airfoilTipThickness, leadingEdgePercentage, leadingEdgeAngle, controlSurfacePercentage, controlSurfaceAngle, isAirfoilInverted, washoutAngle, isRootAttachedLerxExist, negativeRootAttachedLerxEfficiency, positiveRootAttachedLerxEfficiency, rootAttachedLerxCriticalAngleRaise, rootAttachedLerxPostCriticalEfficiency, out float tipLiftCoefficientPerDegree, out _);

			Keyframe[] CyKeys = CurveMerger.MergeCurves(rootCyKeys, tipCyKeys);

			liftCoefficientPerDegree = (rootLiftCoefficientPerDegree + tipLiftCoefficientPerDegree) / 2f;

			return CyKeys;
        }

		private static Keyframe[] CalculateKeys(AirfoilType airfoilType, float airfoilThickness, int leadingEdgePercentage, float leadingEdgeAngle, int controlSurfacePercentage, float controlSurfaceAngle, bool isAirfoilInverted, float washoutAngle, bool isRootAttachedLerxExist, float negativeRootAttachedLerxEfficiency, float positiveRootAttachedLerxEfficiency, float rootAttachedLerxCriticalAngleRaise, float rootAttachedLerxPostCriticalEfficiency, out float liftCoefficientPerDegree, out float controlSurfaceRotationAngleLiftEfficiencyCoefficient)
		{
			AirfoilCalculator.CalculateLiftCharacteristics(
				airfoilType,
				airfoilThickness,
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
                out float negativeFortyFiveCoefficient,
                out float positiveFortyFiveAngle,
                out float positiveFortyFiveCoefficient,

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

				out liftCoefficientPerDegree);

            Keyframe[] CyKeys = new Keyframe[17];

            // Zero angle
            CyKeys[8].time = zeroAngle;
			CyKeys[8].value = zeroCoefficient;

			// Critical angle
			CyKeys[7].time = negativeCriticalAngle;
			CyKeys[7].value = negativeCriticalCoefficient;
			CyKeys[9].time = positiveCriticalAngle;
			CyKeys[9].value = positiveCriticalCoefficient;

			// Post-critical angle
			CyKeys[6].time = negativePostCriticalAngle;
			CyKeys[6].value = negativePostCriticalCoefficient;
			CyKeys[10].time = positivePostCriticalAngle;
			CyKeys[10].value = positivePostCriticalCoefficient;

			// Forty-five angle
			CyKeys[5].time = negativeFortyFiveAngle;
			CyKeys[5].value = negativeFortyFiveCoefficient;
			CyKeys[11].time = positiveFortyFiveAngle;
			CyKeys[11].value = positiveFortyFiveCoefficient;

			// Perpendicular angle
			CyKeys[4].time = negativePerpendicularAngle;
			CyKeys[4].value = negativePerpendicularCoefficient;
			CyKeys[12].time = positivePerpendicularAngle;
			CyKeys[12].value = positivePerpendicularCoefficient;

			// Reverted forty-five angle
			CyKeys[3].time = negativeRevertedFortyFiveAngle;
			CyKeys[3].value = negativeRevertedFortyFiveCoefficient;
			CyKeys[13].time = positiveRevertedFortyFiveAngle;
			CyKeys[13].value = positiveRevertedFortyFiveCoefficient;

			// Reverted post-critical angle
			CyKeys[2].time = negativeRevertedPostCriticalAngle;
			CyKeys[2].value = negativeRevertedPostCriticalCoefficient;
			CyKeys[14].time = positiveRevertedPostCriticalAngle;
			CyKeys[14].value = positiveRevertedPostCriticalCoefficient;

			// Reverted critical angle
			CyKeys[1].time = negativeRevertedCriticalAngle;
			CyKeys[1].value = negativeRevertedCriticalCoefficient;
			CyKeys[15].time = positiveRevertedCriticalAngle;
			CyKeys[15].value = positiveRevertedCriticalCoefficient;

			// Last angle
			CyKeys[0].time = negativeLastAngle;
			CyKeys[0].value = negativeLastCoefficient;
			CyKeys[16].time = positiveLastAngle;
			CyKeys[16].value = positiveLastCoefficient;

            for (int i = 0; i < CyKeys.Length; i++)
			{
				CyKeys[i].outTangent = 0f;
				CyKeys[i].inTangent = 0f;
				CyKeys[i].weightedMode = WeightedMode.Both;
				CyKeys[i].inWeight = 0f;
				CyKeys[i].outWeight = 0f;
			}

			LiftCurveCalculator.SmoothKeys(CyKeys, airfoilType); // ORDER IS IMPORTANT!

			if (isAirfoilInverted)
			{
				CurveInverter.InvertXY(CyKeys); // ORDER IS IMPORTANT!
            }

			LiftCurveCalculator.AnalyzeCurve(CyKeys, out float negativePostCriticalLength, out float positivePostCriticalLength); // ORDER IS IMPORTANT!

			if (leadingEdgePercentage > 0)
			{
				LiftCurveCalculator.LeadingEdgeAffectKeys(
					CyKeys,
					leadingEdgePercentage,
					leadingEdgeAngle,
					negativePostCriticalLength,
					positivePostCriticalLength,
					liftCoefficientPerDegree); // ORDER IS IMPORTANT!
			}

			controlSurfaceRotationAngleLiftEfficiencyCoefficient = 0f;
			if (controlSurfacePercentage > 0)
			{
				LiftCurveCalculator.ControlSurfaceAffectKeys(
					CyKeys,
					controlSurfacePercentage,
					controlSurfaceAngle,
					negativePostCriticalLength,
					positivePostCriticalLength,
					liftCoefficientPerDegree,
					out controlSurfaceRotationAngleLiftEfficiencyCoefficient); // ORDER IS IMPORTANT!
			}

			if (isRootAttachedLerxExist)
			{
                LiftCurveCalculator.LerxAffectKeys(
					CyKeys,
					liftCoefficientPerDegree,
					negativeRootAttachedLerxEfficiency,
					positiveRootAttachedLerxEfficiency,
					rootAttachedLerxCriticalAngleRaise,
                    rootAttachedLerxPostCriticalEfficiency,
                    negativePostCriticalLength,
					positivePostCriticalLength); // ORDER IS IMPORTANT!
			}

			LiftCurveCalculator.FixWrongSmoothing(CyKeys); // ORDER IS IMPORTANT!

			LiftCurveCalculator.WashoutKeys(CyKeys, washoutAngle); // ORDER IS IMPORTANT!

			return CyKeys;
		}

        private static void SmoothKeys(Keyframe[] CyKeys, AirfoilType airfoilType)
        {
			// Critical angle
			(float insideCriticalWeight, float outsideCriticalWeight, float insidePostCriticalWeight) criticalWeights
				= AirfoilCalculator.GetCriticalWeights(airfoilType);
			CyKeys[7].inWeight = criticalWeights.outsideCriticalWeight;
            CyKeys[7].outWeight = criticalWeights.insideCriticalWeight;
            CyKeys[9].inWeight = criticalWeights.insideCriticalWeight;
            CyKeys[9].outWeight = criticalWeights.outsideCriticalWeight;

			// Post-critical angle
			CyKeys[6].inWeight = 0.25f;
            CyKeys[6].outWeight = criticalWeights.insidePostCriticalWeight;
            CyKeys[10].inWeight = criticalWeights.insidePostCriticalWeight;
            CyKeys[10].outWeight = 0.25f;

            // Forty five angle
            CyKeys[5].inWeight = 0.25f;
            CyKeys[5].outWeight = 0.5f;
            CyKeys[11].inWeight = 0.5f;
            CyKeys[11].outWeight = 0.25f;

            // Reverted forty-five angle
            CyKeys[3].inWeight = 0.5f;
			CyKeys[3].outWeight = 0.25f;
			CyKeys[13].inWeight = 0.25f;
			CyKeys[13].outWeight = 0.5f;

			// Reverted post-critical angle
			CyKeys[2].inWeight = 0.75f;
			CyKeys[2].outWeight = 0.25f;
			CyKeys[14].inWeight = 0.25f;
			CyKeys[14].outWeight = 0.75f;

			// Reverted critical angle
			CyKeys[1].inWeight = 0.25f;
			CyKeys[1].outWeight = 0.1f;
			CyKeys[15].inWeight = 0.1f;
			CyKeys[15].outWeight = 0.25f;
        }

		private static void AnalyzeCurve(Keyframe[] CyKeys, out float negativePostCriticalLength, out float positivePostCriticalLength)
		{
			negativePostCriticalLength = CyKeys[7].time - CyKeys[6].time;
			positivePostCriticalLength = CyKeys[10].time - CyKeys[9].time;
		}

		private static void LeadingEdgeAffectKeys(
			Keyframe[] CyKeys,
			int leadingEdgePercentage,
			float leadingEdgeAngle,
			float negativePostCriticalLength,
			float positivePostCriticalLength,
			float liftCoefficientPerDegree)
        {
			float positiveInducedLiftMultiplier = 0.5f;
			float negativeInducedLiftMultiplier = 1f;
			if (leadingEdgeAngle < 0f)
			{
				float temp = positiveInducedLiftMultiplier;
                positiveInducedLiftMultiplier = negativeInducedLiftMultiplier;
                negativeInducedLiftMultiplier = temp;
            }
            float criticalAngleIncrease = leadingEdgeAngle * ((float)leadingEdgePercentage) / 100f;

			float negativeCriticalCoefficientIncrease = criticalAngleIncrease * liftCoefficientPerDegree * negativeInducedLiftMultiplier;
			float positiveCriticalCoefficientIncrease = criticalAngleIncrease * liftCoefficientPerDegree * positiveInducedLiftMultiplier;

            // Critical angle
            CyKeys[7].time += criticalAngleIncrease;
            CyKeys[7].value += negativeCriticalCoefficientIncrease;
            CyKeys[9].time += criticalAngleIncrease;
            CyKeys[9].value += positiveCriticalCoefficientIncrease;

			// Limits
			CyKeys[7].time = Mathf.Clamp(CyKeys[7].time, CyKeys[7].time, CyKeys[8].time);
			CyKeys[7].value = Mathf.Clamp(CyKeys[7].value, CyKeys[7].value, CyKeys[8].value);
			CyKeys[9].time = Mathf.Clamp(CyKeys[9].time, CyKeys[8].time, CyKeys[9].time);
			CyKeys[9].value = Mathf.Clamp(CyKeys[9].value, CyKeys[8].value, CyKeys[9].value);

			// Forty-five angle
			if (CyKeys[9].time + positivePostCriticalLength > CyKeys[11].time)
			{
				CyKeys[11].time = CyKeys[9].time + positivePostCriticalLength;
			}
			if (CyKeys[7].time - negativePostCriticalLength < CyKeys[5].time)
			{
				CyKeys[5].time = CyKeys[7].time - negativePostCriticalLength;
			}

			AirfoilCalculator.CalculatePostCriticalAngleAndCoefficient(
				CyKeys[8].time,
                CyKeys[8].value,
                CyKeys[7].time,
                CyKeys[9].time,
				negativePostCriticalLength,
				positivePostCriticalLength,
				CyKeys[5].time,
				CyKeys[5].value,
				CyKeys[11].time,
				CyKeys[11].value,
                out float negativePostCriticalAngle,
				out float negativePostCriticalCoefficient,
				out float positivePostCriticalAngle,
				out float positivePostCriticalCoefficient);

            // Post-critical angle
            CyKeys[6].time = negativePostCriticalAngle;
            CyKeys[6].value = negativePostCriticalCoefficient;
            CyKeys[10].time = positivePostCriticalAngle;
            CyKeys[10].value = positivePostCriticalCoefficient;
        }

		private static void ControlSurfaceAffectKeys(
			Keyframe[] CyKeys,
			int controlSurfacePercentage,
			float controlSurfaceAngle,
			float negativePostCriticalLength,
			float positivePostCriticalLength,
			float liftCoefficientPerDegree,
			out float controlSurfaceRotationAngleLiftEfficiencyCoefficient)
		{
			const float LiftIncreasingAngleLimit = 45f;
			const float ControlSurfaceRotationAngleLimit = 90f;
			controlSurfaceRotationAngleLiftEfficiencyCoefficient = Mathf.Lerp(1f, 0f, Mathf.InverseLerp(LiftIncreasingAngleLimit, ControlSurfaceRotationAngleLimit, Mathf.Abs(controlSurfaceAngle)));

			float liftIncrease = controlSurfaceAngle * liftCoefficientPerDegree * ((float)controlSurfacePercentage) / 100f * controlSurfaceRotationAngleLiftEfficiencyCoefficient;

			#region Step 1. Moving lift curve to up.
			// Zero angle
			CyKeys[8].value += liftIncrease;

			// Critical angle
			CyKeys[7].value += liftIncrease;
			CyKeys[9].value += liftIncrease;
			#endregion

			#region Step 2. Decrease critical angle and critical coefficient.
			const float decreaseMultiplier = 0.5f;
            float criticalAngleDecrease = Mathf.Abs(controlSurfaceAngle) * ((float)controlSurfacePercentage) / 100f * decreaseMultiplier;
			float criticalCoefficientDecrease = criticalAngleDecrease * liftCoefficientPerDegree;

            // Critical angle
            CyKeys[7].time += criticalAngleDecrease;
            CyKeys[7].value += criticalCoefficientDecrease;
            CyKeys[9].time -= criticalAngleDecrease;
            CyKeys[9].value -= criticalCoefficientDecrease;

			// Forty-five angle
			const float fortyFiveAngleLiftIncreaseMultiplier = 0.1f;
            float fortyFiveAngleLiftIncrease = liftIncrease * fortyFiveAngleLiftIncreaseMultiplier;
            CyKeys[5].value += fortyFiveAngleLiftIncrease;
			CyKeys[11].value += fortyFiveAngleLiftIncrease;

            float fortyFiveAngleLiftEfficiencyCoefficient = Mathf.Lerp(1f, 1f - ((float)controlSurfacePercentage) / 100f, Mathf.InverseLerp(LiftIncreasingAngleLimit, ControlSurfaceRotationAngleLimit, Mathf.Abs(controlSurfaceAngle)));
            CyKeys[5].value *= fortyFiveAngleLiftEfficiencyCoefficient;
            CyKeys[11].value *= fortyFiveAngleLiftEfficiencyCoefficient;
			#endregion

			#region Step 3. Limits.
			// Critical angle
			CyKeys[7].time = Mathf.Clamp(CyKeys[7].time, CyKeys[7].time, CyKeys[8].time);
			CyKeys[7].value = Mathf.Clamp(CyKeys[7].value, CyKeys[7].value, CyKeys[8].value);
			CyKeys[9].time = Mathf.Clamp(CyKeys[9].time, CyKeys[8].time, CyKeys[9].time);
			CyKeys[9].value = Mathf.Clamp(CyKeys[9].value, CyKeys[8].value, CyKeys[9].value);
			#endregion

			#region Step 4. Calculate post-critical angles.
			AirfoilCalculator.CalculatePostCriticalAngleAndCoefficient(
				0f,
				0f + fortyFiveAngleLiftIncrease,
				CyKeys[7].time,
				CyKeys[9].time,
				negativePostCriticalLength,
				positivePostCriticalLength,
				CyKeys[5].time,
				CyKeys[5].value,
				CyKeys[11].time,
				CyKeys[11].value,
				out float negativePostCriticalAngle,
				out float negativePostCriticalCoefficient,
				out float positivePostCriticalAngle,
				out float positivePostCriticalCoefficient);

			// Post-critical angle
			CyKeys[6].time = negativePostCriticalAngle;
			CyKeys[6].value = negativePostCriticalCoefficient;
			CyKeys[10].time = positivePostCriticalAngle;
			CyKeys[10].value = positivePostCriticalCoefficient;
			#endregion
		}

		private static void LerxAffectKeys(
			Keyframe[] CyKeys,
			float liftCoefficientPerDegree,
			float negativeRootAttachedLerxEfficiency,
			float positiveRootAttachedLerxEfficiency,
			float rootAttachedLerxCriticalAngleRaise,
			float rootAttachedLerxPostCriticalEfficiency,
            float negativePostCriticalLength,
			float positivePostCriticalLength)
		{
			#region Step 1. Increase critical angles and coefficients.
			float negativeCriticalAngleIncrease = rootAttachedLerxCriticalAngleRaise * negativeRootAttachedLerxEfficiency;
			float positiveCriticalAngleIncrease = rootAttachedLerxCriticalAngleRaise * positiveRootAttachedLerxEfficiency;

            float negativeCriticalCoefficientIncrease = negativeCriticalAngleIncrease * liftCoefficientPerDegree;
			float positiveCriticalCoefficientIncrease = positiveCriticalAngleIncrease * liftCoefficientPerDegree;

			// Critical angle
			CyKeys[7].time -= negativeCriticalAngleIncrease;
			CyKeys[7].value -= negativeCriticalCoefficientIncrease;
			CyKeys[9].time += positiveCriticalAngleIncrease;
			CyKeys[9].value += positiveCriticalCoefficientIncrease;
            #endregion

            #region Step 2. Move forty-five angles.
            negativePostCriticalLength += rootAttachedLerxCriticalAngleRaise * rootAttachedLerxPostCriticalEfficiency;
            positivePostCriticalLength += rootAttachedLerxCriticalAngleRaise * rootAttachedLerxPostCriticalEfficiency;

            // Forty-five angle
            if (CyKeys[9].time + positivePostCriticalLength > CyKeys[11].time)
			{
				CyKeys[11].time = CyKeys[9].time + positivePostCriticalLength;
			}
			if (CyKeys[7].time - negativePostCriticalLength < CyKeys[5].time)
			{
				CyKeys[5].time = CyKeys[7].time - negativePostCriticalLength;
			}
			#endregion

			#region Step 3. Calculate post-critical angles and post-critical coefficients.
			AirfoilCalculator.CalculatePostCriticalAngleAndCoefficient(
			CyKeys[8].time,
			CyKeys[8].value,
			CyKeys[7].time,
			CyKeys[9].time,
			negativePostCriticalLength,
			positivePostCriticalLength,
			CyKeys[5].time,
			CyKeys[5].value,
			CyKeys[11].time,
			CyKeys[11].value,
			out float negativePostCriticalAngle,
			out float negativePostCriticalCoefficient,
			out float positivePostCriticalAngle,
			out float positivePostCriticalCoefficient);

			// Post-critical angle
			CyKeys[6].time = negativePostCriticalAngle;
			CyKeys[6].value = negativePostCriticalCoefficient;
			CyKeys[10].time = positivePostCriticalAngle;
			CyKeys[10].value = positivePostCriticalCoefficient;
			#endregion

			#region Step 4. Post-critical stability.
			float oldValue1 = CyKeys[6].value;
			float oldValue2 = CyKeys[10].value;
			float newValue1 = Mathf.Lerp(CyKeys[7].value, CyKeys[5].value, Mathf.InverseLerp(CyKeys[7].time, CyKeys[5].time, CyKeys[6].time));
			float newValue2 = Mathf.Lerp(CyKeys[9].value, CyKeys[11].value, Mathf.InverseLerp(CyKeys[9].time, CyKeys[11].time, CyKeys[10].time));
			CyKeys[6].value = Mathf.Lerp(oldValue1, newValue1, 1f);
			CyKeys[10].value = Mathf.Lerp(oldValue2, newValue2, 1f);

			// Post-critical angle, inside
			float postCriticalTangetMultiplier = 2f;
			float newTangent1 = CurveHelper.TangentLookAt(CyKeys, 6, 7, postCriticalTangetMultiplier);
			float newTangent2 = CurveHelper.TangentLookAt(CyKeys, 10, 9, postCriticalTangetMultiplier);
			CyKeys[6].outTangent = Mathf.Lerp(CyKeys[6].outTangent, newTangent1, negativeRootAttachedLerxEfficiency);
			CyKeys[10].inTangent = Mathf.Lerp(CyKeys[10].inTangent, newTangent2, positiveRootAttachedLerxEfficiency);

			// Post-critical angle, outside
			newTangent1 = CurveHelper.TangentLookAt(CyKeys, 6, 5, postCriticalTangetMultiplier);
			newTangent2 = CurveHelper.TangentLookAt(CyKeys, 10, 11, postCriticalTangetMultiplier);
			CyKeys[6].inTangent = Mathf.Lerp(CyKeys[6].inTangent, newTangent1, negativeRootAttachedLerxEfficiency);
			CyKeys[10].outTangent = Mathf.Lerp(CyKeys[10].outTangent, newTangent2, positiveRootAttachedLerxEfficiency);

			// Forty-five angle, inside
			float fortyFiveTangentMultiplier = 0.5f;
			newTangent1 = CurveHelper.TangentLookAt(CyKeys, 11, 10, fortyFiveTangentMultiplier);
			newTangent2 = CurveHelper.TangentLookAt(CyKeys, 5, 6, fortyFiveTangentMultiplier);
			CyKeys[11].inTangent = Mathf.Lerp(CyKeys[11].inTangent, newTangent1, positiveRootAttachedLerxEfficiency);
			CyKeys[5].outTangent = Mathf.Lerp(CyKeys[5].outTangent, newTangent2, negativeRootAttachedLerxEfficiency);

			// Forty-five angle, outside
			newTangent1 = CurveHelper.TangentLookFrom(CyKeys, 11, 10, fortyFiveTangentMultiplier);
			newTangent2 = CurveHelper.TangentLookFrom(CyKeys, 5, 6, fortyFiveTangentMultiplier);
			CyKeys[11].outTangent = Mathf.Lerp(CyKeys[11].outTangent, newTangent1, positiveRootAttachedLerxEfficiency);
			CyKeys[5].inTangent = Mathf.Lerp(CyKeys[5].inTangent, newTangent2, negativeRootAttachedLerxEfficiency);

			CyKeys[6].value = Mathf.Lerp(oldValue1, newValue1, negativeRootAttachedLerxEfficiency);
			CyKeys[10].value = Mathf.Lerp(oldValue2, newValue2, positiveRootAttachedLerxEfficiency);
			#endregion
		}

		private static void FixWrongSmoothing(Keyframe[] CyKeys)
		{
			// From critical to post-critical, from post-critical to forty-five angle
			if (CyKeys[9].value < CyKeys[10].value)
			{
				CyKeys[9].outWeight = 0f;
				CyKeys[9].inWeight = 0f;

				CyKeys[10].inTangent = CurveHelper.TangentLookAt(CyKeys, 10, 9, 1f);
				float limitOutTangent = CurveHelper.TangentLookAt(CyKeys, 10, 11, 1f);
				CyKeys[10].outTangent = Mathf.Clamp(CurveHelper.TangentLookFrom(CyKeys, 10, 9, 1f), 0f, limitOutTangent);
			}
            if (CyKeys[7].value > CyKeys[6].value)
            {
				CyKeys[7].inWeight = 0f;
				CyKeys[7].outWeight = 0f;

				CyKeys[6].outTangent = CurveHelper.TangentLookAt(CyKeys, 6, 7, 1f);
				float limitInTangent = CurveHelper.TangentLookAt(CyKeys, 6, 5, 1f);
				CyKeys[6].inTangent = Mathf.Clamp(CurveHelper.TangentLookFrom(CyKeys, 6, 7, 1f), 0f, limitInTangent);
			}

			// Negative reverted angle
			if (CyKeys[1].value < CyKeys[2].value)
			{
				CyKeys[1].outWeight = 0f;
				CyKeys[1].inWeight = 0f;

				CyKeys[2].inTangent = CurveHelper.TangentLookAt(CyKeys, 2, 1, 1f);
				float limitOutTangent = CurveHelper.TangentLookAt(CyKeys, 2, 11, 1f);
				CyKeys[2].outTangent = Mathf.Clamp(CurveHelper.TangentLookFrom(CyKeys, 2, 1, 1f), 0f, limitOutTangent);
			}
			// Positive reverted angle
			if (CyKeys[15].value > CyKeys[14].value)
			{
				CyKeys[15].inWeight = 0f;
				CyKeys[15].outWeight = 0f;

				CyKeys[14].outTangent = CurveHelper.TangentLookAt(CyKeys, 14, 15, 1f);
				float limitInTangent = CurveHelper.TangentLookAt(CyKeys, 14, 5, 1f);
				CyKeys[14].inTangent = Mathf.Clamp(CurveHelper.TangentLookFrom(CyKeys, 14, 15, 1f), 0f, limitInTangent);
			}
		}

		private static void WashoutKeys(Keyframe[] CyKeys, float washoutAngle)
		{
			for (int i = 0; i < CyKeys.Length; i++)
			{
				CyKeys[i].time -= washoutAngle;
			}
		}
	}
}
