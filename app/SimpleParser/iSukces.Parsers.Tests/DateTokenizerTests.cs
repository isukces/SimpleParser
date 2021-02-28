using System;
using iSukces.Parsers.TokenParsers;
using Xunit;

namespace SimpleParser.Tests
{
    public class DateTokenizerTests
    {
        [Fact]
        public void T01_Should_parse_date()
        {
            var t = new RegexpDateTokenizer();
            var c = t.Parse("2020-01-03");
            Assert.Equal(new DateTime(2020, 1, 3), (DateTime)c.Token);
        }
        
        [Fact]
        public void T02_Should_parse_date()
        {
            var t = new ManualDateTokenizer();
            var c = t.Parse("2020-01-03");
            Assert.Equal(new DateTime(2020, 1, 3), (DateTime)c.Token);
        }
        
        [Fact]
        public void T03_Should_handle_overflow_excption()
        {
            var t = new RegexpDateTokenizer();
            var c = t.Parse("2020-31-03");
            Assert.Null(c);
        }
        
        [Fact]
        public void T04_Should_handle_overflow_excption()
        {
            var t = new ManualDateTokenizer();
            var c = t.Parse("2020-31-03");
            Assert.Null(c);
        }
    }
}