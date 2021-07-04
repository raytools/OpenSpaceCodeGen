using System;
using System.Collections.Generic;
using System.IO;
using OpenSpaceCodeGen.AIModels;
using OpenSpaceCodeGen.Translation;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Concatenators;
using OpenSpaceCodeGen.Config;

namespace OpenSpaceCodeGen {
    class Program {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="argument">The filename (.osb) or directory with files to translate.</param>
        /// <param name="game">The game from which the scripts originate</param>
        /// <param name="mode">Translation output (CSharp or Raw)</param>
        /// <param name="outputdir">The directory to export to</param>
        /// <param name="nonStandardLayout">Whether to use the game's original byte layout instead of raymap's export format</param>
        /// <param name="generateHtml">Generate debug HTML files</param>
        public static void Main(string argument, Game game = Game.R2PC, LanguageTranslation.TranslationMode mode = LanguageTranslation.TranslationMode.CSharp, string outputdir = "", bool nonStandardLayout = false, bool generateHtml = false)
        {
            if (!Directory.Exists(outputdir)) {
                throw new DirectoryNotFoundException($"The export directory \"{outputdir}\" couldn't be found!");
            }

            var config = GameConfig.FromGame(game);

            // For standard layout assume the format that Raymap exports in
            if (!nonStandardLayout) {
                config = new GameConfig(config.AITypes, NodeSettings.SettingsRaymapExport);
            }

            var translation = LanguageTranslation.TranslationFromMode(mode);

            if (Directory.Exists(argument)) {

                var folders = Directory.GetDirectories(argument);
                foreach (var modelFolder in folders) {
                    var aiModel = AIModel.FromFolder(Path.GetFileName(modelFolder), config, modelFolder);
                    var aiModelOutput = aiModel.Translate(config, mode);
                    
                    File.WriteAllText(Path.Combine(outputdir, aiModel.Name+"."+translation.FileExtension), aiModelOutput);

                    if (generateHtml) {
                        var aiModelHtml = aiModel.TranslateHTML(config, mode);
                        File.WriteAllText(Path.Combine(outputdir, aiModel.Name + ".html"), aiModelHtml);
                    }

                    translation.PostTranslationStep(config.AITypes, outputdir);
                }

            } else if (File.Exists(argument)) {

                var script = Script.FromBytes(config, File.ReadAllBytes(argument));
                var output = script.Translate(config, mode);

                string scriptName = Path.GetFileNameWithoutExtension(argument);

                File.WriteAllText(Path.Combine(outputdir, scriptName+"."+translation.FileExtension), output);
                
                if (generateHtml) {
                    var scriptHtml = script.TranslateHTML(config, mode);
                    File.WriteAllText(Path.Combine(outputdir, scriptName + ".html"), scriptHtml);
                }

            } else {
                throw new FileNotFoundException($"No directory or file \"{argument}\"");
            }

            //PythonScriptCode

        }
    }
}
