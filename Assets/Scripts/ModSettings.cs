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

		/// <summary>
		/// Length scale of velocity vector in Debug mode.
		/// </summary>
		public NumericSetting<float> VelocityVectorLengthScale { get; private set; }

		/// <summary>
		/// Length scale of force vectors in Debug mode.
		/// </summary>
		public NumericSetting<float> ForceVectorsLengthScale { get; private set; }

		/// <summary>
		/// Width scale of force vectors in Debug mode.
		/// </summary>
		public NumericSetting<float> ForceVectorsWidthScale { get; private set; }

		/// <summary>
		/// Information output of SimpleWingData. 0 - Disabled, 1 - Lift data, 2 - Drag data, 3 - Wing data, 4 - LERX influence,
		/// 5 - Lift and Drag data, 6 - Wing data and LERX influence, 7 - All
		/// </summary>
		public NumericSetting<int> SimpleWingDataInformationOutput { get; private set; }

		/// <summary>
		/// Initializes the settings in the category.
		/// </summary>
		protected override void InitializeSettings()
        {
            this.Debug = this.CreateBool("Debug")
                .SetDescription("If enabled shows velocity and force vectors.")
                .SetDefault(false);

			this.VelocityVectorLengthScale = this.CreateNumeric<float>("Debug - Velocity vector length scale", 1f, 10f, 0.1f)
				.SetDescription("Length scale of velocity vector in Debug mode.")
				.SetDefault(1f);

			this.ForceVectorsLengthScale = this.CreateNumeric<float>("Debug - Force vectors length scale", 1f, 10f, 0.1f)
				.SetDescription("Length scale of force vectors in Debug mode.")
				.SetDefault(1f);

			this.ForceVectorsWidthScale = this.CreateNumeric<float>("Debug - Force vectors width scale", 0.25f, 2f, 0.25f)
				.SetDescription("Width scale of force vectors in Debug mode.")
				.SetDefault(1f);

			this.SimpleWingDataInformationOutput = this.CreateNumeric<int>("SimpleWingData information output", 0, 7, 1)
				.SetDescription("Information output of SimpleWingData. 0 - Disabled, 1 - Lift data, 2 - Drag data, 3 - Wing data, 4 - LERX influence, 5 - Lift and Drag data, 6 - Wing data and LERX influence, 7 - All.")
				.SetDefault(0);
		}
	}
}