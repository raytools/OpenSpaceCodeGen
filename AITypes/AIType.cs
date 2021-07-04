
using System.Collections.Generic;

namespace OpenSpaceCodeGen.AITypes {

    public partial class AIType {
        //public string[] functionTypes;
        public EnumKeyword[] KeywordTable;
        public EnumOperator[] OperatorTable;
        public string[] FunctionTable;
        public string[] ProcedureTable;
        public string[] ConditionTable;
        public string[] FieldTable;
        public string[] MetaActionTable;
        public NodeType[] NodeTypes;

        public NodeType GetNodeType(byte functionType) {
            if (functionType < NodeTypes.Length) return NodeTypes[functionType];
            return NodeType.Unknown;
        }

        public AIType() { }

        public AIType(AIType original)
        {
            KeywordTable = original.KeywordTable;
            OperatorTable = original.OperatorTable;
            FunctionTable = original.FunctionTable;
            ProcedureTable = original.ProcedureTable;
            ConditionTable = original.ConditionTable;
            FieldTable = original.FieldTable;
            MetaActionTable = original.MetaActionTable;
            NodeTypes = original.NodeTypes;
        }

    }
}
