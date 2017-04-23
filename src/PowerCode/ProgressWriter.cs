namespace PowerCode {
    public class ProgressWriter {
        private readonly Progress _progress;
        private readonly IProgressWriter _progressWriter;

        public ProgressWriter(IProgressWriter progressWriter, string activity, string statusDescription, long totalItemCount)
            : this(progressWriter, new Progress(activity, statusDescription, totalItemCount)) { }

        public ProgressWriter(IProgressWriter progressWriter, string activity, string statusDescription, long totalItemCount, int activityId)
            : this(progressWriter, new Progress(activity, statusDescription, totalItemCount, activityId)) { }


        public ProgressWriter(IProgressWriter progressWriter, string activity, string statusDescription, long totalItemCount, int activityId,
            int parentActivityId)
            : this(progressWriter, new Progress(activity, statusDescription, totalItemCount, activityId, parentActivityId)) { }

        private ProgressWriter(IProgressWriter progressWriter, Progress progress) {
            _progressWriter = progressWriter;
            _progress = progress;
        }

        public void WriteNext(long currentItemIndex, string currentOperation) {
            _progressWriter.WriteProgress(_progress.Next(currentItemIndex, currentOperation));
        }

        public void WriteCompleted() {
            _progressWriter.WriteProgress(_progress.Completed);
        }
    }
}