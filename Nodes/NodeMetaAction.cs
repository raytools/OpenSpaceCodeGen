using System;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeMetaAction : Node {

        protected override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.MetaActionTranslator(gen, this);
        }

        public override string ToString(CodeGenerator generator)
        {
            return generator.Types.MetaActionTable[param];
        }
    }
}
