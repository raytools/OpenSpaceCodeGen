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

        /*
        public override void Visit(CodeGenerator codeGenerator)
        {
            var op = codeGenerator.Types.OperatorTable[param];

            switch (op) {
                case EnumOperator.Operator_Plus:
                    codeGenerator.Append("(");
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("+");
                    Children[1].Visit(codeGenerator);
                    codeGenerator.Append(")");
                    break;
                case EnumOperator.Operator_Minus:
                    codeGenerator.Append("(");
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("-");
                    Children[1].Visit(codeGenerator);
                    codeGenerator.Append(")");
                    break;
                case EnumOperator.Operator_Mul:
                    codeGenerator.Append("(");
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("*");
                    Children[1].Visit(codeGenerator);
                    codeGenerator.Append(")");
                    break;
                case EnumOperator.Operator_Div:
                    codeGenerator.Append("(");
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("/");
                    Children[1].Visit(codeGenerator);
                    codeGenerator.Append(")");
                    break;
                case EnumOperator.Operator_UnaryMinus:
                    codeGenerator.Append("-");
                    Children[0].Visit(codeGenerator);
                    break;
                case EnumOperator.Operator_PlusAffect:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("+=");
                    Children[1].Visit(codeGenerator);
                    break;
                case EnumOperator.Operator_MinusAffect:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("-=");
                    Children[1].Visit(codeGenerator);
                    break;
                case EnumOperator.Operator_MulAffect:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("*=");
                    Children[1].Visit(codeGenerator);
                    break;
                case EnumOperator.Operator_DivAffect:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("/=");
                    Children[1].Visit(codeGenerator);
                    break;
                case EnumOperator.Operator_PlusPlusAffect:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("++");
                    break;
                case EnumOperator.Operator_MinusMinusAffect:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("--");
                    break;
                case EnumOperator.Operator_Affect:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("=");
                    Children[1].Visit(codeGenerator);
                    break;
                case EnumOperator.Operator_Dot:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append(".");
                    Children[1].Visit(codeGenerator);
                    break;
                case EnumOperator.Dot_X:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append(".x");
                    break;
                case EnumOperator.Dot_Y:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append(".y");
                    break;
                case EnumOperator.Dot_Z:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append(".z");
                    break;
                case EnumOperator.Operator_VectorPlusVector:
                    codeGenerator.Append("(");
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("+");
                    Children[1].Visit(codeGenerator);
                    codeGenerator.Append(")");
                    break;
                case EnumOperator.Operator_VectorMinusVector:
                    codeGenerator.Append("(");
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("-");
                    Children[1].Visit(codeGenerator);
                    codeGenerator.Append(")");
                    break;
                case EnumOperator.Operator_VectorMulScalar:
                    codeGenerator.Append("(");
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("*");
                    Children[1].Visit(codeGenerator);
                    codeGenerator.Append(")");
                    break;
                case EnumOperator.Operator_VectorDivScalar:
                    codeGenerator.Append("(");
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("/");
                    Children[1].Visit(codeGenerator);
                    codeGenerator.Append(")");
                    break;
                case EnumOperator.Operator_VectorUnaryMinus:
                    codeGenerator.Append("-");
                    Children[0].Visit(codeGenerator);
                    break;
                case EnumOperator.Dot_X_Assign:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append(".x = ");
                    Children[1].Visit(codeGenerator);
                    break;
                case EnumOperator.Dot_Y_Assign:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append(".y = ");
                    Children[1].Visit(codeGenerator);
                    break;
                case EnumOperator.Dot_Z_Assign:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append(".z = ");
                    Children[1].Visit(codeGenerator);
                    break;
                case EnumOperator.Operator_Ultra:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append(".");
                    Children[1].Visit(codeGenerator);
                    break;
                case EnumOperator.Operator_ModelCast:
                    codeGenerator.Append("((");
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append(")(");
                    Children[1].Visit(codeGenerator);
                    codeGenerator.Append(")");
                    break;
                case EnumOperator.Operator_Array:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("[");
                    Children[1].Visit(codeGenerator);
                    codeGenerator.Append("]");
                    break;
                case EnumOperator.Operator_AffectArray:
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("[");
                    Children[1].Visit(codeGenerator);
                    codeGenerator.Append("] = ");
                    Children[2].Visit(codeGenerator);
                    break;
                case EnumOperator.Operator_Mod:
                    codeGenerator.Append("(");
                    Children[0].Visit(codeGenerator);
                    codeGenerator.Append("%");
                    Children[1].Visit(codeGenerator);
                    codeGenerator.Append(")");
                    break;
            }
        }*/

    }
}
