using System;
using lib.Models;

namespace lib.Services;

public static class TreeExtensions
{
	public static List<TreeNode> GetChildren(this Dictionary<int, List<TreeNode>> nodeToChildren, TreeNode node, HashSet<TreeNode>? alreadyVisited = null)
	{
		List<TreeNode> children = nodeToChildren.ContainsKey(node.AccountId)
			? nodeToChildren[node.AccountId]
			: Array.Empty<TreeNode>().ToList();

		if (alreadyVisited != null && children.Any())
		{
			children = children.Where(a => !alreadyVisited.Contains(a)).ToList();
		}

		return children;
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

		TreeNode root = nodes.Single(a => a.ParentAccountId == 0);

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

	public static void BuildTree_Iterative(TreeNode? root, Dictionary<int, List<TreeNode>> uplineToChildren)
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

			foreach (TreeNode child in uplineToChildren.GetChildren(node, alreadyVisited))
			{
				traversal.Push(child);
			}
		}

		while (order.Any())
		{
			TreeNode node = order.Pop();

			List<TreeNode> children = uplineToChildren.GetChildren(node);

			foreach (TreeNode child in children)
			{
				child.ParentAccountId = node.AccountId;
				child.Parent = node;
			}

			node.Children = children;
		}
	}
}

