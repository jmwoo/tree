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
			ITree tree = TreeFactory.BuildTree(new List<TreeNode>()
			{
				new TreeNode { UplineAssociateId = 0, AssociateId = 1 },
				new TreeNode { UplineAssociateId = 1, AssociateId = 2 },
				new TreeNode { UplineAssociateId = 2, AssociateId = 3 },
				new TreeNode { UplineAssociateId = 2, AssociateId = 4 },
				new TreeNode { UplineAssociateId = 3, AssociateId = 5 },
				new TreeNode { UplineAssociateId = 3, AssociateId = 6 },
			});

			Assert.NotNull(tree);
			Assert.NotNull(tree.Root);
			Assert.Equal(0, tree.Root.UplineAssociateId);
			Assert.Equal(1, tree.Root.AssociateId);

			var node = tree.Nodes[2];
			Assert.Equal(2, node.ChildTreeNodes.Count);
		}
	}
}

