namespace OpenSpaceCodeGen.AITypes {
    public enum EnumOperator {
        Operator_Plus, // 0
        Operator_Minus,
        Operator_Mul,
        Operator_Div,
        Operator_UnaryMinus,
        Operator_PlusAffect, // 5
        Operator_MinusAffect,
        Operator_MulAffect,
        Operator_DivAffect,
        Operator_PlusPlusAffect,
        Operator_MinusMinusAffect, // 10
        Operator_Affect,
        Operator_Dot,
        Dot_X,
        Dot_Y,
        Dot_Z, // 15
        Operator_VectorPlusVector,
        Operator_VectorMinusVector,
        Operator_VectorMulScalar,
        Operator_VectorDivScalar,
        Operator_VectorUnaryMinus, // 20
        Dot_X_Assign,
        Dot_Y_Assign,
        Dot_Z_Assign,
        Operator_Ultra,
        Operator_ModelCast, // 25
        Operator_Array,
        Operator_AffectArray,
        Operator_Mod
    }
}
