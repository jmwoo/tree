using lib.Services;
using Microsoft.Extensions.DependencyInjection;

namespace lib.Config;

public static class LibServiceConfig
{
	public static IServiceCollection ConfigureLibServices(this IServiceCollection services)
	{
		services.AddSingleton<ITestService, TestService>();

		return services;
	}
}
