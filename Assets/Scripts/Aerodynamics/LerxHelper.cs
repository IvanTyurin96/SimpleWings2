using Assets.Scripts.Craft.Parts.Modifiers;

namespace Assets.Scripts.Aerodynamics
{
	public static class LerxHelper
	{
		public static float CalculateLerxCoverageMultiplier(AirfoilType airfoilRootType, AirfoilType airfoilTipType)
		{
			return (AirfoilCalculator.GetLerxCoverageMultiplier(airfoilRootType)
				+ AirfoilCalculator.GetLerxCoverageMultiplier(airfoilTipType)) / 2f;
		}

		public static float CalculateLerxCriticalAngleRaise(AirfoilType airfoilRootType, AirfoilType airfoilTipType)
		{
			return (AirfoilCalculator.GetLerxCriticalAngleRaise(airfoilRootType)
				+ AirfoilCalculator.GetLerxCriticalAngleRaise(airfoilTipType)) / 2f;
		}

		public static float CalculateLerxEfficiencyAsymmetryMultiplier(AirfoilType airfoilRootType, AirfoilType airfoilTipType)
		{
			return (AirfoilCalculator.GetLerxEfficiencyAsymmetryMultiplier(airfoilRootType)
				+ AirfoilCalculator.GetLerxEfficiencyAsymmetryMultiplier(airfoilTipType)) / 2f;
		}
	}
}
