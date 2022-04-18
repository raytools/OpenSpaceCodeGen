using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Concatenators;
using OpenSpaceCodeGen.Config;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.AIModels {
    public class Macro
    {
        public readonly string Name;
        public readonly Script Script;

        public Macro(string name, Script script)
        {
            Name = name;
            Script = script;
        }

        public static Macro FromFile(GameConfig gameConfig, string file)
        {
            var script = (Script.FromBytes(gameConfig, File.ReadAllBytes(file)));

            return new Macro(Path.GetFileNameWithoutExtension(file), script);
        }

        public string Translate(GameConfig config, TranslationContext context,
            LanguageTranslation.TranslationMode mode, AIModel model)
        {
            var concat = Concatenator.FromMode(mode);

            return concat.ConcatenateScripts(new[] {Script.Translate(config, context, mode, model)}, Name, Behavior.BehaviorType.Macro);
        }

        public string TranslateHTML(GameConfig config, TranslationContext context, LanguageTranslation.TranslationMode mode, AIModel model)
        {
            var concat = Concatenator.FromMode(mode);

            return concat.ConcatenateScripts(new[] { Script.TranslateHTML(config, context, mode, model) }, Name, Behavior.BehaviorType.Macro);
        }
    }
}
