using System;
using System.Runtime.ConstrainedExecution;

namespace wordCounting
{
    static class fileParser
    {
        public static void fileParse(string fileName)
        {
            char ch = '\0';
            string word = "";

            StreamReader reader = new StreamReader(fileName);
            do
            {
                ch = (char)reader.Read();
                Console.Write(ch);

            } while (!reader.EndOfStream);
            reader.Close();
            reader.Dispose();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1) {
                Console.WriteLine("Argument Error");
                return;
            } else
            {

                try
                {
                    fileParser.fileParse(args[0]);
                }
                catch //(Exception ex)
                {
                    Console.WriteLine("File Error");
                }
            }
            for (int i = 0; i < args.Length; i++) { Console.WriteLine(args[i]); }
            Console.WriteLine("Hello, World!");
        }
    }
}