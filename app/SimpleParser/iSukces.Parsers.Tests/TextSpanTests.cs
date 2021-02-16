using iSukces.Parsers;
using Xunit;

namespace SimpleParser.Tests
{
    public class TextSpanTests
    {
        [Fact]
        public void T01_should_get_empty()
        {
            var e = TextSpan.Empty;
            Assert.Equal(0, e.Length);
        }
    }
}