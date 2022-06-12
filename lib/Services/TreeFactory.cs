using System;
using lib.Models;

namespace lib.Services;

public static class TreeExtensions
{
	public static Dictionary<int, List<TreeNode>> GetUplineToChildren(this IEnumerable<TreeNode> nodes)
	{
		return nodes
			.GroupBy(a => a.UplineAssociateId)
			.ToDictionary(a => a.Key, a => a.ToList());
	}

	public static List<TreeNode> GetChildren(this Dictionary<int, List<TreeNode>> nodeToChildren, TreeNode node, HashSet<TreeNode>? alreadyVisited = null)
	{
		var children = nodeToChildren.ContainsKey(node.AssociateId)
			? nodeToChildren[node.AssociateId]
			: Array.Empty<TreeNode>().ToList();

		if (alreadyVisited != null)
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
		Dictionary<int, TreeNode> idToNode = nodes.ToDictionary(a => a.AssociateId);

		Dictionary<int, List<TreeNode>> uplineToChildren = nodes.GetUplineToChildren();

		TreeNode root = nodes.Single(a => a.UplineAssociateId == 0);

		BuildTree_Iterative(root, uplineToChildren);

		return new Tree(root, idToNode);
	}

	public static void BuildTree_Recurse(TreeNode node, Dictionary<int, List<TreeNode>> uplineToChildren, HashSet<int> alreadyTraversed)
	{
		List<TreeNode> children = uplineToChildren.ContainsKey(node.AssociateId)
			? uplineToChildren[node.AssociateId]
			: Array.Empty<TreeNode>().ToList();

		alreadyTraversed.Add(node.AssociateId);

		foreach (TreeNode child in children.Where(c => !alreadyTraversed.Contains(c.AssociateId)))
		{
			BuildTree_Recurse(child, uplineToChildren, alreadyTraversed);
			child.UplineAssociateId = node.AssociateId;
			child.UplineTreeNode = node;
		}

		node.ChildTreeNodes = children;
	}

	public static void BuildTree_Iterative(TreeNode root, Dictionary<int, List<TreeNode>> uplineToChildren)
	{
		Stack<TreeNode> traversal = new();
		traversal.Push(root);

		Stack<TreeNode> order = new();
		HashSet<TreeNode> alreadyVisited = new();

		List<TreeNode> GetChildren(TreeNode node) => uplineToChildren.ContainsKey(node.AssociateId)
				? uplineToChildren[node.AssociateId]
				: Array.Empty<TreeNode>().ToList();

		while (traversal.Any())
		{
			TreeNode node = traversal.Pop();
			order.Push(node);

			foreach  (TreeNode child in GetChildren(node))
			{
				traversal.Push(child);
			}
		}

		while (order.Any())
		{
			TreeNode node = order.Pop();

			List<TreeNode> children = GetChildren(node);

			foreach (TreeNode child in children)
			{
				child.UplineAssociateId = node.AssociateId;
				child.UplineTreeNode = node;
			}

			node.ChildTreeNodes = children;
		}
	}
}

