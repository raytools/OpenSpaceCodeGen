using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenSpaceCodeGen.AIModels;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Translation;
using OpenSpaceCodeGen.Util;

namespace OpenSpaceCodeGen.Concatenators {
    public class ConcatenatorCSharp : Concatenator
    {

        public static Dictionary<DsgVarType, string> DsgVarTypeTranslationTable = new Dictionary<DsgVarType, string>()
        {
            {DsgVarType.None, "object"},
            {DsgVarType.Boolean, "bool"},
            {DsgVarType.Byte, "sbyte"},
            {DsgVarType.UByte, "byte"},
            {DsgVarType.Short, "short"},
            {DsgVarType.UShort, "ushort"},
            {DsgVarType.Int, "int"},
            {DsgVarType.UInt, "uint"},
            {DsgVarType.Float, "float"},
            {DsgVarType.Vector, "Vector3"},
            {DsgVarType.List, "List<object>"},
            {DsgVarType.Comport, "Func<Task>"},
            {DsgVarType.Action, "Action"},
            {DsgVarType.Caps, "Caps"},
            {DsgVarType.Input, "Input"},
            {DsgVarType.SoundEvent, "SoundEvent"},
            {DsgVarType.Light, "Light"},
            {DsgVarType.GameMaterial, "GameMaterial"},
            {DsgVarType.VisualMaterial, "VisualMaterial"},
            {DsgVarType.Perso, "Perso"},
            {DsgVarType.WayPoint, "WayPoint"},
            {DsgVarType.Graph, "Graph"},
            {DsgVarType.Text, "string"},
            {DsgVarType.SuperObject, "SuperObject"},
            {DsgVarType.SOLinks, "SOLinks"},
            {DsgVarType.PersoArray, "Perso[]"},
            {DsgVarType.VectorArray, "Vector3[]"},
            {DsgVarType.FloatArray, "float[]"},
            {DsgVarType.IntegerArray, "int[]"},
            {DsgVarType.WayPointArray, "WayPoint[]"},
            {DsgVarType.TextArray, "Text[]"},
            {DsgVarType.TextRefArray, "TextRef[]"},
            {DsgVarType.GraphArray, "Graph[]"},
            {DsgVarType.SOLinksArray, "SOLinks[]"},
            {DsgVarType.SoundEventArray, "SoundEvent[]"},
            {DsgVarType.VisualMatArray, "VisualMaterial[]"},
            {DsgVarType.Way,  "Way"},
            {DsgVarType.ActionArray,  "Action[]"},
            {DsgVarType.SuperObjectArray, "SuperObject[]"},
            {DsgVarType.ObjectList,  "ObjectList"},
        };

        public override string ConcatenateScripts(string[] scripts, string name, Behavior.BehaviorType behaviorType)
        {
            string nl = Environment.NewLine;

            string code = string.Join(Environment.NewLine, scripts);
            string method = $"public async Task {name}(){nl}" +
                            $"{{{nl}" +
                            $"{code.IndentMultilineString(LanguageTranslation.IndentSize)}{nl}" +
                            $"}}";

            return method;
        }

        public override string ConcatenateBehaviours(string[] behaviours)
        {
            return string.Join(Environment.NewLine.Repeat(2), behaviours);
        }

        private static string[] _usingItems = new string[] { "System", "System.Collections.Generic", "System.Numerics", "System.Threading.Tasks" };
        public static string UsingLines => string.Concat(_usingItems.Select(item=> $"using {item};{Environment.NewLine}").ToArray());

        public override string ConcatenateAIModel(AIModelMetaData metaData, string rules, string reflexes,
            string macros)
        {
            string code = string.Join(Environment.NewLine,
               "#region RULES", rules, "#endregion",
               "#region REFLEXES", reflexes, "#endregion",
               "#region SUBROUTINES", macros, "#endregion");

            string nl = Environment.NewLine;

            string classStr = $"{UsingLines}{nl}" +
                              $"public class {metaData.Name}: Perso{nl}" +
                              $"{{{nl}" +
                              $"{GenerateDsgVars(metaData).IndentMultilineString(LanguageTranslation.IndentSize)}{nl}" +
                              $"{nl}" +
                              $"{GenerateConstructor(metaData).IndentMultilineString(LanguageTranslation.IndentSize)}" +
                              $"{nl}" +
                              $"{code.IndentMultilineString(LanguageTranslation.IndentSize)}{nl}" +
                              $"}}";

            return classStr;
        }

        private static string GenerateConstructor(AIModelMetaData metaData)
        {
            string nl = Environment.NewLine;

            string initRule = metaData.InitialRules != null ? string.Join(Environment.NewLine, metaData.InitialRules.Distinct().Where(r=>!string.IsNullOrWhiteSpace(r)).Select(r => $"{r}();")) : string.Empty;
            string initReflex = metaData.InitialReflexes!= null ? string.Join(Environment.NewLine, metaData.InitialReflexes.Distinct().Where(r => !string.IsNullOrWhiteSpace(r)).Select(r => $"{r}();")) : string.Empty;

            return $"public {metaData.Name}() {{{nl}" +
                   $"// Init Rules{nl}"+
                   ($"{initRule.IndentMultilineString(LanguageTranslation.IndentSize)}{nl}") +
                   $"// Init Reflexes{nl}" +
                   ($"{initReflex.IndentMultilineString(LanguageTranslation.IndentSize)}{nl}") +
                   $"}}{nl}";
        }

        private static string GenerateDsgVars(AIModelMetaData metaData)
        {
            return string.Join(Environment.NewLine, metaData.DsgVars.Select(GenerateSingleDsgVar).ToArray());
        }

        private static string GenerateSingleDsgVar(AIModelMetaData.DsgVar dsgVar, int num)
        {
            return $"public {DsgVarTypeTranslationTable[dsgVar.Type]} DsgVar_{num}; // #{num}";
        }

        // TODO: MACROS
    }
}
