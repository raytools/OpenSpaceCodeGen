using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSpaceCodeGen.Concatenators {
    public class ConcatenatorRaw : Concatenator {
        public override string ConcatenateScripts(string[] scripts, string name)
        {
            return string.Join(Environment.NewLine, scripts);
        }

        public override string ConcatenateBehaviours(string[] behaviours)
        {
            return string.Join(Environment.NewLine, behaviours);
        }

        public override string ConcatenateAIModel(string modelName, string rules, string reflexes)
        {
            return string.Join(Environment.NewLine, rules, reflexes);
        }
    }
}
