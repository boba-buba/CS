using System;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Reflection.Metadata;

namespace ParagraphWordCounter
{
    class FileInfo
    {
        public FileInfo() { }

        int ParagraphWordCounter = 0;

        public void ResetCount()
        {
            ParagraphWordCounter = 0;
        }

        public void IncrementCount()
        {
            ParagraphWordCounter++;
        }

        public void PrintCount()
        {
            Console.WriteLine(ParagraphWordCounter.ToString());
        }
    }
    class FileParser
    {
        public FileParser(FileInfo fi) { info = fi; }
        FileInfo info;
        public void ParseFile(string fileName)
        {
            char ch;
            string word = "";
            char[] whiteChars = { ' ', '\t', '\r' };
            int charInt = 0;

            StreamReader reader = new StreamReader(fileName);
            charInt = reader.Read();

            while (charInt != -1)
            {
                ch = (char)charInt;
                if ((ch == '\n') && word.Length == 0)
                {
                    info.PrintCount();
                    info.ResetCount();
                }
                else if (!(whiteChars.Contains(ch)))
                {
                    word += ch;
                }
                else if ((whiteChars.Contains(ch)) && (word.Length > 0))
                {
                    info.IncrementCount();
                    word = "";
                }
                charInt = reader.Read();
            }
            if (word.Length > 0) { info.IncrementCount(); info.PrintCount(); }


            reader.Close();
            reader.Dispose();
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Argument Error");
                return;
            }

            try
            {
                //string f = args[0];
                string f = "C:\\Users\\ncoro\\Source\\Repos\\CS\\ParagraphWordCounter\\Hamleto.txt";
                FileInfo fi = new FileInfo();
                FileParser fp = new FileParser(fi);
                fp.ParseFile(f);
            }
            catch (IOException)
            {
                Console.WriteLine("File Error");
            }
        }
    }
}