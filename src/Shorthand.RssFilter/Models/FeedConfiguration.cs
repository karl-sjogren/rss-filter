using System;

namespace Shorthand.RssFilter.Models {
    public class FeedConfiguration {
        public FeedConfiguration() {
            Items = new FeedItem[0];
            Filters = new FilterBase[0];
        }

        public Uri Uri { get; set; }
        public FeedItem[] Items { get; set; }
        public FilterBase[] Filters { get; set; }
    }
}