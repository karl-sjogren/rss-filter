using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Shorthand.RssFilter.Contracts;
using Shorthand.RssFilter.Models;

namespace Shorthand.RssFilter.Services {
    public class RssService : IRssService {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IRssFilterService _rssFilterService;

        public RssService(IHttpClientFactory clientFactory, IRssFilterService rssFilterService) {
            _clientFactory = clientFactory;
            _rssFilterService = rssFilterService;
        }

        public async Task<XDocument> GetFilteredFeed(FeedConfiguration feedConfiguration) {
            var client = _clientFactory.CreateClient();

            var xml = await client.GetStringAsync(feedConfiguration.Uri);

            var document = XDocument.Parse(xml);

            _rssFilterService.FilterFeed(feedConfiguration, document);

            return document;
        }
    }
}