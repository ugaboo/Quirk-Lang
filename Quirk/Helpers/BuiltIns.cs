using System;
using System.Collections.Generic;
using LLVMSharp;
using Quirk.AST;

namespace Quirk.Helpers
{
    public static class BuiltIns
    {
        public static readonly TypeObj Int = new TypeObj("Int");
        public static readonly TypeObj Float = new TypeObj("Float");
        public static readonly TypeObj Bool = new TypeObj("Bool");

        public static readonly Function Print_Int = new Function(null, "print", Int);
        public static readonly Function Print_Float = new Function(null, "print", Float);
        public static readonly Function Print_Bool = new Function(null, "print", Bool);

        public static readonly Function Add_Int_Int = new Function(Int, "__add__", Int, Int);
        public static readonly Function Add_Int_Float = new Function(Float, "__add__", Int, Float);
        public static readonly Function Add_Float_Int = new Function(Float, "__add__", Float, Int);
        public static readonly Function Add_Float_Float = new Function(Float, "__add__", Float, Float);
        public static readonly Function Add_Int_Bool = new Function(Int, "__add__", Int, Bool);
        public static readonly Function Add_Bool_Int = new Function(Int, "__add__", Bool, Int);
        public static readonly Function Add_Bool_Bool = new Function(Int, "__add__", Bool, Bool);
        public static readonly Function Add_Float_Bool = new Function(Float, "__add__", Float, Bool);
        public static readonly Function Add_Bool_Float = new Function(Float, "__add__", Bool, Float);

        public static readonly Function Sub_Int_Int = new Function(Int, "__sub__", Int, Int);
        public static readonly Function Sub_Int_Float = new Function(Float, "__sub__", Int, Float);
        public static readonly Function Sub_Float_Int = new Function(Float, "__sub__", Float, Int);
        public static readonly Function Sub_Float_Float = new Function(Float, "__sub__", Float, Float);
        public static readonly Function Sub_Int_Bool = new Function(Int, "__sub__", Int, Bool);
        public static readonly Function Sub_Bool_Int = new Function(Int, "__sub__", Bool, Int);
        public static readonly Function Sub_Bool_Bool = new Function(Int, "__sub__", Bool, Bool);
        public static readonly Function Sub_Float_Bool = new Function(Float, "__sub__", Float, Bool);
        public static readonly Function Sub_Bool_Float = new Function(Float, "__sub__", Bool, Float);

        public static readonly Function Mul_Int_Int = new Function(Int, "__mul__", Int, Int);
        public static readonly Function Mul_Int_Float = new Function(Float, "__mul__", Int, Float);
        public static readonly Function Mul_Float_Int = new Function(Float, "__mul__", Float, Int);
        public static readonly Function Mul_Float_Float = new Function(Float, "__mul__", Float, Float);
        public static readonly Function Mul_Int_Bool = new Function(Int, "__mul__", Int, Bool);
        public static readonly Function Mul_Bool_Int = new Function(Int, "__mul__", Bool, Int);
        public static readonly Function Mul_Bool_Bool = new Function(Int, "__mul__", Bool, Bool);
        public static readonly Function Mul_Float_Bool = new Function(Float, "__mul__", Float, Bool);
        public static readonly Function Mul_Bool_Float = new Function(Float, "__mul__", Bool, Float);

        public static readonly Function TrueDiv_Int_Int = new Function(Float, "__truediv__", Int, Int);
        public static readonly Function TrueDiv_Int_Float = new Function(Float, "__truediv__", Int, Float);
        public static readonly Function TrueDiv_Float_Int = new Function(Float, "__truediv__", Float, Int);
        public static readonly Function TrueDiv_Float_Float = new Function(Float, "__truediv__", Float, Float);
        public static readonly Function TrueDiv_Int_Bool = new Function(Float, "__truediv__", Int, Bool);
        public static readonly Function TrueDiv_Bool_Int = new Function(Float, "__truediv__", Bool, Int);
        public static readonly Function TrueDiv_Bool_Bool = new Function(Float, "__truediv__", Bool, Bool);
        public static readonly Function TrueDiv_Float_Bool = new Function(Float, "__truediv__", Float, Bool);
        public static readonly Function TrueDiv_Bool_Float = new Function(Float, "__truediv__", Bool, Float);

        public static readonly Function FloorDiv_Int_Int = new Function(Int, "__floordiv__", Int, Int);
        public static readonly Function FloorDiv_Int_Float = new Function(Float, "__floordiv__", Int, Float);
        public static readonly Function FloorDiv_Float_Int = new Function(Float, "__floordiv__", Float, Int);
        public static readonly Function FloorDiv_Float_Float = new Function(Float, "__floordiv__", Float, Float);
        public static readonly Function FloorDiv_Int_Bool = new Function(Int, "__floordiv__", Int, Bool);
        public static readonly Function FloorDiv_Bool_Int = new Function(Int, "__floordiv__", Bool, Int);
        public static readonly Function FloorDiv_Bool_Bool = new Function(Int, "__floordiv__", Bool, Bool);
        public static readonly Function FloorDiv_Float_Bool = new Function(Float, "__floordiv__", Float, Bool);
        public static readonly Function FloorDiv_Bool_Float = new Function(Float, "__floordiv__", Bool, Float);

        public static readonly Function Mod_Int_Int = new Function(Int, "__mod__", Int, Int);
        public static readonly Function Mod_Int_Float = new Function(Float, "__mod__", Int, Float);
        public static readonly Function Mod_Float_Int = new Function(Float, "__mod__", Float, Int);
        public static readonly Function Mod_Float_Float = new Function(Float, "__mod__", Float, Float);
        public static readonly Function Mod_Int_Bool = new Function(Int, "__mod__", Int, Bool);
        public static readonly Function Mod_Bool_Int = new Function(Int, "__mod__", Bool, Int);
        public static readonly Function Mod_Bool_Bool = new Function(Int, "__mod__", Bool, Bool);
        public static readonly Function Mod_Float_Bool = new Function(Float, "__mod__", Float, Bool);
        public static readonly Function Mod_Bool_Float = new Function(Float, "__mod__", Bool, Float);

        public static readonly Function Pow_Int_Int = new Function(Int, "__pow__", Int, Int);
        public static readonly Function Pow_Int_Float = new Function(Float, "__pow__", Int, Float);
        public static readonly Function Pow_Float_Int = new Function(Float, "__pow__", Float, Int);
        public static readonly Function Pow_Float_Float = new Function(Float, "__pow__", Float, Float);
        public static readonly Function Pow_Int_Bool = new Function(Int, "__pow__", Int, Bool);
        public static readonly Function Pow_Bool_Int = new Function(Int, "__pow__", Bool, Int);
        public static readonly Function Pow_Bool_Bool = new Function(Int, "__pow__", Bool, Bool);
        public static readonly Function Pow_Float_Bool = new Function(Float, "__pow__", Float, Bool);
        public static readonly Function Pow_Bool_Float = new Function(Float, "__pow__", Bool, Float);

        public static readonly Function Pos_Int = new Function(Int, "__pos__", Int);
        public static readonly Function Pos_Float = new Function(Float, "__pos__", Float);
        public static readonly Function Pos_Bool = new Function(Int, "__pos__", Bool);

        public static readonly Function Neg_Int = new Function(Int, "__neg__", Int);
        public static readonly Function Neg_Float = new Function(Float, "__neg__", Float);
        public static readonly Function Neg_Bool = new Function(Int, "__neg__", Bool);

        public static readonly Function Lt_Int_Int = new Function(Bool, "__lt__", Int, Int);
        public static readonly Function Lt_Int_Float = new Function(Bool, "__lt__", Int, Float);
        public static readonly Function Lt_Float_Int = new Function(Bool, "__lt__", Float, Int);
        public static readonly Function Lt_Float_Float = new Function(Bool, "__lt__", Float, Float);
        public static readonly Function Lt_Int_Bool = new Function(Bool, "__lt__", Int, Bool);
        public static readonly Function Lt_Bool_Int = new Function(Bool, "__lt__", Bool, Int);
        public static readonly Function Lt_Bool_Bool = new Function(Bool, "__lt__", Bool, Bool);
        public static readonly Function Lt_Float_Bool = new Function(Bool, "__lt__", Float, Bool);
        public static readonly Function Lt_Bool_Float = new Function(Bool, "__lt__", Bool, Float);

        public static readonly Function Gt_Int_Int = new Function(Bool, "__gt__", Int, Int);
        public static readonly Function Gt_Int_Float = new Function(Bool, "__gt__", Int, Float);
        public static readonly Function Gt_Float_Int = new Function(Bool, "__gt__", Float, Int);
        public static readonly Function Gt_Float_Float = new Function(Bool, "__gt__", Float, Float);
        public static readonly Function Gt_Int_Bool = new Function(Bool, "__gt__", Int, Bool);
        public static readonly Function Gt_Bool_Int = new Function(Bool, "__gt__", Bool, Int);
        public static readonly Function Gt_Bool_Bool = new Function(Bool, "__gt__", Bool, Bool);
        public static readonly Function Gt_Float_Bool = new Function(Bool, "__gt__", Float, Bool);
        public static readonly Function Gt_Bool_Float = new Function(Bool, "__gt__", Bool, Float);

        public static readonly Function Le_Int_Int = new Function(Bool, "__le__", Int, Int);
        public static readonly Function Le_Int_Float = new Function(Bool, "__le__", Int, Float);
        public static readonly Function Le_Float_Int = new Function(Bool, "__le__", Float, Int);
        public static readonly Function Le_Float_Float = new Function(Bool, "__le__", Float, Float);
        public static readonly Function Le_Int_Bool = new Function(Bool, "__le__", Int, Bool);
        public static readonly Function Le_Bool_Int = new Function(Bool, "__le__", Bool, Int);
        public static readonly Function Le_Bool_Bool = new Function(Bool, "__le__", Bool, Bool);
        public static readonly Function Le_Float_Bool = new Function(Bool, "__le__", Float, Bool);
        public static readonly Function Le_Bool_Float = new Function(Bool, "__le__", Bool, Float);

        public static readonly Function Ge_Int_Int = new Function(Bool, "__ge__", Int, Int);
        public static readonly Function Ge_Int_Float = new Function(Bool, "__ge__", Int, Float);
        public static readonly Function Ge_Float_Int = new Function(Bool, "__ge__", Float, Int);
        public static readonly Function Ge_Float_Float = new Function(Bool, "__ge__", Float, Float);
        public static readonly Function Ge_Int_Bool = new Function(Bool, "__ge__", Int, Bool);
        public static readonly Function Ge_Bool_Int = new Function(Bool, "__ge__", Bool, Int);
        public static readonly Function Ge_Bool_Bool = new Function(Bool, "__ge__", Bool, Bool);
        public static readonly Function Ge_Float_Bool = new Function(Bool, "__ge__", Float, Bool);
        public static readonly Function Ge_Bool_Float = new Function(Bool, "__ge__", Bool, Float);

        public static readonly Function Eq_Int_Int = new Function(Bool, "__eq__", Int, Int);
        public static readonly Function Eq_Int_Float = new Function(Bool, "__eq__", Int, Float);
        public static readonly Function Eq_Float_Int = new Function(Bool, "__eq__", Float, Int);
        public static readonly Function Eq_Float_Float = new Function(Bool, "__eq__", Float, Float);
        public static readonly Function Eq_Int_Bool = new Function(Bool, "__eq__", Int, Bool);
        public static readonly Function Eq_Bool_Int = new Function(Bool, "__eq__", Bool, Int);
        public static readonly Function Eq_Bool_Bool = new Function(Bool, "__eq__", Bool, Bool);
        public static readonly Function Eq_Float_Bool = new Function(Bool, "__eq__", Float, Bool);
        public static readonly Function Eq_Bool_Float = new Function(Bool, "__eq__", Bool, Float);

        public static readonly Function Ne_Int_Int = new Function(Bool, "__ne__", Int, Int);
        public static readonly Function Ne_Int_Float = new Function(Bool, "__ne__", Int, Float);
        public static readonly Function Ne_Float_Int = new Function(Bool, "__ne__", Float, Int);
        public static readonly Function Ne_Float_Float = new Function(Bool, "__ne__", Float, Float);
        public static readonly Function Ne_Int_Bool = new Function(Bool, "__ne__", Int, Bool);
        public static readonly Function Ne_Bool_Int = new Function(Bool, "__ne__", Bool, Int);
        public static readonly Function Ne_Bool_Bool = new Function(Bool, "__ne__", Bool, Bool);
        public static readonly Function Ne_Float_Bool = new Function(Bool, "__ne__", Float, Bool);
        public static readonly Function Ne_Bool_Float = new Function(Bool, "__ne__", Bool, Float);

        public static readonly Function Not_Int = new Function(Bool, "__not__", Int);
        public static readonly Function Not_Float = new Function(Bool, "__not__", Float);
        public static readonly Function Not_Bool = new Function(Bool, "__not__", Bool);
    }
}
