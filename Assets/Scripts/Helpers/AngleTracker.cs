using UnityEngine;

namespace Assets.Scripts.Helpers
{
    /// <summary>
    /// Auxiliary class that contains method for tracking angle.
    /// </summary>
    public static class AngleTracker
	{
		/// <summary>
		/// The multiplier of "decreaser" angular error between heading angle and current angle.
		/// Affect the speed how control surface get into correct rotation when angular error <= frameRotationIncrease.
		/// </summary>
		private static readonly float _decreaseErrorMultiplier = 30f;

		/// <summary>
		/// Track the angle.
		/// </summary>
		/// <param name="headingAngle">Heading angle value.</param>
		/// <param name="currentAngle">Current angle value.</param>
		/// <param name="rotationSpeed">Rotation speed (of control surface or leading edge).</param>
		/// <param name="minLimitAngle">Minimal limit of new angle value.</param>
		/// <param name="maxLimitAngle">Maximal limit of new angle value.</param>
		/// <returns>New angle value.</returns>
		public static float TrackAngle(
			float headingAngle,
			float currentAngle,
			float rotationSpeed,
			float minLimitAngle,
			float maxLimitAngle)
		{
			float frameRotationIncrease = 2f * rotationSpeed * Time.deltaTime;

			float error = headingAngle - currentAngle;

			if (Mathf.Abs(headingAngle - currentAngle) <= frameRotationIncrease)
			{
				currentAngle += error * Time.deltaTime * _decreaseErrorMultiplier;
			}
			else
			{
				currentAngle = currentAngle > headingAngle
					? currentAngle - rotationSpeed * Time.deltaTime
					: currentAngle + rotationSpeed * Time.deltaTime;
			}

			if (currentAngle > maxLimitAngle)
				currentAngle = maxLimitAngle;

			if (currentAngle < minLimitAngle)
				currentAngle = minLimitAngle;

			return currentAngle;
		}
	}
}
