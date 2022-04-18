using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Concatenators;
using OpenSpaceCodeGen.Config;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.AIModels {
    public class Behavior
    {
        public readonly string Name;
        public readonly List<Script> Scripts;
        public readonly BehaviorType Type;

        public enum BehaviorType
        {
           Rule,
           Reflex,
           Macro,
        }

        public Behavior(string name, List<Script> scripts, BehaviorType type)
        {
            Name = name;
            Scripts = scripts;
            Type = type;
        }

        public static Behavior FromFolder(GameConfig gameConfig, string folder, BehaviorType type)
        {
            List<Script> scripts = new List<Script>();
            foreach (var script in Directory.GetFiles(folder)) {
                scripts.Add(Script.FromBytes(gameConfig, File.ReadAllBytes(script)));
            }

            return new Behavior(Path.GetFileName(folder), scripts, type);
        }

        public string Translate(GameConfig config, TranslationContext context,
            LanguageTranslation.TranslationMode mode, AIModel model = null)
        {
            var concat = Concatenator.FromMode(mode);

            return concat.ConcatenateScripts(this.Scripts.Select(s => s.Translate(config, context, mode, model)).ToArray(), Name, Type);
        }

        public string TranslateHTML(GameConfig config, TranslationContext context, LanguageTranslation.TranslationMode mode, AIModel model = null)
        {
            var concat = Concatenator.FromMode(mode);

            return concat.ConcatenateScripts(this.Scripts.Select(s => s.TranslateHTML(config, context, mode)).ToArray(), Name, Type);
        }
    }
}
