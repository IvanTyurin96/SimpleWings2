namespace Assets.Scripts
{
    using ModApi.Settings.Core;

    /// <summary>
    /// The settings for the mod.
    /// </summary>
    /// <seealso cref="ModApi.Settings.Core.SettingsCategory{Assets.Scripts.ModSettings}" />
    public class ModSettings : SettingsCategory<ModSettings>
    {
        /// <summary>
        /// The mod settings instance.
        /// </summary>
        private static ModSettings _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModSettings"/> class.
        /// </summary>
        public ModSettings() : base("SimpleWings2")
        {
        }

        /// <summary>
        /// Gets the mod settings instance.
        /// </summary>
        /// <value>
        /// The mod settings instance.
        /// </value>
        public static ModSettings Instance => _instance ?? (_instance = Game.Instance.Settings.ModSettings.GetCategory<ModSettings>());

		/// <summary>
		/// If enabled, shows velocity, lift, drag and wave drag vectors.
		/// </summary>
		public BoolSetting Debug { get; private set; }

		//-------------------------------------------------------------------------------------
		/// <summary>
		/// Length scale of velocity vector in Debug mode.
		/// </summary>
		public NumericSetting<float> VelocityVectorLengthScale { get; private set; }

		/// <summary>
		/// Length scale of force vectors in Debug mode.
		/// </summary>
		public NumericSetting<float> ForceVectorsLengthScale { get; private set; }
		//-------------------------------------------------------------------------------------

		//-------------------------------------------------------------------------------------
		/// <summary>
		/// If enabled, shows the SimpleWingData window.
		/// </summary>
		public BoolSetting SimpleWingDataVisible { get; private set; }

		/// <summary>
		/// Page of SimpleWingData window. 1 - Main, 2- LiftDrag.
		/// </summary>
		public NumericSetting<int> SimpleWingDataPage { get; private set; }

		/// <summary>
		/// Size of SimpleWingData window.
		/// </summary>
		public NumericSetting<float> SimpleWingDataSize { get; private set; }

		/// <summary>
		/// Screen position of SimpleWingData window.
		/// </summary>
		public NumericSetting<int> SimpleWingDataPosition { get; private set; }

		/// <summary>
		/// Point count for curves in SimpleWingData window. Needs to restart the game to apply changes.
		/// </summary>
		public NumericSetting<int> SimpleWingDataCurvePointCount { get; private set; }

		/// <summary>
		/// Angle range in SimpleWingData window at Lift & Drag page.
		/// </summary>
		public NumericSetting<int> SimpleWingData_LiftDragPage_AngleRange { get; private set; }

		/// <summary>
		/// Coefficient range in SimpleWingData window at Lift & Drag page.
		/// </summary>
		public NumericSetting<float> SimpleWingData_LiftDragPage_CoefficientRange { get; private set; }

		/// <summary>
		/// Drag coefficient range in SimpleWingData window at Polar page.
		/// </summary>
		public NumericSetting<float> SimpleWingData_PolarPage_DragCoefficientRange { get; private set; }

		/// <summary>
		/// Lift coefficient range in SimpleWingData window at Polar page.
		/// </summary>
		public NumericSetting<float> SimpleWingData_PolarPage_LiftCoefficientRange { get; private set; }

		/// <summary>
		/// Angle of attack range for curve drawing in SimpleWingData window at Polar page.
		/// </summary>
		public NumericSetting<float> SimpleWingData_PolarPage_AngleOfAttackRange { get; private set; }

		/// <summary>
		/// If enabled, paint the text of Main page in SimpleWingData window.
		/// </summary>
		public BoolSetting SimpleWingDataMainPagePaint { get; private set; }
		//-------------------------------------------------------------------------------------

		/// <summary>
		/// Initializes the settings in the category.
		/// </summary>
		protected override void InitializeSettings()
        {
            this.Debug = this.CreateBool("Debug")
                .SetDescription("If enabled shows velocity and force vectors.")
                .SetDefault(false);

			//-------------------------------------------------------------------------------------
			this.VelocityVectorLengthScale = this.CreateNumeric<float>("Debug - Velocity vector length scale", 1f, 10f, 0.1f)
				.SetDescription("Length scale of velocity vector in Debug mode.")
				.SetDefault(5f);

			this.ForceVectorsLengthScale = this.CreateNumeric<float>("Debug - Force vectors length scale", 1f, 10f, 0.1f)
				.SetDescription("Length scale of force vectors in Debug mode.")
				.SetDefault(1f);
			//-------------------------------------------------------------------------------------

			//-------------------------------------------------------------------------------------
			this.SimpleWingDataVisible = this.CreateBool("Window visible")
				.SetDescription("If enabled shows the SimpleWingData window.")
				.SetDefault(false);

			this.SimpleWingDataPage = this.CreateNumeric<int>("Window page", 1, 4, 1)
				.SetDescription("Page of SimpleWingData window.")
				.SetDefault(1);

			this.SimpleWingDataSize = this.CreateNumeric<float>("Window size", 0.5f, 5f, 0.1f)
				.SetDescription("Size of SimpleWingData window.")
				.SetDefault(2f);

			this.SimpleWingDataPosition = this.CreateNumeric<int>("Window position", 1, 5, 1)
				.SetDescription("Screen position of SimpleWingData window.")
				.SetDefault(4);

			this.SimpleWingDataCurvePointCount = this.CreateNumeric<int>("Window curve point count", 500, 5000, 500)
				.SetDescription("Point count for curves in SimpleWingData window. Needs to restart the game to apply changes.")
				.SetDefault(1000);

			this.SimpleWingData_LiftDragPage_AngleRange = this.CreateNumeric<int>("Window - Lift Drag page - angle range", 40, 360, 10)
				.SetDescription("Angle range in SimpleWingData window at Lift & Drag page.")
				.SetDefault(360);

			this.SimpleWingData_LiftDragPage_CoefficientRange = this.CreateNumeric<float>("Window - Lift Drag page - coefficient range", 0.02f, 6f, 0.03f)
				.SetDescription("Coefficient range in SimpleWingData window at Lift & Drag page.")
				.SetDefault(6f);

			this.SimpleWingData_PolarPage_DragCoefficientRange = this.CreateNumeric<float>("Window - Polar page - drag coefficient range", 0.01f, 1.8f, 0.03f)
				.SetDescription("Drag coefficient range in SimpleWingData window at Polar page.")
				.SetDefault(0.5f);

			this.SimpleWingData_PolarPage_LiftCoefficientRange = this.CreateNumeric<float>("Window - Polar page - lift coefficient range", 0.02f, 6f, 0.03f)
				.SetDescription("Lift coefficient range in SimpleWingData window at Polar page.")
				.SetDefault(6f);

			this.SimpleWingData_PolarPage_AngleOfAttackRange = this.CreateNumeric<float>("Window - Polar page - angle of attack range", 10f, 180f, 10f)
				.SetDescription("Angle of attack range for curve drawing in SimpleWingData window at Polar page.")
				.SetDefault(60f);

			this.SimpleWingDataMainPagePaint = this.CreateBool("Window - Main page - text painting")
				.SetDescription("If enabled paint the text of Main page in SimpleWingData window.")
				.SetDefault(true);
			//-------------------------------------------------------------------------------------
		}
	}
}