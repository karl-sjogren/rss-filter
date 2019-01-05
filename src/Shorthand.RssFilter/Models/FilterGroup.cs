namespace Shorthand.RssFilter.Models {
    public class FilterGroup {
        public FilterGroup() {
            Filters = new FilterBase[0];
        }

        public string Name { get; set; }
        public FilterBase[] Filters { get; set; }
    }
}