using System;
using System.Collections.Generic;
using System.Linq;
using OpenSpaceCodeGen.Nodes;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen {
    public class CodeGenerator
    {
        public readonly AITypes.AITypes Types;
        public readonly LanguageTranslation Translation;
        public readonly ReferenceResolver ReferenceResolver;

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
                return new string(' ',Indent * 2) + string.Join(string.Empty, Elements.Select(e => e.ToString()));
            }
        }

        private List<CodeLine> Lines = new List<CodeLine>();
        private CodeLine CurrentLine => Lines.Last();

        private CodeGenerator() { }

        public CodeGenerator(AITypes.AITypes types, LanguageTranslation translation, ReferenceResolver referenceResolver = null)
        {
            Types = types;
            Translation = translation;
            ReferenceResolver = referenceResolver ?? ReferenceResolver.DummyResolver;

            NextLine();
        }

        public string Result => string.Join(Environment.NewLine, Lines.Select(l => l.ToString()));

        public string GetDebugHTML()
        {
            string html = "<html><head><style>body {font-family: Courier New;}</style></head><body>";

            foreach (var l in Lines) {

                for (int r = 0; r < l.Indent * 2; r++) {
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
