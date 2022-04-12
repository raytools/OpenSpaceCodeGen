using System.Collections.Generic;
using OpenSpaceCodeGen.Nodes.Generic;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeComportRef : ReferenceNode {
        public override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            if (Parent != null && Parent is NodeProcedure && Parent.Children.Count > 1) {

                if (Parent.Children[0] is NodeDsgVarRef || Parent.Children[0] is NodePersoRef) {

                    var actions = new List<TranslateAction>(Parent.Children[0].GetTranslator(gen).Actions);
                    actions.Add(".");
                    actions.AddRange(gen.Translation.ReferenceTranslator(gen, this).Actions);
                    return NodeTranslator.Sequence(actions.ToArray());

                }
            }

            return gen.Translation.ReferenceTranslator(gen, this);
        }
    }
}
