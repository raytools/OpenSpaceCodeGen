using System;
using System.Collections.Generic;
using System.Text;
using OpenSpaceCodeGen.Nodes;

namespace OpenSpaceCodeGen.Translation.CBase.CSharp {

    public class LanguageTranslationPlainC : LanguageTranslationCBase
    {
        public override string IfDefSyntax => "#if ";
        public override string IfNotDefSyntax => "#if !";
        public override string EndIfDefSyntax => "#endif";

        public override NodeTranslator VectorTranslator(CodeGenerator gen, NodeVector node)
        {
            return NodeTranslator.Sequence("new Vector3(",0,",",1,",",2,")");
        }
    }
}
