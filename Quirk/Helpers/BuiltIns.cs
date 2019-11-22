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
    }
}
