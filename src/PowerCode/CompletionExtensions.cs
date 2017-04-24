using System;
using System.Collections.Generic;
using System.Management.Automation;

namespace PowerCode {
    public static class CompletionExtensions {

        public static List<CompletionResult> AddMatching(this List<CompletionResult> list, string wordToComplete, string completionText) =>
            AddMatching(list, wordToComplete, completionText, completionText, completionText, CompletionResultType.ParameterValue);

        public static List<CompletionResult> AddMatching(this List<CompletionResult> list, string wordToComplete, string completionText, string listText, string toolTip) =>
            AddMatching(list, wordToComplete, completionText, listText, toolTip, CompletionResultType.ParameterValue);

        public static List<CompletionResult> AddMatching(this List<CompletionResult> list, string wordToComplete, string completionText, string listText, string toolTip, CompletionResultType resultType) {
            if (completionText.StartsWith(wordToComplete, StringComparison.OrdinalIgnoreCase))
                list.Add(new CompletionResult(completionText, listText, resultType, toolTip));
            return list;
        }
    }
}