using System;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Nodes;
using OpenSpaceCodeGen.Nodes.Generic;

namespace OpenSpaceCodeGen.Translation.CpaScript
{
   public class LanguageTranslationCpaScript : LanguageTranslation
   {
      private static NodeTranslator BasicTranslateAction(CodeGenerator gen, Node node) => NodeTranslator.Sequence(
         TranslateAction.String(node.ToString(gen)),
         TranslateAction.VisitChildren()
      );

      public override NodeTranslator BasicTranslator(CodeGenerator gen, BasicNode node) =>
         BasicTranslateAction(gen, node);

      public override NodeTranslator FieldTranslator(CodeGenerator gen, NodeField node) =>
         BasicTranslateAction(gen, node);

      public override NodeTranslator ConditionTranslator(CodeGenerator gen, NodeCondition node) =>
         ConditionTranslation.TranslateCondition(node, gen);

      public override NodeTranslator ConstantTranslator(CodeGenerator gen, NodeConstant node) =>
         BasicTranslateAction(gen, node);

      public override NodeTranslator RealTranslator(CodeGenerator gen, NodeReal node)
      {
         var nodeString = node.ToString(gen);
         float.TryParse(nodeString, out float nodeFloat);

         // Robin, that's ugly, maybe find something better?

         return NodeTranslator.Sequence(
            TranslateAction.String(nodeFloat.ToString("0.0##############################################")),
            TranslateAction.VisitChildren()
         );
      }

      public override NodeTranslator NullTranslator(CodeGenerator gen, NodeNull node) =>
         NodeTranslator.Sequence("null");

      public override NodeTranslator KeywordTranslator(CodeGenerator gen, NodeKeyWord node)
      {
         return KeywordTranslation.TranslateKeyword(node, gen, this);
      }

      public override NodeTranslator VectorTranslator(CodeGenerator gen, NodeVector node)
      {
         return NodeTranslator.Sequence($"{EnumKeyword.Vector}(", 0, ",", 1, ",", 2, ")");
      }

      public override NodeTranslator OperatorTranslator(CodeGenerator gen, NodeOperator node) =>
         OperatorTranslation.TranslateOperator(node, gen);

      public override NodeTranslator MetaActionTranslator(CodeGenerator gen, NodeMetaAction node) =>
         NodeTranslator.Sequence(node.ToString(gen) + "(", TranslateAction.VisitChildren(", "), ")",
            TranslateAction.NextLine);

      public override NodeTranslator FunctionTranslator(CodeGenerator gen, NodeFunction node) =>
         FunctionTranslation.TranslateFunction(gen, node);

      public override NodeTranslator ProcedureTranslator(CodeGenerator gen, NodeProcedure procedureNode) =>
         NodeTranslator.Sequence(procedureNode.ToString(gen) + "(", TranslateAction.VisitChildren(", "), ")",
            TranslateAction.NextLine);

      public override NodeTranslator SubroutineTranslator(CodeGenerator gen, NodeSubRoutine node) =>
         NodeTranslator.Sequence(node.ToString(gen), TranslateAction.NextLine);

      public override NodeTranslator ReferenceTranslator(CodeGenerator gen, ReferenceNode node)
      {
         if (node is NodeDsgVarRef || node is NodeConstantRef || node is NodeRealRef) {
            return NodeTranslator.Sequence(gen.ReferenceResolver.ResolveFunc.Invoke(node));
         }
         return NodeTranslator.Sequence("\"" + gen.ReferenceResolver.ResolveFunc.Invoke(node) + "\"");
      }

      public override string StringQuoteCharacter => "\'";

      public override string FileExtension => "cpa";

      public override void PostTranslationStep(AIType aiType, TranslationContext translationContext,
         string outputDirectory)
      {
         // Sorry, nothing
      }
   }
}