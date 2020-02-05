using System;

namespace RedBlackTree
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Add ____________________________________\n");
            //Add
            RBTree tree = new RBTree();
            tree.Insert(2);
            tree.Insert(25);
            tree.DisplayTree();
            Console.WriteLine("Delete ____________________________________\n");
            ////Delete
            RBTree tree1 = new RBTree();
            //root
            tree1.Delete(8);
            tree1.DisplayTree();
            Console.WriteLine("  ");
            // node
            tree1.Delete(6);
            tree1.DisplayTree();
            Console.WriteLine("Methods ____________________________________\n");
            //Methods
            RBTree tree2 = new RBTree();
            tree2.DisplayTree();
            tree2.AllSubTreea();
            tree2.FindValue(10);
            tree2.FindValue(99);
            tree2.MaxValue();
            tree2.MinValue();
            Console.WriteLine("Tree root => {0}", tree2.treeRoot);
            Console.WriteLine("Tree height => {0}", tree2.height);
            Console.ReadLine();
        } 
    }
}
