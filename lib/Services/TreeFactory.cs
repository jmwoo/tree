using System;
using lib.Models;

namespace lib.Services;

public static class TreeExtensions
{
	public static List<TreeNode> GetChildren(this Dictionary<int, List<TreeNode>> nodeToChildren, TreeNode node, HashSet<TreeNode>? alreadyVisited = null)
	{
		IEnumerable<TreeNode> children = nodeToChildren.ContainsKey(node.AccountId)
			? nodeToChildren[node.AccountId]
			: Enumerable.Empty<TreeNode>();

		if (alreadyVisited != null && children.Any())
		{
			children = children.Where(a => !alreadyVisited.Contains(a));
		}

		return children.ToList();
	}
}

public enum TreeBuildMethod
{
	Recursive,
	Iterative
}

public class TreeFactory
{
	public static Tree BuildTree(IEnumerable<TreeNode> nodes, TreeBuildMethod treeBuildMethod)
	{
		if (nodes == null || !nodes.Any())
		{
			throw new InvalidTreeException("nodes is null or empty");
		}

		Dictionary<int, TreeNode> idToNode = nodes.ToDictionary(a => a.AccountId);

		Dictionary<int, List<TreeNode>> parentToChildren = nodes
			.GroupBy(a => a.ParentAccountId)
			.ToDictionary(a => a.Key, a => a.ToList());

		List<TreeNode> roots = nodes
			.Where(a => a.ParentAccountId == 0)
			.ToList();

		if (!roots.Any())
		{
			throw new InvalidTreeException("no root found");
		}

		if (roots.Count > 1)
		{
			throw new InvalidTreeException("more than one root found");
		}

		TreeNode root = roots.Single();

		if (treeBuildMethod == TreeBuildMethod.Iterative)
		{
			BuildTree_Iterative(root, parentToChildren);
		}
		else if (treeBuildMethod == TreeBuildMethod.Recursive)
		{
			BuildTree_Recursive(root, parentToChildren, new HashSet<TreeNode>());
		}

		return new Tree(root, idToNode);
	}

	public static void BuildTree_Recursive(TreeNode node, Dictionary<int, List<TreeNode>> parentToChildren, HashSet<TreeNode> alreadyVisited)
	{
		if (node == null)
		{
			return;
		}

		alreadyVisited.Add(node);

		List<TreeNode> children = parentToChildren.GetChildren(node, alreadyVisited);

		foreach (TreeNode child in children)
		{
			BuildTree_Recursive(child, parentToChildren, alreadyVisited);
			child.ParentAccountId = node.AccountId;
			child.Parent = node;
		}

		node.Children = children;
	}

	public static void BuildTree_Iterative(TreeNode? root, Dictionary<int, List<TreeNode>> parentToChildren)
	{
		if (root == null)
		{
			return;
		}

		Stack<TreeNode> traversal = new();
		traversal.Push(root);

		Stack<TreeNode> order = new();
		HashSet<TreeNode> alreadyVisited = new();

		while (traversal.Any())
		{
			TreeNode node = traversal.Pop();
			order.Push(node);

			alreadyVisited.Add(node);

			foreach (TreeNode child in parentToChildren.GetChildren(node, alreadyVisited))
			{
				traversal.Push(child);
			}
		}

		while (order.Any())
		{
			TreeNode node = order.Pop();

			List<TreeNode> children = parentToChildren.GetChildren(node);

			foreach (TreeNode child in children)
			{
				child.ParentAccountId = node.AccountId;
				child.Parent = node;
			}

			node.Children = children;
		}
	}

	public static List<TreeNode> GetNodeSet1()
	{
		return new()
		{
			new TreeNode { AccountId = 1, ParentAccountId = 0 },
			new TreeNode { AccountId = 2, ParentAccountId = 1 },
			new TreeNode { AccountId = 3, ParentAccountId = 2 },
			new TreeNode { AccountId = 4, ParentAccountId = 2 },
			new TreeNode { AccountId = 5, ParentAccountId = 3 },
			new TreeNode { AccountId = 6, ParentAccountId = 3 },
		};
	}

	public static List<TreeNode> GetNodeSetN(int size)
	{

		TreeNode root = new() { AccountId = 1, ParentAccountId = 0 };

		List<TreeNode> allNodes = new() { root };


		int chunkSize = 25;

		if (size < 5)
		{
			chunkSize = 1;
		}
		else if (size < 25)
		{
			chunkSize = 5;
		}

		foreach (int[] chunk in Enumerable.Range(2, size).Chunk(chunkSize))
		{
			List<TreeNode> newNodes = chunk
				.Select(i => new TreeNode { AccountId = i })
				.ToList();

			foreach (var newNode in newNodes)
			{
				newNode.ParentAccountId = allNodes[Random.Shared.Next(0, allNodes.Count)].AccountId;
			}

			allNodes.AddRange(newNodes);
		}

		return allNodes;
	}
}

