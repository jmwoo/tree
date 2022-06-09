using System;
namespace lib.Models
{
	public class TreeNode
	{
		public int UplineAssociateId { get; set; }
		public TreeNode? UplineTreeNode { get; set; }

		public int AssociateId { get; set; }

		public ICollection<TreeNode> ChildTreeNodes { get; set; } = new List<TreeNode>();
	}
}

