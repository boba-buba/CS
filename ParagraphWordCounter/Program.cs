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

        int ParagraphWordCount = 0;
        int LineWordCount = 0;
        int ParagraphCount = 0;

        List<int> ParagraphsCouner = new List<int>();

        public void ResetCount()
        {
            LineWordCount = 0;
        }

        public void IncrementCount()
        {
            LineWordCount++;
        }

        public void PrintCount()
        {
            if (LineWordCount != 0)
            {
                ParagraphWordCount += LineWordCount;
                LineWordCount = 0;
            }
            else
            {
                if ( ParagraphWordCount != 0)
                {
                    //if (ParagraphCount != 0) Console.Write('\n');
                    //Console.Write(ParagraphWordCount.ToString());
                    ParagraphsCouner.Add(ParagraphWordCount);
                    ParagraphWordCount = 0;
                    ParagraphCount++;
                }
            }
        }

        public void PrintParagraphs()
        {
            for (int i = 0; i < ParagraphCount; i++)
            {
                if (i != 0) Console.Write('\n');
                Console.Write(ParagraphsCouner[i]);
            }
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
            char[] whiteChars = { ' ', '\t', '\r', '\n' };
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

            info.PrintCount();
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
                string f = args[0];
                FileInfo fi = new FileInfo();
                FileParser fp = new FileParser(fi);
                fp.ParseFile(f);
                fi.PrintParagraphs();
            }
            catch (IOException)
            {
                Console.WriteLine("File Error");
            }
        }
    }
}