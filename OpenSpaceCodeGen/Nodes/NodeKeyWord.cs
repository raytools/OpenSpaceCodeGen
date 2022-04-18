using System;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeKeyWord : Node {
        public override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.KeywordTranslator(gen, this);
        }

        public EnumKeyword GetKeyword(CodeGenerator generator)
        {
            return generator.Type.KeywordTable[param];
        }

        public override string ToString(CodeGenerator generator)
        {
            return GetKeyword(generator).ToString();
        }
    }
}
