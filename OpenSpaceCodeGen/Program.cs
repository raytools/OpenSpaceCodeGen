using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using OpenSpaceCodeGen.AIModels;
using OpenSpaceCodeGen.Translation;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Concatenators;
using OpenSpaceCodeGen.Config;
using OpenSpaceCodeGen.NodeScriptToBinary;
using OpenSpaceCodeGen.Util;

namespace OpenSpaceCodeGen {
    class Program {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument">The filename (.osb) or directory with files to translate.</param>
        /// <param name="game">The game from which the scripts originate</param>
        /// <param name="mode">Translation output (CSharp, Raw or CpaScript)</param>
        /// <param name="outputdir">The directory to export to</param>
        /// <param name="testa">Testa</param>
        /// <param name="nonStandardLayout">Whether to use the game's original byte layout instead of raymap's export format</param>
        /// <param name="generateHtml">Generate debug HTML files</param>
        /// <param name="convertNodeScript">Set to true to convert old node scripts</param>
        public static void Main(string argument, Game game = Game.R2PC, LanguageTranslation.TranslationMode mode = LanguageTranslation.TranslationMode.CSharp, string outputdir = "", bool nonStandardLayout = false, bool generateHtml = false, bool convertNodeScript = false, bool useDsgVarNames = false)
        {
            if (!Directory.Exists(outputdir)) {
                throw new DirectoryNotFoundException($"The export directory \"{outputdir}\" couldn't be found!");
            }

            var config = GameConfig.FromGame(game);

            // For standard layout assume the format that Raymap exports in
            if (!nonStandardLayout) {
                config = new GameConfig(config.AITypes, NodeSettings.SettingsRaymapExport);
            }

            if (convertNodeScript) {
               var converter = new NodeScriptToBinaryConverter(config.NodeSettings, argument, outputdir);
               converter.ConvertAll();

               return;
            }

            var translation = LanguageTranslation.TranslationFromMode(mode);

            if (Directory.Exists(argument)) {

                var folders = Directory.GetDirectories(argument);

                List<AIModel> aiModels = new List<AIModel>();

                TranslationContext context = new TranslationContext()
                {
                   UseDsgVarNames = useDsgVarNames
                };

                foreach (var modelFolder in folders) {

                    var newModel = AIModel.FromFolder(Path.GetFileName(modelFolder), config, modelFolder);

                    aiModels.Add(newModel);
                    context.PointerNames[new PointerInMap(newModel.Map, newModel.MetaData.Offset)] = newModel.Name;
                    foreach (var offsetNames in newModel.MetaData.OffsetToBehaviourNameMap) {
                        context.PointerNames[offsetNames.Key] = offsetNames.Value;
                    }

                    foreach (var offsetNames in newModel.MetaData.OffsetToPersoNamesMap) {
                        context.PersoModelNames[offsetNames.Key] = offsetNames.Value;
                    }

                    context.ReadPointerNames(newModel.Map, Path.Combine(modelFolder, "pointers.json"));
                }

                File.WriteAllText(Path.Combine(outputdir,"pointerNames.json"), JsonConvert.SerializeObject(context));

                foreach(var aiModel in aiModels) {
                    var aiModelOutput = aiModel.Translate(config, context, mode);
                    
                    File.WriteAllText(Path.Combine(outputdir, aiModel.Name+"."+translation.FileExtension), aiModelOutput);

                    if (generateHtml) {
                        var aiModelHtml = aiModel.TranslateHTML(config, context, mode);
                        File.WriteAllText(Path.Combine(outputdir, aiModel.Name + ".html"), aiModelHtml);
                    }

                    translation.PostTranslationStep(config.AITypes, context, outputdir);
                }

                /*
                var debugLines =
                    context.TypeHints.FunctionTypeHints.Hints.Select(s => s.Key + " = " + s.Value.ReturnType);
                foreach(var l in debugLines)
                Debug.WriteLine(l);*/

            } else if (File.Exists(argument)) {

                TranslationContext map = new TranslationContext();

                var script = Script.FromBytes(config, File.ReadAllBytes(argument));
                var output = script.Translate(config, map, mode);

                string scriptName = Path.GetFileNameWithoutExtension(argument);

                File.WriteAllText(Path.Combine(outputdir, scriptName+"."+translation.FileExtension), output);
                
                if (generateHtml) {
                    var scriptHtml = script.TranslateHTML(config, map, mode);
                    File.WriteAllText(Path.Combine(outputdir, scriptName + ".html"), scriptHtml);
                }

            } else {
                throw new FileNotFoundException($"No directory or file \"{argument}\"");
            }

            //PythonScriptCode

        }
    }
}
