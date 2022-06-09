﻿using System;
using lib.Models;

namespace lib.Services;

public interface ITree
{
	public TreeNode Root { get; }
	public IDictionary<int, TreeNode> Nodes { get; }
}

public record Tree(TreeNode Root, IDictionary<int, TreeNode> Nodes) : ITree;

