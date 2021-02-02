
namespace OpenSpaceCodeGen.AITypes {
    public partial class AITypes {
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

    }
}
