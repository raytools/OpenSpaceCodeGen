using OpenSpaceCodeGen.Nodes.Generic;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeSubRoutine : ReferenceNode {
        public override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.SubroutineTranslator(gen, this);
        }

        public override string ToString(CodeGenerator gen)
        {
            return gen.ReferenceResolver.ResolveFunc.Invoke(this);
        }
    }
}
