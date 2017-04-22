using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NativeOutputTestExe
{
    public class Program
    {
        static int Main(string[] args)
        {
            switch (args.Length) {
                case 2:
                    WriteOutput(Int32.Parse(args[0]), Int32.Parse(args[1]));
                    return 0;
                case 3:
                    WriteOutput(Int32.Parse(args[0]), Int32.Parse(args[1]), Int32.Parse(args[2]));
                    return 0;
                default:
                Console.Error.WriteLine($"Usage: NativeOutputTestExe <outputCount> <errorCount> [<progressCount>]");
                    return 1;
            }
        }

        private static void WriteOutput(int outputCount, int errorCount, int processCount = 0) {
            var rnd = new Random(DateTime.Now.TimeOfDay.Seconds);
            while (outputCount != 0 || errorCount != 0 || processCount != 0) {
                var i  = rnd.Next(0, 2);
                switch (i) {
                    case 0:
                        if (outputCount > 0) {
                            Console.WriteLine($"Output: {outputCount}");
                            --outputCount;
                            break;
                        }
                        goto case 1;
                    case 1:
                        if (errorCount > 0)
                        {
                            Console.Error.WriteLine($"Error: {errorCount}");
                            --errorCount;
                            break;
                        }
                        goto default;
                    default:
                        if (processCount > 0) {
                            Console.Error.WriteLine($"Progress: {processCount}");
                            --processCount;
                        }
                        else {
                            goto case 0;
                        }
                    break;
                }
            }
        }
    }
}
