using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;



namespace Huffman
{
    public class ProgramInputOutputState : IDisposable
    {
        public const string ArgumentErrorMessage = "Argument Error";
        public const string FileErrorMessage = "File Error";

        public FileStream? Reader { get; private set; }

        public bool InitializeFromCommandLineArgs(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine(ArgumentErrorMessage);
                return false;
            }

            try
            {
                Reader = File.OpenRead(args[0]);
            }
            catch (IOException)
            {
                Console.WriteLine(FileErrorMessage);
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine(FileErrorMessage);
                return false;
            }
            catch (ArgumentException)
            {
                Console.WriteLine(FileErrorMessage);
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            Reader?.Dispose();
        }
    }

    
    class FileParser
    {
        public Dictionary<int, int> byteDict { get; private set; } = new Dictionary<int, int>();
        FileStream reader;
        public FileParser(FileStream reader)
        {
            this.reader = reader;
        }

        void IncrementByteCount(int b)
        {
            try 
            { 
                byteDict[b]++;
            }
            catch (KeyNotFoundException)
            {
                byteDict[b] = 1;
            }
        }

        public void ProcessData()
        {   
            int b = reader.ReadByte();
            while (b != -1)
            {
                IncrementByteCount(b);
                b = reader.ReadByte();
            }
        }
    }


    class Tree
    {
        
        class Node
        {
            public int characterValue;
            public int characterWeight;
            public Node? Right;
            public Node? Left;     
        }

        Dictionary<int, int> byteDict;
        List<Node> treeRoots = new List<Node>();
        int maxCharValue = 256;
        
        public Tree(Dictionary<int, int> byteDict) 
        {
            this.byteDict = byteDict;
        }

        Node? FindTheLightestNode()
        {
            Node LightestNode = new Node()
            {
                characterValue = 256,
                characterWeight = int.MaxValue,
            };
        
            for (int i = 0; i < treeRoots.Count;i++)
            {
                if (treeRoots[i].characterValue == maxCharValue) //inner nodes
                {
                    if (treeRoots[i].characterWeight < LightestNode.characterWeight)
                    {
                        LightestNode = treeRoots[i];
                    }
                }
                else if (treeRoots[i].characterWeight <= LightestNode.characterWeight) //leaf
                {
                    if (treeRoots[i].characterWeight == LightestNode.characterWeight)
                    {
                        if (treeRoots[i].characterValue < LightestNode.characterValue)
                        {
                            LightestNode = treeRoots[i];
                        }
                    }                   
                    else
                    {
                        LightestNode = treeRoots[i];
                    }
                }

            }
            return LightestNode;
        }


        void BuildForest()
        {
            foreach (KeyValuePair<int, int> pair in byteDict)
            {
                treeRoots.Add(new Node()
                {
                    characterValue = pair.Key,
                    characterWeight = pair.Value,
                    Left = null,
                    Right = null,
                });
            }
        }

        void DeleteTheLightest(Node node)
        {
            treeRoots.Remove(node);
        }

        void BuildTreeFromForest() 
        {

            while (treeRoots.Count > 1)
            {
                Node leftNode = FindTheLightestNode();
                DeleteTheLightest(leftNode);
                Node rightNode = FindTheLightestNode();
                DeleteTheLightest(rightNode);
                Node newRoot = new Node()
                {
                    characterValue = maxCharValue,
                    characterWeight = leftNode.characterWeight + rightNode.characterWeight,
                    Left = leftNode,
                    Right = rightNode,

                };
                treeRoots.Add(newRoot);
            }
            if (treeRoots.Count == 1)
            {
                return;
            }
        }

        void PrintTreePreOrder(Node root, bool space)
        {
            if (root == null) return;
            if (space)
                Console.Write(" ");

            if (root.Left == null && root.Right == null)
                Console.Write("*" + root.characterValue + ":" + root.characterWeight);
            else
                Console.Write(root.characterWeight);
            PrintTreePreOrder(root.Left, true);
            PrintTreePreOrder(root.Right, true);

        }

        public void Report()
        {
            BuildForest();
            BuildTreeFromForest();
            PrintTreePreOrder(treeRoots[0], false);
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            var state = new ProgramInputOutputState();
            if (!state.InitializeFromCommandLineArgs(args))
            {
                return;
            }
            FileParser fileParser = new FileParser(state.Reader);
            fileParser.ProcessData();
            Tree tree = new Tree(fileParser.byteDict);
            tree.Report();
            state.Dispose();
        }
    }
}