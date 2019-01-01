namespace Shorthand.RssFilter.Models {
    public class FeedItem {
        public FeedItem() {
            Filters = new FilterBase[0];
        }

        public string Name { get; set; }
        public FilterBase[] Filters { get; set; }
    }
}