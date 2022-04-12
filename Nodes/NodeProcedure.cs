using System;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeProcedure : Node {
        public override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.ProcedureTranslator(gen, this);
        }

        public override string ToString(CodeGenerator generator)
        {
            if (param < generator.Type.ProcedureTable.Length) {
                return generator.Type.ProcedureTable[param];
            }

            return "UnknownProcedure_" + param;
        }

        protected override void UpdateTypeHints(CodeGenerator gen)
        {
            gen.Context.TypeHints.ProcedureTypeHints.UpdateParams(ToString(gen), Children, gen);
        }
    }
}
