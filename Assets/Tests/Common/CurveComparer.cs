using Assets.Scripts.Craft.Parts.Modifiers;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Tests.Common
{
    /// <summary>
    /// Auxiliary class for lift and drag tests, provides different method for comparing curves.
    /// </summary>
    public static class CurveComparer
	{
		/// <summary>
		/// Compare not inverted curve with inverted curve of symmetrical airfoil like <see cref="AirfoilType.NACA_0012"/>.
		/// </summary>
		/// <param name="curve">Not inverted curve.</param>
		/// <param name="curveInverted">Inverted curve.</param>
		/// <param name="keyComparer">Key comparer.</param>
		public static void CompareCurvesOfSymmmetricalAirfoil(AnimationCurve curve, AnimationCurve curveInverted, KeyComparer keyComparer)
		{
			for (int i = 0; i < curve.keys.Length; i++)
			{
				Assert.AreEqual(curve.keys[i].time, curveInverted.keys[i].time, null, keyComparer);
				Assert.AreEqual(curve.keys[i].value, curveInverted.keys[i].value, null, keyComparer);

				Assert.AreEqual(curve.keys[i].inTangent, curveInverted.keys[i].inTangent, null, keyComparer);
				Assert.AreEqual(curve.keys[i].outTangent, curveInverted.keys[i].outTangent, null, keyComparer);

				Assert.AreEqual(curve.keys[i].weightedMode, curveInverted.keys[i].weightedMode, null);
				Assert.AreEqual(curve.keys[i].inWeight, curveInverted.keys[i].inWeight, null, keyComparer);
				Assert.AreEqual(curve.keys[i].outWeight, curveInverted.keys[i].outWeight, null, keyComparer);
			}
		}

		/// <summary>
		/// Compare not inverted lift curve with inverted lift curve of asymmetrical airfoil.
		/// </summary>
		/// <param name="Cy">Not inverted lift curve.</param>
		/// <param name="CyInverted">Inverted lift curve.</param>
		/// <param name="keyComparer">Key comparer.</param>
		public static void CompareLiftCurvesOfAsymmmetricalAirfoil(AnimationCurve Cy, AnimationCurve CyInverted, KeyComparer keyComparer)
		{
			for (int i = 0; i < Cy.keys.Length / 2; i++)
			{
				int revertedIndex = Cy.keys.Length - 1 - i;

				Assert.AreEqual(Cy.keys[i].time, -CyInverted.keys[revertedIndex].time, null, keyComparer);
				Assert.AreEqual(Cy.keys[i].value, -CyInverted.keys[revertedIndex].value, null, keyComparer);

				Assert.AreEqual(Cy.keys[i].inTangent, CyInverted.keys[revertedIndex].outTangent, null, keyComparer);
				Assert.AreEqual(Cy.keys[i].outTangent, CyInverted.keys[revertedIndex].inTangent, null, keyComparer);

				Assert.AreEqual(Cy.keys[i].weightedMode, CyInverted.keys[revertedIndex].weightedMode, null);
				Assert.AreEqual(Cy.keys[i].inWeight, CyInverted.keys[revertedIndex].outWeight, null, keyComparer);
				Assert.AreEqual(Cy.keys[i].outWeight, CyInverted.keys[revertedIndex].inWeight, null, keyComparer);
			}
			if (Cy.keys.Length % 2 != 0)
			{
				int middlePointIndex = Cy.keys.Length / 2;

				Assert.AreEqual(Cy.keys[middlePointIndex].time, -CyInverted.keys[middlePointIndex].time, null, keyComparer);
				Assert.AreEqual(Cy.keys[middlePointIndex].value, -CyInverted.keys[middlePointIndex].value, null, keyComparer);

				Assert.AreEqual(Cy.keys[middlePointIndex].inTangent, CyInverted.keys[middlePointIndex].outTangent, null, keyComparer);
				Assert.AreEqual(Cy.keys[middlePointIndex].outTangent, CyInverted.keys[middlePointIndex].inTangent, null, keyComparer);

				Assert.AreEqual(Cy.keys[middlePointIndex].weightedMode, CyInverted.keys[middlePointIndex].weightedMode, null);
				Assert.AreEqual(Cy.keys[middlePointIndex].inWeight, CyInverted.keys[middlePointIndex].outWeight, null, keyComparer);
				Assert.AreEqual(Cy.keys[middlePointIndex].outWeight, CyInverted.keys[middlePointIndex].inWeight, null, keyComparer);
			}
		}

        /// <summary>
        /// Compare not inverted drag curve with inverted drag curve of asymmetrical airfoil.
        /// </summary>
        /// <param name="Cx">Not inverted drag curve.</param>
        /// <param name="CxInverted">Inverted drag curve.</param>
        /// <param name="keyComparer">Key comparer.</param>
        public static void CompareDragCurvesOfAsymmmetricalAirfoil(AnimationCurve Cx, AnimationCurve CxInverted, KeyComparer keyComparer)
		{
            for (int i = 0; i < Cx.keys.Length / 2; i++)
            {
                int revertedIndex = Cx.keys.Length - 1 - i;

                Assert.AreEqual(Cx.keys[i].time, -CxInverted.keys[revertedIndex].time, null, keyComparer);
                Assert.AreEqual(Cx.keys[i].value, CxInverted.keys[revertedIndex].value, null, keyComparer);

                Assert.AreEqual(Cx.keys[i].inTangent, -CxInverted.keys[revertedIndex].outTangent, null, keyComparer);
                Assert.AreEqual(Cx.keys[i].outTangent, -CxInverted.keys[revertedIndex].inTangent, null, keyComparer);

                Assert.AreEqual(Cx.keys[i].weightedMode, CxInverted.keys[revertedIndex].weightedMode, null);
                Assert.AreEqual(Cx.keys[i].inWeight, CxInverted.keys[revertedIndex].outWeight, null, keyComparer);
                Assert.AreEqual(Cx.keys[i].outWeight, CxInverted.keys[revertedIndex].inWeight, null, keyComparer);
            }
            if (Cx.keys.Length % 2 != 0)
            {
                int middlePointIndex = Cx.keys.Length / 2;

                Assert.AreEqual(Cx.keys[middlePointIndex].time, -CxInverted.keys[middlePointIndex].time, null, keyComparer);
                Assert.AreEqual(Cx.keys[middlePointIndex].value, CxInverted.keys[middlePointIndex].value, null, keyComparer);

                Assert.AreEqual(Cx.keys[middlePointIndex].inTangent, -CxInverted.keys[middlePointIndex].outTangent, null, keyComparer);
                Assert.AreEqual(Cx.keys[middlePointIndex].outTangent, -CxInverted.keys[middlePointIndex].inTangent, null, keyComparer);

                Assert.AreEqual(Cx.keys[middlePointIndex].weightedMode, CxInverted.keys[middlePointIndex].weightedMode, null);
                Assert.AreEqual(Cx.keys[middlePointIndex].inWeight, CxInverted.keys[middlePointIndex].outWeight, null, keyComparer);
                Assert.AreEqual(Cx.keys[middlePointIndex].outWeight, CxInverted.keys[middlePointIndex].inWeight, null, keyComparer);
            }
        }
    }
}
