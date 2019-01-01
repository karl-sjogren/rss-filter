using Newtonsoft.Json;
using Shorthand.RssFilter.Models;
using Xunit;
using Shorthand.RssFilter.Services;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Shorthand.RssFilter.Tests {
    public class RssFilterServiceTests {
        private readonly RssFilterService _service;
        private readonly XDocument _document;

        public RssFilterServiceTests() {
            _service = new RssFilterService();
        }

        private async Task<XDocument> GetFeedFromResource(string resourceName) {
            var xml = await Resources.GetStringResource(resourceName);
            return XDocument.Parse(xml);
        }

        [Fact]
        public void TestNullConfigurationThrows() {
            Assert.Throws<ArgumentNullException>(() => _service.FilterFeed(null, new XDocument()));
        }

        [Fact]
        public void TestNullDocumentThrows() {
            Assert.Throws<ArgumentNullException>(() => _service.FilterFeed(new FeedConfiguration(), null));
        }

        [Fact]
        public async Task TestEmptyConfiguration() {
            var document = await GetFeedFromResource("Aftonbladet.xml");
            
            _service.FilterFeed(new FeedConfiguration(), document);

            var itemCount = document.Descendants("item").Count();
            Assert.Equal(0, itemCount);
        }
    }
}