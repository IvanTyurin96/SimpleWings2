using UnityEngine;

namespace Assets.Scripts.Aerodynamics
{
	/// <summary>
	/// Calculator of aerodynamic center curve.
	/// At subcritical AOA aerodynamic center curve have position 25% of Mean Aerodynamic Chord (MAC).
	/// After critical AOA aerodynamic center curve moves from 25% of MAC to 50% of MAC at 90° of AOA.
	/// </summary>
	public static class AerodynamicCenterCurveCalculator
	{
		/// <summary>
		/// Calculate aerodynamic center curve.
		/// </summary>
		/// <param name="negativeCriticalAngle">Negative critical AOA.</param>
		/// <param name="positiveCriticalAngle">Positive critical AOA.</param>
		/// <returns>Keyframes of aerodynamic center curve.</returns>
		public static Keyframe[] CalculateAerodynamicCenterCurve(float negativeCriticalAngle, float positiveCriticalAngle)
		{
			Keyframe[] keys = new Keyframe[4];

			keys[0].time = -90f;
			keys[0].value = 0.5f;

			keys[1].time = negativeCriticalAngle;
			keys[1].value = 0.25f;

			keys[2].time = positiveCriticalAngle;
			keys[2].value = 0.25f;

			keys[3].time = 90f;
			keys[3].value = 0.5f;

			for (int i = 0; i < keys.Length; i++)
			{
				keys[i].outTangent = 0f;
				keys[i].inTangent = 0f;
				keys[i].weightedMode = WeightedMode.Both;
				keys[i].inWeight = 0f;
				keys[i].outWeight = 0f;
			}

			keys[0].outWeight = 0.5f;
			keys[1].inWeight = 0.5f;
			keys[2].outWeight = 0.5f;
			keys[3].inWeight = 0.5f;

			return keys;
		}
	}
}
