using System.Collections.Generic;
using System.IO;
using System.Management.Automation;
using Microsoft.PowerShell.Commands;

namespace PowerCode {
    public static class Paths {
        public static PathResult[] ResolveLiteralPaths(string[] paths, PathIntrinsics pathIntrinsics) {
            var retVal = new PathResult[paths.Length];
            for (var i = 0; i < retVal.Length; i++) {
                var aPath = paths[i];
                ProviderInfo provider;
                PSDriveInfo drive;
                var unresolved = pathIntrinsics.GetUnresolvedProviderPathFromPSPath(aPath, out provider, out drive);
                if (provider.ImplementingType != typeof(FileSystemProvider)) retVal[i] = new PathResult(CreateNotAFileSystemPath(aPath));
                else if (!File.Exists(unresolved)) retVal[i] = new PathResult(CreatePathNotFoundErrorRecord(aPath));
                else retVal[i] = new PathResult(unresolved);
            }
            return retVal;
        }

        public static List<PathResult> ResolvePaths(string[] paths, PathIntrinsics pathIntrinsics) {
            var retVal = new List<PathResult>(paths.Length);
            for (var index = 0; index < paths.Length; index++) {
                var aPath = paths[index];
                ProviderInfo provider;
                var resolvedPaths = pathIntrinsics.GetResolvedProviderPathFromPSPath(aPath, out provider);
                if (resolvedPaths.Count == 0) retVal.Add(new PathResult(CreateWildcardingFailureErrorRecord(aPath)));
                else
                    for (var i = 0; i < resolvedPaths.Count; i++) {
                        var resolved = resolvedPaths[i];
                        retVal.Add(new PathResult(resolved));
                    }
            }
            return retVal;
        }

        public static string[] ResolveNonExistingPaths(string[] paths, PathIntrinsics pathIntrinsics) {
            var retVal = new string[paths.Length];
            for (var i = 0; i < retVal.Length; i++) retVal[i] = pathIntrinsics.GetUnresolvedProviderPathFromPSPath(paths[i]);
            return retVal;
        }

        private static ErrorRecord CreatePathNotFoundErrorRecord(string path) {
            var ex = new ItemNotFoundException("Cannot find path 'path' because it does not exist.");
            var errRecord = new ErrorRecord(ex, "PathNotFound", ErrorCategory.ObjectNotFound, path);
            return errRecord;
        }

        internal static ErrorRecord CreateWildcardingFailureErrorRecord(string filePath) {
            var msg = $"The path '{filePath}' did not resolve to any existing file.";

            var errorRecord = new ErrorRecord(
                new FileNotFoundException(msg, filePath),
                "FileOpenFailure",
                ErrorCategory.OpenError,
                filePath) {ErrorDetails = new ErrorDetails(msg)};

            return errorRecord;
        }

        private static ErrorRecord CreateNotAFileSystemPath(string path) {
            var ex = new ItemNotFoundException("The path is not a file system path.");
            var category = ErrorCategory.InvalidArgument;
            var errRecord = new ErrorRecord(ex, "PathNotFileSystemPath", category, path);
            return errRecord;
        }
    }
}