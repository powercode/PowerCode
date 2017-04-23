using System.Management.Automation;

namespace PowerCode {
    public class IncludeExcludeFilter {
        private readonly WildcardPattern[] _excludes;
        private readonly WildcardPattern[] _includes;

        public IncludeExcludeFilter(string[] includes, string[] excludes) {
            if (null != includes) {
                _includes = new WildcardPattern[includes.Length];
                for (var i = 0; i < includes.Length; i++) _includes[i] = new WildcardPattern(includes[i]);
            }

            if (null != excludes) {
                _excludes = new WildcardPattern[excludes.Length];
                for (var i = 0; i < excludes.Length; i++) _excludes[i] = new WildcardPattern(excludes[i]);
            }
        }

        public bool ShouldOutput(string value) {
            if (null != _excludes)
                for (var i = 0; i < _excludes.Length; i++) {
                    var excludePattern = _excludes[i];
                    if (excludePattern.IsMatch(value)) return false; // if any exclude pattern matches, we shouldn't output our value
                }

            if (null != _includes) {
                for (var i = 0; i < _includes.Length; i++) {
                    var includePattern = _includes[i];
                    if (includePattern.IsMatch(value)) return true; // if there are include patterns and any one matches, we should output it
                }
                return false; // we have include filters, but not matches
            }
            return true; // we don't have any filters
        }
    }
}