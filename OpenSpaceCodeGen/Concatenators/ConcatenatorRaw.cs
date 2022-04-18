using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSpaceCodeGen.AIModels;

namespace OpenSpaceCodeGen.Concatenators {
    public class ConcatenatorRaw : Concatenator {
        public override string ConcatenateScripts(string[] scripts, string name, Behavior.BehaviorType behaviorType)
        {
            return string.Join(Environment.NewLine, scripts);
        }

        public override string ConcatenateBehaviours(string[] behaviours)
        {
            return string.Join(Environment.NewLine, behaviours);
        }

        public override string ConcatenateAIModel(AIModelMetaData metaData, string rules, string reflexes,
            string macros)
        {
            return string.Join(Environment.NewLine, rules, reflexes, macros);
        }
    }
}
