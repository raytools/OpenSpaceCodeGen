using System;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeNull : Node {

        protected override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.NullTranslator(gen, this);
        }

        public override string ToString(CodeGenerator generator)
        {
            return "Null";
        }
    }
}
