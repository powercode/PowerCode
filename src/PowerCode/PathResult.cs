using System;
using System.Management.Automation;

namespace PowerCode {
    public class PathResult {
        private readonly object _result;

        internal PathResult(string path) {
            _result = path;
        }

        internal PathResult(ErrorRecord errorRecord) {
            _result = errorRecord;
        }

        public bool IsError => _result is ErrorRecord;

        public string Path {
            get {
                switch (_result) {
                    case string s: return s;
                    default: throw new InvalidOperationException("Result is not a path");
                }
            }
        }

        public ErrorRecord Error {
            get {
                switch (_result) {
                    case ErrorRecord er: return er;
                    default: throw new InvalidOperationException("Result is not an ErrorRecord");
                }
            }
        }
    }
}