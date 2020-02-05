using System;
using System.Collections.Generic;
using System.Text;

namespace RedBlackTree
{
    public interface RBTreeRepository
    {
        public void Insert(int value);
        public void Delete(int value);
        public void MaxValue();
        public void MinValue();
        public void FindValue(int value);
        public void AllSubTreea();
        public void DisplayTree();
    }

	public class RBTree : RBTreeRepository
    {
        private Node root;
		public int height { get; private set; } = 0;
        public int treeRoot => root.data;
        public RBTree() { SetDefaultData(); }
        public void DisplayTree()
        {
            if (root == null)
            {
                Console.WriteLine("Nothing in the tree!");
                return;
            }
            if (root != null)
            {
                InOrderDisplay(root);
            }
        }
        public void MaxValue()
        {
            Node tempTree = root;
            while (tempTree.right != null)
            {
                tempTree = tempTree.right;
            }
            Console.WriteLine("Max value in tree => {0}\n", tempTree.data);
        }
        public void MinValue()
        {
            Node tempTree = root;
            while (tempTree.left != null)
            {
                tempTree = tempTree.left;
            }
            Console.WriteLine("Min value in tree => {0}\n", tempTree.data);
        }
        public void FindValue(int value)
        {
            bool isFound = false;
            var way = "";
            Node temp = root;
            while (!isFound)
            {
                if (value < temp.data)
                {
                    if (temp.left == null)
                    {
                        break;
                    }
                    way += $"    from {temp.data} move to the left child {temp.left.data}\n";
                    temp = temp.left;
                }
                if (value > temp.data)
                {
                    if (temp.right == null)
                    {
                        break;
                    }
                    way += $"    from {temp.data} move to the right child {temp.right.data}\n";
                    temp = temp.right;
                }
                if (value == temp.data)
                {
                    isFound = true;
                }
            }
            if (isFound)
            {
                Console.Write("=> {0} was found. ", value);
                if (!string.IsNullOrEmpty(way)) 
                {
                    Console.WriteLine("Way to this value: \n{0}", way);
                }
            }
            else
            {
                Console.WriteLine("=> {0} not found", value);
            }
        }
        public void AllSubTreea()
        {
            Node tempTree = root;
            Console.WriteLine("This tree has {0} subtrees.\n", GetSubTreesNumber(tempTree));
        }
        public void Insert(int item)
        {
            Node newItem = new Node(item);
            if (root == null)
            {
                root = newItem;
                root.colour = Color.Black;
                return;
            }
            Node nodeY = null;
            Node nodeX = root;
            while (nodeX != null)
            {
                nodeY = nodeX;
                if (newItem.data < nodeX.data)
                {
                    nodeX = nodeX.left;
                }
                else
                {
                    nodeX = nodeX.right;
                }
            }
            newItem.parent = nodeY;
            if (nodeY == null)
            {
                root = newItem;
            }
            else if (newItem.data < nodeY.data)
            {
                nodeY.left = newItem;
            }
            else
            {
                nodeY.right = newItem;
            }
            newItem.left = null;
            newItem.right = null;
            newItem.colour = Color.Red;
            InsertFixUp(newItem);
            height = GetHeight();
        }
        public void Delete(int value)
        {
            //first find the node in the tree to delete and assign to item pointer/reference
            Node item = Find(value);
            Node nodeX = null;
            Node nodeY = null;

            if (item == null)
            {
                Console.WriteLine("Nothing to delete!");
                return;
            }
            if (item.left == null && item.right == null)
            {
                nodeY = item.parent;
                if(nodeY.left == item)
                {
                    nodeY.left = null;
                }
                else
                {
                    nodeY.right = null;
                }
            }
            else if(item.left == null)
            {
                nodeY = item.parent;
                if (nodeY.left == item)
                {
                    nodeY.left = item.right;
                }
                else
                {
                    nodeY.right = item.right;
                }
            }
            else if (item.right == null)
            {
                nodeY = item.parent;
                if (nodeY.left == item)
                {
                    nodeY.left = item.left;
                }
                else
                {
                    nodeY.right = item.left;
                }
            }
            else
            {
                nodeX = GetMaximum(item.left);
                item.data = nodeX.data;
                nodeY = nodeX.parent;
                if(nodeY.left == nodeX)
                {
                    nodeY.left = nodeX.left == null ? null : nodeX.left;
                    if (nodeX.colour == Color.Black)
                    {
                        DeleteFixUp(nodeY.left);
                    }
                }
                else
                {
                    nodeY.right = nodeX.left == null ? null : nodeX.left;
                    if (nodeX.colour == Color.Black)
                    {
                        DeleteFixUp(nodeY.right);
                    }
                }
            }
            height = GetHeight();
        }

        #region Private Methods
        private int GetSubTreesNumber(Node node)
        {
            if(node.left == null && node.right == null)
            {
                return 1;
            }
            else if(node.left == null && node.right != null)
            {
                return 1 + GetSubTreesNumber(node.right);
            }
            else if (node.left != null && node.right == null)
            {
                return 1 + GetSubTreesNumber(node.left);
            }
            else
            {
                return 1 + GetSubTreesNumber(node.left) + GetSubTreesNumber(node.right);
            }
        }
        private int GetHeight()
        {
            if (root == null) return 0;
            Node tempTree = root;
            return GetSubTreeHeight(tempTree);
        }
        private int GetSubTreeHeight(Node node)
        {
            if (node.left == null && node.right == null)
            {
                return node.colour == Color.Black ? 
                    1 : 0;
            }
            else if (node.left == null && node.right != null)
            {
                return node.colour == Color.Black ? 
                    1 + GetSubTreeHeight(node.right) : GetSubTreeHeight(node.right);
            }
            else if (node.left != null && node.right == null)
            {
                return node.colour == Color.Black ? 
                    1 + GetSubTreeHeight(node.left) : GetSubTreeHeight(node.left);
            }
            else
            {
                return node.colour == Color.Black ? 
                    1 + Math.Max(GetSubTreeHeight(node.left), GetSubTreeHeight(node.right))
                    : Math.Max(GetSubTreeHeight(node.left), GetSubTreeHeight(node.right));
            }
        }
        private void InOrderDisplay(Node current, string child = "root")
        {
            if (current != null)
            {
                InOrderDisplay(current.left, $"L for {current.data}");
                Console.Write("(val: {0}   child: {1}   col: {2})\n", current.data, child, current.colour);
                InOrderDisplay(current.right, $"R for { current.data}");
            }
        }
        private Node Find(int value)
        {
            bool isFound = false;
            Node temp = root;
            while (!isFound)
            {
                if (temp == null)
                {
                    break;
                }
                if (value < temp.data)
                {
                    temp = temp.left;
                }
                if (value > temp.data)
                {
                    temp = temp.right;
                }
                if (value == temp.data)
                {
                    isFound = true;
                }
            }
            return isFound ? temp : null;
        }
        private Node GetMaximum(Node node)
        {
            while (node.right != null)
            {
                node = node.right;
            }
            return node;
        }
        private void SetDefaultData()
        {
            root = null;
            Insert(8);
            Insert(3);
            Insert(13);
            Insert(1);
            Insert(6);
            Insert(10);
            Insert(14);
            Insert(4);
            Insert(7);
        }
        #endregion

        #region FixUp Methods
        private void InsertFixUp(Node nodeX)
        {
            while (nodeX != root && nodeX.parent.colour == Color.Red)
            {
                if (nodeX.parent == nodeX.parent.parent.left)
                {
                    Node nodeY = nodeX.parent.parent.right;
                    if (nodeY != null && nodeY.colour == Color.Red)//Case 1: uncle is red
                    {
                        nodeX.parent.colour = Color.Black;
                        nodeY.colour = Color.Black;
                        nodeX.parent.parent.colour = Color.Red;
                        nodeX = nodeX.parent.parent;
                    }
                    else //Case 2: uncle is black
                    {
                        if (nodeX == nodeX.parent.right)
                        {
                            nodeX = nodeX.parent;
                            LeftRotate(nodeX);
                        }
                        //Case 3: recolour & rotate
                        nodeX.parent.colour = Color.Black;
                        nodeX.parent.parent.colour = Color.Red;
                        RightRotate(nodeX.parent.parent);
                    }

                }
                else
                {
                    //mirror image of code above
                    Node X = null;

                    X = nodeX.parent.parent.left;
                    if (X != null && X.colour == Color.Red)//Case 1
                    {
                        nodeX.parent.colour = Color.Black;
                        X.colour = Color.Black;
                        nodeX.parent.parent.colour = Color.Red;
                        nodeX = nodeX.parent.parent;
                    }
                    else //Case 2
                    {
                        if (nodeX == nodeX.parent.left)
                        {
                            nodeX = nodeX.parent;
                            RightRotate(nodeX);
                        }
                        //Case 3: recolour & rotate
                        nodeX.parent.colour = Color.Black;
                        nodeX.parent.parent.colour = Color.Red;
                        LeftRotate(nodeX.parent.parent);

                    }
                }
                root.colour = Color.Black;//re-colour the root black as necessary
            }
        }
        private void DeleteFixUp(Node nodeX)
        {

            while (nodeX != null && nodeX != root && nodeX.colour == Color.Black)
            {
                if (nodeX == nodeX.parent.left)
                {
                    Node nodeW = nodeX.parent.right;
                    if (nodeW.colour == Color.Red)
                    {
                        nodeW.colour = Color.Black; //case 1
                        nodeX.parent.colour = Color.Red; //case 1
                        LeftRotate(nodeX.parent); //case 1
                        nodeW = nodeX.parent.right; //case 1
                    }
                    if (nodeW.left.colour == Color.Black && nodeW.right.colour == Color.Black)
                    {
                        nodeW.colour = Color.Red; //case 2
                        nodeX = nodeX.parent; //case 2
                    }
                    else if (nodeW.right.colour == Color.Black)
                    {
                        nodeW.left.colour = Color.Black; //case 3
                        nodeW.colour = Color.Red; //case 3
                        RightRotate(nodeW); //case 3
                        nodeW = nodeX.parent.right; //case 3
                    }
                    nodeW.colour = nodeX.parent.colour; //case 4
                    nodeX.parent.colour = Color.Black; //case 4
                    nodeW.right.colour = Color.Black; //case 4
                    LeftRotate(nodeX.parent); //case 4
                    nodeX = root; //case 4
                }
                else //mirror code from above with "right" & "left" exchanged
                {
                    Node W = nodeX.parent.left;
                    if (W.colour == Color.Red)
                    {
                        W.colour = Color.Black;
                        nodeX.parent.colour = Color.Red;
                        RightRotate(nodeX.parent);
                        W = nodeX.parent.left;
                    }
                    if (W.right.colour == Color.Black && W.left.colour == Color.Black)
                    {
                        W.colour = Color.Black;
                        nodeX = nodeX.parent;
                    }
                    else if (W.left.colour == Color.Black)
                    {
                        W.right.colour = Color.Black;
                        W.colour = Color.Red;
                        LeftRotate(W);
                        W = nodeX.parent.left;
                    }
                    W.colour = nodeX.parent.colour;
                    nodeX.parent.colour = Color.Black;
                    W.left.colour = Color.Black;
                    RightRotate(nodeX.parent);
                    nodeX = root;
                }
            }
            if (nodeX != null)
                nodeX.colour = Color.Black;
        }
        #endregion

        #region Rotates
        private void LeftRotate(Node nodeX)
        {
            Node nodeY = nodeX.right; // set Y
            nodeX.right = nodeY.left;//turn Y's left subtree into X's right subtree
            if (nodeY.left != null)
            {
                nodeY.left.parent = nodeX;
            }
            if (nodeY != null)
            {
                nodeY.parent = nodeX.parent;//link X's parent to Y
            }
            if (nodeX.parent == null)
            {
                root = nodeY;
            }
            if (nodeY.parent != null)
            {
                if (nodeX == nodeX.parent.left)
                {
                    nodeX.parent.left = nodeY;
                }
                else
                {
                    nodeX.parent.right = nodeY;
                }
            }
            nodeY.left = nodeX; //put X on Y's left
            if (nodeX != null)
            {
                nodeX.parent = nodeY;
            }

        }
        private void RightRotate(Node Y)
        {
            // right rotate is simply mirror code from left rotate
            Node X = Y.left;
            Y.left = X.right;
            if (X.right != null)
            {
                X.right.parent = Y;
            }
            if (X != null)
            {
                X.parent = Y.parent;
            }
            if (Y.parent == null)
            {
                root = X;
            }
            if (Y.parent != null)
            {
                if (Y == Y.parent.right)
                {
                    Y.parent.right = X;
                }
                if (Y == Y.parent.left)
                {
                    Y.parent.left = X;
                }
            }

            X.right = Y;//put Y on X's right
            if (Y != null)
            {
                Y.parent = X;
            }
        }
        #endregion
    }
}
