using OpenSpaceCodeGen.Nodes.Generic;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeCustomBits : BasicNode {
        public override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return NodeTranslator.Sequence($"0x{param:x}");
        }

        public override string ToString(CodeGenerator gen)
        {
            return this.GetType().Name;
        }
    }
}
