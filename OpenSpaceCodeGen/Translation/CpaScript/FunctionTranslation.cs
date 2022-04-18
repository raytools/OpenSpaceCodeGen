using System;
using System.Collections.Generic;
using System.Text;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Nodes;

namespace OpenSpaceCodeGen.Translation.CpaScript {
    public static class FunctionTranslation {

        public static NodeTranslator TranslateFunction(CodeGenerator gen, NodeFunction node)
        {
            return NodeTranslator.Sequence(node.ToString(gen), "(", TranslateAction.VisitChildren(", "), ")");
        }
    }
}
