using Newtonsoft.Json;
using Shorthand.RssFilter.Serialization;
using Shorthand.RssFilter.Models;
using Xunit;

namespace Shorthand.RssFilter.Tests {
    public class FilterBaseConverterTests {
        [Fact]
        public void TestCantWrite() {
            var converter = new FilterBaseConverter();
            Assert.False(converter.CanWrite);
        }

        [Fact]
        public void TestReadNull() {
            var json = @"null";
            var result = JsonConvert.DeserializeObject<FilterBase>(json, new FilterBaseConverter());

            Assert.Null(result);
        }

        [Fact]
        public void TestReadContainsFilter() {
            var json = @"{
                ""MatchOn"": ""Title"",
                ""Type"": ""Contains"",
                ""Value"": ""zebra""
              }";

            var result = JsonConvert.DeserializeObject<FilterBase>(json, new FilterBaseConverter());

            Assert.NotNull(result);
            Assert.Equal(typeof(ContainsFilter), result.GetType());
        }
    }
}