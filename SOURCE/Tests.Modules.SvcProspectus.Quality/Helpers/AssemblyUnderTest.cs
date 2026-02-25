using System.Reflection;

namespace Tests.Modules.SvcProspectus.Quality.Helpers
{
	/// <summary>
	/// Provides reflection-based access to all SvcProspectus module assemblies
	/// for quality proof tests.
	/// </summary>
	public static class AssemblyUnderTest
	{
		/// <summary>All SvcProspectus module production assemblies (non-test).</summary>
		public static IReadOnlyList<Assembly> AllAssemblies { get; } = LoadAll();

		/// <summary>Assemblies in the Application layer.</summary>
		public static IReadOnlyList<Assembly> ApplicationAssemblies { get; } =
			AllAssemblies.Where(a => GetName(a).Contains(".Application")).ToList();

		/// <summary>Assemblies in the Domain layer.</summary>
		public static IReadOnlyList<Assembly> DomainAssemblies { get; } =
			AllAssemblies.Where(a => GetName(a).Contains(".Domain")).ToList();

		/// <summary>Assemblies in the Infrastructure layer.</summary>
		public static IReadOnlyList<Assembly> InfrastructureAssemblies { get; } =
			AllAssemblies.Where(a => GetName(a).Contains(".Infrastructure")).ToList();

		/// <summary>Assemblies in the Interface layer (REST, OData, GraphQL, Web).</summary>
		public static IReadOnlyList<Assembly> InterfaceAssemblies { get; } =
			AllAssemblies.Where(a => GetName(a).Contains(".Interfaces")).ToList();

		/// <summary>All public types across all assemblies.</summary>
		public static IReadOnlyList<Type> AllPublicTypes { get; } =
			AllAssemblies.SelectMany(SafeGetTypes).Where(t => t.IsPublic).ToList();

		/// <summary>All types (public and non-public) across all module assemblies.</summary>
		public static IReadOnlyList<Type> AllTypes { get; } =
			AllAssemblies.SelectMany(SafeGetTypes).ToList();

		/// <summary>All concrete (non-abstract, non-interface) types. Includes internal.</summary>
		public static IReadOnlyList<Type> AllConcreteTypes { get; } =
			AllTypes.Where(t => !t.IsAbstract && !t.IsInterface).ToList();

		/// <summary>All enum types across all assemblies.</summary>
		public static IReadOnlyList<Type> AllEnums { get; } =
			AllAssemblies.SelectMany(SafeGetTypes)
				.Where(t => t.IsEnum && !t.IsNested).Distinct().ToList();

		private static string GetName(Assembly a) => a.GetName().Name ?? string.Empty;

		private static List<Assembly> LoadAll()
		{
			App.AssemblyDiscoveryExtensions.PreloadModuleAssembliesFromDisk();

			return AppDomain.CurrentDomain.GetAssemblies()
				.Where(a =>
				{
					var name = a.GetName().Name ?? string.Empty;
					return name.Contains("App.Modules.SvcProspectus", StringComparison.OrdinalIgnoreCase)
						&& !name.Contains("Tests", StringComparison.OrdinalIgnoreCase);
				})
				.OrderBy(a => a.GetName().Name)
				.ToList();
		}

		/// <summary>Safe type loader that handles ReflectionTypeLoadException.</summary>
		public static Type[] SafeGetTypes(Assembly assembly)
		{
			try { return assembly.GetTypes(); }
			catch (ReflectionTypeLoadException ex)
			{ return ex.Types.Where(t => t != null).ToArray()!; }
		}
	}
}
