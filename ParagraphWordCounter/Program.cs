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
    public class ProgramInputOutputState : IDisposable
    {
        public const string ArgumentErrorMessage = "Argument Error";
        public const string FileErrorMessage = "File Error";

        public StreamReader? Reader { get; private set; }

        public bool InitializeFromCommandLineArgs(string[] args)
        {
            if (args.Length != 1)
            {
                Console.Write(ArgumentErrorMessage + "\n");
                return false;
            }

            try
            {
                Reader = new StreamReader(args[0]);
            }
            catch (IOException)
            {
                Console.Write(FileErrorMessage + "\n");
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                Console.Write(FileErrorMessage + "\n");
                return false;
            }
            catch (ArgumentException)
            {
                Console.Write(FileErrorMessage + "\n");
                return false;
            }

            return true;
        }

        public void Dispose()
        {
            Reader?.Dispose();
        }
    }

    public class ConsoleOutput : IDisposable
    {
        /// <summary>
        /// This code was taken from http://www.vtrifonov.com/2012/11/getting-console-output-within-unit-test.html
        /// </summary>
        private StringWriter stringWriter;
        private TextWriter originalOutput;

        public ConsoleOutput()
        {
            stringWriter = new StringWriter();
            originalOutput = Console.Out;
            Console.SetOut(stringWriter);
        }

        public string GetOuput()
        {
            return stringWriter.ToString();
        }

        public void Dispose()
        {
            Console.SetOut(originalOutput);
            stringWriter.Dispose();
        }
    }
    
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

        public void PrintReport()
        {
            Console.Write(WordCounts);
        }
    }

    public class FileParser
    {        
        WordProcessor info;
        StreamReader _reader;
        public FileParser(WordProcessor fi, StreamReader reader) 
        { 
            info = fi;
            _reader = reader;

        }
        
        public void ParseFile()
        {
            char ch;
            string word = "";
            char[] whiteChars = { ' ', '\t', '\n' };
            int charInt = 0;

            charInt = _reader.Read();

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
                charInt = _reader.Read();
            }
            if (word.Length > 0) { info.IncrementCount(); info.AddLineWordsCount(); }

            info.AddLineWordsCount();      
        }

        /*public void Dispose()
        {
            _reader.Dispose();
        }*/
        
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var state = new ProgramInputOutputState();
            if (!state.InitializeFromCommandLineArgs(args))
            {
                return;
            }

            WordProcessor fi = new WordProcessor();
            FileParser fp = new FileParser(fi, state.Reader!);
            fp.ParseFile();
            fi.PrintReport();
            state.Dispose();

        }
    }
}

