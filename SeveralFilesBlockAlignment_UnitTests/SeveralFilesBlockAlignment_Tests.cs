namespace SeveralFilesBlockAlignment_UnitTests
{
    public class SeveralFilesBlockAlignment_Tests
    {
        [Fact]
        public void HiglightSpacesOption_LessThanFourParameters()
        {
            // Arrange
            string[] args = { "--highlight-spaces", "ex01.out", "17" };
            var state = new ProgramInputOutputState();

            using (var consoleOutput = new ConsoleOutputManager())
            {
                // Act
                Assert.False(state.InitializeFromCommandLineArgs(args));
                
                // Assert
                Assert.Equal("Argument Error\n", consoleOutput.GetOuput());
            }
        }

        [Fact]
        public void test()
        {

        }
        //ParseAllFilesFromCommandLine
        //neexistujici soubor
        //prazdny sobor 

    }
}