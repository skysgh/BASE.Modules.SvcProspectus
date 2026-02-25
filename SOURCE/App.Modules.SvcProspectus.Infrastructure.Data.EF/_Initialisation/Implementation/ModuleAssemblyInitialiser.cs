using App.Modules.Sys.Initialisation.Implementation.Base;
using App.Modules.SvcProspectus.Substrate;
using App.Modules.SvcProspectus.Infrastructure.Data.EF.DbContexts.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace App.Modules.SvcProspectus.Infrastructure.Data.EF.Initialisation.Implementation
{
	/// <summary>
	/// Module assembly initialiser for the SvcProspectus EF data layer.
	/// Registers <see cref="ModuleDbContext"/> via the shared helper (ADR-006).
	/// </summary>
	public class ModuleAssemblyInitialiser : ModuleAssemblyInitialiserBase
	{
		/// <inheritdoc/>
		public override void DoBeforeBuild(IServiceCollection services)
		{
			services.AddModuleDbContext<ModuleDbContext>(ModuleConstants.DbSchemaKey);
		}

		/// <inheritdoc/>
		public override void DoAfterBuild(System.IServiceProvider serviceProvider)
		{
			serviceProvider.EnsureModuleSchemaAsync<ModuleDbContext>()
				.GetAwaiter().GetResult();
		}
	}
}