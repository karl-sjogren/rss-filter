using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Shorthand.RssFilter.Models;

namespace Shorthand.RssFilter.Contracts {
    public interface IRssFilterService {
        void FilterFeed(FeedConfiguration feedConfiguration, XDocument document);
    }
}