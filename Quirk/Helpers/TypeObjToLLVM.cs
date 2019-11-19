using System;
using Quirk.AST;
using LLVMSharp;

namespace Quirk.Helpers
{
    public static class Extensions
    {
        public static LLVMTypeRef ToLLVM(this TypeObj obj)
        {
            if (obj == BuiltIns.Int) {
                return LLVM.Int32Type();
            } else if (obj == BuiltIns.Float) {
                return LLVM.FloatType();
            } else if (obj == BuiltIns.Bool) {
                return LLVM.Int8Type();
            } else {
                throw new ArgumentException();
            }
        }
    }
}
