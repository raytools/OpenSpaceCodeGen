using OpenSpaceCodeGen.Nodes;
using OpenSpaceCodeGen.Translation.CFamily;
using OpenSpaceCodeGen.Translation.Raw;

namespace OpenSpaceCodeGen.Translation
{
    public abstract class LanguageTranslation
    {

        public abstract NodeTranslator BasicTranslator(CodeGenerator gen, BasicNode node);
        public abstract NodeTranslator KeywordTranslator(CodeGenerator gen, NodeKeyWord node);

        public abstract NodeTranslator OperatorTranslator(CodeGenerator gen, NodeOperator node);
        public abstract NodeTranslator MetaActionTranslator(CodeGenerator gen, NodeMetaAction node);
        public abstract NodeTranslator FieldTranslator(CodeGenerator gen, NodeField node);
        public abstract NodeTranslator NullTranslator(CodeGenerator gen, NodeNull node);
        public abstract NodeTranslator ConditionTranslator(CodeGenerator gen, NodeCondition node);
        public abstract NodeTranslator ConstantTranslator(CodeGenerator gen, NodeConstant node);
        public abstract NodeTranslator RealTranslator(CodeGenerator gen, NodeReal node);

        public abstract NodeTranslator ProcedureTranslator(CodeGenerator gen, NodeProcedure node);
        public abstract NodeTranslator FunctionTranslator(CodeGenerator gen, NodeFunction node);

        public static LanguageTranslation CSharp = new LanguageTranslationCFamily();
        public static LanguageTranslation Raw = new LanguageTranslationRaw();
    }
}