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
            var t = WhitecharsTokenizer.ZeroOrMoreWhitechars;
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
            var t = WhitecharsTokenizer.OneOrMoreWhitechars;
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
            var t = WhitecharsTokenizer.OneOrMoreWhitechars;
            var c = t.Parse("");
            Assert.Null(c);
            c = t.Parse("XYZ");
            Assert.Null(c);
        }
    }
}