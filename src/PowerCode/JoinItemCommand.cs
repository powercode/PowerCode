using System.Collections;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Language;
using PowerCode;

namespace JoinItem {
    [Cmdlet(VerbsCommon.Join, "Item", DefaultParameterSetName = "Default")]
    [Alias("ji")]
    public class JoinItemCommand : Cmdlet {
        // ReSharper disable once CollectionNeverQueried.Local
        private readonly List<PSObject> _inputObjects = new List<PSObject>(50);

        [Parameter(Position = 0)]
        [ArgumentCompleter(typeof(JoinItemCompleter))]
        public string Delimiter { get; set; } = ",";

        [Parameter(ParameterSetName = "Quote")]
        public SwitchParameter Quote { get; set; }

        [Parameter(ParameterSetName = "DoubleQuote")]
        public SwitchParameter DoubleQuote { get; set; }

        // This should be completed with property names from inputobject. Not yet exposed by engine
        [Parameter]
        public string PropertyName { get; set; }

        [Parameter(ValueFromPipeline = true)]
        public PSObject[] InputObject { get; set; }

        protected override void ProcessRecord() {
            _inputObjects.AddRange(InputObject);
        }

        protected override void EndProcessing() {
            var res = new string[_inputObjects.Count];
            for (var i = 0; i < _inputObjects.Count; i++) {
                var inputObject = _inputObjects[i];
                var value = string.IsNullOrEmpty(PropertyName)
                    ? inputObject.ToString()
                    : inputObject.GetPropertyValue(PropertyName)?.ToString() ?? string.Empty;
                if (Quote)
                    value = $"'{value}'";
                else if (DoubleQuote)
                    value = $"\"{value}\"";
                res[i] = value;
            }
            var joined = string.Join(Delimiter, res);
            WriteObject(joined, false);
        }
    }

    public class JoinItemCompleter : IArgumentCompleter {
        public IEnumerable<CompletionResult> CompleteArgument(string commandName, string parameterName,
            string wordToComplete, CommandAst commandAst,
            IDictionary fakeBoundParameters) {
            var res = new List<CompletionResult>(10);
            res.AddMatching(wordToComplete, "', '", "Comma-Space", "', ' - Comma-Space");
            res.AddMatching(wordToComplete, "';'", "Semi-Colon", "';'  - Semi-Colon ");
            res.AddMatching(wordToComplete, "'; '", "Semi-Colon-Space", "'; ' - Semi-Colon-Space");
            res.AddMatching(wordToComplete, "\"`r`n\"", "Newline", "`r`n - Newline");
            res.AddMatching(wordToComplete, "','", "Comma", "','  - Comma");
            res.AddMatching(wordToComplete, "'-'", "Dash", "'-'  - Dash");
            res.AddMatching(wordToComplete, "' '", "Space", "' '  - Space");
            return res;
        }
    }
}