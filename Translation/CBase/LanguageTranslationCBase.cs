using System;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Nodes;
using OpenSpaceCodeGen.Nodes.Generic;

namespace OpenSpaceCodeGen.Translation.CBase
{
    public abstract class LanguageTranslationCBase : LanguageTranslation
    {
        private static NodeTranslator BasicTranslateAction(CodeGenerator gen, Node node) => NodeTranslator.Sequence(
            TranslateAction.String(node.ToString(gen)),
            TranslateAction.VisitChildren()
        );

        public override NodeTranslator BasicTranslator(CodeGenerator gen, BasicNode node) => BasicTranslateAction(gen, node);
        public override NodeTranslator FieldTranslator(CodeGenerator gen, NodeField node) => BasicTranslateAction(gen, node);

        public override NodeTranslator ConditionTranslator(CodeGenerator gen, NodeCondition node) =>
            ConditionTranslation.TranslateCondition(node, gen);
        public override NodeTranslator ConstantTranslator(CodeGenerator gen, NodeConstant node) => BasicTranslateAction(gen, node);
        public override NodeTranslator RealTranslator(CodeGenerator gen, NodeReal node) => NodeTranslator.Sequence(
            TranslateAction.String(node.ToString(gen)+"f"),
            TranslateAction.VisitChildren()
        );
        public override NodeTranslator NullTranslator(CodeGenerator gen, NodeNull node) => NodeTranslator.Sequence("null");

        public override NodeTranslator KeywordTranslator(CodeGenerator gen, NodeKeyWord node)
        {
            // Scripts may contain 'if (1)' or 'if (0)' conditions, which are translated to 'if (true)' and 'if (false)' here.
            if (node.GetKeyword(gen) == EnumKeyword.If && node.Children.Count == 1 &&
                node.Children[0].GetNodeType(gen.Type) == NodeType.Constant) {
                if (node.Children[0].param == 0) {
                    return NodeTranslator.Sequence("if (false)");
                } else if (node.Children[0].param == 1) {
                    return NodeTranslator.Sequence("if (true)");
                } else {
                    throw new Exception("if (constant) with constant not being 1 or 0");
                }
            }

            return KeywordTranslation.TranslateKeyword(node, gen, this);
        }

        public override NodeTranslator OperatorTranslator(CodeGenerator gen, NodeOperator node) => OperatorTranslation.TranslateOperator(node, gen);

        public override NodeTranslator MetaActionTranslator(CodeGenerator gen, NodeMetaAction node) =>
            NodeTranslator.Sequence(node.ToString(gen) + "(", TranslateAction.VisitChildren(", "), ");", TranslateAction.NextLine);

        public override NodeTranslator FunctionTranslator(CodeGenerator gen, NodeFunction node) => FunctionTranslation.TranslateFunction(gen, node);

        public override NodeTranslator ProcedureTranslator(CodeGenerator gen, NodeProcedure procedureNode) =>
            NodeTranslator.Sequence(procedureNode.ToString(gen)+"(",TranslateAction.VisitChildren(", "),");", TranslateAction.NextLine);

        public abstract string IfDefSyntax { get; }
        public abstract string IfNotDefSyntax { get; }
        public abstract string EndIfDefSyntax { get; }
    }
}