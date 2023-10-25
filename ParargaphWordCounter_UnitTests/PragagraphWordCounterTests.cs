namespace ParargaphWordCounter_UnitTests
{
    public class PragagraphWordCounterTests
    {
        [Fact]
        public void PrettyHamlet()
        {
            // Arrange
            string f = "Hamlet.txt";
            var fi = new WordProcessor();
            FileParser fp = new FileParser(fi, f);      
            
            // Act  
            fp.ParseFile();

            // Assert
            Assert.Equal("107\n109\n59", fi.GetWordCounts());
        }

        [Fact]
        public void NothingToDo()
        {
            // Arrange
            string f = "Hamlet.txt";
            var fi = new WordProcessor();
            FileParser fp = new FileParser(fi, f);

            // Act  
            // DO NOTHING

            // Assert
            Assert.Equal("", fi.GetWordCounts());


        }

        [Fact]
        public void TabsSeparated()
        {
            // Arrange
            string f = "TabSeparated.txt";
            var fi = new WordProcessor();
            FileParser fp = new FileParser(fi, f);

            // Act  
            fp.ParseFile();

            // Assert
            Assert.Equal("16\n10\n14", fi.GetWordCounts());

        }

        [Fact]
        public void EmptyWhiteLines() 
        {
            // Arrange
            string f = "EmptyWhiteLines.txt";
            var fi = new WordProcessor();
            FileParser fp = new FileParser(fi, f);

            // Act  
            fp.ParseFile();

            // Assert
            Assert.Equal("", fi.GetWordCounts());

        }

    }
}