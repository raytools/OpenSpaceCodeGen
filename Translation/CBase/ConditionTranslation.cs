using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Nodes;

namespace OpenSpaceCodeGen.Translation.CBase
{
    public static class ConditionTranslation
    {
        public static NodeTranslator TranslateCondition(NodeCondition node, CodeGenerator gen)
        {
            string cond = node.ToString(gen);
            switch (cond) {
                case Conditions.Cond_And: return NodeTranslator.Sequence("(",0,"&&",1,")");
                case Conditions.Cond_Or: return NodeTranslator.Sequence("(", 0, "||", 1, ")");
                case Conditions.Cond_Not: return NodeTranslator.Sequence("!(", 0,")");
                case Conditions.Cond_XOR: return NodeTranslator.Sequence("(", 0, "|", 1, ")");
                case Conditions.Cond_Equal: return NodeTranslator.Sequence("(", 0, "==", 1, ")");
                case Conditions.Cond_Different: return NodeTranslator.Sequence("(", 0, "!=", 1, ")");
                case Conditions.Cond_Lesser: return NodeTranslator.Sequence("(", 0, "<", 1, ")");
                case Conditions.Cond_Greater: return NodeTranslator.Sequence("(", 0, ">", 1, ")");
                case Conditions.Cond_LesserOrEqual: return NodeTranslator.Sequence("(", 0, "<=", 1, ")");
                case Conditions.Cond_GreaterOrEqual: return NodeTranslator.Sequence("(", 0, ">=", 1, ")");
                default: return NodeTranslator.Sequence(cond, "(", TranslateAction.VisitChildren(","), ")");
            }
        }
    }
}