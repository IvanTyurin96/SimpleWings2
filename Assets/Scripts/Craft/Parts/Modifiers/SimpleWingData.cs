namespace Assets.Scripts.Craft.Parts.Modifiers
{
	using Assets.Scripts.Design;
	using ModApi.Craft.Parts;
	using ModApi.Craft.Parts.Attributes;
	using ModApi.Design.PartProperties;
	using System;

	[Serializable]
    [DesignerPartModifier("SimpleWing")]
    [PartModifierTypeId("SimpleWings2.SimpleWing")]
    public class SimpleWingData : PartModifierData<SimpleWingScript>
	{
		// AIRFOIL
		[DesignerPropertySpinner("NACA 0012", "Clark Y", "T-10 root", "T-10 wing", "NACA 64-208", Label = "Root type", Header = "Airfoil")]
		public string airfoilRootType = "NACA 0012";

		[DesignerPropertySpinner("NACA 0012", "Clark Y", "T-10 root", "T-10 wing", "NACA 64-208", Label = "Tip type")]
		public string airfoilTipType = "NACA 0012";

		[DesignerPropertySlider(1f, 24f, 47, Label = "Root thickness, %")]
		public float airfoilRootThickness = 12f;

		[DesignerPropertySlider(1f, 24f, 47, Label = "Tip thickness, %")]
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

		// RESHAPE
		[DesignerPropertySlider(-0.5f, 0.5f, 21, Label = "Root reshape", Header = "Reshape", HeaderCollapsed = true)]
		public float rootReshape = 0f;

		[DesignerPropertySlider(-0.5f, 0.5f, 21, Label = "Tip reshape")]
		public float tipReshape = 0f;

		// HIDDEN
		[DesignerPropertyToggleButton(IsHidden = true)]
		public bool wingPhysicsEnabled = true;

		[DesignerPropertySpinner(IsHidden = true)]
		public float lerxSearchSphereRadius = 0.01f;

		[DesignerPropertySpinner(IsHidden = true)]
		public string lerxPartId = "auto";

		[DesignerPropertyToggleButton(IsHidden = true)]
		public bool autoAirfoilInvertEnabled = true;

		[DesignerPropertyToggleButton(IsHidden = true)]
		public bool overrideStockMeshInDesignerEnabled = true;

		protected override void OnDesignerInitialization(IDesignerPartPropertiesModifierInterface d)
		{
			base.OnDesignerInitialization(d);

			d.OnAnyPropertyChanged(delegate
			{
				Symmetry.SynchronizePartModifiers(base.Script.PartScript);
				this.Script.Start();
			});

			d.OnPartMaterialsChanged(delegate
			{
				this.Script.ApplyStockMaterialToAirfoil();
			});
		}
	}
}