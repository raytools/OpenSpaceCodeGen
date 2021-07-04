using System;
using System.Collections.Generic;
using System.Linq;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Config;
using OpenSpaceCodeGen.Nodes;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.AIModels {
    public class Script
    {
        public List<Node> Nodes;

        private Script() { }

        public static Script FromBytes(GameConfig gameConfig, byte[] bytes, int offsetBase = 0)
        {
            var settings = gameConfig.NodeSettings;
            var type = gameConfig.AITypes;

            Script script = new Script();

            script.Nodes = new List<Node>();

            for (int i = 0; i < bytes.Length; i+=settings.sizeOfNode) {
                byte[] nodeBytes = new byte[settings.sizeOfNode];
                Array.Copy(bytes, i, nodeBytes, 0, nodeBytes.Length);
                
                script.Nodes.Add(Node.FromBytes(settings, type, nodeBytes, i+offsetBase));
            }

            // Set parents and children by indent
            for (int i = 0; i < script.Nodes.Count; i++) {

                var node = script.Nodes[i];
                Node parent = null;

                for (int j = i; j >= 0; j--) {
                    if (script.Nodes[j].depth < node.depth) {
                        parent = script.Nodes[j];
                        break;
                    }
                }

                if (parent != null) {
                    node.Parent = parent;
                    parent.Children.Add(node);
                }
            }

            // Keep only root nodes
            script.Nodes = script.Nodes.Where(n => n.Parent == null).ToList();

            return script;
        }

        public string Translate(GameConfig config, LanguageTranslation.TranslationMode translationMode)
        {
            CodeGenerator v = new CodeGenerator(config.AITypes, LanguageTranslation.TranslationFromMode(translationMode));
            v.VisitScript(this);

            return v.Result;
        }

        public string TranslateHTML(GameConfig config, LanguageTranslation.TranslationMode translationMode)
        {
            CodeGenerator v = new CodeGenerator(config.AITypes, LanguageTranslation.TranslationFromMode(translationMode));
            v.VisitScript(this);

            return v.GetDebugHTML();
        }
    }
}
