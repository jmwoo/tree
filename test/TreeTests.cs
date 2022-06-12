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
				new TreeNode { AssociateId = 1, UplineAssociateId = 0 },
				new TreeNode { AssociateId = 2, UplineAssociateId = 1 },
				new TreeNode { AssociateId = 3, UplineAssociateId = 2 },
				new TreeNode { AssociateId = 4, UplineAssociateId = 2 },
				new TreeNode { AssociateId = 5, UplineAssociateId = 3 },
				new TreeNode { AssociateId = 6, UplineAssociateId = 3 },
			});

			// basics
			Assert.NotNull(tree);
			Assert.NotNull(tree.Root);
			Assert.Equal(0, tree.Root.UplineAssociateId);
			Assert.Equal(1, tree.Root.AssociateId);

			// children
			Assert.Equal(1, tree.Nodes[1].ChildTreeNodes.Count);
			Assert.Equal(2, tree.Nodes[2].ChildTreeNodes.Count);
			Assert.Equal(2, tree.Nodes[3].ChildTreeNodes.Count);

			// depth
			//Assert.Equal(1, tree.Nodes[1].ChildTreeNodes.Count);
		}
	}

	public class TreeTestCase
	{
		public ITree Tree { get; }
		public List<Func<bool>> Assertions { get; } = new();
	}
}

