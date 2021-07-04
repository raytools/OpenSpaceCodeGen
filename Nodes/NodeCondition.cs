using System;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeCondition : Node {

        protected override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.ConditionTranslator(gen, this);
        }

        public override string ToString(CodeGenerator generator)
        {
            return generator.Type.ConditionTable[param];
        }
    }
}
