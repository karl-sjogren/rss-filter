using Newtonsoft.Json;
using Shorthand.RssFilter.Models;
using Xunit;
using Shorthand.RssFilter.Services;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Shorthand.RssFilter.Tests {
    public class FilterTests {
        [Theory]

        [InlineData("StartsWith", "abc", null, false)]
        [InlineData("StartsWith", "abc", "", false)]
        [InlineData("StartsWith", "abc", "abcdef", true)]
        [InlineData("StartsWith", "ABC", "abcdef", true)]
        [InlineData("StartsWith", "ABC", "def", false)]

        [InlineData("EndsWith", "def", null, false)]
        [InlineData("EndsWith", "def", "", false)]
        [InlineData("EndsWith", "def", "abcdef", true)]
        [InlineData("EndsWith", "DEF", "abcdef", true)]
        [InlineData("EndsWith", "DEF", "abc", false)]

        [InlineData("Matches", "abc", null, false)]
        [InlineData("Matches", "abc", "", false)]
        [InlineData("Matches", "abc", "abc", true)]
        [InlineData("Matches", "abc", "abcdef", false)]
        [InlineData("Matches", "abc", " abc ", false)]

        [InlineData("Contains", "abc", null, false)]
        [InlineData("Contains", "abc", "", false)]
        [InlineData("Contains", "abc", "abc", true)]
        [InlineData("Contains", "abc", "abcdef", true)]
        [InlineData("Contains", "abc", " abc ", true)]
        [InlineData("Contains", "abc", "def", false)]

        [InlineData("Regex", "abc", null, false)]
        [InlineData("Regex", "abc", "", false)]
        [InlineData("Regex", "abc", "abc", true)]
        [InlineData("Regex", "abc", "abcdef", true)]
        [InlineData("Regex", "abc", " abc ", true)]
        [InlineData("Regex", "^abc", " abc ", false)]
        [InlineData("Regex", "^abc$", "ABC", false)]
        [InlineData("Regex", "Year \\d{1,4}", "Year 2019", true)]

        public void FilterTest(string filterName, string value, string input, bool expected) {
            var filterTypeName = $"Shorthand.RssFilter.Models.{filterName}Filter, Shorthand.RssFilter";
            var filterType = Type.GetType(filterTypeName);
            var filter = (FilterBase)Activator.CreateInstance(filterType);
            filter.Value = value;

            var result = filter.Evaluate(input);
            Assert.Equal(expected, result);
        }
    }
}