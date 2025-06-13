using UnityEngine;

namespace Assets.Scripts.Aerodynamics
{
    /// <summary>
    /// Class for inverting lift and drag curves.
    /// </summary>
    public static class CurveInverter
    {
        /// <summary>
        /// Invert curve relatively horizontal and vertical axis.
        /// </summary>
        /// <param name="keys">Keys of curve.</param>
        public static void InvertXY(Keyframe[] keys)
        {
            float temp;
            for (int i = 0; i < keys.Length / 2; i++)
            {
				int otherSideIndex = keys.Length - 1 - i;
				temp = keys[i].time;
                keys[i].time = keys[otherSideIndex].time * -1f;
                keys[otherSideIndex].time = temp * -1f;

                temp = keys[i].value;
                keys[i].value = keys[otherSideIndex].value * -1f;
                keys[otherSideIndex].value = temp * -1f;

                temp = keys[i].inTangent;
                keys[i].inTangent = keys[otherSideIndex].outTangent;
                keys[otherSideIndex].outTangent = temp;

                temp = keys[i].outTangent;
                keys[i].outTangent = keys[otherSideIndex].inTangent;
                keys[otherSideIndex].inTangent = temp;

                WeightedMode tempWeightedMode = keys[i].weightedMode;
                keys[i].weightedMode = keys[otherSideIndex].weightedMode;
                keys[otherSideIndex].weightedMode = tempWeightedMode;

                temp = keys[i].inWeight;
                keys[i].inWeight = keys[otherSideIndex].outWeight;
                keys[otherSideIndex].outWeight = temp;

                temp = keys[i].outWeight;
                keys[i].outWeight = keys[otherSideIndex].inWeight;
                keys[otherSideIndex].inWeight = temp;
            }

            if (keys.Length % 2 != 0)
            {
                int middlePointIndex = keys.Length / 2;
                keys[middlePointIndex].time = keys[middlePointIndex].time * -1f;
                keys[middlePointIndex].value = keys[middlePointIndex].value * -1f;

                temp = keys[middlePointIndex].inTangent;
                keys[middlePointIndex].inTangent = keys[middlePointIndex].outTangent;
                keys[middlePointIndex].outTangent = temp;

                temp = keys[middlePointIndex].inWeight;
                keys[middlePointIndex].inWeight = keys[middlePointIndex].outWeight;
                keys[middlePointIndex].outWeight = temp;
            }
        }

        /// <summary>
        /// Invert curve relatively vertical axis.
        /// </summary>
        /// <param name="keys">Keys of curve.</param>
        public static void InvertY(Keyframe[] keys)
        {
            float temp;
            for (int i = 0; i < keys.Length / 2; i++)
            {
                int otherSideIndex = keys.Length - 1 - i;
                temp = keys[i].time;
                keys[i].time = keys[otherSideIndex].time * -1f;
                keys[otherSideIndex].time = temp * -1f;

                temp = keys[i].value;
                keys[i].value = keys[otherSideIndex].value;
                keys[otherSideIndex].value = temp;

                temp = keys[i].inTangent;
                keys[i].inTangent = keys[otherSideIndex].outTangent * -1f;
                keys[otherSideIndex].outTangent = temp * -1f;

                temp = keys[i].outTangent;
                keys[i].outTangent = keys[otherSideIndex].inTangent * -1f;
                keys[otherSideIndex].inTangent = temp * -1f;

                WeightedMode tempWeightedMode = keys[i].weightedMode;
                keys[i].weightedMode = keys[otherSideIndex].weightedMode;
                keys[otherSideIndex].weightedMode = tempWeightedMode;

                temp = keys[i].inWeight;
                keys[i].inWeight = keys[otherSideIndex].outWeight;
                keys[otherSideIndex].outWeight = temp;

                temp = keys[i].outWeight;
                keys[i].outWeight = keys[otherSideIndex].inWeight;
                keys[otherSideIndex].inWeight = temp;
            }

            if (keys.Length % 2 != 0)
            {
                int middlePointIndex = keys.Length / 2;
                keys[middlePointIndex].time = keys[middlePointIndex].time * -1f;

                temp = keys[middlePointIndex].inTangent;
                keys[middlePointIndex].inTangent = keys[middlePointIndex].outTangent * -1f;
                keys[middlePointIndex].outTangent = temp * -1f;

                temp = keys[middlePointIndex].inWeight;
                keys[middlePointIndex].inWeight = keys[middlePointIndex].outWeight;
                keys[middlePointIndex].outWeight = temp;
            }
        }
	}
}
