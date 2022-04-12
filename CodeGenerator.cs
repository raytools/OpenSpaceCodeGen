using System;
using System.Collections.Generic;
using System.Linq;
using OpenSpaceCodeGen.AIModels;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Nodes;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen {
    public class CodeGenerator
    {
        public readonly AIType Type;
        public readonly LanguageTranslation Translation;
        public readonly ReferenceResolver ReferenceResolver;
        public readonly TranslationContext Context;

        private int indentation = 0;
        public int Indentation
        {
            get => indentation;
            set
            {
                if (value >= 0) {
                    indentation = value;
                    CurrentLine.Indent = indentation;
                } else {
                    throw new ArgumentException("Indentation must be 0 or bigger");
                }
            }
        }

        //public struct CodeLine

        public class CodeLine {

            public class LineElement
            {
                public Node Node;
                public string String;

                public LineElement(Node node, string str)
                {
                    this.Node = node;
                    this.String = str;
                }

                public override string ToString()
                {
                    return String;
                }
            }

            public List<LineElement> Elements = new List<LineElement>();
            public int Indent;

            public CodeLine(int indent)
            {
                Indent = indent;
            }

            public override string ToString()
            {
                return new string(' ',Indent * LanguageTranslation.IndentSize) + string.Join(string.Empty, Elements.Select(e => e.ToString()));
            }
        }

        private List<CodeLine> Lines = new List<CodeLine>();
        private CodeLine CurrentLine => Lines.Last();
        public AIModel AIModel { get; }

        private CodeGenerator() { }

        public CodeGenerator(AIType type, LanguageTranslation translation, TranslationContext context,
            ReferenceResolver referenceResolver = null, AIModel aiModel = null)
        {
            Type = type;
            Translation = translation;
            Context = context;
            ReferenceResolver = referenceResolver ?? ReferenceResolver.DummyResolver;
            AIModel = aiModel;

            NextLine();
        }

        public void TrimEmptyLines()
        {
            while (string.IsNullOrWhiteSpace(Lines.First().ToString())) {
                Lines.RemoveAt(0);
            }

            while (string.IsNullOrWhiteSpace(Lines.Last().ToString())) {
                Lines.RemoveAt(Lines.Count - 1);
            }
        }

        public string Result
        {
            get
            {
                TrimEmptyLines();
                return string.Join(Environment.NewLine, Lines.Select(l => l.ToString()));
            }
        }

        public string GetDebugHTML()
        {
            string html = "<html><head><style>body {font-family: Courier New;}</style></head><body>";

            foreach (var l in Lines) {

                for (int r = 0; r < l.Indent * LanguageTranslation.IndentSize; r++) {
                    html += "&nbsp;";
                }

                foreach (var e in l.Elements) {
                    html += $"<span title=\"Node @0x{e.Node.Offset:X} {e.Node.ToString(this)}, param {e.Node.param}\">{e.String}</span>";
                }

                html += "<br>";
            }

            html += "</body></html>";

            return html;
        }

        public void Append(Node node, string str)
        {
            CurrentLine.Elements.Add(new CodeLine.LineElement(node, str));
        }

        public void RemoveLastLine(Node node)
        {
           Lines.RemoveAt(Lines.Count-1);
        }

      public void NextLine()
        {
            Lines.Add(new CodeLine(Indentation));
        }

        public void VisitScript(Script script)
        {
            foreach (Node node in script.Nodes) {
                node.Visit(this);
            }
        }
    }
}
