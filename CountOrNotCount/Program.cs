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
using System.Xml.Linq;

namespace CountOrNotCount
{ 
    internal class Program
    {
        static void Main(string[] args)

        {
            const string ArgumentError = "Argument Error";
            const string FileError = "File Error";
            if (args.Length < 3)
            {
                Console.WriteLine(ArgumentError);
                return;
            }

            try
            {
                string inputFile = args[0];
                string outFile = args[1];
                string columnName = args[2];

                FileInfo fInfo = new FileInfo(columnName);
                FileInputOutput fIO = new FileInputOutput(inputFile, outFile, fInfo);

                fIO.ParseFile();
                fIO.WriteReport();
                fIO.Dispose();
            }
            catch (IOException)
            {
                Console.WriteLine(FileError);
            }
            catch (UnauthorizedAccessException)

            {
                Console.WriteLine(FileError);
            }
            catch (ArgumentException)
            {
                Console.WriteLine(FileError);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    public class FileInfo
    {
        public const string ColumnError = "Non-existent Column Name";
        int _columnNumber = -1;
        public string columnName { get; private set; }
        public long ItemsSum { get; private set; }
        public FileInfo(string colName)
        {
            columnName = colName;
            ItemsSum = 0;
        }

        void AddToSum(int item)
        {
            ItemsSum += item;
        }

        public void ParseHeader(string[] line)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == columnName && _columnNumber == -1)
                {
                    _columnNumber  = i;
                }
            }
            if (_columnNumber == -1)
            {
                throw new Exception("Non-existent Column Name");
            }
        }

        public void ParseLine(string[] line)
        {
            int num = 0;
            for (int i = 0; i < line.Length; i++)
            {
                if (_columnNumber == i)
                {
                    try
                    {
                        num = Convert.ToInt32(line[i]);
                        AddToSum(num);
                    }
                    catch (Exception)
                    {
                        throw new Exception("Invalid Integer Value");
                    }
                }
            }
        }
    }


    class FileInputOutput
    {
        StreamReader _fr;
        StreamWriter _fw;
        FileInfo info;

        public FileInputOutput(string inputFile, string outputFile, FileInfo fi)
        {
            _fr = new StreamReader(inputFile);
            _fw = new StreamWriter(outputFile);
            info = fi;
        }

        public void ParseFile()
        {
            int columns = 0;
            string file_line = _fr.ReadLine();
            if (file_line == null)
            {
                Dispose();
                throw new Exception("Invalid File Format");
            }

           
            char[] whitechars = new char[] { ' ', '\t' };

            string[] line = file_line.Split(whitechars, StringSplitOptions.RemoveEmptyEntries);

            if (line.Count() == 0)
            {
                Dispose();
                throw new Exception("Invalid File Format");
            }
            try
            {
                info.ParseHeader(line);

                columns = line.Count();
                while ((file_line = _fr.ReadLine()) != null)
                {

                    line = file_line.Split(whitechars, StringSplitOptions.RemoveEmptyEntries);
                    if (line.Count() != columns)
                    {
                        throw new Exception("Invalid File Format");
                    }
                    info.ParseLine(line);
                }
            }
            catch (Exception e)
            {
                Dispose();
                throw new Exception(e.Message);
            }
        }

        public void Dispose()
        {
            _fr?.Dispose();
            _fw?.Dispose();
        }

        void WriteChar(char ch)
        {
            _fw?.Write(ch);
        }

        void WriteString(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                WriteChar(text[i]);
            }
        }

        public void WriteReport()
        {
            WriteString(info.columnName + '\n');

            for (int i = 0; i < info.columnName.Length; i++)
            {
                WriteChar('-');
            }
            WriteChar('\n');
            if (info.ItemsSum > 1000000000000)
            {
                WriteString(1000000000000.ToString());
            }
            else if (info.ItemsSum < -1000000000000)
                WriteString(1000000000000.ToString());
            else
                WriteString(info.ItemsSum.ToString());
            WriteChar('\n');
        }
    }
}