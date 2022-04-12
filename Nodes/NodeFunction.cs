using System;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeFunction : Node {
        public override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.FunctionTranslator(gen, this);
        }

        public override string ToString(CodeGenerator generator)
        {
            if (param < generator.Type.FunctionTable.Length) {
                return generator.Type.FunctionTable[param];
            }
            
            return "UnknownFunction_" + param;
        }

        protected override void UpdateTypeHints(CodeGenerator gen)
        {
            gen.Context.TypeHints.FunctionTypeHints.UpdateParams(ToString(gen), Children, gen);
        }
    }
}
