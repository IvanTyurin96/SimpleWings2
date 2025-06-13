using UnityEngine;

namespace Assets.Scripts.Aerodynamics
{
    /// <summary>
    /// Helper class for animation curves.
    /// </summary>
    public static class CurveHelper
    {
        /// <summary>
        /// Return new tangent from key to key.
        /// </summary>
        /// <param name="keys">Keys of animation curve.</param>
        /// <param name="fromIndex">From key index.</param>
        /// <param name="toIndex">To key index.</param>
        /// <param name="tangentMultiplier">Tangent multiplier.</param>
        /// <returns>New tangent value.</returns>
        public static float TangentLookAt(Keyframe[] keys, int fromIndex, int toIndex, float tangentMultiplier = 1f)
        {
            if (Mathf.Abs(keys[fromIndex].time - keys[toIndex].time) <= Mathf.Epsilon)
            {
                return 0f;
            }

            if (keys[fromIndex].time > keys[toIndex].time)
            {
				return (keys[fromIndex].value - keys[toIndex].value) / (keys[fromIndex].time - keys[toIndex].time) * tangentMultiplier;
            }
            else
            {
				return (keys[fromIndex].value - keys[toIndex].value) / (keys[fromIndex].time - keys[toIndex].time) * tangentMultiplier;
            }
        }

		/// <summary>
		/// Return new tangent from key to key.
		/// </summary>
		/// <param name="from">From key.</param>
		/// <param name="to">To key.</param>
		/// <param name="tangentMultiplier">Tangent multiplier.</param>
		/// <returns>New tangent value.</returns>
		public static float TangentLookAt(Keyframe from, Keyframe to, float tangentMultiplier = 1f)
		{
			if (Mathf.Abs(from.time - to.time) <= Mathf.Epsilon)
			{
				return 0f;
			}

			if (from.time > to.time)
			{
				return (from.value - to.value) / (from.time - to.time) * tangentMultiplier;
			}
			else
			{
				return (from.value - to.value) / (from.time - to.time) * tangentMultiplier;
			}
		}

		/// <summary>
		/// Return new tangent from key to key.
		/// </summary>
		/// <param name="keys">Keys of animation curve.</param>
		/// <param name="tangentIndex">Index of key which tangent should look from fromIndex key.</param>
		/// <param name="fromIndex">From index key.</param>
		/// <param name="tangentMultiplier">Tangent multiplier.</param>
		/// <returns>New tangent value.</returns>
		public static float TangentLookFrom(Keyframe[] keys, int tangentIndex, int fromIndex, float tangentMultiplier = 1f)
        {
            if (Mathf.Abs(keys[tangentIndex].time - keys[fromIndex].time) <= Mathf.Epsilon)
            {
                return 0f;
            }

            if (keys[tangentIndex].time > keys[fromIndex].time)
            {
				return (keys[tangentIndex].value - keys[fromIndex].value) / (keys[tangentIndex].time - keys[fromIndex].time) * tangentMultiplier;
            }
            else
            {
				return (keys[tangentIndex].value - keys[fromIndex].value) / (keys[tangentIndex].time - keys[fromIndex].time) * tangentMultiplier;
            }
        }

		/// <summary>
		/// Return new tangent from key to key.
		/// </summary>
		/// <param name="tangentKey">Key which tangent should look from from key.</param>
		/// <param name="from">From key.</param>
		/// <param name="tangentMultiplier">Tangent multiplier.</param>
		/// <returns>New tangent value.</returns>
		public static float TangentLookFrom(Keyframe tangentKey, Keyframe from, float tangentMultiplier = 1f)
		{
			if (Mathf.Abs(tangentKey.time - from.time) <= Mathf.Epsilon)
			{
				return 0f;
			}

			if (tangentKey.time > from.time)
			{
				return (tangentKey.value - from.value) / (tangentKey.time - from.time) * tangentMultiplier;
			}
			else
			{
				return (tangentKey.value - from.value) / (tangentKey.time - from.time) * tangentMultiplier;
			}
		}
	}
}
