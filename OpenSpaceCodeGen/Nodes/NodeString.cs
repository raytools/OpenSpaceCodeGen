using OpenSpaceCodeGen.Nodes.Generic;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeString : ReferenceNode {
       public override NodeTranslator GetTranslator(CodeGenerator gen)
       {
          return gen.Translation.StringTranslator(gen, this);
       }
   }
}
