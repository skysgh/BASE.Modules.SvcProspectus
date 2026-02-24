using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.DbContexts.Implementations.Base;
using App.Modules.Sys.Shared.Queries;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.Schema;
using App.Modules.Sys.Infrastructure.Domains.Persistence.Relational.EF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace App.Modules.SvcProspectus.Infrastructure.Data.EF.DbContexts.Implementations
{
	/// <summary>
	/// Database context for the SvcProspectus module.
	/// <para>
	/// Inherits from <see cref="ModuleDbContextBase"/> which provides:
	/// <list type="bullet">
	/// <item>Automatic schema configuration discovery via <c>IEntityTypeConfiguration</c>.</item>
	/// <item>Automatic seed data discovery via <c>IHasDataSeeder</c>.</item>
	/// <item>Pre-commit save interception for auditing and record state management.</item>
	/// </list>
	/// </para>
	/// </summary>
	/// <seealso cref="ModuleDbContextBase"/>
	public class ModuleDbContext : ModuleDbContextBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ModuleDbContext"/> class.
		/// </summary>
		/// <param name="options">The database context options.</param>
		/// <param name="modelBuilderOrchestrator">Orchestrator for schema and seed discovery.</param>
		/// <param name="dbContextPreCommitService">Pre-commit service for save interception.</param>
		/// <param name="loggerFactory">Logger factory for diagnostics.</param>
		/// <param name="watermarkContext">Optional watermark context for incremental sync filtering.</param>
		public ModuleDbContext(
			DbContextOptions<ModuleDbContext> options,
			IModelBuilderOrchestrator modelBuilderOrchestrator,
			IDbContextPreCommitService dbContextPreCommitService,
			ILoggerFactory loggerFactory,
			IWatermarkQueryContext? watermarkContext = null)
			: base(options,
				  modelBuilderOrchestrator,
				  dbContextPreCommitService,
				  watermarkContext: watermarkContext,
				  loggerFactory: loggerFactory)
		{
		}

		// DbSet properties will be added as domain entities are defined.
		// Entity configurations and seeders are discovered automatically
		// from this assembly by the ModelBuilderOrchestrator.
	}
}