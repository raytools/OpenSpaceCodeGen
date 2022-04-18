using System;
using System.Collections.Generic;
using System.Text;
using OpenSpaceCodeGen.AIModels;
using OpenSpaceCodeGen.Nodes.Generic;

namespace OpenSpaceCodeGen {
    public class ReferenceResolver
    {
        public Func<ReferenceNode, string> ResolveFunc;
        public Func<ReferenceNode, AIModelMetaData.PersoNames> ResolveFuncModelNames;

        public ReferenceResolver(Func<ReferenceNode, string> func, Func<ReferenceNode, AIModelMetaData.PersoNames> funcModelNames)
        {
            this.ResolveFunc = func;
            this.ResolveFuncModelNames = funcModelNames;
        }

        private static Func<ReferenceNode, string> dummyFunc = node => $"{node.GetType().Name}(0x{node.param:x})";

        private static Func<ReferenceNode, AIModelMetaData.PersoNames> dummyFuncNames = node =>
        {
            return new AIModelMetaData.PersoNames(node.param);
        };

        public static ReferenceResolver DummyResolver => new ReferenceResolver(dummyFunc, dummyFuncNames);
    }
}
