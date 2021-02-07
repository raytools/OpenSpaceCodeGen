using System;
using System.Collections.Generic;
using System.Text;
using OpenSpaceCodeGen.Nodes.Generic;

namespace OpenSpaceCodeGen {
    public class ReferenceResolver
    {
        public Func<ReferenceNode, string> ResolveFunc;

        public ReferenceResolver(Func<ReferenceNode, string> func)
        {
            this.ResolveFunc = func;
        }

        public static ReferenceResolver DummyResolver => new ReferenceResolver(node => $"{node.GetType().Name}(0x{node.param:x})");
    }
}
