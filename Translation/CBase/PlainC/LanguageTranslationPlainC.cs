using System;
using System.Collections.Generic;
using System.Text;
using OpenSpaceCodeGen.Nodes;

namespace OpenSpaceCodeGen.Translation.CBase.PlainC {

    public class LanguageTranslationPlainC : LanguageTranslationCBase
    {
        public override string IfDefSyntax => "#ifdef ";
        public override string IfNotDefSyntax => "#ifndef ";
        public override string EndIfDefSyntax => "#endif";

        public override NodeTranslator VectorTranslator(CodeGenerator gen, NodeVector node)
        {
            return NodeTranslator.Sequence("Vector3(", 0, ",", 1, ",", 2, ")");
        }
    }
}
