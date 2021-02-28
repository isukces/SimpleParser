using iSukces.Parsers;
using iSukces.Parsers.TokenParsers;
using Xunit;
using Xunit.Abstractions;

namespace SimpleParser.Tests
{
    public class ManualDoubleTokenizerTest
    {
        public ManualDoubleTokenizerTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("")]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("  ")]
        [InlineData("   +")]
        [InlineData("  -")]
        public void T01_Should_not_parse_int(string prefix)
        {
            var txt = prefix + "1 22 ww";
            _testOutputHelper.WriteLine("Parse '{0}'", txt);
            var x = new ManualDoubleTokenizer(NumerFlags.AllowLedingSpaces).Parse(txt);
            Assert.Null(x);
        }

        [Theory]
        [InlineData("")]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("  ")]
        [InlineData("   +")]
        [InlineData("  -")]
        public void T02_Should_parse_fract(string prefix)
        {
            var txt = prefix + "1.23 22 ww";
            _testOutputHelper.WriteLine("Parse '{0}'", txt);
            var x = new ManualDoubleTokenizer(NumerFlags.AllowLedingSpaces).Parse(txt);
            Assert.NotNull(x);
            var expected = prefix.Contains('-') ? -1.23 : 1.23;
            Assert.Equal(expected, (double)x.Token);
        }

        [Theory]
        [InlineData("")]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("  ")]
        [InlineData("   +")]
        [InlineData("  -")]
        public void T03_Should_parse_no_fract_exp(string prefix)
        {
            var txt = prefix + "12e3 22 ww";
            _testOutputHelper.WriteLine("Parse '{0}'", txt);
            var x = new ManualDoubleTokenizer(NumerFlags.AllowLedingSpaces).Parse(txt);
            Assert.NotNull(x);
            var expected = prefix.Contains('-') ? -12e3 : 12e3;
            Assert.Equal(expected, (double)x.Token);
        }

        [Theory]
        [InlineData("")]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("  ")]
        [InlineData("   +")]
        [InlineData("  -")]
        public void T04_Should_parse_fract_exp(string prefix)
        {
            var txt = prefix + "1.23e3 22 ww";
            _testOutputHelper.WriteLine("Parse '{0}'", txt);
            var x = new ManualDoubleTokenizer(NumerFlags.AllowLedingSpaces).Parse(txt);
            Assert.NotNull(x);
            var expected = prefix.Contains('-') ? -1.23e3 : 1.23e3;
            Assert.Equal(expected, (double)x.Token);
        }


        [Theory]
        [InlineData("")]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("  ")]
        [InlineData("   +")]
        [InlineData("  -")]
        public void T05_Should_parse_with_comma_decimal_separator(string prefix)
        {
            var txt = prefix + "1,23e3 22 ww";
            _testOutputHelper.WriteLine("Parse '{0}'", txt);
            var x = new ManualDoubleTokenizer(NumerFlags.AllowLedingSpaces, ',', '.').Parse(txt);
            Assert.NotNull(x);
            var expected = prefix.Contains('-') ? -1.23e3 : 1.23e3;
            Assert.Equal(expected, (double)x.Token);
        }

        [InlineData("1.33")]
        [InlineData("1.3300")]
        [InlineData("01.3300")]
        [InlineData("+01.3300")]
        [InlineData("+1.3300")]
        [Theory]
        public void T06_Sould_parse_just_number(string text)
        {
            var t = new ManualDoubleTokenizer(NumerFlags.None, '.');
            {
                var candidate = t.Parse(text);
                Assert.NotNull(candidate);
                Assert.Equal(1.33, (double)candidate.Token);
            }
            {
                var candidate = t.Parse(text + "   ");
                Assert.NotNull(candidate);
                Assert.Equal(1.33, (double)candidate.Token);
            }
        }

        [InlineData("1")]
        [InlineData("01")]
        [InlineData("+01")]
        [InlineData("+1")]
        [InlineData("-01")]
        [InlineData("-1")]
        [Theory]
        public void T07_Sould_parse_int_as_number(string text)
        {
            var expected = text.Trim().StartsWith("-") ? -1d : 1d;
            {
                var t = new ManualDoubleTokenizer(NumerFlags.AllowParseInteger, '.');
                {
                    var candidate = t.Parse(text);
                    Assert.NotNull(candidate);
                    Assert.Equal(expected, (double)candidate.Token);
                }
                {
                    var candidate = t.Parse(text + "   ");
                    Assert.NotNull(candidate);
                    Assert.Equal(expected, (double)candidate.Token);
                }
            }
            {
                var t         = new ManualDoubleTokenizer(NumerFlags.None, '.');
                var candidate = t.Parse(text);
                Assert.Null(candidate);
            }
        }

        [Fact]
        public void T08_Sould_not_parse_extreme_numbers()
        {

            var t = new ManualDoubleTokenizer(NumerFlags.AllowParseInteger, '.');

            var candidate = t.Parse("1.23");
            Assert.NotNull(candidate);
            Assert.Equal(1.23, (double)candidate.Token);
            
            candidate = t.Parse("1.23e9999999");
            Assert.Null(candidate);
            
        }
        
        private readonly ITestOutputHelper _testOutputHelper;
    }
}