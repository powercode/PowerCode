using System.Diagnostics;
using System.Management.Automation;
using System.Threading;

namespace PowerCode
{
    [OutputType(typeof(ProcessOutput))]
    [Cmdlet(VerbsLifecycle.Start, "ProcessEx")]
    public class StartProcessExCommand : PSCmdlet
    {
        [Parameter(Mandatory = true)]
        public string FilePath { get; set; }

        [Parameter]
        public string[] ArgumentList { get; set; }

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public SwitchParameter NoStandardError { get; set; }
        public SwitchParameter NoStandardOutput { get; set; }

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
