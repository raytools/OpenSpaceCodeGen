using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Nodes;
using OpenSpaceCodeGen.Util;

namespace OpenSpaceCodeGen.Translation.CBase.CSharp {

    public class LanguageTranslationPlainC : LanguageTranslationCBase
    {
        public override string IfDefSyntax => "#if ";
        public override string IfNotDefSyntax => "#if !";
        public override string EndIfDefSyntax => "#endif";

        public override NodeTranslator VectorTranslator(CodeGenerator gen, NodeVector node)
        {
            return NodeTranslator.Sequence("new Vector3(",0,",",1,",",2,")");
        }

        public override string FileExtension => "cs";

        public override void PostTranslationStep(AIType aiType, string outputDirectory)
        {
            File.WriteAllText(Path.Combine(outputDirectory, "PersoConditions.cs"), GeneratePersoClassConditions(aiType));
            File.WriteAllText(Path.Combine(outputDirectory, "PersoFields.cs"), GeneratePersoClassFields(aiType));
            File.WriteAllText(Path.Combine(outputDirectory, "PersoFunctions.cs"), GeneratePersoClassFunctions(aiType));
            File.WriteAllText(Path.Combine(outputDirectory, "PersoProcedures.cs"), GeneratePersoClassProcedures(aiType));
            File.WriteAllText(Path.Combine(outputDirectory, "PersoMetaActions.cs"), GeneratePersoClassMetaActions(aiType));
        }

        private static string GeneratePersoClassConditions(AIType type)
        {
            string nl = Environment.NewLine;

            string conditions = string.Join(nl, type.ConditionTable.Select(c => $"public bool {c}(params object[] args) => true;"));

            return GenerateClassString(conditions);
        }

        private static string GeneratePersoClassFields(AIType type)
        {
            string nl = Environment.NewLine;

            string fields = string.Join(nl, type.FieldTable.Select(c => $"public object {c} {{get; set;}}"));
            
            return GenerateClassString(fields);
        }

        private static string GeneratePersoClassFunctions(AIType type)
        {
            string nl = Environment.NewLine;

            string functions = string.Join(nl, type.FunctionTable.Select(c => $"public object {c}(params object[] args) => null;"));

            return GenerateClassString(functions);
        }

        private static string GeneratePersoClassProcedures(AIType type)
        {
            string nl = Environment.NewLine;

            string procedures = string.Join(nl, type.ProcedureTable.Select(c => $"public void {c}(params object[] args) {{}}"));

            return GenerateClassString(procedures);
        }

        private static string GeneratePersoClassMetaActions(AIType type)
        {
            string nl = Environment.NewLine;

            string metaActions = string.Join(nl, type.MetaActionTable.Select(c => $"public object {c}(params object[] args) => null;"));

            return GenerateClassString(metaActions);
        }

        private static string GenerateClassString(string contents)
        {
            return $"public partial class Perso {{{Environment.NewLine}" +
                   contents.IndentMultilineString(LanguageTranslation.IndentSize) + Environment.NewLine +
                   $"{Environment.NewLine}}}";
        }
    }
}
