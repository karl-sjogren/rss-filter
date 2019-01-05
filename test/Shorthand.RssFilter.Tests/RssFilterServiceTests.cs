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
        public async Task TestEmptyConfigurationRemovesAll() {
            var document = await GetFeedFromResource("Aftonbladet.xml");

            _service.FilterFeed(new FeedConfiguration(), document);

            var itemCount = document.Descendants("item").Count();
            Assert.Equal(0, itemCount);
        }

        [Fact]
        public async Task TestMatchAllConfiguration() {
            var document = await GetFeedFromResource("RealAndFake.xml");
            var feedConfiguration = new FeedConfiguration {
                Items = new FeedItem[] {
                    new FeedItem {
                        Filters = new FilterBase[] {
                            new StartsWithFilter { MatchOn = MatchType.Title, Value = "A real" },
                            new EndsWithFilter { MatchOn = MatchType.Description, Value = "real description" },
                            new ContainsFilter { MatchOn = MatchType.Link, Value = "real-url" },
                            new MatchesFilter { MatchOn = MatchType.Category, Value = "news/real" }
                        }
                    }
                }
            };

            _service.FilterFeed(feedConfiguration, document);

            var itemCount = document.Descendants("item").Count();
            Assert.Equal(1, itemCount);
        }

        [Fact]
        public async Task TestMatchAllWithOnlyNamedFilterGroupsConfiguration() {
            var document = await GetFeedFromResource("RealAndFake.xml");
            var feedConfiguration = new FeedConfiguration {
                FilterGroups = new[] {
                    new FilterGroup { 
                    Name = "Named filter",
                        Filters = new FilterBase[] {
                            new StartsWithFilter { MatchOn = MatchType.Title, Value = "A real" },
                            new EndsWithFilter { MatchOn = MatchType.Description, Value = "real description" },
                            new ContainsFilter { MatchOn = MatchType.Link, Value = "real-url" },
                            new MatchesFilter { MatchOn = MatchType.Category, Value = "news/real" }
                        }
                    }
                },
                Items = new FeedItem[] {
                    new FeedItem {
                        Filters = new FilterBase[] {
                            new ImportGroupFilter { Value = "Named filter" }
                        }
                    }
                }
            };

            _service.FilterFeed(feedConfiguration, document);

            var itemCount = document.Descendants("item").Count();
            Assert.Equal(1, itemCount);
        }

        [Fact]
        public async Task TestUndoMatches() {
            var document = await GetFeedFromResource("RealAndFake.xml");
            var feedConfiguration = new FeedConfiguration {
                Items = new FeedItem[] {
                    new FeedItem {
                        Filters = new FilterBase[] {
                            new StartsWithFilter { MatchOn = MatchType.Title, Value = "A real" },
                            new EndsWithFilter { MatchOn = MatchType.Description, Value = "real description" },
                            new ContainsFilter { MatchOn = MatchType.Link, Value = "real-url", Negative = true },
                            new MatchesFilter { MatchOn = MatchType.Category, Value = "news/real" }
                        }
                    }
                }
            };

            _service.FilterFeed(feedConfiguration, document);

            var itemCount = document.Descendants("item").Count();
            Assert.Equal(0, itemCount);
        }
    }
}