using System.Diagnostics;
using lib.Config;
using lib.Models;
using lib.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((_, services) =>
		services.ConfigureLibServices())
	.Build();

var stopwatch = new Stopwatch();

stopwatch.Restart(); stopwatch.Restart();
List<TreeNode> nodes = TreeFactory.GetNodeSetN(1000000);
stopwatch.Stop();
Console.WriteLine($"t: {stopwatch.Elapsed.TotalSeconds}");

stopwatch.Restart();
var r_tree = TreeFactory.BuildTree(nodes, TreeBuildMethod.Recursive);
stopwatch.Stop();
Console.WriteLine($"r: {stopwatch.Elapsed.TotalSeconds}");

stopwatch.Restart();
var i_tree = TreeFactory.BuildTree(nodes, TreeBuildMethod.Iterative);
stopwatch.Stop();
Console.WriteLine($"i: {stopwatch.Elapsed.TotalSeconds}");
