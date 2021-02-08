using System;
using System.Collections.Generic;
using System.Text;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Nodes;

namespace OpenSpaceCodeGen.Translation.CBase
{
    public static class OperatorTranslation
    {
        private static NodeTranslator SimpleOperator(string op) => NodeTranslator.Sequence("(", 0, op, 1, ")");
        private static NodeTranslator AffectOperator(string op) => NodeTranslator.Sequence(0, $" {op} ", 1, ";", TranslateAction.NextLine);

        public static NodeTranslator TranslateOperator(NodeOperator node, CodeGenerator generator)
        {
            var op = node.GetOperator(generator);

            switch (op) {
                case EnumOperator.Operator_Plus: return SimpleOperator("+");
                case EnumOperator.Operator_Minus: return SimpleOperator("-");
                case EnumOperator.Operator_Mul: return SimpleOperator("*");
                case EnumOperator.Operator_Div: return SimpleOperator("/");
                case EnumOperator.Operator_Mod: return SimpleOperator("%");
                case EnumOperator.Operator_UnaryMinus: return NodeTranslator.Sequence("-", 0);
                case EnumOperator.Operator_PlusAffect: return AffectOperator("+=");
                case EnumOperator.Operator_MinusAffect: return AffectOperator("-=");
                case EnumOperator.Operator_MulAffect: return AffectOperator("*=");
                case EnumOperator.Operator_DivAffect: return AffectOperator("/=");
                case EnumOperator.Operator_PlusPlusAffect: return NodeTranslator.Sequence(0, "++;", TranslateAction.NextLine);
                case EnumOperator.Operator_MinusMinusAffect: return NodeTranslator.Sequence(0, "--;", TranslateAction.NextLine);
                case EnumOperator.Operator_Affect: return NodeTranslator.Sequence(0, " = ", 1, ";", TranslateAction.NextLine);
                case EnumOperator.Operator_Dot: return NodeTranslator.Sequence(0, ".", 1);
                case EnumOperator.Dot_X: return NodeTranslator.Sequence(0, ".x");
                case EnumOperator.Dot_Y: return NodeTranslator.Sequence(0, ".y");
                case EnumOperator.Dot_Z: return NodeTranslator.Sequence(0, ".z");
                case EnumOperator.Operator_VectorPlusVector: return SimpleOperator("+");
                case EnumOperator.Operator_VectorMinusVector: return SimpleOperator("-");
                case EnumOperator.Operator_VectorMulScalar: return SimpleOperator("*");
                case EnumOperator.Operator_VectorDivScalar: return SimpleOperator("/");
                case EnumOperator.Operator_VectorUnaryMinus: return NodeTranslator.Sequence("-", 0);
                case EnumOperator.Dot_X_Assign: return NodeTranslator.Sequence(0, ".x = ", 1, ";", TranslateAction.NextLine);
                case EnumOperator.Dot_Y_Assign: return NodeTranslator.Sequence(0, ".y = ", 1, ";", TranslateAction.NextLine);
                case EnumOperator.Dot_Z_Assign: return NodeTranslator.Sequence(0, ".z = ", 1, ";", TranslateAction.NextLine);
                case EnumOperator.Operator_Ultra: return NodeTranslator.Sequence(0, ".", 1);
                case EnumOperator.Operator_ModelCast: return NodeTranslator.Sequence("((", 0, ")(", 1, "))");
                case EnumOperator.Operator_Array: return NodeTranslator.Sequence(0, "[", 1, "]");
                case EnumOperator.Operator_AffectArray: return NodeTranslator.Sequence(0, "[", 1, "] = ", 2);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}