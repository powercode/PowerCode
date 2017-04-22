using System.Diagnostics;

namespace PowerCode {
    public class ProcessStartOptions {
        public bool RedirectStandardOutput { get; set; } = true;
        public bool RedirectStandardError { get; set; } = true;
        public ProcessPriorityClass PriorityClass { get; set; } = ProcessPriorityClass.Normal;
        public int SleepTime { get; set; } = 50;
    }
}