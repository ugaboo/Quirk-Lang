using System;
using System.Collections.Generic;
using LLVMSharp;
using Quirk.AST;

namespace Quirk.Helpers
{
    public class BuiltInsLLVM
    {
        LLVMBuilderRef builder;
        LLVMModuleRef module;

        Dictionary<ProgObj, LLVMValueRef> lib = new Dictionary<ProgObj, LLVMValueRef>();
               

        public BuiltInsLLVM(LLVMBuilderRef builder, LLVMModuleRef module)
        {
            this.builder = builder;
            this.module = module;

            Gen_Add_Int_Int();
            Gen_Add_Int_Float();
            Gen_Add_Float_Int();
            Gen_Add_Float_Float();
        }

        public LLVMValueRef Find(ProgObj obj)
        {
            return lib[obj];
        }

        LLVMValueRef[] GenHeader(Function f)
        {
            var retType = ((TypeObj)f.RetType).ToLLVM();

            var paramTypes = new LLVMTypeRef[f.Parameters.Count];
            for (var i = 0; i < f.Parameters.Count; i += 1) {
                paramTypes[i] = ((TypeObj)f.Parameters[i].Type).ToLLVM();
            }

            var funcType = LLVM.FunctionType(retType, paramTypes, false);

            var func = LLVM.AddFunction(module, f.Name, funcType);
            MakeInline(func);

            var par = new LLVMValueRef[f.Parameters.Count];
            for (var i = 0; i < f.Parameters.Count; i += 1) {
                var param = LLVM.GetParam(func, (uint)i);
                LLVM.SetValueName(param, f.Parameters[i].Name);                
                par[i] = param;
            }

            var block = LLVM.AppendBasicBlock(func, "entry");
            LLVM.PositionBuilderAtEnd(builder, block);

            lib[f] = func;
            return par;
        }

        void MakeInline(LLVMValueRef func)
        {
            var key = "alwaysinline";
            var attr = LLVM.CreateStringAttribute(LLVM.GetGlobalContext(), key, (uint)key.Length, "", 0);
            LLVM.AddAttributeAtIndex(func, LLVMAttributeIndex.LLVMAttributeFunctionIndex, attr);
        }

        void Gen_Add_Int_Int()
        {
            var par = GenHeader(BuiltIns.Add_Int_Int);
            var add = LLVM.BuildAdd(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, add);
        }

        void Gen_Add_Int_Float()
        {
            var par = GenHeader(BuiltIns.Add_Int_Float);
            var conv = LLVM.BuildSIToFP(builder, par[0], LLVM.FloatType(), "");
            var add = LLVM.BuildAdd(builder, conv, par[1], "");
            LLVM.BuildRet(builder, add);
        }

        void Gen_Add_Float_Int()
        {
            var par = GenHeader(BuiltIns.Add_Float_Int);
            var conv = LLVM.BuildSIToFP(builder, par[1], LLVM.FloatType(), "");
            var add = LLVM.BuildAdd(builder, par[0], conv, "");
            LLVM.BuildRet(builder, add);
        }

        void Gen_Add_Float_Float()
        {
            var par = GenHeader(BuiltIns.Add_Float_Float);
            var add = LLVM.BuildAdd(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, add);
        }
    }
}
