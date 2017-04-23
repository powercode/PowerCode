using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Xunit;

namespace PowerCode.Tests
{
    public class ProcessOutputTests
    {
        [Fact]
        public void TestMethod1() {

            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            var exe = Path.Combine(dirPath, "NativeOutputTestExe.exe");

            var runner = new ProcessRunner();
            var output = runner.Run(exe, "10 4").ToArray();
            var actualOutput = output.Where(c => c.Kind == OutputKind.Output).Count();
            var actualError = output.Where(c => c.Kind == OutputKind.Error).Count();
            Assert.Equal(10, actualOutput);
            Assert.Equal(4, actualError);
        }

        [Fact]
        public void LongRunning()
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            var exe = Path.Combine(dirPath, "NativeOutputTestExe.exe");

            var expectedOutput = 1000000;
            var expectedError = 400000;
            var runner = new ProcessRunner();
            var output = runner.Run(exe, $"{expectedOutput} {expectedError}").ToArray();
            var actualOutput = output.Where(c => c.Kind == OutputKind.Output).Count();
            var actualError = output.Where(c => c.Kind == OutputKind.Error).Count();
            Assert.Equal(expectedOutput, actualOutput);
            Assert.Equal(expectedError, actualError);
        }
    }
}
