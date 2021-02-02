using System;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeProcedure : Node {

        protected override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.ProcedureTranslator(gen, this);
        }

        public override string ToString(CodeGenerator generator)
        {
            return generator.Types.ProcedureTable[param];
        }
    }
}
