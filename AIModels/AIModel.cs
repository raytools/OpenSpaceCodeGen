using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Concatenators;
using OpenSpaceCodeGen.Config;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.AIModels {
    public class AIModel
    {
        public readonly string Name;
        public readonly List<Behavior> Rules;
        public readonly List<Behavior> Reflexes;

        public AIModel(string name, List<Behavior> rules, List<Behavior> reflexes)
        {
            Name = name;
            Rules = rules;
            Reflexes = reflexes;
        }

        public static AIModel FromFolder(string name, GameConfig gameConfig, string folder)
        {

            var ruleDir = Path.Combine(folder, "rule");
            var reflexDir = Path.Combine(folder, "reflex");

            var ruleFolders = Directory.Exists(ruleDir) ? Directory.GetDirectories(ruleDir) : new string[]{};
            var reflexFolders = Directory.Exists(reflexDir) ? Directory.GetDirectories(reflexDir) : new string[]{};

            List<Behavior> rules = new List<Behavior>();
            List<Behavior> reflexes = new List<Behavior>();

            foreach (var rule in ruleFolders) {
                rules.Add(Behavior.FromFolder(gameConfig, rule));
            }

            foreach (var reflex in reflexFolders) {
                reflexes.Add(Behavior.FromFolder(gameConfig, reflex));
            }

            return new AIModel(name, rules, reflexes);
        }

        public string Translate(GameConfig config, LanguageTranslation.TranslationMode mode)
        {
            var concat = Concatenator.FromMode(mode);

            var rules = concat.ConcatenateBehaviours(Rules.Select(r => r.Translate(config, mode)).ToArray());
            var reflexes = concat.ConcatenateBehaviours(Reflexes.Select(r => r.Translate(config, mode)).ToArray());

            return concat.ConcatenateAIModel(Name, rules, reflexes);
        }

        public string TranslateHTML(GameConfig config, LanguageTranslation.TranslationMode mode)
        {
            var concat = Concatenator.FromMode(mode);

            var rules = concat.ConcatenateBehaviours(Rules.Select(r => r.TranslateHTML(config, mode)).ToArray());
            var reflexes = concat.ConcatenateBehaviours(Reflexes.Select(r => r.TranslateHTML(config, mode)).ToArray());

            return concat.ConcatenateAIModel(Name, rules, reflexes).Replace(Environment.NewLine, "<br>");
        }
    }
}
