using System;

namespace Shorthand.RssFilter.Models {
    public class FeedConfiguration {
        public FeedConfiguration() {
            Items = new FeedItem[0];
            FilterGroups = new FilterGroup[0];
        }

        public Uri Uri { get; set; }
        public FeedItem[] Items { get; set; }
        public FilterGroup[] FilterGroups { get; set; }
    }
}