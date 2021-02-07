using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes.Generic {
    public class ReferenceNode : Node {

        protected override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return NodeTranslator.Sequence(gen.ReferenceResolver.ResolveFunc.Invoke(this));
        }

        public override string ToString(CodeGenerator gen)
        {
            return this.GetType().Name;
        }
    }
}
