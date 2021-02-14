using iSukces.Parsers;
using Xunit;

namespace SimpleParser.Tests
{
    public class RegexpChunkTest : RegexpBuilder
    {
        [Fact]
        public void T01_should_wrap_whitechar()
        {
            var source = WhiteChar;
            Assert.True(source is RegexpChunk.RcChar);
            Assert.Equal("\\s", source.Code);

            var d = source.Plus();
            Assert.True(d is RegexpChunk.RcChar);
            Assert.Equal("\\s+", d.Code);

            d = source.Star();
            Assert.True(d is RegexpChunk.RcChar);
            Assert.Equal("\\s*", d.Code);
        }

        [Fact]
        public void T02_should_wrap_digit()
        {
            var source = Digit;
            Assert.True(source is RegexpChunk.RcChar);
            Assert.Equal("\\d", source.Code);

            var d = source.Plus();
            Assert.True(d is RegexpChunk.RcChar);
            Assert.Equal("\\d+", d.Code);

            d = source.Star();
            Assert.True(d is RegexpChunk.RcChar);
            Assert.Equal("\\d*", d.Code);
        }

        [Fact]
        public void T03_should_concat()
        {
            var d = Digit + WhiteChar;
            Assert.True(d is RegexpChunk.RcConcat);
            Assert.False(d is RegexpChunk.RcQuantified);
            Assert.Equal(@"\d\s", d.Code);

            d = d.Plus();
            Assert.Equal(@"(?:\d\s)+", d.Code);
        }


        [Fact]
        public void T04_should_make_alternative()
        {
            var d = Digit | WhiteChar;
            Assert.True(d is RegexpChunk.RcSquareBrackets);
            Assert.True(d is RegexpChunk.RcQuantified);
            Assert.Equal(@"[\d\s]", d.Code);

            d = d.Plus();
            Assert.Equal(@"[\d\s]+", d.Code);
        }

        [Fact]
        public void T05_should_concat_complex()
        {
            var d = Digits | WhiteChar;
            Assert.True(d is RegexpChunk.RcAlternative);
            Assert.False(d is RegexpChunk.RcQuantified);
            Assert.Equal(@"\d+|\s", d.Code);

            d = d.Plus();
            Assert.Equal(@"(?:\d+|\s)+", d.Code);
        }
    }
}