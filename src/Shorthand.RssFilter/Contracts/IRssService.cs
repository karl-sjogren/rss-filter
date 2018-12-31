using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Shorthand.RssFilter.Models;

namespace Shorthand.RssFilter.Contracts {
    public interface IRssService {
        Task<XDocument> GetFilteredFeed(FeedConfiguration feedConfiguration);
    }
}