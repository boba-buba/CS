using static System.Net.Mime.MediaTypeNames;

namespace ParargaphWordCounter_UnitTests
{
    public class PragagraphWordCounterTests
    {
        [Fact]
        public void PrettyHamlet()
        {
            // Arrange
            string[] MockCommandLine = new string[1] { "Hamlet.txt" };
            var state = new ProgramInputOutputState();
            var currentConsoleOut = Console.Out;


            // Act   
            using (var consoleOutput = new ConsoleOutput())
            {
                state.InitializeFromCommandLineArgs(MockCommandLine);
                var fi = new WordProcessor();
                FileParser fp = new FileParser(fi, state.Reader!);
                fp.ParseFile();
                fi.PrintReport();
                state.Dispose();

                // Assert
                Assert.Equal("107\n109\n59", consoleOutput.GetOuput());
            }
        }  

        [Fact]
        public void TabSeparated()
        {
            // Arrange
            string[] MockCommandLine = new string[1] { "TabSeparated.txt" };
            var state = new ProgramInputOutputState();
            var currentConsoleOut = Console.Out;

            // Act  
            using (var consoleOutput = new ConsoleOutput())
            {
                state.InitializeFromCommandLineArgs(MockCommandLine);
                var fi = new WordProcessor();
                FileParser fp = new FileParser(fi, state.Reader!);
                fp.ParseFile();
                fi.PrintReport();
                state.Dispose();

                // Assert
                Assert.Equal("16\n10\n14", consoleOutput.GetOuput());
            }
        }

        [Fact]
        public void EmptyWhiteLines() 
        {
            // Arrange
            string[] MockCommandLine = new string[1] { "EmptyWhiteLines.txt" };
            var state = new ProgramInputOutputState();
            var currentConsoleOut = Console.Out;


            // Act  
            using (var consoleOutput = new ConsoleOutput())
            {
                state.InitializeFromCommandLineArgs(MockCommandLine);
                var fi = new WordProcessor();
                FileParser fp = new FileParser(fi, state.Reader!);
                fp.ParseFile();
                fi.PrintReport();
                state.Dispose();

                // Assert
                Assert.Equal("", consoleOutput.GetOuput());
            }
        }

        [Fact]
        public void MoreThanOneEmptyLineBetweenParagraphs()
        {
            // Arrange
            string[] MockCommandLine = new string[1] { "MoreThanOneEmptyLineBetweenParagraphs.txt" };
            var state = new ProgramInputOutputState();
            var currentConsoleOut = Console.Out;


            // Act  
            using (var consoleOutput = new ConsoleOutput())
            {
                state.InitializeFromCommandLineArgs(MockCommandLine);
                var fi = new WordProcessor();
                FileParser fp = new FileParser(fi, state.Reader!);
                fp.ParseFile();
                fi.PrintReport();
                state.Dispose();

                // Assert
                Assert.Equal("40\n12\n12\n8\n12", consoleOutput.GetOuput());
            }
        }

        [Fact]
        public void EmpyWhiteLinesAfterBeforeText()
        {
            // Arrange
            string[] MockCommandLine = new string[1] { "EmptyWhiteLinesAfterBeforeText.txt" };
            var state = new ProgramInputOutputState();
            var currentConsoleOut = Console.Out;


            // Act
            using (var consoleOutput = new ConsoleOutput())
            {
                state.InitializeFromCommandLineArgs(MockCommandLine);
                var fi = new WordProcessor();
                FileParser fp = new FileParser(fi, state.Reader!);
                fp.ParseFile();
                fi.PrintReport();
                state.Dispose();

                // Assert
                Assert.Equal("40\n12\n12\n8\n12", consoleOutput.GetOuput());
            }
        }

        [Fact]
        public void FileErrorTest()
        {
            // Arrange
            string[] MockCommandLine = new string[1] {"NonExistentFile.txt"};
            var state = new ProgramInputOutputState();            
            var currentConsoleOut = Console.Out;
            
            // Act  
            using (var consoleOutput = new ConsoleOutput())
            {            
                state.InitializeFromCommandLineArgs(MockCommandLine);
                 
                // Assert
                Assert.Equal("File Error\n", consoleOutput.GetOuput());
            }
        }

        [Fact]
        public void ZeroArgumentsTest()
        {
            // Arrange
            string[] MockCommandLine = new string[] { };
            var state = new ProgramInputOutputState();
            var currentConsoleOut = Console.Out;

            // Act  
            using (var consoleOutput = new ConsoleOutput())
            {
                state.InitializeFromCommandLineArgs(MockCommandLine);

                // Assert
                Assert.Equal("Argument Error\n", consoleOutput.GetOuput());
            }
        }

        [Fact]
        public void MoreThanOneArgumentTest()
        {
            // Arrange
            string[] MockCommandLine = new string[2] { "Hamlet.txt", "EmptyWhiteLinesAfterBeforeText.txt" };
            var state = new ProgramInputOutputState();
            var currentConsoleOut = Console.Out;

            // Act  
            using (var consoleOutput = new ConsoleOutput())
            {
                state.InitializeFromCommandLineArgs(MockCommandLine);

                // Assert
                Assert.Equal("Argument Error\n", consoleOutput.GetOuput());
            }
        }

        [Fact]
        public void Test9()
        {

        }
    }
}