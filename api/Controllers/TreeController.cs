using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using lib.Models;
using lib.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("")]
public class TreeController : ControllerBase
{
	readonly ILogger<TreeController> _logger;
	readonly ITree _tree;
	readonly IWebHostEnvironment _environment;
	readonly Stopwatch _stopwatch;

	public TreeController(ILogger<TreeController> logger, ITree tree, IWebHostEnvironment environment)
	{
		_logger = logger;
		_tree = tree;
		_environment = environment;
		_stopwatch = new();
	}

	public class TreeTestSet
	{
		public double AverageSecondsIterative { get; set; }
		public double AverageSecondsRecursive { get; set; }

		public TreeTestSet(IEnumerable<TreeTest> treeTests)
		{
			AverageSecondsIterative = treeTests.Select(a => a.ElapsedIterative.TotalSeconds).Average();
			AverageSecondsRecursive = treeTests.Select(a => a.ElapsedRecursive.TotalSeconds).Average();
		}

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public TreeBuildMethod Winner => AverageSecondsRecursive < AverageSecondsIterative
			? TreeBuildMethod.Recursive
			: TreeBuildMethod.Iterative;
	}

	public class TreeTest
	{
		public static TreeTest Get(IEnumerable<TreeNode> nodes)
		{
			var stopwatch = new Stopwatch();

			stopwatch.Start();
			var r_tree = TreeFactory.BuildTree(nodes, TreeBuildMethod.Recursive);
			stopwatch.Stop();
			var r_elapsed = stopwatch.Elapsed;

			stopwatch.Restart();
			var i_tree = TreeFactory.BuildTree(nodes, TreeBuildMethod.Recursive);
			stopwatch.Stop();
			var i_elapsed = stopwatch.Elapsed;

			return new TreeTest
			{
				NodeCount = nodes.Count(),
				ElapsedRecursive = r_elapsed,
				ElapsedIterative = i_elapsed
			};
		}

		public int NodeCount { get; set; }
		public TimeSpan ElapsedRecursive { get; set; }
		public TimeSpan ElapsedIterative { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public TreeBuildMethod Winner => ElapsedRecursive < ElapsedIterative
			? TreeBuildMethod.Recursive
			: TreeBuildMethod.Iterative;
	}

	[HttpGet("")]
	public string Get(int id = 1)
	{
		var options = new JsonSerializerOptions();
		options.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
		var obj = JsonSerializer.Serialize(_tree.Nodes[1], options: options);

		return obj;
		//return _tree.Nodes[1];
	}

	[HttpGet("build-test")]
	public TreeTest BuildTest(int size = 100)
	{
		var nodes = TreeFactory.GetNodeSetN(size <= 1 ? 0 : size - 1);
		var test = TreeTest.Get(nodes);
		return test;
	}

	[HttpGet("build-test-set")]
	public TreeTestSet BuildTestSet(int size = 100, int setSize = 10)
	{
		var tests = Enumerable.Range(0, setSize).Select(i =>
		{
			var nodes = TreeFactory.GetNodeSetN(size <= 1 ? 0 : size - 1);
			return TreeTest.Get(nodes);
		});
		var set = new TreeTestSet(tests);
		return set;
	}
}
