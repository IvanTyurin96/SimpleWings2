namespace Assets.Scripts.Craft.Parts.Modifiers
{
	using Assets.Scripts.Design;
	using ModApi;
	using ModApi.Craft.Parts;
	using ModApi.Craft.Parts.Attributes;
	using ModApi.Design.PartProperties;
	using System;
	using UnityEngine;

	[Serializable]
    [DesignerPartModifier("SimpleWing")]
    [PartModifierTypeId("SimpleWings2.SimpleWing")]
    public class SimpleWingData : PartModifierData<SimpleWingScript>
	{
		// WING
		[DesignerPropertySpinner(0f, 100f, 0.1f, Label = "Root leading offset, m", Header = "Geometry")]
		public float rootLeadingOffset = 1.0f;

		[DesignerPropertySpinner(0f, 100f, 0.1f, Label = "Root trailing offset, m")]
		public float rootTrailingOffset = 0.5f;

		[DesignerPropertySpinner(0f, 100f, 0.1f, Label = "Tip leading offset, m")]
		public float tipLeadingOffset = 0.5f;

		[DesignerPropertySpinner(0f, 100f, 0.1f, Label = "Tip trailing offset, m")]
		public float tipTrailingOffset = 0.5f;

		[DesignerPropertySpinner(0f, 100f, 0.1f, Label = "Length, m")]
		public float length = 1f;

		[DesignerPropertySpinner(-100f, 100f, 0.1f, Label = "Tip offset, m")]
		public float tipOffset = 0.0f;

		// AIRFOIL
		[DesignerPropertySpinner("NACA 0012", "Clark Y", "T-10 root", "T-10 wing", "NACA 64-208", Label = "Root type", Header = "Airfoil")]
		public string airfoilRootType = "NACA 0012";

		[DesignerPropertySpinner("NACA 0012", "Clark Y", "T-10 root", "T-10 wing", "NACA 64-208", Label = "Tip type")]
		public string airfoilTipType = "NACA 0012";

		[DesignerPropertySlider(1f, 24f, 24, Label = "Root thickness, %")]
		public float airfoilRootThickness = 12f;

		[DesignerPropertySlider(1f, 24f, 24, Label = "Tip thickness, %")]
		public float airfoilTipThickness = 12f;

		[DesignerPropertyToggleButton(Label = "Invert")]
		public bool isAirfoilInverted = false;

		// CONTROL SURFACE
		[DesignerPropertySlider(0f, 45, 46, Label = "Percentage, minimal = 7 %", Header = "Control surface", HeaderCollapsed = true)]
		public int CoSu_percentage = 0;

		[DesignerPropertySlider(0f, 45f, 46, Label = "Deflection angle, °")]
		public float CoSu_deflectionAngle = 25f;

		[DesignerPropertySlider(0f, 90f, 91, Label = "Rotation speed, °/s")]
		public float CoSu_rotationSpeed = 60f;

		[DesignerPropertySlider(0f, 10f, 41, Label = "Spacing, %")]
		public float CoSu_spacing = 1.0f;

		[DesignerPropertyToggleButton(Label = "Border rounded")]
		public bool CoSu_isBorderRounded = true;

		// LEADING EDGE
		[DesignerPropertySlider(0f, 40, 41, Label = "Percentage, %", Header = "Leading edge", HeaderCollapsed = true)]
		public int LeEd_percentage = 0;

		[DesignerPropertySlider(0f, 30f, 31, Label = "Deflection angle, °")]
		public float LeEd_deflectionAngle = 30f;

		[DesignerPropertySlider(0f, 90f, 91, Label = "Rotation speed, °/s")]
		public float LeEd_rotationSpeed = 60f;

		[DesignerPropertySlider(1f, 2f, 11, Label = "Sensitivity, °/AoA")]
		public float LeEd_angleOfAttackSensitivity = 1.0f;

		[DesignerPropertySlider(0f, 10f, 41, Label = "Spacing, %")]
		public float LeEd_spacing = 1.0f;

		[DesignerPropertyToggleButton(Label = "Border rounded")]
		public bool LeEd_isBorderRounded = true;

		[DesignerPropertySlider(1, 10, 10, Label = "Full deflect Activation Group")]
		public int LeEd_fullDeflectActivationGroup = 8;

		[DesignerPropertyToggleButton(IsHidden = true)]
		public bool LeEd_isAttachedToRoot = false;

		// WASHOUT
		[DesignerPropertySlider(-15f, 15f, 31, Label = "Washout angle, °", Header = "Washout", HeaderCollapsed = true)]
		public float washoutAngle = 0f;

		[DesignerPropertySlider(0f, 1f, 11, Label = "Relative point")]
		public float washoutRelativePoint = 0.5f;

		// AIRFOIL ROUNDING
		[DesignerPropertyToggleButton(Label = "Rounded", Header = "Airfoil rounding", HeaderCollapsed = true)]
		public bool isAirfoilRounded = false;

		[DesignerPropertySlider(0.1f, 3f, 30, Label = "Length")]
		public float airfoilRoundingLength = 1f;

		// PARTICLE SYSTEM TRAIL
		[DesignerPropertyToggleButton(Label = "Trail vortex", Header = "Particle system", HeaderCollapsed = true)]
		public bool TrVo_enabled = false;

		[DesignerPropertyToggleButton(IsHidden = true)]
		public bool TrVo_forOtherSideEnabled = true;

		[DesignerPropertyToggleButton(IsHidden = true)]
		public bool TrVo_inWorldSpaceEnabled = false;

		[DesignerPropertySlider(IsHidden = true)]
		public float TrVo_sizeMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float TrVo_lengthMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float TrVo_speedMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float TrVo_emissionMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float TrVo_randomAngleMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float TrVo_randomLengthMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public int TrVo_maxParticles = 3000;

        [DesignerPropertySlider(IsHidden = true)]
        public float TrVo_opacityMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float TrVo_growStartVisibilityAOA = 7.5f;

		[DesignerPropertySlider(IsHidden = true)]
		public float TrVo_growEndVisibilityAOA = 15f;

		[DesignerPropertySlider(IsHidden = true)]
		public float TrVo_fadeStartVisibilityAOA = 20f;

		[DesignerPropertySlider(IsHidden = true)]
		public float TrVo_fadeEndVisibilityAOA = 45f;

		[DesignerPropertySlider(IsHidden = true)]
		public float TrVo_minVisibilitySpeed = 27.7f; // 100 kmh

		[DesignerPropertySlider(IsHidden = true)]
		public float TrVo_maxVisibilitySpeed = 41.55f; // 150 kmh

        // PARTICLE SYSTEM LERX
        [DesignerPropertyToggleButton(Label = "LERX vortex")]
		public bool LxVo_enabled = false;

		[DesignerPropertyToggleButton(IsHidden = true)]
		public bool LxVo_forOtherSideEnabled = true;

		[DesignerPropertyToggleButton(IsHidden = true)]
		public bool LxVo_inWorldSpaceEnabled = false;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_sizeMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_randomSizeMultiplier = 0.25f;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_lengthMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_speedMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_emissionMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_randomAngleMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_angleOfAttackSensitivity = 0.5f;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_angleOfSlipSensitivity = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_maxAngleOfAttack = 60f;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_maxAngleOfSlip = 5f;

		[DesignerPropertySlider(IsHidden = true)]
		public int LxVo_maxParticles = 3000;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_opacityMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_growStartVisibilityAOA = 15f;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_growEndVisibilityAOA = 20f;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_fadeStartVisibilityAOA = 25f;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_fadeEndVisibilityAOA = 60f;

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_minVisibilitySpeed = 27.7f; // 100 kmh

		[DesignerPropertySlider(IsHidden = true)]
		public float LxVo_maxVisibilitySpeed = 41.55f; // 150 kmh

		// PARTICLE SYSTEM WING VORTEX
		[DesignerPropertyToggleButton(Label = "Wing vortex")]
		public bool WiVo_enabled = false;

		[DesignerPropertyToggleButton(IsHidden = true)]
		public bool WiVo_forOtherSideEnabled = true;

		[DesignerPropertySlider(IsHidden = true)]
		public float WiVo_sizeMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float WiVo_lengthMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float WiVo_speedMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float WiVo_emissionMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public int WiVo_maxParticles = 3000;

		[DesignerPropertySlider(IsHidden = true)]
		public float WiVo_opacityMultiplier = 1f;

		[DesignerPropertySlider(IsHidden = true)]
		public float WiVo_growStartVisibilityAOA = 15f;

		[DesignerPropertySlider(IsHidden = true)]
		public float WiVo_growEndVisibilityAOA = 20f;

		[DesignerPropertySlider(IsHidden = true)]
		public float WiVo_fadeStartVisibilityAOA = 25f;

		[DesignerPropertySlider(IsHidden = true)]
		public float WiVo_fadeEndVisibilityAOA = 90f;

		[DesignerPropertySlider(IsHidden = true)]
		public float WiVo_minVisibilitySpeed = 27.7f; // 100 kmh

		[DesignerPropertySlider(IsHidden = true)]
		public float WiVo_maxVisibilitySpeed = 41.55f; // 150 kmh

		// FLEX
		[DesignerPropertyToggleButton(Label = "Wing flex", Header = "Flex", HeaderCollapsed = true)]
        public bool wingFlexEnabled = false;

		[DesignerPropertySlider(IsHidden = true)]
		public float wingFlexRigidityMultiplier = 1f;

		// DESIGNER INPUT
		[DesignerPropertySlider(-30f, 30f, 61, Label = "Leading edge deflect, °", Header = "Designer deflect", HeaderCollapsed = true)]
		public float designerDeflectLeadingEdge = 0.0f;

		[DesignerPropertySlider(-45f, 45f, 91, Label = "Control surface deflect, °")]
		public float designerDeflectControlSurface = 0.0f;

		// ADDITIONAL OFFSETS
		[DesignerPropertySpinner(IsHidden = true)]
		public float additionalRootLeadingOffset = 0.0f;

		[DesignerPropertySpinner(IsHidden = true)]
		public float additionalRootTrailingOffset = 0.0f;

		[DesignerPropertySpinner(IsHidden = true)]
		public float additionalTipLeadingOffset = 0.0f;

		[DesignerPropertySpinner(IsHidden = true)]
		public float additionalTipTrailingOffset = 0.0f;

		[DesignerPropertySpinner(IsHidden = true)]
		public float additionalRootLeadingRelativePercentage = 25.0f;

		[DesignerPropertySpinner(IsHidden = true)]
		public float additionalRootTrailingRelativePercentage = 25.0f;

		[DesignerPropertySpinner(IsHidden = true)]
		public float additionalTipLeadingRelativePercentage = 25.0f;

		[DesignerPropertySpinner(IsHidden = true)]
		public float additionalTipTrailingRelativePercentage = 25.0f;

		// DECORATIONS
		[DesignerPropertyToggleButton(Label = "Actuator enabled", Header = "Decorations, Actuator", HeaderCollapsed = true)]
		public bool actuatorEnabled = false;

		[DesignerPropertyToggleButton(Label = "Inverted")]
		public bool isActuatorInverted = false;

		[DesignerPropertySlider(1f, 20f, 191, Label = "Scale")]
		public float actuatorScale = 2.0f;

		[DesignerPropertySlider(0f, 100f, 101, Label = "Length position")]
		public float actuatorLengthPosition = 50.0f;

		[DesignerPropertySlider(0f, 100f, 101, Label = "Chord position")]
		public float actuatorChordPosition = 50.0f;

		[DesignerPropertySpinner(-1000f, 1000f, 1f, Label = "Up offset")]
		public float actuatorUpOffset = 0.0f;

		[DesignerPropertySlider(1f, 10f, 91, Label = "Back lug offset")]
		public float actuatorBackLugOffset = 1.0f;

		[DesignerPropertySlider(0f, 30f, 301, Label = "Shell length")]
		public float actuatorShellLength = 5.0f;

		[DesignerPropertySlider(1f, 10f, 91, Label = "Front lug initial offset")]
		public float actuatorFrontLugInitialOffset = 5.0f;

		[DesignerPropertySlider(-45f, 45f, 91, Label = "Initial rotation, °")]
		public float actuatorInitialRotation = 0.0f;

		[DesignerPropertySlider(-45f, 45f, 91, Label = "Back mount initial rotation, °")]
		public float actuatorBackMountInitialRotation = 0.0f;

		[DesignerPropertySlider(-45f, 45f, 91, Label = "Front mount initial rotation, °")]
		public float actuatorFrontMountInitialRotation = 0.0f;

		[DesignerPropertySlider(0f, 10f, 101, Label = "Back mount length")]
		public float actuatorBackMountLength = 2.0f;

		[DesignerPropertySlider(0f, 10f, 101, Label = "Front mount length")]
		public float actuatorFrontMountLength = 2.0f;

		[DesignerPropertyToggleButton(Label = "Back mount visible")]
		public bool isActuatorBackMountVisible = true;

		[DesignerPropertyToggleButton(Label = "Front mount visible")]
		public bool isActuatorFrontMountVisible = true;

		[DesignerPropertyToggleButton(Label = "Cables visible")]
		public bool isActuatorCablesVisible = true;

		// DECORATIONS
		[DesignerPropertySlider(0f, 100f, 101, Label = "Fuel amount, %", Header = "Fuel", HeaderCollapsed = true)]
		public float fuelAmount = 0f;

		// HIDDEN
		[DesignerPropertyToggleButton(IsHidden = true)]
		public bool wingPhysicsEnabled = true;

		[DesignerPropertyToggleButton(IsHidden = true)]
		public bool wingDestructibilityEnabled = true;

		[DesignerPropertyToggleButton(IsHidden = true)]
		public bool transformRelativeRootEnabled = true;

		[DesignerPropertySpinner(IsHidden = true)]
		public float lerxSearchSphereRadius = 0.001f;

		[DesignerPropertySpinner(IsHidden = true)]
		public string lerxPartId = "auto";

		[DesignerPropertyToggleButton(IsHidden = true)]
		public bool autoAirfoilInvertEnabled = true;

		// MASS LOGIC
		private const float TwelveThicknessOneMeterAreaWingMass = 25f; // 25 kg = mass of the 12% thickness wing with 1 m^2 area
		private const float TwelveThickness = 12f;
		private const float ControlSurfaceMassMultiplierPerPercent = 0.4f; // 45% control surface will increase wing mass at 0.4 * 45% = 18%
		private const float LeadingEdgeMassMultiplierPerPercent = 0.3f; // 40% leading edge will increase wing mass at 0.3 * 40% = 12%
		public override float MassDry
		{
			get
			{
				const float WingMinimalValue = 0f;
				const float WingMinimalLength = 0.001f;
				float clampedRootLeadingOffset = Mathf.Clamp(rootLeadingOffset, WingMinimalValue, Mathf.Infinity);
				float clampedRootTrailingOffset = Mathf.Clamp(rootTrailingOffset, WingMinimalValue, Mathf.Infinity);
				float clampedTipLeadingOffset = Mathf.Clamp(tipLeadingOffset, WingMinimalValue, Mathf.Infinity);
				float clampedTipTrailingOffset = Mathf.Clamp(tipTrailingOffset, WingMinimalValue, Mathf.Infinity);
				float clampedLength = Mathf.Clamp(length, WingMinimalLength, Mathf.Infinity);

                const float AirfoilMinimalThickness = 1f;
                const float AirfoilMaximalThickness = 24f;
				float clampedAirfoilRootThickness = Mathf.Clamp(airfoilRootThickness, AirfoilMinimalThickness, AirfoilMaximalThickness);
				float clampedAirfoilTipThickness = Mathf.Clamp(airfoilTipThickness, AirfoilMinimalThickness, AirfoilMaximalThickness);
				float factWingRootLength = clampedRootLeadingOffset + clampedRootTrailingOffset;
				float factWingTipLength = clampedTipLeadingOffset + clampedTipTrailingOffset;
				float factAirfoilRootThickness = clampedAirfoilRootThickness / 100f * factWingRootLength;
				float factAirfoilTipThickness = clampedAirfoilTipThickness / 100f * factWingTipLength;
				float factMidThickness = (factAirfoilRootThickness + factAirfoilTipThickness) / 2f;

				float wingArea = (clampedRootLeadingOffset + clampedRootTrailingOffset + clampedTipLeadingOffset + clampedTipTrailingOffset) * clampedLength / 2f;

                const int ControlSurfaceMinimalPercentage = 7;
                const int ControlSurfaceMaximalPercentage = 45;
				float clampedControlSurfacePercentage = (float)Mathf.Clamp(CoSu_percentage, 0, ControlSurfaceMaximalPercentage);
				if (CoSu_percentage < ControlSurfaceMinimalPercentage)
				{
					clampedControlSurfacePercentage = 0f;
				}
				float controlSurfaceMassMultiplier = clampedControlSurfacePercentage * ControlSurfaceMassMultiplierPerPercent;

                const int LeadingEdgeMaximalPercentage = 40;
				float clampedLeadingEdgePercentage = (float)Mathf.Clamp(LeEd_percentage, 0, LeadingEdgeMaximalPercentage);
				float leadingEdgeMassMultiplier = clampedLeadingEdgePercentage * LeadingEdgeMassMultiplierPerPercent;

				float surfacesMassMultiplier = 1f + ((controlSurfaceMassMultiplier + leadingEdgeMassMultiplier) / 100f);

				float mass = wingArea * TwelveThicknessOneMeterAreaWingMass * surfacesMassMultiplier * factMidThickness / (TwelveThickness / 100f);

				return mass * Constants.MassScale;
			}
		}

		protected override void OnDesignerInitialization(IDesignerPartPropertiesModifierInterface d)
		{
			base.OnDesignerInitialization(d);

			d.OnAnyPropertyChanged(delegate
			{
				Symmetry.SynchronizePartModifiers(base.Script.PartScript);
				this.Script.InvokeAllStartFunctions();
			});
		}
	}
}