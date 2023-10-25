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

        int LineWordCount = 0;
        int paragraphCount = 0;
        int paragraphWords = 0;

        public void IncrementCount()
        {
            LineWordCount++;
        }

        public void PrintCount()
        {

            if (LineWordCount == 0 && paragraphWords != 0)
            {
                if (paragraphCount > 0) Console.Write('\n');
                Console.Write(paragraphWords);
                paragraphCount++;
                paragraphWords = 0;
                LineWordCount = 0;
            }
            else
            {
                paragraphWords += LineWordCount;
                LineWordCount = 0;
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
            char[] whiteChars = { ' ', '\t', '\n' };
            int charInt = 0;

            StreamReader reader = new StreamReader(fileName);
            charInt = reader.Read();

            while (charInt != -1)
            {
                ch = (char)charInt;
                
                if (!(whiteChars.Contains(ch)))
                {
                    word += ch;
                }
                else if ((whiteChars.Contains(ch)) && (word.Length > 0))
                {
                    info.IncrementCount();
                    word = "";
                }
                if ((ch == '\n'))
                {
                    info.PrintCount();
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
            }
            catch (IOException)
            {
                Console.WriteLine("File Error");
            }
        }
    }
}

