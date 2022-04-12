using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenSpaceCodeGen.AIModels;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Concatenators;
using OpenSpaceCodeGen.Nodes;
using OpenSpaceCodeGen.Nodes.Generic;
using OpenSpaceCodeGen.Util;

namespace OpenSpaceCodeGen.Translation.CBase.CSharp {

    public class LanguageTranslationCSharp : LanguageTranslationCBase
    {
        public static Dictionary<Type, string> ReferenceTypeNames = new Dictionary<Type, string>()
        {
            {typeof(NodeActionRef), "Action"},
            {typeof(NodeConstantRef), "Constant"},
            {typeof(NodeEnvironmentRef), "Environment"},
            {typeof(NodeFamilyRef), "Family"},
            {typeof(NodeFontRef), "Font"},
            {typeof(NodeGameMaterialRef), "GameMaterial"},
            {typeof(NodeGraphRef), "Graph"},
            {typeof(NodeLipsSynchroRef), "LipSynchro"},
            {typeof(NodeModelRef), "AIModel"},
            {typeof(NodeObjectTableRef), "ObjectTable"},
            {typeof(NodePersoRef), "Perso"},
            {typeof(NodeRealRef), "Real"},
            {typeof(NodeSectorRef), "Sector"},
            {typeof(NodeSoundEventRef), "SoundEvent"},
            {typeof(NodeSuperObjectRef), "SuperObject"},
            {typeof(NodeSurfaceRef), "Surface"},
            {typeof(NodeTextRef), "Text"},
            {typeof(NodeWayPointRef), "WayPoint"},
            {typeof(NodeString), "String"},
        };

        public override string IfDefSyntax => "#if ";
        public override string IfNotDefSyntax => "#if !";
        public override string EndIfDefSyntax => "#endif";

        public override NodeTranslator VectorTranslator(CodeGenerator gen, NodeVector node)
        {
            return NodeTranslator.Sequence("new Vector3(",0,",",1,",",2,")");
        }

        public override NodeTranslator MetaActionTranslator(CodeGenerator gen, NodeMetaAction node) =>
            NodeTranslator.Sequence("await "+node.ToString(gen) + "(", TranslateAction.VisitChildren(", "), ");", TranslateAction.NextLine);

        public override NodeTranslator ReferenceTranslator(CodeGenerator gen, ReferenceNode node)
        {
            if (node is NodeComportRef || node is NodeSubRoutine || node is NodeString) {
                return NodeTranslator.Sequence(gen.ReferenceResolver.ResolveFunc.Invoke(node));
            }

            string referenceType = "Refs." + ReferenceTypeNames[node.GetType()];

            if (node is NodePersoRef) {
                var persoNames = gen.ReferenceResolver.ResolveFuncModelNames.Invoke(node);
                return NodeTranslator.Sequence($"(({persoNames.ModelName})",referenceType, "[\"", persoNames.InstanceName, "\"])");
            }

            return NodeTranslator.Sequence(referenceType, "[\"", gen.ReferenceResolver.ResolveFunc.Invoke(node), "\"]");
        }

        public override NodeTranslator SubroutineTranslator(CodeGenerator gen, NodeSubRoutine node) => NodeTranslator.Sequence("await "+node.ToString(gen) + "();", TranslateAction.NextLine);

        public override string FileExtension => "cs";

        public override void PostTranslationStep(AIType aiType, TranslationContext context, string outputDirectory)
        {
            File.WriteAllText(Path.Combine(outputDirectory, "Refs.cs"), GenerateReferenceStubs());
            File.WriteAllText(Path.Combine(outputDirectory, "PersoConditions.cs"), GeneratePersoClassConditions(aiType, context));
            File.WriteAllText(Path.Combine(outputDirectory, "PersoFields.cs"), GeneratePersoClassFields(aiType, context));
            File.WriteAllText(Path.Combine(outputDirectory, "PersoFunctions.cs"), GeneratePersoClassFunctions(aiType, context));
            File.WriteAllText(Path.Combine(outputDirectory, "PersoProcedures.cs"), GeneratePersoClassProcedures(aiType, context));
            File.WriteAllText(Path.Combine(outputDirectory, "PersoMetaActions.cs"), GeneratePersoClassMetaActions(aiType, context));
            File.WriteAllText(Path.Combine(outputDirectory, "PersoKeywords.cs"), GeneratePersoClassKeywords(aiType, context));
        }

        private static string GenerateReferenceStubs()
        {
            string nl = Environment.NewLine;

            string usingStrings = "using System.Collections.Generic;";
            string dictionaries = string.Join(nl, ReferenceTypeNames.Select(c => $"public static Dictionary<string, {c.Value}> {c.Value};"));

            return usingStrings + Environment.NewLine + GenerateClassString(dictionaries, "class Refs");
        }

        private static string GeneratePersoClassConditions(AIType type, TranslationContext context)
        {
            string nl = Environment.NewLine;

            var hints = context.TypeHints.ConditionTypeHints;
            var table = ConcatenatorCSharp.DsgVarTypeTranslationTable;
            string conditions = string.Join(nl, type.ConditionTable.Select(c =>
            {
                return $"public bool {c}({GenerateParamsString(hints[c].ParameterTypes)}) => true;";
            }));

            return GenerateClassString(conditions);
        }

        private static string GeneratePersoClassFields(AIType type, TranslationContext context)
        {
            string nl = Environment.NewLine;

            var hints = context.TypeHints.FieldTypeHints;
            var table = ConcatenatorCSharp.DsgVarTypeTranslationTable;
            string fields = string.Join(nl, type.FieldTable.Select(c =>
            {
                return $"public {table[hints[c].ReturnType]} {c} {{get; set;}}";
            }));
            
            return GenerateClassString(fields + Environment.NewLine + "public int globalRandomizer = 0;");
        }

        private static string GeneratePersoClassFunctions(AIType type, TranslationContext context)
        {
            string nl = Environment.NewLine;

            var hints = context.TypeHints.FunctionTypeHints;
            var table = ConcatenatorCSharp.DsgVarTypeTranslationTable;
            string functions = string.Join(nl, type.FunctionTable.Select(c =>
            {
                return $"public {table[hints[c].ReturnType]} {c}({GenerateParamsString(hints[c].ParameterTypes)}) => default;";
            }));

            return GenerateClassString(functions);
        }

        private static string GeneratePersoClassProcedures(AIType type, TranslationContext context)
        {
            string nl = Environment.NewLine;

            var hints = context.TypeHints.ProcedureTypeHints;
            var table = ConcatenatorCSharp.DsgVarTypeTranslationTable;
            string procedures = string.Join(nl, type.ProcedureTable.Select(c =>
            {
                return $"public {table[hints[c].ReturnType]} {c}({GenerateParamsString(hints[c].ParameterTypes)}) => default;";
            }));

            return GenerateClassString(procedures);
        }

        private static string GeneratePersoClassMetaActions(AIType type, TranslationContext context)
        {
            string nl = Environment.NewLine;

            string metaActions = string.Join(nl, type.MetaActionTable.Select(c =>
            {
                return $"public Task {c}(params object[] args) => default;";
            }));

            return GenerateClassString(metaActions);
        }

        private static string GeneratePersoClassKeywords(AIType type, TranslationContext context)
        {
            string nl = Environment.NewLine;

            string keywords = string.Join(nl, type.ExportedKeywords.Select(c =>
            {
                return $"public {c.Value} {c.Key} => default;";
            }));

            return GenerateClassString(keywords);
        }

        private static string GenerateClassString(string contents, string classText = "partial class Perso")
        {
            return $"{ConcatenatorCSharp.UsingLines}" +
                   $"{Environment.NewLine}" +
                   $"public {classText} {{{Environment.NewLine}" +
                   contents.IndentMultilineString(LanguageTranslation.IndentSize) + Environment.NewLine +
                   $"{Environment.NewLine}}}";
        }

        private static string GenerateParamsString(List<DsgVarType> parameters)
        {
            if (parameters == null) {
                return "params object[] args";
            } else {

                if (parameters.Count == 0) return string.Empty;

                List<string> paramStrings = new List<string>();
                int i = 0;
                foreach (var p in parameters) {
                    paramStrings.Add($"{ConcatenatorCSharp.DsgVarTypeTranslationTable[p]} arg{i}");
                    i++;
                }
                return string.Join(", ", paramStrings);
            }
        }
    }
}
