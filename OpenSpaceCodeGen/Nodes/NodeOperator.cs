using System;
using System.Diagnostics;
using OpenSpaceCodeGen.AITypes;
using OpenSpaceCodeGen.Translation;
using OpenSpaceCodeGen.Translation.CBase;
using OpenSpaceCodeGen.Translation.Context;

namespace OpenSpaceCodeGen.Nodes {
    public class NodeOperator : Node {
        public override NodeTranslator GetTranslator(CodeGenerator gen)
        {
            return gen.Translation.OperatorTranslator(gen, this);
        }

        public EnumOperator GetOperator(CodeGenerator generator)
        {
            return generator.Type.OperatorTable[param];
        }

        public override string ToString(CodeGenerator generator)
        {
            return GetOperator(generator).ToString();
        }

        protected override void UpdateTypeHints(CodeGenerator gen)
        {
            if (OperatorTranslation.IsAssignmentOperator(GetOperator(gen))) {
                if (Children[0] is NodeDsgVarRef dsgVar) {
                    if (param < 0 || param >= gen.AIModel.MetaData.DsgVars.Count) {
                       return;
                    }

                    var dsgVarType = gen.AIModel.MetaData.DsgVars[dsgVar.param].Type;

                    void ModifyHint(TypeHints.TypeHintMap map, string name)
                    {
                        var h = map[name];
                        h.ReturnType = dsgVarType;
                        map.Hints[name] = h;
                    }

                    switch (Children[1]) {
                        case NodeField nodeField: ModifyHint(gen.Context.TypeHints.FieldTypeHints, nodeField.ToString(gen)); break;
                        case NodeFunction nodeFunction: ModifyHint(gen.Context.TypeHints.FunctionTypeHints, nodeFunction.ToString(gen)); break;
                        case NodeProcedure nodeProcedure: ModifyHint(gen.Context.TypeHints.ProcedureTypeHints, nodeProcedure.ToString(gen)); break;
                    }
                }
            }
        }
    }
}
