using System;
using System.Linq;
using System.Xml.Linq;
using Shorthand.RssFilter.Contracts;
using Shorthand.RssFilter.Extensions;
using Shorthand.RssFilter.Models;

namespace Shorthand.RssFilter.Services {
    public class RssFilterService : IRssFilterService {
        public void FilterFeed(FeedConfiguration feedConfiguration, XDocument document) {
            _ = feedConfiguration ?? throw new ArgumentNullException(nameof(feedConfiguration));
            _ = document ?? throw new ArgumentNullException(nameof(document));

            var items = document.Descendants("item").ToArray();

            foreach(var item in items) {
                var keepItem = false;

                var title = item.Descendants("title").FirstValueOrDefault();
                var description = item.Descendants("description").FirstValueOrDefault();
                var category = item.Descendants("category").FirstValueOrDefault();
                var link = item.Descendants("link").FirstValueOrDefault();

                foreach(var feedItem in feedConfiguration.Items) {
                    if(keepItem)
                        continue;

                    var allPassed = true;

                    foreach(var filter in feedItem.Filters.Concat(feedConfiguration.Filters)) {
                        if(filter == null)
                            continue;

                        var input = string.Empty;
                        switch(filter.MatchOn) {
                            case MatchType.Category:
                                input = category;
                                break;
                            case MatchType.Description:
                                input = description;
                                break;
                            case MatchType.Link:
                                input = link;
                                break;
                            case MatchType.Title:
                                input = title;
                                break;
                        }

                        var result = filter.Evaluate(input);

                        if(!result && !filter.Negative) {
                            allPassed = false;
                        } else if(result && filter.Negative) {
                            allPassed = false;
                        }
                    }

                    if(allPassed) {
                        keepItem = true;
                        break;
                    }
                }

                if(!keepItem)
                    item.Remove();
            }
        }
    }
}