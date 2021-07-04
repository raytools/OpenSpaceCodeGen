using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSpaceCodeGen.Translation;
using OpenSpaceCodeGen.Util;

namespace OpenSpaceCodeGen.Concatenators {
    public class ConcatenatorCSharp : Concatenator {



        public override string ConcatenateScripts(string[] scripts, string name)
        {
            string nl = Environment.NewLine;

            string code = string.Join(Environment.NewLine, scripts);
            string method = $"private async Task {name}(){nl}" +
                            $"{{{nl}" +
                            $"{code.IndentMultilineString(LanguageTranslation.IndentSize)}{nl}" +
                            $"}}";

            return method;
        }

        public override string ConcatenateBehaviours(string[] behaviours)
        {
            return string.Join(Environment.NewLine.Repeat(2), behaviours);
        }

        public override string ConcatenateAIModel(string modelName, string rules, string reflexes)
        {
            string code = string.Join(Environment.NewLine, rules, reflexes);

            string nl = Environment.NewLine;

            string classStr = $"public class {modelName}: Perso{nl}" +
                              $"{{{nl}" +
                              $"{code.IndentMultilineString(LanguageTranslation.IndentSize)}{nl}" +
                              $"}}";

            return classStr;
        }
    }
}
