﻿using OpenSpaceCodeGen.Translation;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeModuleRef : Node {
        protected override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return NodeTranslator.Sequence(this.ToString(gen));
        }

        public override string ToString(CodeGenerator gen)
        {
            return param.ToString();
        }
    }
}
