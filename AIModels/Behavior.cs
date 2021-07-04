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

        public Behavior(string name, List<Script> scripts)
        {
            Name = name;
            Scripts = scripts;
        }

        public static Behavior FromFolder(GameConfig gameConfig, string folder)
        {
            List<Script> scripts = new List<Script>();
            foreach (var script in Directory.GetFiles(folder)) {
                scripts.Add(Script.FromBytes(gameConfig, File.ReadAllBytes(script)));
            }

            return new Behavior(Path.GetFileName(folder), scripts);
        }

        public string Translate(GameConfig config, LanguageTranslation.TranslationMode mode)
        {
            var concat = Concatenator.FromMode(mode);

            return concat.ConcatenateScripts(this.Scripts.Select(s => s.Translate(config, mode)).ToArray(), Name);
        }

        public string TranslateHTML(GameConfig config, LanguageTranslation.TranslationMode mode)
        {
            var concat = Concatenator.FromMode(mode);

            return concat.ConcatenateScripts(this.Scripts.Select(s => s.TranslateHTML(config, mode)).ToArray(), Name);
        }
    }
}
