using System;
using System.Collections.Generic;
using System.Diagnostics;
using OpenSpaceCodeGen.Nodes;

namespace OpenSpaceCodeGen.Translation {
    public class NodeTranslator {

        private readonly TranslateAction[] Actions;

        public static NodeTranslator RawTranslator(Node node, CodeGenerator gen)
        {
            return NodeTranslator.Sequence(
                TranslateAction.String(new string(' ', node.depth * 2) + node.ToString()),
                TranslateAction.NextLine,
                TranslateAction.VisitChildren());
        }

        private static Action<Node, CodeGenerator> WriteString(string str)
        {
            return (node, gen) =>
            {
                if (str == Environment.NewLine) {
                    gen.NextLine();
                } else {
                    gen.Append(node, str);
                }
            };
        }

        public static NodeTranslator Sequence(params TranslateAction[] actions)
        {
            return new NodeTranslator(actions);
        }

        private NodeTranslator(TranslateAction[] actions)
        {
            this.Actions = actions;
        }

        public void Translate(Node node, CodeGenerator gen)
        {
            string indent = new string(' ', node.depth*2);

#if DEBUG
            Debug.WriteLine($"{indent}{node.GetType().Name} {node.ToString(gen)}");
            Debug.WriteLine(indent+"{");
#endif

            foreach (var action in Actions) {
#if DEBUG
                Debug.WriteLine($"{indent}  ({gen.Indentation}){action}");
#endif
                action.Invoke(node, gen);
            }

#if DEBUG
            Debug.WriteLine(indent+"}");
#endif
        }
    }

}
