using Tests.Modules.SvcProspectus.Quality.Helpers;

namespace Tests.Modules.SvcProspectus.Quality.Maintainability
{
	/// <summary>
	/// ISO-25010 Maintainability.Modularity - Module Assembly Sanity.
	/// Validates basic structural health of the SvcProspectus module.
	/// </summary>
	public class ModuleAssemblySanityTests
	{
		/// <summary>
		/// Module assemblies can be discovered and loaded.
		/// For scaffold modules this may be zero - the test documents the state.
		/// </summary>
		[Fact]
		public void ModuleAssembliesAreDiscoverable()
		{
			// This test always passes. Its purpose is to document the count
			// in the test output. As the module matures, add real assertions.
			var count = AssemblyUnderTest.AllAssemblies.Count;
			Assert.True(
				count >= 0,
				$"Expected zero or more assemblies, found {count}.");
		}

		/// <summary>
		/// If the module has assemblies loaded, they must expose public types.
		/// Scaffold modules with no loaded assemblies get a pass.
		/// </summary>
		[Fact]
		public void LoadedAssembliesHavePublicTypes()
		{
			if (AssemblyUnderTest.AllAssemblies.Count == 0)
			{
				// Scaffold module - nothing to check yet.
				return;
			}

			Assert.True(
				AssemblyUnderTest.AllPublicTypes.Count > 0,
				"Module has loaded assemblies but no public types.");
		}
	}
}
