using System;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeCondition : Node {
        public override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.ConditionTranslator(gen, this);
        }

        public override string ToString(CodeGenerator generator)
        {
            return generator.Type.ConditionTable[param];
        }

        protected override void UpdateTypeHints(CodeGenerator gen)
        {
            gen.Context.TypeHints.ConditionTypeHints.UpdateParams(ToString(gen), Children, gen);
        }
    }
}
