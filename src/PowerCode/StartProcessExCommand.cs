using System.Diagnostics;
using System.Management.Automation;
using System.Threading;

namespace PowerCode {
    [Cmdlet(VerbsLifecycle.Start, "ProcessEx")]
    [OutputType(typeof(ProcessOutput))]
    public class StartProcessExCommand : PSCmdlet {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        [Parameter(Mandatory = true, Position = 0)]
        public string FilePath { get; set; }

        [Parameter(Position = 1)]
        public string[] ArgumentList { get; set; }

        [Parameter]
        public SwitchParameter NoStandardError { get; set; }

        [Parameter]
        public SwitchParameter NoStandardOutput { get; set; }

        [Parameter]
        public ProcessPriorityClass Priority { get; set; }


        protected override void EndProcessing() {
            FilePath = GetUnresolvedProviderPathFromPSPath(FilePath);
            var runner = new ProcessRunner();
            var options = new ProcessStartOptions {
                RedirectStandardOutput = !NoStandardOutput,
                RedirectStandardError = !NoStandardError,
                PriorityClass = Priority
            };

            var res = runner.Run(FilePath, string.Join(" ", ArgumentList), _cts.Token, options);
            WriteObject(res, true);
        }

        protected override void StopProcessing() {
            _cts.Cancel();
        }
    }
}