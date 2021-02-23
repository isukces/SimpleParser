using iSukces.Parsers.TokenParsers;
using Xunit;

namespace SimpleParser.Tests
{
    public class WhitecharsTokenizerTests
    {
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(" \n ")]
        [Theory]
        public void T01_Should_parse_zero_or_more(string text)
        {
            var t = WhitecharsTokenizer.ZeroOrMore;
            var c = t.Parse(text);
            Assert.NotNull(c);
            Assert.Equal(text.Length - text.TrimStart().Length, c.StringLength);
            c = t.Parse(text + "XYZ");
            Assert.NotNull(c);
            Assert.Equal(text.Length - text.TrimStart().Length, c.StringLength);
        }

        [InlineData(" ")]
        [InlineData(" \n ")]
        [Theory]
        public void T02_Should_parse_one_or_more(string text)
        {
            var t = WhitecharsTokenizer.OneOrMore;
            var c = t.Parse(text);
            Assert.NotNull(c);
            Assert.Equal(text.Length - text.TrimStart().Length, c.StringLength);
            c = t.Parse(text + "XYZ");
            Assert.NotNull(c);
            Assert.Equal(text.Length - text.TrimStart().Length, c.StringLength);
        }

        [Fact]
        public void T03_Should_not_parse_one_or_more()
        {
            var t = WhitecharsTokenizer.OneOrMore;
            var c = t.Parse("");
            Assert.Null(c);
            c = t.Parse("XYZ");
            Assert.Null(c);
        }
        
        
        [InlineData("")]
        [InlineData("  ")]
        [Theory]
        public void T04_Should_parse_all0(string text)
        {
            var t = WhitecharsTokenizer.AllOrNone;
            var c = t.Parse(text);
            Assert.NotNull(c);
            Assert.Equal(text.Length, c.StringLength);

            c = t.Parse(text + "a");
            Assert.Null(c);
        }
        
        [Fact]
        public void T05_Should_parse_all1()
        {
            const string text = "   ";
            var          t    = WhitecharsTokenizer.All;
            var          c    = t.Parse(text);
            Assert.NotNull(c);
            Assert.Equal(text.Length, c.StringLength);

            c = t.Parse(text + "a");
            Assert.Null(c);
        }
        
                
        [Fact]
        public void T06_Should_not_parse_all1()
        {
            const string text = "";
            var          t    = WhitecharsTokenizer.All;
            var          c    = t.Parse(text);
            Assert.Null(c);

            c = t.Parse(text + "a");
            Assert.Null(c);
        }
    }
    
}