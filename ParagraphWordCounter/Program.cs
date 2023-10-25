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
    public class WordProcessor
    {
        public WordProcessor() { }

        int LineWordCount = 0;
        int paragraphCount = 0;
        int paragraphWords = 0;
        string WordCounts = "";
        public void IncrementCount()
        {
            LineWordCount++;
        }

        public void AddLineWordsCount()
        {

            if (LineWordCount == 0 && paragraphWords != 0)
            {
                if (paragraphCount > 0) WordCounts += '\n';
                WordCounts += paragraphWords;
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

        public string GetWordCounts() { return WordCounts; }
    }

    public class FileParser
    {        
        WordProcessor info;
        StreamReader reader;
        public FileParser(WordProcessor fi, string fileName) 
        { 
            info = fi;
            reader = new StreamReader(fileName);

        }
        
        public void ParseFile()
        {
            char ch;
            string word = "";
            char[] whiteChars = { ' ', '\t', '\n' };
            int charInt = 0;

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
                    info.AddLineWordsCount();
                }
                charInt = reader.Read();
            }
            if (word.Length > 0) { info.IncrementCount(); info.AddLineWordsCount(); }

            info.AddLineWordsCount();      
        }

        public void Dispose()
        {
            reader.Dispose();
        }
        
    }


    public class Program
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
                WordProcessor fi = new WordProcessor();
                FileParser fp = new FileParser(fi, f);
                fp.ParseFile();
                Console.Write(fi.GetWordCounts());
                fp.Dispose();
            }
            catch (IOException)
            {
                Console.WriteLine("File Error");
            }
        }
    }
}

