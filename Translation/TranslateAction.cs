using System;
using OpenSpaceCodeGen.Nodes;

namespace OpenSpaceCodeGen.Translation
{
    public class TranslateAction
    {
        public Action<Node, CodeGenerator> Action;
        private string debug;

        public void Invoke(Node node, CodeGenerator gen)
        {
            Action.Invoke(node, gen);
        }

        private TranslateAction(Action<Node, CodeGenerator> action, string debug)
        {
            this.Action = action;
            this.debug = debug;
        }

        public static TranslateAction String(string str) =>
            new TranslateAction((node,gen) => gen.Append(node, str), str);

        public static TranslateAction RemoveLastLine() =>
           new TranslateAction((node, gen) => gen.RemoveLastLine(node), "RemoveLastLine");

      public static TranslateAction NextLine =>
            new TranslateAction((node,gen) => gen.NextLine(), "NextLine");
        public static TranslateAction VisitChild(int index) =>
            new TranslateAction((node, gen) => node.Children[index].Visit(gen), $"VisitChild({index})");

        public static TranslateAction VisitChildren(TranslateAction separator = null) => new TranslateAction(
            (node, gen) =>
            {
                node.Children.ForEach(n =>
                {
                    n.Visit(gen);
                    if (n != node.Children[^1]) {
                        separator?.Invoke(node, gen);
                    }
                });
            }, $"VisitChildren{(separator!=null?"("+separator+")":"")}");

        public static TranslateAction Indent => new TranslateAction((node,gen) => gen.Indentation++, "Indent");
        public static TranslateAction Unindent => new TranslateAction((node,gen) => gen.Indentation--, "Unindent");

        public static implicit operator TranslateAction(string str) => String(str);
        public static implicit operator TranslateAction(int index) => VisitChild(index);

        public override string ToString()
        {
            return debug;
        }
    }
}