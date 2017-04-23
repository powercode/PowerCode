using System;
using System.Diagnostics;
using System.Management.Automation;

namespace PowerCode {
    public class Progress {
        private readonly string _activity;
        private readonly int _activityId;
        private readonly int _parentActivityId;
        private readonly string _statusDescription;
        private readonly Stopwatch _stopwatch;
        private readonly long _totalItemCount;

        public Progress(string activity, string statusDescription, long totalItemCount)
            : this(activity, statusDescription, totalItemCount, 1) { }

        public Progress(string activity, string statusDescription, long totalItemCount, int activityId)
            : this(activity, statusDescription, totalItemCount, activityId, -1) { }

        public Progress(string activity, string statusDescription, long totalItemCount, int activityId, int parentActivityId) {
            _totalItemCount = totalItemCount;
            _activityId = activityId;
            _parentActivityId = parentActivityId;
            _activity = activity;
            _statusDescription = statusDescription;
            _stopwatch = Stopwatch.StartNew();
        }

        public ProgressRecord Completed => new ProgressRecord(_activityId, _activity, _statusDescription) {RecordType = ProgressRecordType.Completed};

        public ProgressRecord Next(long currentItemIndex, string currentOperation) {
            if (currentItemIndex < 0) throw new ArgumentException("Index must be positive", nameof(currentItemIndex));
            var pr = new ProgressRecord(_activityId, _activity, _statusDescription) {
                CurrentOperation = currentOperation,
                ParentActivityId = _parentActivityId,
                PercentComplete = PercentComplete(currentItemIndex),
                SecondsRemaining = SecondsRemaining(currentItemIndex)
            };
            return pr;
        }

        private int SecondsRemaining(long currentItemIndex) {
            if (_stopwatch.ElapsedMilliseconds < 3000) return -1;
            if (currentItemIndex >= _totalItemCount) return 0;
            return (int) ((_totalItemCount - currentItemIndex) * _stopwatch.Elapsed.TotalSeconds / currentItemIndex);
        }

        private int PercentComplete(long currentItemIndex) {
            if (currentItemIndex >= _totalItemCount) return 100;
            return Math.Min((int) (currentItemIndex * 100 / _totalItemCount), 100);
        }
    }
}