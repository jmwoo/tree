using lib.Models;
using lib.Services;
using Microsoft.Extensions.DependencyInjection;

namespace lib.Config;

public static class LibServiceConfig
{
	public static IServiceCollection ConfigureLibServices(this IServiceCollection services)
	{
		List<TreeNode> nodes = TreeFactory.GetNodeSet1();
		services.AddSingleton<ITree>(TreeFactory.BuildTree(nodes, TreeBuildMethod.Iterative));

		return services;
	}
}
