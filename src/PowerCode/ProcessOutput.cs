using System;
using System.Diagnostics;

namespace PowerCode {
    [DebuggerDisplay("{DebuggerDisplay},nq")]
    public struct ProcessOutput {
        public OutputKind Kind { get; }
        private readonly string _stringValue;
        private readonly int _intValue;

        private ProcessOutput(string value, OutputKind kind) {
            Kind = kind;
            _stringValue = value;
            _intValue = 0;
        }

        private ProcessOutput(int exitCode) {
            Kind = OutputKind.ExitCode;
            _intValue = exitCode;
            _stringValue = null;
        }

        internal static ProcessOutput CreateOutput(string value) {
            return new ProcessOutput(value, OutputKind.Output);
        }

        internal static ProcessOutput CreateError(string value) {
            return new ProcessOutput(value, OutputKind.Error);
        }

        internal static ProcessOutput CreateExitCode(int exitCode) {
            return new ProcessOutput(exitCode);
        }

        public int ExitCode => Kind == OutputKind.ExitCode ? _intValue : throw new InvalidOperationException("Kind is not 'ExitCode'");

        public string Output => Kind == OutputKind.Output ? _stringValue : throw new InvalidOperationException("Kind is not 'Output'");

        public string Error => Kind == OutputKind.Error ? _stringValue : throw new InvalidOperationException("Kind is not 'Error'");

        public override string ToString() {
            switch (Kind) {
                case OutputKind.None:
                    return string.Empty;
                case OutputKind.Output:

                case OutputKind.Error:
                    return _stringValue;
                case OutputKind.ExitCode:
                    return _intValue.ToString();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string DebuggerDisplay {
            get {
                switch (Kind) {
                    case OutputKind.None: return "None";
                    case OutputKind.Output: return $"O: {_stringValue}";
                    case OutputKind.Error: return $"E: {_stringValue}";

                    case OutputKind.ExitCode: return $"Exit: {_intValue}";
                    default:
                        return string.Empty;
                }
            }
        }
    }
}