using System;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Reflection.Metadata;

namespace CountOrNotCount
{

    class FileWriter
    {
        string file = "";
        StreamWriter writer;

        public FileWriter(string fileName) 
        { 
            this.file = fileName;  
            this.writer = new StreamWriter(fileName); 
        }


        public void WriteInfo(string info)
        {
            for (int i = 0; i < info.Length; i++)
            {
                WriteChar(info[i]);
            }
        }

        public void WriteChar(char ch)
        {
            writer.Write(ch);
        }
        public void CloseWriter()
        {
            writer.Close();
            writer.Dispose();
        }
    }
    class FileInfo
    {
        string _columnName = "";
        int _columnNumber = 0;

        int _countItem = 0;
        public FileInfo(string column) { this._columnName = column; }

        void AddToCount(int quantity)
        {
            _countItem += quantity;
        }

        public void PrintCount(FileWriter fw)
        {
            fw.WriteInfo(this._columnName + '\n');

            //Console.WriteLine(_columnName);
            for (int i = 0; i < _columnName.Length; i++) { fw.WriteChar('-'); }
            fw.WriteChar('\n');
            fw.WriteInfo(_countItem.ToString());
        }

        
        public void ParseWord(int lineNumber, int columnNumber, string word)
        {
            if (lineNumber == 0 && word == _columnName) { _columnNumber = columnNumber; }
            else if (lineNumber != 0 && columnNumber == _columnNumber)
            {
                int num = Int32.Parse(word);
                AddToCount(num);
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
            char[] whiteChars = { ' ', '\n', '\t', '\r' };
            int charInt = 0;
            int lineNumber = 0;
            int columnNumber = 0;

            StreamReader reader = new StreamReader(fileName);
            charInt = reader.Read();

            while (charInt != -1)
            {
                ch = (char)charInt;
                if ((ch == '\n'))
                {
                    lineNumber++;
                    columnNumber = 0;
                }
                else if (!(whiteChars.Contains(ch)))
                {
                    word += ch;
                }
                else if ((whiteChars.Contains(ch)) && (word.Length > 0))
                {
                    info.ParseWord(lineNumber, columnNumber, word);
                    columnNumber++;
                    word = "";
                    
                }
                charInt = reader.Read();
            }
            if (word.Length > 0) { info.ParseWord(lineNumber, columnNumber, word);  }

            reader.Close();
            reader.Dispose();
        }
    }



    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Argument Error");
                return;
            }

            try
            {
                string f = args[0];
                string outFile = args[1];
                string name = args[2];
                FileInfo fi = new FileInfo(name);
                FileParser fp = new FileParser(fi);
                fp.ParseFile(f);
                FileWriter fw = new FileWriter(outFile);

                fi.PrintCount(fw);
                fw.CloseWriter();
            }
            catch (IOException)
            {
                Console.WriteLine("File Error");
            }

        }
    }
}