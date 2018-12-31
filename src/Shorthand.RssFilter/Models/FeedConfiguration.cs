using System;

namespace Shorthand.RssFilter.Models {
    public class FeedConfiguration {
        public Uri Uri { get; set; }
        public FeedItem[] Items { get; set; }
    }
}