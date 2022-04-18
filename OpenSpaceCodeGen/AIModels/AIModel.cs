using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Concatenators;
using OpenSpaceCodeGen.Config;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.AIModels {
    public class AIModel
    {
        public string Map => MetaData.Map;
        public readonly string Name;
        public readonly List<Behavior> Rules;
        public readonly List<Behavior> Reflexes;
        public readonly List<Macro> Macros;
        public readonly AIModelMetaData MetaData;

        public AIModel(string name, List<Behavior> rules, List<Behavior> reflexes, List<Macro> macros, AIModelMetaData metaData)
        {
            Name = name;
            Rules = rules;
            Reflexes = reflexes;
            Macros = macros;
            MetaData = metaData;
        }

        public static AIModel FromFolder(string name, GameConfig gameConfig, string folder)
        {

            var ruleDir = Path.Combine(folder, "rule");
            var reflexDir = Path.Combine(folder, "reflex");
            var macroDir = Path.Combine(folder, "macros");

            var ruleFolders = Directory.Exists(ruleDir) ? Directory.GetDirectories(ruleDir) : new string[]{};
            var reflexFolders = Directory.Exists(reflexDir) ? Directory.GetDirectories(reflexDir) : new string[]{};
            var macroFiles = Directory.Exists(macroDir) ? Directory.GetFiles(macroDir) : new string[] { };

            List<Behavior> rules = new List<Behavior>();
            List<Behavior> reflexes = new List<Behavior>();
            List<Macro> macros = new List<Macro>();

            foreach (var rule in ruleFolders) {
                rules.Add(Behavior.FromFolder(gameConfig, rule, Behavior.BehaviorType.Rule));
            }

            foreach (var reflex in reflexFolders) {
                reflexes.Add(Behavior.FromFolder(gameConfig, reflex, Behavior.BehaviorType.Reflex));
            }

            foreach (var macroFile in macroFiles) {
                macros.Add(Macro.FromFile(gameConfig, macroFile));
            }

            var aiModelMetaDataString = File.ReadAllText(Path.Combine(folder, "aimodel.json"));
            var metaData = JsonConvert.DeserializeObject<AIModelMetaData>(aiModelMetaDataString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.None
            });

            return new AIModel(name, rules, reflexes, macros, metaData);
        }

        public string Translate(GameConfig config, TranslationContext context, LanguageTranslation.TranslationMode mode)
        {
            var concat = Concatenator.FromMode(mode);

            var rules = concat.ConcatenateBehaviours(Rules.Select(r => r.Translate(config, context, mode, this)).ToArray());
            var reflexes = concat.ConcatenateBehaviours(Reflexes.Select(r => r.Translate(config, context, mode, this)).ToArray());
            var macros = concat.ConcatenateBehaviours(Macros.Select(r => r.Translate(config, context, mode, this)).ToArray());

            return concat.ConcatenateAIModel(MetaData, rules, reflexes, macros);
        }

        public string TranslateHTML(GameConfig config, TranslationContext context, LanguageTranslation.TranslationMode mode)
        {
            var concat = Concatenator.FromMode(mode);

            var rules = concat.ConcatenateBehaviours(Rules.Select(r => r.TranslateHTML(config, context, mode, this)).ToArray());
            var reflexes = concat.ConcatenateBehaviours(Reflexes.Select(r => r.TranslateHTML(config, context, mode, this)).ToArray());
            var macros = concat.ConcatenateBehaviours(Macros.Select(r => r.TranslateHTML(config, context, mode, this)).ToArray());

            return concat.ConcatenateAIModel(MetaData, rules, reflexes, macros).Replace(Environment.NewLine, "<br>");
        }
    }
}
