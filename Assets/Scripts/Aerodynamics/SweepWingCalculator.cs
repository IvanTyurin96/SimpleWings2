using UnityEngine;

namespace Assets.Scripts.Aerodynamics
{
    /// <summary>
    /// Calculates special coefficients, needs for visible-correct rotation of leading edge for sweeped wing.
    /// </summary>
    public static class SweepWingCalculator
    {
		/// <summary>
		/// For straight wing not need an additional rotation of leading edge. If angle of attack = 30°, then leading edge
		/// should have 30° rotation. But additional rotation needs for sweeped wing. If sweeped wing have angle of atttack = 30°
		/// and leading edge have 30° rotation, then leading edge still will not "look" to incoming airflow.
		/// This method return coefficient, that shows how many times more rotation angle should have leading edge.
		/// </summary>
		/// <param name="leadingEdgeRotationAxisSweepAngle">Sweep angle of leading edge rotation axis in degrees.</param>
		/// <returns>Coefficient that shows how many times more rotation angle should have leading edge.</returns>
		public static float GetLeadingEdgeRotationCoefficient(float leadingEdgeRotationAxisSweepAngle)
        {
            AnimationCurve curve = new AnimationCurve();

            Keyframe[] keys = new Keyframe[9];

            // Time - angle of wing sweeping
            keys[0].time = 0f; // 0° - straight wing
			keys[1].time = 10f;
            keys[2].time = 20f;
            keys[3].time = 30f;
            keys[4].time = 40f;
            keys[5].time = 50f;
            keys[6].time = 60f;
			keys[7].time = 70f;
			keys[8].time = 80f;

			// Value - rotation coefficient for leading edge
			keys[0].value = 1f; // Straight wing with 0° sweeping have coefficient = 1
			keys[1].value = 1.0126f;
			keys[2].value = 1.0522f;
			keys[3].value = 1.123f;
			keys[4].value = 1.2334f;
			keys[5].value = 1.3976f;
			keys[6].value = 1.6369f;
			keys[7].value = 1.9785f;
			keys[8].value = 2.4421f;

			for (int i = 0; i < keys.Length; i ++)
            {
                keys[i].inTangent = 0f;
                keys[i].outTangent = 0f;
                keys[i].weightedMode = WeightedMode.Both;
                keys[i].inWeight = 0f;
                keys[i].outWeight = 0f;
            }

            curve.keys = keys;

			float rotationCoefficient = curve.Evaluate(Mathf.Abs(leadingEdgeRotationAxisSweepAngle));

            return rotationCoefficient;
        }
    }
}
