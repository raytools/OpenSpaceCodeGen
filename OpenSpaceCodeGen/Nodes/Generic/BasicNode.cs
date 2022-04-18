using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes.Generic {
    public class BasicNode : Node {
        public override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.BasicTranslator(gen, this);
        }

        public override string ToString(CodeGenerator gen)
        {
            return this.GetType().Name;
        }
    }
}
