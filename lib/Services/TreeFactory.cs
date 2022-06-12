using System;
using lib.Models;

namespace lib.Services;

public static class TreeExtensions
{
	public static List<TreeNode> GetChildren(this Dictionary<int, List<TreeNode>> nodeToChildren, TreeNode node, HashSet<TreeNode>? alreadyVisited = null)
	{
		List<TreeNode> children = nodeToChildren.ContainsKey(node.AssociateId)
			? nodeToChildren[node.AssociateId]
			: Array.Empty<TreeNode>().ToList();

		if (alreadyVisited != null && children.Any())
		{
			children = children.Where(a => !alreadyVisited.Contains(a)).ToList();
		}

		return children;
	}
}

public class TreeFactory
{
	public static Tree BuildTree(IEnumerable<TreeNode> nodes)
	{
		if (nodes == null || !nodes.Any())
		{
			throw new InvalidTreeException("nodes is null or empty");
		}

		Dictionary<int, TreeNode> idToNode = nodes.ToDictionary(a => a.AssociateId);

		Dictionary<int, List<TreeNode>> uplineToChildren = nodes
			.GroupBy(a => a.UplineAssociateId)
			.ToDictionary(a => a.Key, a => a.ToList());

		List<TreeNode> roots = nodes
			.Where(a => a.UplineAssociateId == 0)
			.ToList();

		if (!roots.Any())
		{
			throw new InvalidTreeException("no root found");
		}

		if (roots.Count > 1)
		{
			throw new InvalidTreeException("more than one root");
		}

		TreeNode root = roots.Single();

		BuildTree_Iterative(root, uplineToChildren);

		return new Tree(root, idToNode);
	}

	public static void BuildTree_Recurse(TreeNode node, Dictionary<int, List<TreeNode>> uplineToChildren, HashSet<TreeNode> alreadyTraversed)
	{
		if (node == null)
		{
			return;
		}

		List<TreeNode> children = uplineToChildren.GetChildren(node, alreadyTraversed);

		foreach (TreeNode child in children)
		{
			BuildTree_Recurse(child, uplineToChildren, alreadyTraversed);
			child.UplineAssociateId = node.AssociateId;
			child.UplineTreeNode = node;
		}

		alreadyTraversed.Add(node);

		node.ChildTreeNodes = children;
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
				child.UplineAssociateId = node.AssociateId;
				child.UplineTreeNode = node;
			}

			node.ChildTreeNodes = children;
		}
	}
}

