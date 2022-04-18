using System;
using System.Collections.Generic;
using System.Text;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Nodes;

namespace OpenSpaceCodeGen.Translation.CBase {
    public static class FunctionTranslation {

        public static NodeTranslator TranslateFunction(CodeGenerator gen, NodeFunction node)
        {
            string ternaryCheck = "";

            switch (node.ToString(gen)) {
                // Ternary real operators (e.g. x > y ? true : false)
                case Functions.Func_TernInf:
                case Functions.Func_TernSup:
                case Functions.Func_TernEq:
                case Functions.Func_TernInfEq:
                case Functions.Func_TernSupEq:
                    switch (node.ToString(gen)) {
                        case Functions.Func_TernInf: ternaryCheck = " < "; break;
                        case Functions.Func_TernSup: ternaryCheck = " > "; break;
                        case Functions.Func_TernEq: ternaryCheck = " == "; break;
                        case Functions.Func_TernInfEq: ternaryCheck = " <= "; break;
                        case Functions.Func_TernSupEq: ternaryCheck = " >= "; break;
                    }
                        
                    return NodeTranslator.Sequence("((", 0,ternaryCheck, 1, ") ? ", 2, " : ", 3, ")");

                    case Functions.Func_TernOp: // conditional ternary operator (cond ? true : false)

                    string childCast1 = "";
                    string childCast2 = "";

                    if (node.Children[1] is NodeDsgVarRef && node.Children[2] is NodeReal) {
                        childCast1 = "(float)";
                    }
                    if (node.Children[2] is NodeDsgVarRef && node.Children[1] is NodeReal) {
                        childCast2 = "(float)";
                    }

                    return NodeTranslator.Sequence("((", 0, ") ? ", childCast1, 1, " : ", childCast2, 2, ")");

                default:
                    return NodeTranslator.Sequence(node.ToString(gen), "(", TranslateAction.VisitChildren(", "), ")");
            }

        }
    }
}
