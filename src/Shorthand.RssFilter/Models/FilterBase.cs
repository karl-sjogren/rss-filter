using System;
using System.Text.RegularExpressions;

namespace Shorthand.RssFilter.Models {
    public abstract class FilterBase {
        protected FilterBase(string type) {
            Type = type;
        }

        public string Type { get; }
        public MatchType MatchOn { get; set; }
        public string Value { get; set; }
        public bool Negative { get; set; }

        public abstract bool Evaluate(string input);
    }

    public class StartsWithFilter : FilterBase {
        public StartsWithFilter() : base("StartsWith") { }

        public override bool Evaluate(string input) {
            return input?.StartsWith(Value, StringComparison.OrdinalIgnoreCase) == true;
        }
    }

    public class EndsWithFilter : FilterBase {
        public EndsWithFilter() : base("EndsWith") { }

        public override bool Evaluate(string input) {
            return input?.EndsWith(Value, StringComparison.OrdinalIgnoreCase) == true;
        }
    }

    public class ContainsFilter : FilterBase {
        public ContainsFilter() : base("Contains") { }

        public override bool Evaluate(string input) {
            return input?.Contains(Value, StringComparison.OrdinalIgnoreCase) == true;
        }
    }

    public class MatchesFilter : FilterBase {
        public MatchesFilter() : base("Matches") { }

        public override bool Evaluate(string input) {
            return input?.Equals(Value, StringComparison.OrdinalIgnoreCase) == true;
        }
    }

    public class RegexFilter : FilterBase {
        private Regex _expression;

        public RegexFilter() : base("Regex") { }

        public override bool Evaluate(string input) {
            if(_expression == null)
                _expression = new Regex(Value, RegexOptions.Compiled | RegexOptions.CultureInvariant);

            return _expression.IsMatch(input ?? string.Empty);
        }
    }
}