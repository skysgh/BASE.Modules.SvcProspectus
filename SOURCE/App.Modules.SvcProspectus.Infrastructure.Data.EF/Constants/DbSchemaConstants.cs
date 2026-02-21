namespace App.Modules.SvcProspectus.Infrastructure.Data.EF.Constants
{
    /// <summary>
    /// Database schema name constants for the Service Prospectus module.
    /// Organises tables into logical groups within this module's schema.
    /// </summary>
    /// <remarks>
    /// Use these constants in <c>IEntityTypeConfiguration</c> implementations
    /// instead of magic strings when specifying schema names.
    /// </remarks>
    public static class DbSchemaConstants
    {
        /// <summary>
        /// Default schema for this module (derived from module key).
        /// </summary>
        public const string Default = App.Modules.SvcProspectus.Substrate.ModuleConstants.Key;

        /// <summary>
        /// Schema for reference/lookup data tables.
        /// </summary>
        public const string ReferenceData = Default + "_ref";

        /// <summary>
        /// Schema for audit/history tables.
        /// </summary>
        public const string Audit = Default + "_audit";
    }
}