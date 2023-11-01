namespace BlockAlignment_UnitTests
{
    public class BlockAlignmentTests
    {
        [Fact]
        public void ArgumentErrorLessParamaters()
        {
            // Arrange
            string[] MockCommandLine = new string[] { };
            var currentConsoleOut = Console.Out;
            // Act  
            using (var consoleOutput = new ConsoleOutputManager())
            {
                ProgramManager.RunMainFunction(MockCommandLine);

                // Assert
                Assert.Equal("Argument Error\n", consoleOutput.GetOuput());
            }

        }

        [Fact]
        public void ArgumentErrorThirdParameter()
        {
            // Arrange
            string[] MockCommandLine = new string[3] { "PrettyTextFromTaskIn.txt", "PrettyTextFromTaskOutActual.txt", "yu" };
            var currentConsoleOut = Console.Out;
            // Act  
            using (var consoleOutput = new ConsoleOutputManager())
            {
                ProgramManager.RunMainFunction(MockCommandLine);

                // Assert
                Assert.Equal("Argument Error\n", consoleOutput.GetOuput());
            }
        }

        [Fact]
        public void PrettyFromTask()
        {
            // Arrange
            string[] MockCommandLine = new string[3] { "PrettyTextFromTaskIn.txt", "PrettyTextFromTaskOutActual.txt", "17" };
            var currentConsoleOut = Console.Out;
            // Act  
            using (var consoleOutput = new ConsoleOutputManager())
            {
                ProgramManager.RunMainFunction(MockCommandLine);

                // Assert
                Assert.Equal("", consoleOutput.GetOuput());
                Assert.True(FileAssert.FileCompare("PrettyTextFromTaskOut.txt", "PrettyTextFromTaskOutActual.txt"));
            }

        }
    }
}