using System;
namespace lib.Models
{
	public class TreeNode
	{
		public int AccountId { get; set; }
		public int ParentAccountId { get; set; }
		public TreeNode? Parent { get; set; }
		public List<TreeNode> Children { get; set; } = new();
	}
}