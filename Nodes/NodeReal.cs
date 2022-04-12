using System;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeReal : Node
    {
        public override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.RealTranslator(gen, this);
        }

        public override string ToString(CodeGenerator gen)
        {
            return BitConverter.ToSingle(BitConverter.GetBytes(param), 0).ToString();
        }
    }


}
