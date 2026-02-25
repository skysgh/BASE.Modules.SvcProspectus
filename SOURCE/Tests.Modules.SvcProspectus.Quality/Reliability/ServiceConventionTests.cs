using System.Reflection;
using Tests.Modules.SvcProspectus.Quality.Helpers;

namespace Tests.Modules.SvcProspectus.Quality.Reliability
{
	/// <summary>
	/// ISO-25010 Reliability.Maturity - DI Service Convention Enforcement.
	/// </summary>
	public class ServiceConventionTests
	{
		private static readonly string[] AppServiceSuffixes = ["AppService", "ApplicationService"];

		private static readonly HashSet<string> KnownFrameworkTypes =
		[
			"ILogger", "IConfiguration", "IOptions", "IHttpContextAccessor",
			"IWebHostEnvironment", "IHostEnvironment", "IServiceProvider",
			"IMediator", "IMapper", "IObjectMappingService",
		];

		private static readonly Type? AppServiceMarker = FindMarkerType("IHasApplicationService");
		private static readonly Type? ScopedLifecycleMarker = FindMarkerType("IHasScopedLifecycle");
		private static readonly Type? LifecycleMarker = FindMarkerType("IHasLifecycle");

		[Fact]
		[Trait(QualityTraits.Category, QualityTraits.Iso25010.Reliability.Maturity)]
		public void WhenAppServiceInterfaceDeclaredThenImplementsIHasApplicationService()
		{
			if (AppServiceMarker == null) { Assert.Fail("IHasApplicationService not found."); return; }

			var violations = AssemblyUnderTest.ApplicationAssemblies
				.SelectMany(AssemblyUnderTest.SafeGetTypes)
				.Where(t => t.IsInterface && t.IsPublic && IsAppServiceName(t.Name))
				.Where(iface => !AppServiceMarker.IsAssignableFrom(iface))
				.Select(iface => iface.FullName ?? iface.Name).ToList();

			Assert.True(violations.Count == 0,
				$"App service interfaces without IHasApplicationService ({violations.Count}):\n  " +
				string.Join("\n  ", violations));
		}

		[Fact]
		[Trait(QualityTraits.Category, QualityTraits.Iso25010.Reliability.Maturity)]
		public void WhenAppServiceImplementationExistsThenImplementsIHasScopedLifecycle()
		{
			if (AppServiceMarker == null || ScopedLifecycleMarker == null) { Assert.Fail("Marker types not found."); return; }

			var violations = AssemblyUnderTest.AllTypes
				.Where(t => !t.IsAbstract && !t.IsInterface)
				.Where(t => t.GetInterfaces().Any(i => AppServiceMarker.IsAssignableFrom(i)))
				.Where(impl => !ScopedLifecycleMarker.IsAssignableFrom(impl))
				.Select(impl => impl.FullName ?? impl.Name).ToList();

			Assert.True(violations.Count == 0,
				$"Implementations without IHasScopedLifecycle ({violations.Count}):\n  " +
				string.Join("\n  ", violations));
		}

		[Fact]
		[Trait(QualityTraits.Category, QualityTraits.Iso25010.FunctionalSuitability.Completeness)]
		public void WhenAppServiceInterfaceDeclaredThenHasConcreteImplementation()
		{
			if (AppServiceMarker == null) { Assert.Fail("IHasApplicationService not found."); return; }

			var markers = new HashSet<string> { "IHasScopedService", "IHasApplicationService" };
			var ifaces = AssemblyUnderTest.ApplicationAssemblies
				.SelectMany(AssemblyUnderTest.SafeGetTypes)
				.Where(t => t.IsInterface && t.IsPublic && AppServiceMarker.IsAssignableFrom(t))
				.Where(t => t != AppServiceMarker && !markers.Contains(t.Name)).ToList();

			var unimpl = ifaces
				.Where(i => !AssemblyUnderTest.AllConcreteTypes.Any(c => i.IsAssignableFrom(c)))
				.Select(i => i.FullName ?? i.Name).ToList();

			Assert.True(unimpl.Count == 0,
				$"Interfaces with no implementation ({unimpl.Count}):\n  " +
				string.Join("\n  ", unimpl));
		}

		[Fact]
		[Trait(QualityTraits.Category, QualityTraits.Iso25010.Reliability.Maturity)]
		public void WhenControllerInjectsInterfaceThenInterfaceIsResolvable()
		{
			if (LifecycleMarker == null) { Assert.Fail("IHasLifecycle not found."); return; }

			var controllers = AssemblyUnderTest.InterfaceAssemblies
				.SelectMany(AssemblyUnderTest.SafeGetTypes)
				.Where(t => !t.IsAbstract && IsControllerType(t)).ToList();

			var violations = new List<string>();
			foreach (var ctrl in controllers)
			{
				foreach (var ctor in ctrl.GetConstructors(BindingFlags.Public | BindingFlags.Instance))
				{
					foreach (var p in ctor.GetParameters())
					{
						if (!p.ParameterType.IsInterface || IsKnownFrameworkType(p.ParameterType)) { continue; }
						if (!LifecycleMarker.IsAssignableFrom(p.ParameterType))
						{
							violations.Add($"{ctrl.Name} -> {p.ParameterType.Name} (no IHasLifecycle)");
						}
					}
				}
			}

			Assert.True(violations.Count == 0,
				$"Controllers inject non-resolvable interfaces ({violations.Count}):\n  " +
				string.Join("\n  ", violations));
		}

		[Fact]
		[Trait(QualityTraits.Category, QualityTraits.Iso25010.Maintainability.Modularity)]
		public void WhenAppServiceImplementationExistsThenIsNotPublic()
		{
			if (AppServiceMarker == null) { Assert.Fail("IHasApplicationService not found."); return; }

			var violations = AssemblyUnderTest.AllTypes
				.Where(t => t.IsPublic && !t.IsAbstract && !t.IsInterface)
				.Where(t => t.GetInterfaces().Any(i => AppServiceMarker.IsAssignableFrom(i)))
				.Select(t => t.FullName ?? t.Name).ToList();

			Assert.True(violations.Count == 0,
				$"Public app service implementations ({violations.Count}). Should be internal:\n  " +
				string.Join("\n  ", violations));
		}

		private static bool IsAppServiceName(string name)
		{
			return name.StartsWith('I')
				&& AppServiceSuffixes.Any(s => name.EndsWith(s, StringComparison.Ordinal));
		}

		private static bool IsControllerType(Type type)
		{
			var current = type.BaseType;
			while (current != null)
			{
				if (current.Name is "ControllerBase" or "Controller") { return true; }
				current = current.BaseType;
			}
			return false;
		}

		private static bool IsKnownFrameworkType(Type paramType)
		{
			var name = paramType.IsGenericType
				? paramType.Name[..paramType.Name.IndexOf('`')]
				: paramType.Name;
			return KnownFrameworkTypes.Contains(name);
		}

		private static Type? FindMarkerType(string interfaceName)
		{
			var sub = AppDomain.CurrentDomain.GetAssemblies()
				.FirstOrDefault(a => { var n = a.GetName().Name; return n != null && n.Contains("Substrate", StringComparison.OrdinalIgnoreCase) && n.Contains("Sys", StringComparison.OrdinalIgnoreCase); });

			if (sub == null)
			{
				App.AssemblyDiscoveryExtensions.PreloadModuleAssembliesFromDisk();
				sub = AppDomain.CurrentDomain.GetAssemblies()
					.FirstOrDefault(a => { var n = a.GetName().Name; return n != null && n.Contains("Substrate", StringComparison.OrdinalIgnoreCase) && n.Contains("Sys", StringComparison.OrdinalIgnoreCase); });
			}

			if (sub == null) { return null; }
			try { return sub.GetTypes().FirstOrDefault(t => t.IsInterface && t.IsPublic && t.Name == interfaceName); }
			catch (ReflectionTypeLoadException ex) { return ex.Types.Where(t => t != null).FirstOrDefault(t => t!.IsInterface && t.IsPublic && t.Name == interfaceName); }
		}
	}
}

