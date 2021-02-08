using System;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeOperator : Node {

        protected override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.OperatorTranslator(gen, this);
        }

        public EnumOperator GetOperator(CodeGenerator generator)
        {
            return generator.Types.OperatorTable[param];
        }

        public override string ToString(CodeGenerator generator)
        {
            return GetOperator(generator).ToString();
        }
    }
}
