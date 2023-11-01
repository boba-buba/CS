using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.IO;
using System.Text;

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
    }
}