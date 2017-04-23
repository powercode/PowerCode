using System.Management.Automation;

namespace PowerCode {
    public interface IProgressWriter {
        void WriteProgress(ProgressRecord progressRecord);
    }
}