using Assets.Scripts.Craft.Parts.Modifiers;
using System;
using UnityEngine;

namespace Assets.Scripts.Aerodynamics
{
    /// <summary>
    /// Calculator of drag curve.
    /// </summary>
    public static class DragCurveCalculator
	{
		/// <summary>
		/// Calculate the keys of drag curve.
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
		/// <param name="negativeDragCoefficientPerDegree">Drag coefficient per degree of negative AOA.</param>
		/// <param name="positiveDragCoefficientPerDegree">Drag coefficient per degree of positive AOA.</param>
		/// <returns>Keys of drag curve.</returns>
		public static Keyframe[] CalculateDragCurve(AirfoilType airfoilRootType, AirfoilType airfoilTipType, float airfoilRootThickness, float airfoilTipThickness, int leadingEdgePercentage, float leadingEdgeAngle, int controlSurfacePercentage, float controlSurfaceAngle, bool isAirfoilInverted, float washoutAngle, bool isRootAttachedLerxExist, float negativeRootAttachedLerxEfficiency, float positiveRootAttachedLerxEfficiency, float rootAttachedLerxCriticalAngleRaise, out float negativeDragCoefficientPerDegree, out float positiveDragCoefficientPerDegree)
		{
			Keyframe[] rootCxKeys = DragCurveCalculator.CalculateKeys(airfoilRootType, airfoilRootThickness, leadingEdgePercentage, leadingEdgeAngle, controlSurfacePercentage, controlSurfaceAngle, isAirfoilInverted, 0f, isRootAttachedLerxExist, negativeRootAttachedLerxEfficiency, positiveRootAttachedLerxEfficiency, rootAttachedLerxCriticalAngleRaise, out float rootNegativeDragCoefficientPerDegree, out float rootPositiveDragCoefficientPerDegree);
            Keyframe[] tipCxKeys = DragCurveCalculator.CalculateKeys(airfoilTipType, airfoilTipThickness, leadingEdgePercentage, leadingEdgeAngle, controlSurfacePercentage, controlSurfaceAngle, isAirfoilInverted, washoutAngle, isRootAttachedLerxExist, negativeRootAttachedLerxEfficiency, positiveRootAttachedLerxEfficiency, rootAttachedLerxCriticalAngleRaise, out float tipNegativeDragCoefficientPerDegree, out float tipPositiveDragCoefficientPerDegree);

			Keyframe[] CxKeys = CurveMerger.MergeCurves(rootCxKeys, tipCxKeys);

			negativeDragCoefficientPerDegree = (rootNegativeDragCoefficientPerDegree + tipNegativeDragCoefficientPerDegree) / 2f;
			positiveDragCoefficientPerDegree = (rootPositiveDragCoefficientPerDegree + tipPositiveDragCoefficientPerDegree) / 2f;

			return CxKeys;
        }

		private static Keyframe[] CalculateKeys(AirfoilType airfoilType, float airfoilThickness, int leadingEdgePercentage, float leadingEdgeAngle, int controlSurfacePercentage, float controlSurfaceAngle, bool isAirfoilInverted, float washoutAngle, bool isRootAttachedLerxExist, float negativeRootAttachedLerxEfficiency, float positiveRootAttachedLerxEfficiency, float rootAttachedLerxCriticalAngleRaise, out float negativeDragCoefficientPerDegree, out float positiveDragCoefficientPerDegree)
		{
			/// Using keys of lift curve to get negativeCriticalAngle, positiveCriticalAngle,
			/// negativeRevertedCriticalAngle, positiveRevertedCriticalAngle - bad idea.
			/// Lift curve can be already inverted, this is the reason why i'm invoke <see cref="AirfoilCalculator.CalculateLiftCharacteristics"/>.
			AirfoilCalculator.CalculateLiftCharacteristics(
				airfoilType,
				airfoilThickness,
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
				out float negativeRevertedCriticalAngle,
				out float _,
				out float positiveRevertedCriticalAngle,
				out float _,
				out float _,
				out float _,
				out float _,
				out float _,
				out float _);

			AirfoilCalculator.CalculateDragCharacteristics(
                airfoilType,
				airfoilThickness,
                out float zeroAngle,
				out float zeroCoefficient,

				negativeCriticalAngle,
                out float negativeCriticalCoefficient,
				positiveCriticalAngle,
				out float positiveCriticalCoefficient,

				out float negativePerpendicularAngle,
				out float negativePerpendicularCoefficient,
				out float positivePerpendicularAngle,
				out float positivePerpendicularCoefficient,

				negativeRevertedCriticalAngle,
                out float negativeRevertedCriticalCoefficient,
				positiveRevertedCriticalAngle,
                out float positiveRevertedCriticalCoefficient,

				out float negativeLastAngle,
				out float positiveLastAngle,
				out float lastCoefficient);

			Keyframe[] CxKeys = new Keyframe[9];

			// Zero angle
			CxKeys[4].time = zeroAngle;
			CxKeys[4].value = zeroCoefficient;

			// Critical angle
			CxKeys[3].time = negativeCriticalAngle;
			CxKeys[3].value = negativeCriticalCoefficient;
			CxKeys[5].time = positiveCriticalAngle;
			CxKeys[5].value = positiveCriticalCoefficient;

			// Perpendicular angle
			CxKeys[2].time = negativePerpendicularAngle;
			CxKeys[2].value = negativePerpendicularCoefficient;
			CxKeys[6].time = positivePerpendicularAngle;
			CxKeys[6].value = positivePerpendicularCoefficient;

			// Reverted critical angle
			CxKeys[1].time = negativeRevertedCriticalAngle;
			CxKeys[1].value = negativeRevertedCriticalCoefficient;
			CxKeys[7].time = positiveRevertedCriticalAngle;
			CxKeys[7].value = positiveRevertedCriticalCoefficient;

			// Last angle
			CxKeys[0].time = negativeLastAngle;
			CxKeys[0].value = lastCoefficient;
			CxKeys[8].time = positiveLastAngle;
			CxKeys[8].value = lastCoefficient;

			for (int i = 0; i < CxKeys.Length; i++)
			{
				CxKeys[i].outTangent = 0f;
				CxKeys[i].inTangent = 0f;
				CxKeys[i].weightedMode = WeightedMode.Both;
				CxKeys[i].inWeight = 0f;
				CxKeys[i].outWeight = 0f;
			}

            if (isAirfoilInverted)
            {
                CurveInverter.InvertY(CxKeys); // ORDER IS IMPORTANT!
            }

			Keyframe[] CxKeysForControlSurface = new Keyframe[CxKeys.Length]; // ORDER IS IMPORTANT!
			Array.Copy(CxKeys, CxKeysForControlSurface, CxKeys.Length); // ORDER IS IMPORTANT!

			DragCurveCalculator.AnalyzeCurve(CxKeys, out negativeDragCoefficientPerDegree, out positiveDragCoefficientPerDegree); // ORDER IS IMPORTANT!

			if (leadingEdgePercentage > 0)
			{
				DragCurveCalculator.LeadingEdgeAffectKeys(
					CxKeys,
					leadingEdgePercentage,
					leadingEdgeAngle,
					negativeDragCoefficientPerDegree,
					positiveDragCoefficientPerDegree); // ORDER IS IMPORTANT!
			}

			if (controlSurfacePercentage > 0)
			{
				DragCurveCalculator.ControlSurfaceAffectKeys(
					CxKeys,
					CxKeysForControlSurface,
					controlSurfacePercentage,
					controlSurfaceAngle,
					negativeDragCoefficientPerDegree,
					positiveDragCoefficientPerDegree); // ORDER IS IMPORTANT!
			}

			if (isRootAttachedLerxExist)
			{
				DragCurveCalculator.LerxAffectKeys(
					CxKeys,
					negativeDragCoefficientPerDegree,
					positiveDragCoefficientPerDegree,
					negativeRootAttachedLerxEfficiency,
					positiveRootAttachedLerxEfficiency,
					rootAttachedLerxCriticalAngleRaise); // ORDER IS IMPORTANT!
			}

			DragCurveCalculator.CorrectDrag(CxKeys, negativeDragCoefficientPerDegree, positiveDragCoefficientPerDegree); // ORDER IS IMPORTANT!
			DragCurveCalculator.SmoothKeys(CxKeys, negativeDragCoefficientPerDegree, positiveDragCoefficientPerDegree); // ORDER IS IMPORTANT!

			DragCurveCalculator.WashoutKeys(CxKeys, washoutAngle); // ORDER IS IMPORTANT!

			return CxKeys;
		}

		private static void AnalyzeCurve(Keyframe[] CxKeys, out float negativeDragCoefficientPerDegree, out float positiveDragCoefficientPerDegree)
		{
			negativeDragCoefficientPerDegree = Mathf.Abs(CxKeys[3].time) > Mathf.Epsilon
				? Mathf.Abs(CxKeys[3].value / CxKeys[3].time)
				: 0f;

			positiveDragCoefficientPerDegree = Mathf.Abs(CxKeys[5].time) > Mathf.Epsilon
				? Mathf.Abs(CxKeys[5].value / CxKeys[5].time)
				: 0f;
		}

		private static void LeadingEdgeAffectKeys(Keyframe[] CxKeys, int leadingEdgePercentage, float leadingEdgeAngle, float negativeDragCoefficientPerDegree, float positiveDragCoefficientPerDegree)
		{
			float criticalAngleIncrease = Mathf.Abs(leadingEdgeAngle * ((float)leadingEdgePercentage) / 100f);

			float negativeCriticalCoefficientIncrease = criticalAngleIncrease * negativeDragCoefficientPerDegree;
			float positiveCriticalCoefficientIncrease = criticalAngleIncrease * positiveDragCoefficientPerDegree;

			// Min angle
			CxKeys[4].value += leadingEdgeAngle >= 0f ? positiveCriticalCoefficientIncrease : negativeCriticalCoefficientIncrease;

			// Critical angle
			CxKeys[3].time += criticalAngleIncrease * Mathf.Sign(leadingEdgeAngle);
			CxKeys[3].value += negativeCriticalCoefficientIncrease;
			CxKeys[5].time += criticalAngleIncrease * Mathf.Sign(leadingEdgeAngle);
			CxKeys[5].value += positiveCriticalCoefficientIncrease;

			// Limits
			CxKeys[3].value = Mathf.Clamp(CxKeys[3].value, CxKeys[4].value, CxKeys[3].value);
			CxKeys[5].value = Mathf.Clamp(CxKeys[5].value, CxKeys[4].value, CxKeys[5].value);
			CxKeys[3].time = Mathf.Clamp(CxKeys[3].time, CxKeys[3].time, Mathf.Lerp(CxKeys[2].time, CxKeys[4].time, Mathf.InverseLerp(CxKeys[2].value, CxKeys[4].value, CxKeys[3].value)));
			CxKeys[5].time = Mathf.Clamp(CxKeys[5].time, Mathf.Lerp(CxKeys[6].time, CxKeys[4].time, Mathf.InverseLerp(CxKeys[6].value, CxKeys[4].value, CxKeys[5].value)), CxKeys[5].time);
		}

		private static void ControlSurfaceAffectKeys(Keyframe[] CxKeys, Keyframe[] CxKeysForControlSurface, int controlSurfacePercentage, float controlSurfaceAngle, float negativeDragCoefficientPerDegree, float positiveDragCoefficientPerDegree)
		{
			CxKeysForControlSurface[4].time = 0f;// Control surface not increase drag at 0 rotation.
			CxKeysForControlSurface[4].value = 0f;// Control surface not increase drag at 0 rotation.

			DragCurveCalculator.CorrectDrag(CxKeysForControlSurface, negativeDragCoefficientPerDegree, positiveDragCoefficientPerDegree);
			DragCurveCalculator.SmoothKeys(CxKeysForControlSurface, negativeDragCoefficientPerDegree, positiveDragCoefficientPerDegree);

			AnimationCurve controlSurfaceDragCurve = new AnimationCurve(CxKeysForControlSurface);

			float dragIncrease = controlSurfaceDragCurve.Evaluate(controlSurfaceAngle) * ((float)controlSurfacePercentage) / 100f;

			#region Step 1. Moving drag curve to up.
			// Zero angle
			CxKeys[4].value += dragIncrease;

			// Critical angle
			CxKeys[3].value += dragIncrease;
			CxKeys[5].value += dragIncrease;
			#endregion

			#region Step 2. Decrease critical angle and critical coefficient.
			const float decreaseMultiplier = 0.5f;
			float criticalAngleDecrease = Mathf.Abs(controlSurfaceAngle) * ((float)controlSurfacePercentage) / 100f * decreaseMultiplier;
			float negativeCriticalCoefficientIncrease = criticalAngleDecrease * negativeDragCoefficientPerDegree;
			float positiveCriticalCoefficientIncrease = criticalAngleDecrease * positiveDragCoefficientPerDegree;

			// Critical angle
			CxKeys[3].time += criticalAngleDecrease;
			CxKeys[3].value -= negativeCriticalCoefficientIncrease;
			CxKeys[5].time -= criticalAngleDecrease;
			CxKeys[5].value -= positiveCriticalCoefficientIncrease;
			#endregion

			#region Step 3. Limits.
			// Zero angle
			CxKeys[4].value = Mathf.Clamp(CxKeys[4].value, CxKeys[4].value, 1.8f); // 1.8f = CxKeys[2].value = CxKeys[6].value

			// Critical angle
			CxKeys[3].time = Mathf.Clamp(CxKeys[3].time, CxKeys[2].time, CxKeys[4].time);
			CxKeys[3].value = Mathf.Clamp(CxKeys[3].value, CxKeys[4].value, CxKeys[2].value);
			CxKeys[5].time = Mathf.Clamp(CxKeys[5].time, CxKeys[4].time, CxKeys[6].time);
			CxKeys[5].value = Mathf.Clamp(CxKeys[5].value, CxKeys[4].value, CxKeys[6].value);

			if (Mathf.Abs(CxKeys[3].time - CxKeys[4].time) < 1f)
			{
				//CxKeys[3].time = Mathf.Clamp(CxKeys[3].time, CxKeys[2].time, Mathf.Lerp(CxKeys[2].time, CxKeys[4].time, Mathf.InverseLerp(CxKeys[2].value, CxKeys[4].value, CxKeys[3].value)));
			}
			if (Mathf.Abs(CxKeys[5].time - CxKeys[4].time) < 1f)
			{
				//CxKeys[5].time = Mathf.Clamp(CxKeys[5].time, Mathf.Lerp(CxKeys[6].time, CxKeys[4].time, Mathf.InverseLerp(CxKeys[6].value, CxKeys[4].value, CxKeys[5].value)), CxKeys[6].time);
			}
			#endregion
		}

		private static void LerxAffectKeys(
			Keyframe[] CxKeys,
			float negativeDragCoefficientPerDegree,
			float positiveDragCoefficientPerDegree,
			float negativeRootAttachedLerxEfficiency,
			float positiveRootAttachedLerxEfficiency,
			float rootAttachedLerxCriticalAngleRaise)
		{
			float negativeCriticalAngleIncrease = rootAttachedLerxCriticalAngleRaise * negativeRootAttachedLerxEfficiency;
			float positiveCriticalAngleIncrease = rootAttachedLerxCriticalAngleRaise * positiveRootAttachedLerxEfficiency;

			float negativeCriticalCoefficientIncrease = negativeCriticalAngleIncrease * negativeDragCoefficientPerDegree;
			float positiveCriticalCoefficientIncrease = positiveCriticalAngleIncrease * positiveDragCoefficientPerDegree;

			// Critical angle
			CxKeys[3].time -= negativeCriticalAngleIncrease;
			CxKeys[3].value += negativeCriticalCoefficientIncrease;
			CxKeys[5].time += positiveCriticalAngleIncrease;
			CxKeys[5].value += positiveCriticalCoefficientIncrease;
		}

		private static void CorrectDrag(Keyframe[] CxKeys, float negativeDragCoefficientPerDegree, float positiveDragCoefficientPerDegree)
		{
			const float maximalAngleOfLinearDrag = 15f;
			const float increasingDragAngleStep = 5f;
			const float increasingDragCoefficient = 0.03f;

			float positiveCriticalAngle = CxKeys[5].time;
			if (positiveCriticalAngle > maximalAngleOfLinearDrag)
			{
				float overrun = positiveCriticalAngle - maximalAngleOfLinearDrag;
				float stepsCount = Mathf.FloorToInt(overrun / increasingDragAngleStep);
				float remainder = overrun % increasingDragAngleStep;
				float newPositiveDrag = increasingDragCoefficient * stepsCount
					+ Mathf.InverseLerp(0f, increasingDragAngleStep, remainder) * increasingDragCoefficient
					- Mathf.Clamp(overrun, 0f, Mathf.Infinity) * positiveDragCoefficientPerDegree;

				//Debug.Log($"positiveCriticalAngle = {positiveCriticalAngle} overrun = {overrun} stepsCount = {stepsCount} remainder = {remainder} newPositiveDrag = {newPositiveDrag}");

				CxKeys[5].value += newPositiveDrag;
			}

			float negativeCriticalAngle = CxKeys[3].time;
			if (Mathf.Abs(negativeCriticalAngle) > maximalAngleOfLinearDrag)
			{
				float overrun = Mathf.Abs(negativeCriticalAngle) - maximalAngleOfLinearDrag;
				float stepsCount = Mathf.FloorToInt(overrun / increasingDragAngleStep);
				float remainder = overrun % increasingDragAngleStep;
				float newNegativeDrag = increasingDragCoefficient * stepsCount
					+ Mathf.InverseLerp(0f, increasingDragAngleStep, remainder) * increasingDragCoefficient
					- Mathf.Clamp(overrun, 0f, Mathf.Infinity) * negativeDragCoefficientPerDegree;

				//Debug.Log($"negativeCriticalAngle = {negativeCriticalAngle} overrun = {overrun} stepsCount = {stepsCount} remainder = {remainder} newNegativeDrag = {newNegativeDrag}");

				CxKeys[3].value += newNegativeDrag;
			}
		}

		private static void SmoothKeys(Keyframe[] CxKeys, float negativeDragCoefficientPerDegree, float positiveDragCoefficientPerDegree)
        {
			const float maximalAngleOfLinearDrag = 15f;
			float negativeMaxLinearDrag = negativeDragCoefficientPerDegree * maximalAngleOfLinearDrag;
			float positiveMaxLinearDrag = positiveDragCoefficientPerDegree * maximalAngleOfLinearDrag;

			Keyframe negativeMaxLinearDragKey = new Keyframe(-maximalAngleOfLinearDrag, Mathf.Clamp(negativeMaxLinearDrag, CxKeys[4].value, Mathf.Infinity));
			Keyframe positiveMaxLinearDragKey = new Keyframe(maximalAngleOfLinearDrag, Mathf.Clamp(positiveMaxLinearDrag, CxKeys[4].value, Mathf.Infinity));

			// Minimal drag angle
			CxKeys[4].inTangent = CxKeys[3].time < -maximalAngleOfLinearDrag 
				? CurveHelper.TangentLookAt(CxKeys[4], negativeMaxLinearDragKey, 1f)
				: CurveHelper.TangentLookAt(CxKeys[4], CxKeys[3], 1f);

			CxKeys[4].inWeight = Mathf.Abs(CxKeys[3].time) > 0f
				? Mathf.Clamp(negativeMaxLinearDragKey.time / CxKeys[3].time, 0f, 1f)
				: 1f;

			CxKeys[4].outTangent = CxKeys[5].time > maximalAngleOfLinearDrag
				? CurveHelper.TangentLookAt(CxKeys[4], positiveMaxLinearDragKey, 1f)
				: CurveHelper.TangentLookAt(CxKeys[4], CxKeys[5], 1f);

			CxKeys[4].outWeight = Mathf.Abs(CxKeys[5].time) > 0f
				? Mathf.Clamp(positiveMaxLinearDragKey.time / CxKeys[5].time, 0f, 1f)
				: 1f;

			// Critical angle
			CxKeys[3].outTangent = CurveHelper.TangentLookFrom(CxKeys[3], negativeMaxLinearDragKey, 1f);
			CxKeys[3].outWeight = Mathf.Abs(CxKeys[3].time) > 0f
				? 1f - Mathf.Clamp(negativeMaxLinearDragKey.time / CxKeys[3].time, 0f, 1f)
				: 0f;
			CxKeys[5].inTangent = CurveHelper.TangentLookFrom(CxKeys[5], positiveMaxLinearDragKey, 1f);
			CxKeys[5].inWeight = Mathf.Abs(CxKeys[5].time) > 0f
				? 1f - Mathf.Clamp(positiveMaxLinearDragKey.time / CxKeys[5].time, 0f, 1f)
				: 0f;

			CxKeys[3].inTangent = CurveHelper.TangentLookFrom(CxKeys, 3, 4);
			CxKeys[3].inWeight = 0.1f;
			CxKeys[5].outTangent = CurveHelper.TangentLookFrom(CxKeys, 5, 4);
			CxKeys[5].outWeight = 0.1f;

			// Reverted critical angle
			CxKeys[1].outTangent = CurveHelper.TangentLookFrom(CxKeys, 1, 0);
            CxKeys[1].outWeight = 0.1f;
			CxKeys[7].inTangent = CurveHelper.TangentLookFrom(CxKeys, 7, 8);
            CxKeys[7].inWeight = 0.1f;

            // Perpendicular angle
            const float averageCriticalAngle = 15f;
            float positiveitiveWeightMultiplier = GetWeightMultiplier(CxKeys[5].time);
            float negativeativeWeightMultiplier = GetWeightMultiplier(CxKeys[3].time);
            float revertedPositiveWeightMultiplier = GetWeightMultiplier(180f - CxKeys[7].time);
            float revertedNegativeWeightMultiplier = GetWeightMultiplier(-180f - CxKeys[1].time);
            float GetWeightMultiplier(float currentCriticalAngle)
            {
                return Mathf.Abs(currentCriticalAngle) - averageCriticalAngle >= Mathf.Epsilon
                ? 0.25f + Mathf.Abs(currentCriticalAngle) / averageCriticalAngle * 0.25f
                : 0.55f - averageCriticalAngle / Mathf.Abs(currentCriticalAngle) * 0.05f;
            }
			float mimimalWeight = 0.4f;
			CxKeys[2].inWeight = Mathf.Clamp(revertedNegativeWeightMultiplier, mimimalWeight, 1f);
			CxKeys[2].outWeight = Mathf.Clamp(negativeativeWeightMultiplier, mimimalWeight, 1f);
			CxKeys[6].inWeight = Mathf.Clamp(positiveitiveWeightMultiplier, mimimalWeight, 1f);
			CxKeys[6].outWeight = Mathf.Clamp(revertedPositiveWeightMultiplier, mimimalWeight, 1f);

			// Last angle
			CxKeys[0].outTangent = CurveHelper.TangentLookAt(CxKeys, 0, 1, 1f);
			CxKeys[0].outWeight = 0.5f;
			CxKeys[8].inTangent = CurveHelper.TangentLookAt(CxKeys, 8, 7, 1f);
			CxKeys[8].inWeight = 0.5f;

			CxKeys[1].inTangent = CurveHelper.TangentLookFrom(CxKeys, 0, 1, 1f);
			CxKeys[1].inWeight = 0.5f;
			CxKeys[7].outTangent = CurveHelper.TangentLookFrom(CxKeys, 8, 7, 1f);
			CxKeys[7].outWeight = 0.5f;

			//Debug.Log($"3 right weight = {CxKeys[3].outWeight}\n5 left weight = {CxKeys[5].inWeight}");
		}

		private static void WashoutKeys(Keyframe[] CxKeys, float washoutAngle)
		{
			for (int i = 0; i < CxKeys.Length; i++)
			{
				CxKeys[i].time -= washoutAngle;
			}
		}
	}
}
