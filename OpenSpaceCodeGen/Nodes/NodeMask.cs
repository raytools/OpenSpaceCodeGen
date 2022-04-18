using OpenSpaceCodeGen.Nodes.Generic;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeMask : BasicNode {

       public override string ToString(CodeGenerator gen)
       {
          return param.ToString();
       }
   }
}
