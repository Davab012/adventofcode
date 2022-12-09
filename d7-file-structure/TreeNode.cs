using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace d7_file_structure
{
    internal class TreeNode
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public List<TreeNode> Children { get; set; }
        public TreeNode Parent { get; set; }

        public TreeNode(string name)
        {
            Name = name;
            Children = new List<TreeNode>();
        }

        public void AddChild(TreeNode child)
        {
            Children.Add(child);
            child.Parent = this;
        }

        public void Print(int indent = 0)
        {
            Console.WriteLine(new string(' ', indent) + Name + ":" + Size + " Total size: " + GetTotalSize());
            foreach (var child in Children)
            {
                child.Print(indent + 2);
            }
        }

        public string GetPath()
        {
            var path = new StringBuilder();
            var currNode = this;
            while (currNode != null)
            {
                path.Insert(0, currNode.Name + "/");
                currNode = currNode.Parent;
            }
            return path.ToString();
        }

        public int GetTotalSize()
        {
            var total = Size;
            foreach (var child in Children)
            {
                total += child.GetTotalSize();
            }
            return total;
        }

        internal List<TreeNode> GetNodesWithSizeLessThan(int v)
        {
            var nodes = new List<TreeNode>();
            if (GetTotalSize() <= v)
            {
                nodes.Add(this);
            }
            foreach (var child in Children)
            {
                nodes.AddRange(child.GetNodesWithSizeLessThan(v));
            }
            return nodes;
        }

        internal List<TreeNode> GetNodesWithSizeMoreThan(int neededSpace)
        {
            var nodes = new List<TreeNode>();
            if (GetTotalSize() >= neededSpace)
            {
                nodes.Add(this);
            }
            foreach (var child in Children)
            {
                nodes.AddRange(child.GetNodesWithSizeMoreThan(neededSpace));
            }
            return nodes;
        }
    }
}
