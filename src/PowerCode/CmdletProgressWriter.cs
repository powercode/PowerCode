using System.Management.Automation;

namespace PowerCode {
    public class CmdletProgressWriter : IProgressWriter {
        private readonly Cmdlet _cmdlet;

        public CmdletProgressWriter(Cmdlet cmdlet) {
            _cmdlet = cmdlet;
        }

        public void WriteProgress(ProgressRecord progressRecord) {
            _cmdlet.WriteProgress(progressRecord);
        }
    }
}