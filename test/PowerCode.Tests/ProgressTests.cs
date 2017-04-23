using System.ComponentModel;
using System.Management.Automation;
using Xunit;

namespace PowerCode.Tests {
    [Category("Progress")]
    public class ProgressTests {
        [Theory]
        [InlineData(100, 0, 0)]
        [InlineData(100, 10, 10)]
        [InlineData(100, 101, 100)]
        [InlineData(0, 10, 100)]
        public void PercentCompeleteTests(long totalItemCount, long currentItemIndex, int expectedPercentComplete) {
            var p = new Progress("Activity", "Status", totalItemCount);
            var pr = p.Next(currentItemIndex, "item");
            Assert.Equal(expectedPercentComplete, pr.PercentComplete);
        }

        [Fact]
        public void CompletedIsComplete()
        {
            var p = new Progress("Activity", "Status", 100);
            var pr = p.Completed;
            Assert.Equal(ProgressRecordType.Completed, pr.RecordType);
        }
    }
}