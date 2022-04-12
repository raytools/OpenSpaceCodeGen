using System.Collections.Generic;
using OpenSpaceCodeGen.AIModels;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Nodes;
using OpenSpaceCodeGen.Nodes.Generic;
using OpenSpaceCodeGen.Translation.CBase;
using OpenSpaceCodeGen.Translation.CBase.CSharp;
using OpenSpaceCodeGen.Translation.CpaScript;
using OpenSpaceCodeGen.Translation.Raw;
using OpenSpaceCodeGen.Util;

namespace OpenSpaceCodeGen.Translation
{
    public abstract class LanguageTranslation
    {

        public abstract NodeTranslator BasicTranslator(CodeGenerator gen, BasicNode node);
        public abstract NodeTranslator KeywordTranslator(CodeGenerator gen, NodeKeyWord node);
        public abstract NodeTranslator VectorTranslator(CodeGenerator gen, NodeVector node);
        public abstract NodeTranslator OperatorTranslator(CodeGenerator gen, NodeOperator node);
        public abstract NodeTranslator MetaActionTranslator(CodeGenerator gen, NodeMetaAction node);
        public abstract NodeTranslator FieldTranslator(CodeGenerator gen, NodeField node);
        public abstract NodeTranslator NullTranslator(CodeGenerator gen, NodeNull node);
        public abstract NodeTranslator ConditionTranslator(CodeGenerator gen, NodeCondition node);
        public abstract NodeTranslator ConstantTranslator(CodeGenerator gen, NodeConstant node);
        public abstract NodeTranslator RealTranslator(CodeGenerator gen, NodeReal node);
        public abstract NodeTranslator ProcedureTranslator(CodeGenerator gen, NodeProcedure node);
        public abstract NodeTranslator FunctionTranslator(CodeGenerator gen, NodeFunction node);
        public abstract NodeTranslator SubroutineTranslator(CodeGenerator gen, NodeSubRoutine node);
        public abstract NodeTranslator ReferenceTranslator(CodeGenerator gen, ReferenceNode node);

        public NodeTranslator StringTranslator(CodeGenerator gen, NodeString node)
        {
           return NodeTranslator.Sequence(StringQuoteCharacter,
              gen.ReferenceResolver.ResolveFunc.Invoke(node),
              StringQuoteCharacter);
        }

        public abstract string StringQuoteCharacter { get; }
        public abstract string FileExtension { get; }
        public abstract void PostTranslationStep(AIType aiType, TranslationContext translationContext, string outputDirectory);

        public static LanguageTranslation CSharp = new LanguageTranslationCSharp();
        public static LanguageTranslation Raw = new LanguageTranslationRaw();
        public static LanguageTranslation CpaScript = new LanguageTranslationCpaScript();

        public virtual ReferenceResolver ResolverFromPointerMap(TranslationContext context, AIModel model) => new ReferenceResolver(
            node => context.PointerNames[new PointerInMap(model.Map, (uint) node.param)], 
            node => context.PersoModelNames.ContainsKey(new PointerInMap(model.Map, (uint)node.param))?context.PersoModelNames[new PointerInMap(model.Map, (uint)node.param)]:new AIModelMetaData.PersoNames());

        public enum TranslationMode {
            CSharp,
            Raw,
            CpaScript,
        }

        private static Dictionary<TranslationMode, LanguageTranslation> _mapping =
            new Dictionary<TranslationMode, LanguageTranslation>()
            {
                {TranslationMode.CSharp, CSharp},
                {TranslationMode.Raw, Raw},
                {TranslationMode.CpaScript, CpaScript},
            };

        public static LanguageTranslation TranslationFromMode(TranslationMode mode) => _mapping[mode];

        /// <summary>
        /// TODO: add a configuration item + command line option for IndentSize
        /// </summary>
        public const int IndentSize = 4;
    }
}