namespace App.Modules.SvcProspectus.Shared.Constants
{
	/// <summary>
	/// Constants specific to the Service Prospectus logical module.
	/// </summary>
	public static class SvcProspectusModuleConstants
	{
		/// <summary>
		/// Unique key to identify this logical module (lowercase, URL-safe).
		/// </summary>
		public const string Key = "svcprospectus";

		/// <summary>
		/// The display name of this module.
		/// </summary>
		public const string DisplayName = "Service Prospectus";

		/// <summary>
		/// Description of this module's purpose.
		/// </summary>
		public const string Description = "Manages service catalogues, offerings, and prospectus definitions.";

		/// <summary>
		/// Database schema key for this module.
		/// Used as the default schema prefix in EF configurations.
		/// </summary>
		public const string DbSchemaKey = Key;
	}
}