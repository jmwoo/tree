using lib.Config;
using lib.Models;
using lib.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((_, services) =>
		services.ConfigureLibServices())
	.Build();

Console.WriteLine("hello worold");
