using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSpaceCodeGen.AIModels;

namespace OpenSpaceCodeGen.Concatenators {
    public class ConcatenatorCpaScript : Concatenator {
        public override string ConcatenateScripts(string[] scripts, string name, Behavior.BehaviorType behaviorType)
        {
            return $";{behaviorType} {name}{Environment.NewLine}" + string.Join(Environment.NewLine, scripts) + $"{Environment.NewLine};End{behaviorType}";
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
