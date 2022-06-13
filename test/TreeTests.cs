using System;
using lib.Models;
using lib.Services;

namespace test
{
	public class TreeTests
	{
		[Fact]
		public void Test()
		{
			List<TreeNode> nodes = new()
			{
				new TreeNode { AccountId = 1, ParentAccountId = 0 },
				new TreeNode { AccountId = 2, ParentAccountId = 1 },
				new TreeNode { AccountId = 3, ParentAccountId = 2 },
				new TreeNode { AccountId = 4, ParentAccountId = 2 },
				new TreeNode { AccountId = 5, ParentAccountId = 3 },
				new TreeNode { AccountId = 6, ParentAccountId = 3 },
			};

			BuildAndTestTree(nodes, tree =>
			{
				Assert.Equal(1, tree.Nodes[1].Children.Count);
				Assert.Equal(2, tree.Nodes[2].Children.Count);
				Assert.Equal(2, tree.Nodes[3].Children.Count);
			});
		}

		public void BuildAndTestTree(List<TreeNode> nodes, Action<ITree> assertions)
		{
			new TreeTestCase(TreeFactory.BuildTree(nodes, TreeBuildMethod.Iterative), assertions);
			new TreeTestCase(TreeFactory.BuildTree(nodes, TreeBuildMethod.Recursive), assertions);
		}
	}

	public class TreeTestCase
	{
		public TreeTestCase(ITree tree, Action<ITree> assertions)
		{
			AssertFundamentals(tree);
			assertions.Invoke(tree);
		}

		private void AssertFundamentals(ITree tree)
		{
			Assert.NotNull(tree);
			Assert.NotNull(tree.Root);
			Assert.Equal(0, tree.Root.ParentAccountId);
			Assert.Equal(1, tree.Root.AccountId);
		}
	}
}

