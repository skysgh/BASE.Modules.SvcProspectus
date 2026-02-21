using App.Modules.Sys.Infrastructure.Domains.Constants;

namespace App.Modules.SvcProspectus.Substrate
{
    /// <summary>
    /// Constants specific to this 
    /// Logical Module
    /// </summary>
    public class ModuleConstants
    {
        /// <summary>
        /// Unique displayable Name to identify this logical module.
        /// </summary>
        public const string Name = "Prospectus";

        /// <summary>
        /// Unique Key to use as DbSchema prefix
        /// and api route fragment for this Logical Module.
        /// (lower case and url friendly).
        /// </summary>
        public const string Key = "prospectus";

        /// <summary>
        /// Database schema key for this module.
        /// </summary>
        public const string DbSchemaKey = Key;

        /// <summary>
        /// The name of the ConnectionString
        /// in app settings
        /// (it's the same as the Base one).
        /// </summary>
        public const string DbConnectionName = AppConstants.DbConnectionStringKey;

        /// <summary>
        /// The display name of the module.
        /// </summary>
        public const string Title = Name;

        /// <summary>
        /// The description of the module.
        /// </summary>
        public const string Description = "Manages features, comments, faqs, endorsement, and pricing.";
    }
}
