using System;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeFunction : Node {

        protected override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.FunctionTranslator(gen, this);
        }

        public override string ToString(CodeGenerator generator)
        {
            return generator.Type.FunctionTable[param];
        }
    }
}
