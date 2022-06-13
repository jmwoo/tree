using System;
namespace lib.Models
{
	public class TreeNode
	{
		public int AccountId { get; set; }
		public int ParentAccountId { get; set; }
		public TreeNode? Parent { get; set; }
		public ICollection<TreeNode> Children { get; set; } = new List<TreeNode>();
	}
}