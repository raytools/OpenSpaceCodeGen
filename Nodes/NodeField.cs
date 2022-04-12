using System;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeField : Node {
        public override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.FieldTranslator(gen, this);
        }

        public override string ToString(CodeGenerator generator)
        {
            return generator.Type.FieldTable[param];
        }
    }
}
