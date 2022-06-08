using lib.Config;
using lib.Models;
using lib.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((_, services) =>
		services.ConfigureLibServices())
	.Build();

ITestService service = host.Services.GetService<ITestService>();

Test test = await service.GetTestAsync();
Console.WriteLine(test.Message);
