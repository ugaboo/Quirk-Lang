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

        public static readonly Function Add_Int_Int = new Function(Int, "__add__", Int, Int);
        public static readonly Function Add_Int_Float = new Function(Float, "__add__", Int, Float);
        public static readonly Function Add_Float_Int = new Function(Float, "__add__", Float, Int);
        public static readonly Function Add_Float_Float = new Function(Float, "__add__", Float, Float);
    }
}
