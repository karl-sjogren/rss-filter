using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Shorthand.RssFilter.Extensions {
    public static class XDocumentExtensions {
        public static string FirstValueOrDefault(this IEnumerable<XElement> enumerable, Func<XElement, bool> predicate) {
            var first = enumerable.FirstOrDefault(predicate);
            if (first == null)
                return default(string);

            return first.Value;
        }

        public static string FirstValueOrDefault(this IEnumerable<XElement> enumerable) {
            var first = enumerable.FirstOrDefault();
            if (first == null)
                return default(string);

            return first.Value;
        }

        public static string FirstValueOrDefault(this IEnumerable<XAttribute> enumerable, Func<XAttribute, bool> predicate) {
            var first = enumerable.FirstOrDefault(predicate);
            if (first == null)
                return default(string);

            return first.Value;
        }

        public static string FirstValueOrDefault(this IEnumerable<XAttribute> enumerable) {
            var first = enumerable.FirstOrDefault();
            if (first == null)
                return default(string);

            return first.Value;
        }
    }
}