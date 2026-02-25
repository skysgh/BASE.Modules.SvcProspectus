namespace Tests.Modules.SvcProspectus.Quality.Helpers
{
	/// <summary>
	/// ISO-25010 quality attribute trait constants.
	/// </summary>
	public static class QualityTraits
	{
		/// <summary>The trait key used for quality attribute classifications.</summary>
		public const string Category = "Quality";

		/// <summary>ISO-25010 Product Quality Model characteristics.</summary>
		public static class Iso25010
		{
			/// <summary>Functional Suitability.</summary>
			public static class FunctionalSuitability
			{
				/// <summary>Degree to which functions cover all specified tasks.</summary>
				public const string Completeness = "FunctionalSuitability.Completeness";
				/// <summary>Degree to which functions provide correct results.</summary>
				public const string Correctness = "FunctionalSuitability.Correctness";
			}

			/// <summary>Reliability.</summary>
			public static class Reliability
			{
				/// <summary>Degree to which a system meets needs for reliability.</summary>
				public const string Maturity = "Reliability.Maturity";
			}

			/// <summary>Maintainability.</summary>
			public static class Maintainability
			{
				/// <summary>Degree to which a system is composed of discrete components.</summary>
				public const string Modularity = "Maintainability.Modularity";
				/// <summary>Degree to which impact of a change can be assessed.</summary>
				public const string Analysability = "Maintainability.Analysability";
			}
		}
	}
}
