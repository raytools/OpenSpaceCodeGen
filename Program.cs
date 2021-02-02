using System;
using System.IO;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen {
    class Program {
        public static void Main(string[] args)
        {

            string filename = "YLT_Init.0.osb";

            if (args.Length > 0) {
                filename = args[0];
            }

            byte[] bytes = File.ReadAllBytes(filename);

            var script = Script.FromBytes(NodeSettings.SettingsDefault, AITypes.AITypes.R2, bytes);

            CodeGenerator generatorRaw = new CodeGenerator(AITypes.AITypes.R2, LanguageTranslation.Raw);
            generatorRaw.VisitScript(script);

            CodeGenerator generatorCSharp = new CodeGenerator(AITypes.AITypes.R2, LanguageTranslation.CSharp);
            generatorCSharp.VisitScript(script);

            //Console.WriteLine("RAW");
            //Console.WriteLine(generatorRaw.Result);
            Console.WriteLine("C-SHARP");
            Console.WriteLine(generatorCSharp.Result);

            //PythonScriptCode

            Console.ReadKey();
        }
    }
}
