using System;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeConstant : Node {

        protected override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.ConstantTranslator(gen, this);
        }

        public override string ToString(CodeGenerator gen)
        {
            return param.ToString();
        }
    }
}
