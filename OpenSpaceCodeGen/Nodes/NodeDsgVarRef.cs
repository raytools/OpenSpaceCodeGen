using OpenSpaceCodeGen.Nodes.Generic;
using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes
{
   public class NodeDsgVarRef : ReferenceNode
   {
      public override NodeTranslator GetTranslator(CodeGenerator gen)
      {
         if (gen.Context.UseDsgVarNames) {
            return gen.Translation.ReferenceTranslator(gen, this);
         } else {
            return NodeTranslator.Sequence("DsgVar_" + param);
         }
      }

      public override string ToString(CodeGenerator gen)
      {
         return this.GetType().Name;
      }
   }
}