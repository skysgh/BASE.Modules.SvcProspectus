using System.Reflection;

namespace Tests.Quality.SvcProspectus.Helpers
{
	/// <summary>
	/// Provides reflection-based access to all SvcProspectus module assemblies
	/// for quality proof tests.
	/// </summary>
	public static class AssemblyUnderTest
	{
		/// <summary>
		/// All SvcProspectus module production assemblies (non-test).
		/// </summary>
		public static IReadOnlyList<Assembly> AllAssemblies { get; } = LoadAll();

		/// <summary>
		/// All public types across all assemblies.
		/// </summary>
		public static IReadOnlyList<Type> AllPublicTypes { get; } =
			AllAssemblies
				.SelectMany(SafeGetTypes)
				.Where(t => t.IsPublic)
				.ToList();

		/// <summary>
		/// All enum types across all assemblies.
		/// </summary>
		public static IReadOnlyList<Type> AllEnums { get; } =
			AllAssemblies
				.SelectMany(SafeGetTypes)
				.Where(t => t.IsEnum && !t.IsNested)
				.Distinct()
				.ToList();

		private static List<Assembly> LoadAll()
		{
			// Force-load referenced assemblies by scanning the test assembly's references.
			var testAssembly = typeof(AssemblyUnderTest).Assembly;
			foreach (var refName in testAssembly.GetReferencedAssemblies())
			{
				if (refName.Name != null && refName.Name.Contains("App.Modules.SvcProspectus", StringComparison.OrdinalIgnoreCase))
				{
					try
					{
						Assembly.Load(refName);
					}
					catch
					{
						// Some assemblies may not load in test context.
					}
				}
			}

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

		private static Type[] SafeGetTypes(Assembly a)
		{
			try
			{
				return a.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				return ex.Types.Where(t => t != null).Cast<Type>().ToArray();
			}
		}
	}
}
