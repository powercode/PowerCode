using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace PowerCode {
    public class ProcessRunner
    {
        private readonly ConcurrentQueue<ProcessOutput> _outputQueue = new ConcurrentQueue<ProcessOutput>();

        public IEnumerable<ProcessOutput> Run(string file, string arguments) {
            return Run(file, arguments, CancellationToken.None);
        }

        public IEnumerable<ProcessOutput> Run(string file, string arguments, CancellationToken cancellationToken)
        {
            return Run(file, arguments, cancellationToken, new ProcessStartOptions());

        }

        public IEnumerable<ProcessOutput> Run(string file, string arguments, CancellationToken cancellationToken, ProcessStartOptions options) {
            var redirectStandardError = options.RedirectStandardError;
            var redirectStandardOutput = options.RedirectStandardOutput;
            var psi = new ProcessStartInfo(file, arguments) {
                RedirectStandardError = redirectStandardError,
                RedirectStandardOutput = redirectStandardOutput,
                UseShellExecute = false,
                CreateNoWindow = true,
                ErrorDialog = false
            };
            using (var process = Process.Start(psi)) {
                if (process == null)
                    throw new Exception("Failed to start process. Unknown error.");
                if (redirectStandardError) {
                    process.ErrorDataReceived += ProcessOnErrorDataReceived;
                    process.BeginErrorReadLine();
                }
                if (redirectStandardOutput) {
                    process.OutputDataReceived += ProcessOnOutputDataReceived;
                    process.BeginOutputReadLine();
                }
                do {
                    if (!_outputQueue.IsEmpty) {
                        while (_outputQueue.TryDequeue(out ProcessOutput po)) {
                            yield return po;
                            if (cancellationToken.IsCancellationRequested)
                            {
                                yield break;
                            }
                        }
                    }

                    Thread.Sleep(options.SleepTime);
                } while (!process.HasExited);
                process.WaitForExit();
                if (!_outputQueue.IsEmpty) {
                    while (_outputQueue.TryDequeue(out ProcessOutput po)) {
                        yield return po;
                    }
                }

                yield return ProcessOutput.CreateExitCode(process.ExitCode);
            }
        }


        private void ProcessOnErrorDataReceived(object sender, DataReceivedEventArgs e) {
            if (e.Data != null) _outputQueue.Enqueue(ProcessOutput.CreateError(e.Data));
        }

        private void ProcessOnOutputDataReceived(object sender, DataReceivedEventArgs e) {
            if (e.Data != null) _outputQueue.Enqueue(ProcessOutput.CreateOutput(e.Data));
        }
    }
}