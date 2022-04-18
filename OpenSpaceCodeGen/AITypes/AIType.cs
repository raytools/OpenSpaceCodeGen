
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenSpaceCodeGen.AITypes {

    public partial class AIType {
        //public string[] functionTypes;
        public EnumKeyword[] KeywordTable;
        public EnumOperator[] OperatorTable;
        public string[] FunctionTable;
        public string[] ProcedureTable;
        public string[] ConditionTable;
        public DsgVarType[] DsgVarTypeTable;
        public string[] FieldTable;
        public string[] MetaActionTable;
        public NodeType[] NodeTypes;
        public Dictionary<EnumKeyword, DsgVarType> ExportedKeywords;

        public NodeType GetNodeType(byte functionType) {
            if (functionType < NodeTypes.Length) return NodeTypes[functionType];
            return NodeType.Unknown;
        }

        public byte GetNodeTypeID(NodeType nodeType)
        {
           return (byte)Array.IndexOf(NodeTypes, nodeType);
        }

      public AIType() { }

        public AIType(AIType original)
        {
            KeywordTable = original.KeywordTable;
            OperatorTable = original.OperatorTable;
            FunctionTable = original.FunctionTable;
            ProcedureTable = original.ProcedureTable;
            ConditionTable = original.ConditionTable;
            DsgVarTypeTable = original.DsgVarTypeTable;
            FieldTable = original.FieldTable;
            MetaActionTable = original.MetaActionTable;
            NodeTypes = original.NodeTypes;
        }

    }
}
