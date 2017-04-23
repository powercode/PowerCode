using Xunit;

namespace PowerCode.Tests {
    public class IncludeExcludeFilterTests {

        [Theory]
        [InlineData(new[]{"a*"}, null, "ab", true)]
        [InlineData(new[] { "a*" }, new[]{"*b"}, "ab", false)]
        [InlineData(new[] { "a*" }, new[]{"*b"}, "ab", false)]
        [InlineData(new[] { "a*" }, null, "b", false)]
        [InlineData(null, new[]{"a*"}, "b", true)]
        [InlineData(new[] { "a*", "*b*" }, null, "dbc", true)]
        [InlineData(new[] { "a*", "*b*" }, new[]{"*c*"}, "dbc", false)]
        public void IncludeSingle(string[] includes, string[] excludes, string value, bool shouldOutput) {
            var ie = new IncludeExcludeFilter(includes, excludes);
            var actual = ie.ShouldOutput(value);
            Assert.Equal(shouldOutput, actual);
        }
    }
}