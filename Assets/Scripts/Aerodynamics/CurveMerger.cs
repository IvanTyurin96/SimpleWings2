using UnityEngine;

namespace Assets.Scripts.Aerodynamics
{
    /// <summary>
    /// Merger of root and tip curves.
    /// </summary>
    public static class CurveMerger
    {
        /// <summary>
        /// Merge root and tip curves. Expects that <paramref name="rootCurve"/> have same count of keys as <paramref name="tipCurve"/>.
        /// </summary>
        /// <param name="rootCurve">Keys of root curve.</param>
        /// <param name="tipCurve">Keys of tip curve.</param>
        /// <returns>Keys of merged curve.</returns>
        public static Keyframe[] MergeCurves(Keyframe[] rootCurve, Keyframe[] tipCurve) 
        {
            Keyframe[] mergedPoints = new Keyframe[rootCurve.Length];

            for (int i = 0; i < mergedPoints.Length; i++) 
            {
                mergedPoints[i].time = (rootCurve[i].time + tipCurve[i].time) / 2f;
                mergedPoints[i].value = (rootCurve[i].value + tipCurve[i].value) / 2f;

                mergedPoints[i].inTangent = (rootCurve[i].inTangent + tipCurve[i].inTangent) / 2f;
                mergedPoints[i].outTangent = (rootCurve[i].outTangent + tipCurve[i].outTangent) / 2f;

                mergedPoints[i].weightedMode = rootCurve[i].weightedMode;
                mergedPoints[i].inWeight = (rootCurve[i].inWeight + tipCurve[i].inWeight) / 2f;
                mergedPoints[i].outWeight = (rootCurve[i].outWeight + tipCurve[i].outWeight) / 2f;
            }

            return mergedPoints;
        }
    }
}
