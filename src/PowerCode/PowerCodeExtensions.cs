using System.Management.Automation;

namespace PowerCode {
    public static class PowerCodeExtensions {
        public static ProgressWriter CreateProgressWriter(this Cmdlet cmdlet, string activity, string statusDescription, long totalItemCount) {
            return CreateProgressWriter(cmdlet, activity, statusDescription, totalItemCount, 1);
        }

        public static ProgressWriter CreateProgressWriter(this Cmdlet cmdlet, string activity, string statusDescription, long totalItemCount, int activityId) {
            return CreateProgressWriter(cmdlet, activity, statusDescription, totalItemCount, activityId, -1);
        }

        public static ProgressWriter CreateProgressWriter(this Cmdlet cmdlet, string activity, string statusDescription, long totalItemCount, int activityId,
            int parentActivityId) {
            var cmdletWriter = new CmdletProgressWriter(cmdlet);
            var progressWriter = new ProgressWriter(cmdletWriter, activity, statusDescription, totalItemCount, activityId, parentActivityId);
            return progressWriter;
        }
    }
}