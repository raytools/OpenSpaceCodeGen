using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeEndMacro : Node {
        public override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return NodeTranslator.Sequence();
        }

        public override string ToString(CodeGenerator gen)
        {
            return this.GetType().Name;
        }
    }
}
