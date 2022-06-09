using System;
using lib.Models;

namespace lib.Services;

public class TreeFactory
{
	public static Tree BuildTree(IEnumerable<TreeNode> nodes)
	{
		Dictionary<int, List<TreeNode>> nodesGroupedByUpline = nodes
			.GroupBy(a => a.UplineAssociateId)
			.ToDictionary(a => a.Key, a => a.ToList());

		TreeNode root = nodes.Single(a => a.UplineAssociateId == 0);

		Dictionary<int, TreeNode> nodesDict = new();

		BuildTree_Recurse(root, nodesGroupedByUpline, nodesDict, new HashSet<int>());

		return new Tree(root, nodesDict);
	}

	public static void BuildTree_Recurse(TreeNode node, Dictionary<int, List<TreeNode>> nodesGroupedByUpline, Dictionary<int, TreeNode> nodesDict, HashSet<int> alreadyTraversed)
	{
		List<TreeNode> children = nodesGroupedByUpline.ContainsKey(node.AssociateId)
			? nodesGroupedByUpline[node.AssociateId]
			: Array.Empty<TreeNode>().ToList();

		alreadyTraversed.Add(node.AssociateId);

		foreach (TreeNode child in children.Where(c => !alreadyTraversed.Contains(c.AssociateId)))
		{
			BuildTree_Recurse(child, nodesGroupedByUpline, nodesDict, alreadyTraversed);
			child.UplineAssociateId = node.AssociateId;
			child.UplineTreeNode = node;
		}

		node.ChildTreeNodes = children;
		nodesDict[node.AssociateId] = node;
	}
}

