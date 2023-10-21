using System;
using System.Net.NetworkInformation;
using System.Reflection.PortableExecutable;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;

namespace CountOrNotCount
{
    public class ProgramInputOutputState : IDisposable
    {
        public const string ArgumentErrorMessage = "Argument Error";
        public const string FileErrorMessage = "File Error";

        public TextReader? Reader { get; private set; }
        public TextWriter? Writer { get; private set; }

        public bool InitializeFromCommandLineArgs(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine(ArgumentErrorMessage);
                return false;
            }

            try
            {
                Reader = new StreamReader(args[0]);
            }
            catch (IOException)
            {
                Console.WriteLine(FileErrorMessage);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine(FileErrorMessage);
            }
            catch (ArgumentException)
            {
                Console.WriteLine(FileErrorMessage);
            }

            try
            {
                Writer = new StreamWriter(args[1]);
            }
            catch (IOException)
            {
                Console.WriteLine(FileErrorMessage);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine(FileErrorMessage);
            }
            catch (ArgumentException)
            {
                Console.WriteLine(FileErrorMessage);
            }

            return true;
        }

        public void Dispose()
        {
            Reader?.Dispose();
            Writer?.Dispose();
        }
    }

    class FileWriter
    {
        
        TextWriter _writer;

        public FileWriter(TextWriter tw) 
        {   
            _writer = tw; 
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
            _writer.Write(ch);
        }
        public void CloseWriter()
        {
            _writer.Close();
            _writer.Dispose();
        }
    }
    class FileInfo
    {
        string _columnName = "";
        List<int> columnNumbers = new List<int>();
        public bool nonExistentColumn { get; private set; }
        

        Int64 _countItem = 0;
        public FileInfo(string column) { _columnName = column; nonExistentColumn = false; }

        void AddToCount(Int64 quantity)
        {
            _countItem += quantity;
        }

        public void PrintCount(FileWriter fw)
        {
            if (nonExistentColumn) { return; }
            fw.WriteInfo(_columnName + '\n');

            for (int i = 0; i < _columnName.Length; i++) { fw.WriteChar('-'); }
            fw.WriteChar('\n');
            fw.WriteInfo(_countItem.ToString());
        }
   
        public void ParseWord(int lineNumber, int columnNumber, string word)
        {
            Int64 num = 0;
            if (lineNumber == 0 && word == _columnName) 
            { columnNumbers.Add(columnNumber); }
            else if (columnNumbers.Count() == 0 && lineNumber > 0) 
            { nonExistentColumn = true; }
            else if (lineNumber != 0 && columnNumbers.Contains(columnNumber))
            {
                try
                {
                    num = Convert.ToInt64(word);
                    AddToCount(num);
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid Integer Value");
                }
            }
        }
    }
    class FileParser
    {
        TextReader _reader;
        public bool invalidFormat { get; private set; } 
        public FileParser(TextReader tr, FileInfo fi) { info = fi; _reader = tr; invalidFormat = false; }
        FileInfo info;
        public void ParseFile()
        {
            
            char ch;
            string word = "";
            char[] whiteChars = { ' ', '\n', '\t', '\r' };
            int charInt = 0;
            int lineNumber = 0;
            int columnNumber = 0;
            int columns = 0;

            charInt = _reader.Read();

            while (charInt != -1)
            {
                ch = (char)charInt;
                if ((ch == '\n'))
                {
                    if (lineNumber == 0) { columns  = columnNumber; }
                    else
                    {
                        if ((columnNumber != columns) )
                        {
                            invalidFormat = true;
                            Console.WriteLine("Invalid File Format");
                            return;
                        }
                    }
                    lineNumber++;
                    columnNumber = 0;
                }
                else if (!(whiteChars.Contains(ch)))
                {
                    word += ch;
                }
                else if ((whiteChars.Contains(ch)) && (word.Length > 0))
                {
                    if (info.nonExistentColumn) 
                    {
                        Console.WriteLine("Non-existent Column Name");
                        return; 
                    }
                    info.ParseWord(lineNumber, columnNumber, word);
                    columnNumber++;
                    word = "";
                    
                }
                charInt = _reader.Read();
            }
            if (word.Length > 0) { info.ParseWord(lineNumber, columnNumber, word);  }

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

            string columnName = args[2];
            FileInfo fi = new FileInfo(columnName);
            FileParser fp = new FileParser(state.Reader!, fi);
            fp.ParseFile();
            if (fp.invalidFormat) { state.Dispose(); return; }

            FileWriter fw = new FileWriter(state.Writer!);
            fi.PrintCount(fw);
            state.Dispose();
            

        }
    }
}