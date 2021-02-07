using System;
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
        public override NodeTranslator RealTranslator(CodeGenerator gen, NodeReal node) => BasicTranslateAction(gen, node);
        public override NodeTranslator NullTranslator(CodeGenerator gen, NodeNull node) => NodeTranslator.Sequence("null");

        public override NodeTranslator KeywordTranslator(CodeGenerator gen, NodeKeyWord node) => KeywordTranslation.TranslateKeyword(node, gen, this);

        public override NodeTranslator OperatorTranslator(CodeGenerator gen, NodeOperator node) => OperatorTranslation.TranslateOperator(node, gen);

        public override NodeTranslator MetaActionTranslator(CodeGenerator gen, NodeMetaAction node) =>
            BasicTranslateAction(gen, node);

        public override NodeTranslator FunctionTranslator(CodeGenerator gen, NodeFunction node) => FunctionTranslation.TranslateFunction(gen, node);

        public override NodeTranslator ProcedureTranslator(CodeGenerator gen, NodeProcedure procedureNode) =>
            NodeTranslator.Sequence(procedureNode.ToString(gen)+"(",TranslateAction.VisitChildren(","),");", TranslateAction.NextLine);

        public abstract string IfDefSyntax { get; }
        public abstract string IfNotDefSyntax { get; }
        public abstract string EndIfDefSyntax { get; }
    }
}