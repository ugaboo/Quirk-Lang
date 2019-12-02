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

        LLVMValueRef printf, pow;

        Dictionary<ProgObj, LLVMValueRef> lib = new Dictionary<ProgObj, LLVMValueRef>();
               

        public BuiltInsLLVM(LLVMBuilderRef builder, LLVMModuleRef module)
        {
            this.builder = builder;
            this.module = module;

            printf = LLVM.AddFunction(module, "printf", LLVM.FunctionType(LLVM.Int32Type(), new[] { LLVM.PointerType(LLVM.Int8Type(), 0) }, true));
            pow = LLVM.AddFunction(module, "pow", LLVM.FunctionType(LLVM.DoubleType(), new[] { LLVM.DoubleType(), LLVM.DoubleType() }, false));

            #region print

            Gen_Print_Int();
            Gen_Print_Float();
            Gen_Print_Bool();

            #endregion

            #region __add__

            Gen_Add_Int_Int();
            Gen_Add_Int_Float();
            Gen_Add_Float_Int();
            Gen_Add_Float_Float();
            Gen_Add_Int_Bool();
            Gen_Add_Bool_Int();
            Gen_Add_Bool_Bool();
            Gen_Add_Float_Bool();
            Gen_Add_Bool_Float();

            #endregion

            #region __sub__

            Gen_Sub_Int_Int();
            Gen_Sub_Int_Float();
            Gen_Sub_Float_Int();
            Gen_Sub_Float_Float();
            Gen_Sub_Int_Bool();
            Gen_Sub_Bool_Int();
            Gen_Sub_Bool_Bool();
            Gen_Sub_Float_Bool();
            Gen_Sub_Bool_Float();

            #endregion

            #region __mul__

            Gen_Mul_Int_Int();
            Gen_Mul_Int_Float();
            Gen_Mul_Float_Int();
            Gen_Mul_Float_Float();
            Gen_Mul_Int_Bool();
            Gen_Mul_Bool_Int();
            Gen_Mul_Bool_Bool();
            Gen_Mul_Float_Bool();
            Gen_Mul_Bool_Float();

            #endregion

            #region __truediv__

            Gen_TrueDiv_Int_Int();
            Gen_TrueDiv_Int_Float();
            Gen_TrueDiv_Float_Int();
            Gen_TrueDiv_Float_Float();
            Gen_TrueDiv_Int_Bool();
            Gen_TrueDiv_Bool_Int();
            Gen_TrueDiv_Bool_Bool();
            Gen_TrueDiv_Float_Bool();
            Gen_TrueDiv_Bool_Float();

            #endregion

            #region __floordiv__

            Gen_FloorDiv_Int_Int();
            Gen_FloorDiv_Int_Float();
            Gen_FloorDiv_Float_Int();
            Gen_FloorDiv_Float_Float();
            Gen_FloorDiv_Int_Bool();
            Gen_FloorDiv_Bool_Int();
            Gen_FloorDiv_Bool_Bool();
            Gen_FloorDiv_Float_Bool();
            Gen_FloorDiv_Bool_Float();

            #endregion

            #region __mod__

            Gen_Mod_Int_Int();
            Gen_Mod_Int_Float();
            Gen_Mod_Float_Int();
            Gen_Mod_Float_Float();
            Gen_Mod_Int_Bool();
            Gen_Mod_Bool_Int();
            Gen_Mod_Bool_Bool();
            Gen_Mod_Float_Bool();
            Gen_Mod_Bool_Float();

            #endregion

            #region __pow__

            Gen_Pow_Int_Int();
            Gen_Pow_Int_Float();
            Gen_Pow_Float_Int();
            Gen_Pow_Float_Float();
            Gen_Pow_Int_Bool();
            Gen_Pow_Bool_Int();
            Gen_Pow_Bool_Bool();
            Gen_Pow_Float_Bool();
            Gen_Pow_Bool_Float();

            #endregion

            #region __pos__

            Gen_Pos_Int();
            Gen_Pos_Float();
            Gen_Pos_Bool();

            #endregion

            #region __neg__

            Gen_Neg_Int();
            Gen_Neg_Float();
            Gen_Neg_Bool();

            #endregion

            #region __lt__

            Gen_Lt_Int_Int();
            Gen_Lt_Int_Float();
            Gen_Lt_Float_Int();
            Gen_Lt_Float_Float();
            Gen_Lt_Int_Bool();
            Gen_Lt_Bool_Int();
            Gen_Lt_Bool_Bool();
            Gen_Lt_Float_Bool();
            Gen_Lt_Bool_Float();

            #endregion

            #region __gt__

            Gen_Gt_Int_Int();
            Gen_Gt_Int_Float();
            Gen_Gt_Float_Int();
            Gen_Gt_Float_Float();
            Gen_Gt_Int_Bool();
            Gen_Gt_Bool_Int();
            Gen_Gt_Bool_Bool();
            Gen_Gt_Float_Bool();
            Gen_Gt_Bool_Float();

            #endregion

            #region __le__

            Gen_Le_Int_Int();
            Gen_Le_Int_Float();
            Gen_Le_Float_Int();
            Gen_Le_Float_Float();
            Gen_Le_Int_Bool();
            Gen_Le_Bool_Int();
            Gen_Le_Bool_Bool();
            Gen_Le_Float_Bool();
            Gen_Le_Bool_Float();

            #endregion

            #region __ge__

            Gen_Ge_Int_Int();
            Gen_Ge_Int_Float();
            Gen_Ge_Float_Int();
            Gen_Ge_Float_Float();
            Gen_Ge_Int_Bool();
            Gen_Ge_Bool_Int();
            Gen_Ge_Bool_Bool();
            Gen_Ge_Float_Bool();
            Gen_Ge_Bool_Float();

            #endregion

            #region __eq__

            Gen_Eq_Int_Int();
            Gen_Eq_Int_Float();
            Gen_Eq_Float_Int();
            Gen_Eq_Float_Float();
            Gen_Eq_Int_Bool();
            Gen_Eq_Bool_Int();
            Gen_Eq_Bool_Bool();
            Gen_Eq_Float_Bool();
            Gen_Eq_Bool_Float();

            #endregion

            #region __ne__

            Gen_Ne_Int_Int();
            Gen_Ne_Int_Float();
            Gen_Ne_Float_Int();
            Gen_Ne_Float_Float();
            Gen_Ne_Int_Bool();
            Gen_Ne_Bool_Int();
            Gen_Ne_Bool_Bool();
            Gen_Ne_Float_Bool();
            Gen_Ne_Bool_Float();

            #endregion

            #region __not__

            Gen_Not_Int();
            Gen_Not_Float();
            Gen_Not_Bool();

            #endregion

            #region __bitand__

            Gen_BitAnd_Int_Int();
            Gen_BitAnd_Int_Bool();
            Gen_BitAnd_Bool_Int();
            Gen_BitAnd_Bool_Bool();

            #endregion

            #region __bitor__

            Gen_BitOr_Int_Int();
            Gen_BitOr_Int_Bool();
            Gen_BitOr_Bool_Int();
            Gen_BitOr_Bool_Bool();

            #endregion

            #region __bitxor__

            Gen_BitXor_Int_Int();
            Gen_BitXor_Int_Bool();
            Gen_BitXor_Bool_Int();
            Gen_BitXor_Bool_Bool();

            #endregion

            #region __invert__

            Gen_Invert_Int();
            Gen_Invert_Bool();

            #endregion

            #region __lshift__

            Gen_LShift_Int_Int();
            Gen_LShift_Int_Bool();
            Gen_LShift_Bool_Int();
            Gen_LShift_Bool_Bool();

            #endregion

            #region __rshift__

            Gen_RShift_Int_Int();
            Gen_RShift_Int_Bool();
            Gen_RShift_Bool_Int();
            Gen_RShift_Bool_Bool();

            #endregion
        }

        public LLVMValueRef Find(ProgObj obj)
        {
            return lib[obj];
        }

        LLVMValueRef[] GenHeader(Function f, bool inline = true)
        {
            var retType = f.RetType != null ? ((TypeObj)f.RetType).ToLLVM() : LLVM.VoidType();

            var paramTypes = new LLVMTypeRef[f.Parameters.Count];
            for (var i = 0; i < f.Parameters.Count; i += 1) {
                paramTypes[i] = ((TypeObj)f.Parameters[i].Type).ToLLVM();
            }

            var funcType = LLVM.FunctionType(retType, paramTypes, false);

            var func = LLVM.AddFunction(module, f.Name, funcType);
            if (inline) {
                MakeInline(func);
            }

            var par = new LLVMValueRef[f.Parameters.Count];
            for (var i = 0; i < f.Parameters.Count; i += 1) {
                var param = LLVM.GetParam(func, (uint)i);
                LLVM.SetValueName(param, f.Parameters[i].Name);                
                par[i] = param;
            }

            var block = LLVM.AppendBasicBlock(func, "begin");
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

        #region print

        void Gen_Print_Int()
        {
            var par = GenHeader(BuiltIns.Print_Int, inline: false);
            var str = LLVM.BuildGlobalStringPtr(builder, "%d\n", "");
            LLVM.BuildCall(builder, printf, new[] { str, par[0] }, "");
            LLVM.BuildRetVoid(builder);
        }

        void Gen_Print_Float()
        {
            var par = GenHeader(BuiltIns.Print_Float, inline: false);
            var str = LLVM.BuildGlobalStringPtr(builder, "%g\n", "");
            var ext = LLVM.BuildFPExt(builder, par[0], LLVM.DoubleType(), "");
            LLVM.BuildCall(builder, printf, new[] { str, ext }, "");
            LLVM.BuildRetVoid(builder);
        }

        void Gen_Print_Bool()
        {
            var par = GenHeader(BuiltIns.Print_Bool, inline: false);
            var t = LLVM.BuildGlobalStringPtr(builder, "True\n", "");
            var f = LLVM.BuildGlobalStringPtr(builder, "False\n", "");
            var sel = LLVM.BuildSelect(builder, par[0], t, f, "");
            LLVM.BuildCall(builder, printf, new[] { sel }, "");
            LLVM.BuildRetVoid(builder);
        }

        #endregion

        #region __add__

        void Gen_Add_Int_Int()
        {
            var par = GenHeader(BuiltIns.Add_Int_Int);
            var op = LLVM.BuildNSWAdd(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Add_Int_Float()
        {
            var par = GenHeader(BuiltIns.Add_Int_Float);
            var conv0 = LLVM.BuildSIToFP(builder, par[0], LLVM.FloatType(), "");
            var op = LLVM.BuildFAdd(builder, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Add_Float_Int()
        {
            var par = GenHeader(BuiltIns.Add_Float_Int);
            var conv1 = LLVM.BuildSIToFP(builder, par[1], LLVM.FloatType(), "");
            var op = LLVM.BuildFAdd(builder, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Add_Float_Float()
        {
            var par = GenHeader(BuiltIns.Add_Float_Float);
            var op = LLVM.BuildFAdd(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Add_Int_Bool()
        {
            var par = GenHeader(BuiltIns.Add_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildNSWAdd(builder, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Add_Bool_Int()
        {
            var par = GenHeader(BuiltIns.Add_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildNSWAdd(builder, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Add_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.Add_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildNSWAdd(builder, ext0, ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Add_Float_Bool()
        {
            var par = GenHeader(BuiltIns.Add_Float_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.FloatType(), "");
            var op = LLVM.BuildFAdd(builder, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Add_Bool_Float()
        {
            var par = GenHeader(BuiltIns.Add_Bool_Float);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.FloatType(), "");
            var op = LLVM.BuildFAdd(builder, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        #endregion

        #region __sub__

        void Gen_Sub_Int_Int()
        {
            var par = GenHeader(BuiltIns.Sub_Int_Int);
            var op = LLVM.BuildNSWSub(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Sub_Int_Float()
        {
            var par = GenHeader(BuiltIns.Sub_Int_Float);
            var conv0 = LLVM.BuildSIToFP(builder, par[0], LLVM.FloatType(), "");
            var op = LLVM.BuildFSub(builder, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Sub_Float_Int()
        {
            var par = GenHeader(BuiltIns.Sub_Float_Int);
            var conv1 = LLVM.BuildSIToFP(builder, par[1], LLVM.FloatType(), "");
            var op = LLVM.BuildFSub(builder, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Sub_Float_Float()
        {
            var par = GenHeader(BuiltIns.Sub_Float_Float);
            var op = LLVM.BuildFSub(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Sub_Int_Bool()
        {
            var par = GenHeader(BuiltIns.Sub_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildNSWSub(builder, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Sub_Bool_Int()
        {
            var par = GenHeader(BuiltIns.Sub_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildNSWSub(builder, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Sub_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.Sub_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildNSWSub(builder, ext0, ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Sub_Float_Bool()
        {
            var par = GenHeader(BuiltIns.Sub_Float_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.FloatType(), "");
            var op = LLVM.BuildFSub(builder, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Sub_Bool_Float()
        {
            var par = GenHeader(BuiltIns.Sub_Bool_Float);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.FloatType(), "");
            var op = LLVM.BuildFSub(builder, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        #endregion

        #region __mul__

        void Gen_Mul_Int_Int()
        {
            var par = GenHeader(BuiltIns.Mul_Int_Int);
            var op = LLVM.BuildNSWMul(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mul_Int_Float()
        {
            var par = GenHeader(BuiltIns.Mul_Int_Float);
            var conv0 = LLVM.BuildSIToFP(builder, par[0], LLVM.FloatType(), "");
            var op = LLVM.BuildFMul(builder, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mul_Float_Int()
        {
            var par = GenHeader(BuiltIns.Mul_Float_Int);
            var conv1 = LLVM.BuildSIToFP(builder, par[1], LLVM.FloatType(), "");
            var op = LLVM.BuildFMul(builder, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mul_Float_Float()
        {
            var par = GenHeader(BuiltIns.Mul_Float_Float);
            var op = LLVM.BuildFMul(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mul_Int_Bool()
        {
            var par = GenHeader(BuiltIns.Mul_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildNSWMul(builder, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mul_Bool_Int()
        {
            var par = GenHeader(BuiltIns.Mul_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildNSWMul(builder, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mul_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.Mul_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildNSWMul(builder, ext0, ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mul_Float_Bool()
        {
            var par = GenHeader(BuiltIns.Mul_Float_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.FloatType(), "");
            var op = LLVM.BuildFMul(builder, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mul_Bool_Float()
        {
            var par = GenHeader(BuiltIns.Mul_Bool_Float);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.FloatType(), "");
            var op = LLVM.BuildFMul(builder, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        #endregion

        #region __truediv__

        void Gen_TrueDiv_Int_Int()
        {
            var par = GenHeader(BuiltIns.TrueDiv_Int_Int);
            var conv0 = LLVM.BuildSIToFP(builder, par[0], LLVM.FloatType(), "");
            var conv1 = LLVM.BuildSIToFP(builder, par[1], LLVM.FloatType(), "");
            var op = LLVM.BuildFDiv(builder, conv0, conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_TrueDiv_Int_Float()
        {
            var par = GenHeader(BuiltIns.TrueDiv_Int_Float);
            var conv0 = LLVM.BuildSIToFP(builder, par[0], LLVM.FloatType(), "");
            var op = LLVM.BuildFDiv(builder, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_TrueDiv_Float_Int()
        {
            var par = GenHeader(BuiltIns.TrueDiv_Float_Int);
            var conv1 = LLVM.BuildSIToFP(builder, par[1], LLVM.FloatType(), "");
            var op = LLVM.BuildFDiv(builder, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_TrueDiv_Float_Float()
        {
            var par = GenHeader(BuiltIns.TrueDiv_Float_Float);
            var op = LLVM.BuildFDiv(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_TrueDiv_Int_Bool()
        {
            var par = GenHeader(BuiltIns.TrueDiv_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, par[0], LLVM.FloatType(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.FloatType(), "");
            var op = LLVM.BuildFDiv(builder, conv0, conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_TrueDiv_Bool_Int()
        {
            var par = GenHeader(BuiltIns.TrueDiv_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.FloatType(), "");
            var conv1 = LLVM.BuildSIToFP(builder, par[1], LLVM.FloatType(), "");
            var op = LLVM.BuildFDiv(builder, conv0, conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_TrueDiv_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.TrueDiv_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.FloatType(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.FloatType(), "");
            var op = LLVM.BuildFDiv(builder, conv0, conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_TrueDiv_Float_Bool()
        {
            var par = GenHeader(BuiltIns.TrueDiv_Float_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.FloatType(), "");
            var op = LLVM.BuildFDiv(builder, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_TrueDiv_Bool_Float()
        {
            var par = GenHeader(BuiltIns.TrueDiv_Bool_Float);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.FloatType(), "");
            var op = LLVM.BuildFDiv(builder, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        #endregion

        #region __floordiv__

        void Gen_FloorDiv_Int_Int()
        {
            var par = GenHeader(BuiltIns.FloorDiv_Int_Int);
            var op = LLVM.BuildSDiv(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_FloorDiv_Int_Float()
        {
            var par = GenHeader(BuiltIns.FloorDiv_Int_Float);
            var conv1 = LLVM.BuildFPToSI(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildSDiv(builder, par[0], conv1, "");
            var fp = LLVM.BuildSIToFP(builder, op, LLVM.FloatType(), "");
            LLVM.BuildRet(builder, fp);
        }

        void Gen_FloorDiv_Float_Int()
        {
            var par = GenHeader(BuiltIns.FloorDiv_Float_Int);
            var conv0 = LLVM.BuildFPToSI(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildSDiv(builder, conv0, par[1], "");
            var fp = LLVM.BuildSIToFP(builder, op, LLVM.FloatType(), "");
            LLVM.BuildRet(builder, fp);
        }

        void Gen_FloorDiv_Float_Float()
        {
            var par = GenHeader(BuiltIns.FloorDiv_Float_Float);
            var conv0 = LLVM.BuildFPToSI(builder, par[0], LLVM.Int32Type(), "");
            var conv1 = LLVM.BuildFPToSI(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildSDiv(builder, conv0, conv1, "");
            var fp = LLVM.BuildSIToFP(builder, op, LLVM.FloatType(), "");
            LLVM.BuildRet(builder, fp);
        }

        void Gen_FloorDiv_Int_Bool()
        {
            var par = GenHeader(BuiltIns.FloorDiv_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildSDiv(builder, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_FloorDiv_Bool_Int()
        {
            var par = GenHeader(BuiltIns.FloorDiv_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildSDiv(builder, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_FloorDiv_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.FloorDiv_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildSDiv(builder, ext0, ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_FloorDiv_Float_Bool()
        {
            var par = GenHeader(BuiltIns.FloorDiv_Float_Bool);
            var conv0 = LLVM.BuildFPToSI(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildSDiv(builder, conv0, ext1, "");
            var fp = LLVM.BuildSIToFP(builder, op, LLVM.FloatType(), "");
            LLVM.BuildRet(builder, fp);
        }

        void Gen_FloorDiv_Bool_Float()
        {
            var par = GenHeader(BuiltIns.FloorDiv_Bool_Float);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var conv1 = LLVM.BuildFPToSI(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildSDiv(builder, ext0, conv1, "");
            var fp = LLVM.BuildSIToFP(builder, op, LLVM.FloatType(), "");
            LLVM.BuildRet(builder, fp);
        }

        #endregion

        #region __mod__

        void Gen_Mod_Int_Int()
        {
            var par = GenHeader(BuiltIns.Mod_Int_Int);
            var op = LLVM.BuildSRem(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mod_Int_Float()
        {
            var par = GenHeader(BuiltIns.Mod_Int_Float);
            var conv0 = LLVM.BuildSIToFP(builder, par[0], LLVM.FloatType(), "");
            var op = LLVM.BuildFRem(builder, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mod_Float_Int()
        {
            var par = GenHeader(BuiltIns.Mod_Float_Int);
            var conv1 = LLVM.BuildSIToFP(builder, par[1], LLVM.FloatType(), "");
            var op = LLVM.BuildFRem(builder, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mod_Float_Float()
        {
            var par = GenHeader(BuiltIns.Mod_Float_Float);
            var op = LLVM.BuildFRem(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mod_Int_Bool()
        {
            var par = GenHeader(BuiltIns.Mod_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildSRem(builder, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mod_Bool_Int()
        {
            var par = GenHeader(BuiltIns.Mod_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildSRem(builder, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mod_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.Mod_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildSRem(builder, ext0, ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mod_Float_Bool()
        {
            var par = GenHeader(BuiltIns.Mod_Float_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.FloatType(), "");
            var op = LLVM.BuildFRem(builder, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Mod_Bool_Float()
        {
            var par = GenHeader(BuiltIns.Mod_Bool_Float);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.FloatType(), "");
            var op = LLVM.BuildFRem(builder, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        #endregion

        #region __pow__

        void Gen_Pow_Int_Int()
        {
            var par = GenHeader(BuiltIns.Pow_Int_Int);
            var conv0 = LLVM.BuildSIToFP(builder, par[0], LLVM.DoubleType(), "");
            var conv1 = LLVM.BuildSIToFP(builder, par[1], LLVM.DoubleType(), "");
            var call = LLVM.BuildCall(builder, pow, new[] { conv0, conv1 }, "");
            var si = LLVM.BuildFPToSI(builder, call, LLVM.Int32Type(), "");
            LLVM.BuildRet(builder, si);
        }

        void Gen_Pow_Int_Float()
        {
            var par = GenHeader(BuiltIns.Pow_Int_Float);
            var conv = LLVM.BuildSIToFP(builder, par[0], LLVM.DoubleType(), "");
            var ext = LLVM.BuildFPExt(builder, par[1], LLVM.DoubleType(), "");
            var call = LLVM.BuildCall(builder, pow, new[] { conv, ext }, "");
            var fp = LLVM.BuildFPTrunc(builder, call, LLVM.FloatType(), "");
            LLVM.BuildRet(builder, fp);
        }

        void Gen_Pow_Float_Int()
        {
            var par = GenHeader(BuiltIns.Pow_Float_Int);
            var ext = LLVM.BuildFPExt(builder, par[0], LLVM.DoubleType(), "");
            var conv = LLVM.BuildSIToFP(builder, par[1], LLVM.DoubleType(), "");
            var call = LLVM.BuildCall(builder, pow, new[] { ext, conv }, "");
            var fp = LLVM.BuildFPTrunc(builder, call, LLVM.FloatType(), "");
            LLVM.BuildRet(builder, fp);
        }

        void Gen_Pow_Float_Float()
        {
            var par = GenHeader(BuiltIns.Pow_Float_Float);
            var ext0 = LLVM.BuildFPExt(builder, par[0], LLVM.DoubleType(), "");
            var ext1 = LLVM.BuildFPExt(builder, par[1], LLVM.DoubleType(), "");
            var call = LLVM.BuildCall(builder, pow, new[] { ext0, ext1 }, "");
            var fp = LLVM.BuildFPTrunc(builder, call, LLVM.FloatType(), "");
            LLVM.BuildRet(builder, fp);
        }

        void Gen_Pow_Int_Bool()
        {
            var par = GenHeader(BuiltIns.Pow_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, par[0], LLVM.DoubleType(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.DoubleType(), "");
            var call = LLVM.BuildCall(builder, pow, new[] { conv0, conv1 }, "");
            var si = LLVM.BuildFPToSI(builder, call, LLVM.Int32Type(), "");
            LLVM.BuildRet(builder, si);
        }

        void Gen_Pow_Bool_Int()
        {
            var par = GenHeader(BuiltIns.Pow_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.DoubleType(), "");
            var conv1 = LLVM.BuildSIToFP(builder, par[1], LLVM.DoubleType(), "");
            var call = LLVM.BuildCall(builder, pow, new[] { conv0, conv1 }, "");
            var si = LLVM.BuildFPToSI(builder, call, LLVM.Int32Type(), "");
            LLVM.BuildRet(builder, si);
        }

        void Gen_Pow_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.Pow_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.DoubleType(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.DoubleType(), "");
            var call = LLVM.BuildCall(builder, pow, new[] { conv0, conv1 }, "");
            var si = LLVM.BuildFPToSI(builder, call, LLVM.Int32Type(), "");
            LLVM.BuildRet(builder, si);
        }

        void Gen_Pow_Float_Bool()
        {
            var par = GenHeader(BuiltIns.Pow_Float_Bool);
            var ext0 = LLVM.BuildFPExt(builder, par[0], LLVM.DoubleType(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.DoubleType(), "");
            var call = LLVM.BuildCall(builder, pow, new[] { ext0, conv1 }, "");
            var fp = LLVM.BuildFPTrunc(builder, call, LLVM.FloatType(), "");
            LLVM.BuildRet(builder, fp);
        }

        void Gen_Pow_Bool_Float()
        {
            var par = GenHeader(BuiltIns.Pow_Bool_Float);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildFPExt(builder, par[1], LLVM.DoubleType(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.DoubleType(), "");
            var call = LLVM.BuildCall(builder, pow, new[] { conv0, ext1 }, "");
            var fp = LLVM.BuildFPTrunc(builder, call, LLVM.FloatType(), "");
            LLVM.BuildRet(builder, fp);
        }

        #endregion

        #region __pos__

        void Gen_Pos_Int()
        {
            var par = GenHeader(BuiltIns.Pos_Int);
            LLVM.BuildRet(builder, par[0]);
        }

        void Gen_Pos_Float()
        {
            var par = GenHeader(BuiltIns.Pos_Float);
            LLVM.BuildRet(builder, par[0]);
        }

        void Gen_Pos_Bool()
        {
            var par = GenHeader(BuiltIns.Pos_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            LLVM.BuildRet(builder, ext0);
        }

        #endregion

        #region __neg__

        void Gen_Neg_Int()
        {
            var par = GenHeader(BuiltIns.Neg_Int);
            var zero = LLVM.ConstInt(LLVM.Int32Type(), 0, false);
            var op = LLVM.BuildNSWSub(builder, zero, par[0], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Neg_Float()
        {
            var par = GenHeader(BuiltIns.Neg_Float);
            var zero = LLVM.ConstReal(LLVM.FloatType(), 0.0);
            var op = LLVM.BuildFSub(builder, zero, par[0], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Neg_Bool()
        {
            var par = GenHeader(BuiltIns.Neg_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var zero = LLVM.ConstInt(LLVM.Int32Type(), 0, false);
            var op = LLVM.BuildNSWSub(builder, zero, ext0, "");
            LLVM.BuildRet(builder, op);
        }

        #endregion

        #region __lt__

        void Gen_Lt_Int_Int()
        {
            var par = GenHeader(BuiltIns.Lt_Int_Int);
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSLT, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Lt_Int_Float()
        {
            var par = GenHeader(BuiltIns.Lt_Int_Float);
            var conv0 = LLVM.BuildSIToFP(builder, par[0], LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLT, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Lt_Float_Int()
        {
            var par = GenHeader(BuiltIns.Lt_Float_Int);
            var conv1 = LLVM.BuildSIToFP(builder, par[1], LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLT, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Lt_Float_Float()
        {
            var par = GenHeader(BuiltIns.Lt_Float_Float);
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLT, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Lt_Int_Bool()
        {
            var par = GenHeader(BuiltIns.Lt_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSLT, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Lt_Bool_Int()
        {
            var par = GenHeader(BuiltIns.Lt_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSLT, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Lt_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.Lt_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSLT, ext0, ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Lt_Float_Bool()
        {
            var par = GenHeader(BuiltIns.Lt_Float_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLT, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Lt_Bool_Float()
        {
            var par = GenHeader(BuiltIns.Lt_Bool_Float);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLT, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        #endregion

        #region __gt__

        void Gen_Gt_Int_Int()
        {
            var par = GenHeader(BuiltIns.Gt_Int_Int);
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSGT, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Gt_Int_Float()
        {
            var par = GenHeader(BuiltIns.Gt_Int_Float);
            var conv0 = LLVM.BuildSIToFP(builder, par[0], LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGT, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Gt_Float_Int()
        {
            var par = GenHeader(BuiltIns.Gt_Float_Int);
            var conv1 = LLVM.BuildSIToFP(builder, par[1], LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGT, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Gt_Float_Float()
        {
            var par = GenHeader(BuiltIns.Gt_Float_Float);
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGT, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Gt_Int_Bool()
        {
            var par = GenHeader(BuiltIns.Gt_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSGT, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Gt_Bool_Int()
        {
            var par = GenHeader(BuiltIns.Gt_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSGT, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Gt_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.Gt_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSGT, ext0, ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Gt_Float_Bool()
        {
            var par = GenHeader(BuiltIns.Gt_Float_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGT, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Gt_Bool_Float()
        {
            var par = GenHeader(BuiltIns.Gt_Bool_Float);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGT, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        #endregion

        #region __le__

        void Gen_Le_Int_Int()
        {
            var par = GenHeader(BuiltIns.Le_Int_Int);
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSLE, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Le_Int_Float()
        {
            var par = GenHeader(BuiltIns.Le_Int_Float);
            var conv0 = LLVM.BuildSIToFP(builder, par[0], LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLE, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Le_Float_Int()
        {
            var par = GenHeader(BuiltIns.Le_Float_Int);
            var conv1 = LLVM.BuildSIToFP(builder, par[1], LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLE, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Le_Float_Float()
        {
            var par = GenHeader(BuiltIns.Le_Float_Float);
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLE, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Le_Int_Bool()
        {
            var par = GenHeader(BuiltIns.Le_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSLE, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Le_Bool_Int()
        {
            var par = GenHeader(BuiltIns.Le_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSLE, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Le_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.Le_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSLE, ext0, ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Le_Float_Bool()
        {
            var par = GenHeader(BuiltIns.Le_Float_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLE, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Le_Bool_Float()
        {
            var par = GenHeader(BuiltIns.Le_Bool_Float);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOLE, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        #endregion

        #region __ge__

        void Gen_Ge_Int_Int()
        {
            var par = GenHeader(BuiltIns.Ge_Int_Int);
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSGE, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ge_Int_Float()
        {
            var par = GenHeader(BuiltIns.Ge_Int_Float);
            var conv0 = LLVM.BuildSIToFP(builder, par[0], LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGE, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ge_Float_Int()
        {
            var par = GenHeader(BuiltIns.Ge_Float_Int);
            var conv1 = LLVM.BuildSIToFP(builder, par[1], LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGE, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ge_Float_Float()
        {
            var par = GenHeader(BuiltIns.Ge_Float_Float);
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGE, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ge_Int_Bool()
        {
            var par = GenHeader(BuiltIns.Ge_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSGE, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ge_Bool_Int()
        {
            var par = GenHeader(BuiltIns.Ge_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSGE, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ge_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.Ge_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntSGE, ext0, ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ge_Float_Bool()
        {
            var par = GenHeader(BuiltIns.Ge_Float_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGE, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ge_Bool_Float()
        {
            var par = GenHeader(BuiltIns.Ge_Bool_Float);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOGE, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        #endregion

        #region __eq__

        void Gen_Eq_Int_Int()
        {
            var par = GenHeader(BuiltIns.Eq_Int_Int);
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntEQ, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Eq_Int_Float()
        {
            var par = GenHeader(BuiltIns.Eq_Int_Float);
            var conv0 = LLVM.BuildSIToFP(builder, par[0], LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOEQ, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Eq_Float_Int()
        {
            var par = GenHeader(BuiltIns.Eq_Float_Int);
            var conv1 = LLVM.BuildSIToFP(builder, par[1], LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOEQ, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Eq_Float_Float()
        {
            var par = GenHeader(BuiltIns.Eq_Float_Float);
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOEQ, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Eq_Int_Bool()
        {
            var par = GenHeader(BuiltIns.Eq_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntEQ, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Eq_Bool_Int()
        {
            var par = GenHeader(BuiltIns.Eq_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntEQ, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Eq_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.Eq_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntEQ, ext0, ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Eq_Float_Bool()
        {
            var par = GenHeader(BuiltIns.Eq_Float_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOEQ, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Eq_Bool_Float()
        {
            var par = GenHeader(BuiltIns.Eq_Bool_Float);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealOEQ, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        #endregion

        #region __ne__

        void Gen_Ne_Int_Int()
        {
            var par = GenHeader(BuiltIns.Ne_Int_Int);
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntNE, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ne_Int_Float()
        {
            var par = GenHeader(BuiltIns.Ne_Int_Float);
            var conv0 = LLVM.BuildSIToFP(builder, par[0], LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealONE, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ne_Float_Int()
        {
            var par = GenHeader(BuiltIns.Ne_Float_Int);
            var conv1 = LLVM.BuildSIToFP(builder, par[1], LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealONE, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ne_Float_Float()
        {
            var par = GenHeader(BuiltIns.Ne_Float_Float);
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealONE, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ne_Int_Bool()
        {
            var par = GenHeader(BuiltIns.Ne_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntNE, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ne_Bool_Int()
        {
            var par = GenHeader(BuiltIns.Ne_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntNE, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ne_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.Ne_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntNE, ext0, ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ne_Float_Bool()
        {
            var par = GenHeader(BuiltIns.Ne_Float_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var conv1 = LLVM.BuildSIToFP(builder, ext1, LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealONE, par[0], conv1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Ne_Bool_Float()
        {
            var par = GenHeader(BuiltIns.Ne_Bool_Float);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var conv0 = LLVM.BuildSIToFP(builder, ext0, LLVM.FloatType(), "");
            var op = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealONE, conv0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        #endregion

        #region __not__

        void Gen_Not_Int()
        {
            var par = GenHeader(BuiltIns.Not_Int);
            var zero = LLVM.ConstInt(LLVM.Int32Type(), 0, false);
            var ne = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntNE, par[0], zero, "");
            var tru = LLVM.ConstInt(LLVM.Int1Type(), 1, false);
            var xor = LLVM.BuildXor(builder, ne, tru, "");
            LLVM.BuildRet(builder, xor);
        }

        void Gen_Not_Float()
        {
            var par = GenHeader(BuiltIns.Not_Float);
            var zero = LLVM.ConstReal(LLVM.FloatType(), 0.0);
            var ne = LLVM.BuildFCmp(builder, LLVMRealPredicate.LLVMRealUNE, par[0], zero, "");
            var tru = LLVM.ConstInt(LLVM.Int1Type(), 1, false);
            var xor = LLVM.BuildXor(builder, ne, tru, "");
            LLVM.BuildRet(builder, xor);
        }

        void Gen_Not_Bool()
        {
            var par = GenHeader(BuiltIns.Not_Bool);
            var tru = LLVM.ConstInt(LLVM.Int1Type(), 1, false);
            var xor = LLVM.BuildXor(builder, par[0], tru, "");
            LLVM.BuildRet(builder, xor);
        }

        #endregion

        #region __bitand__

        void Gen_BitAnd_Int_Int()
        {
            var par = GenHeader(BuiltIns.BitAnd_Int_Int);
            var op = LLVM.BuildAnd(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_BitAnd_Int_Bool()
        {
            var par = GenHeader(BuiltIns.BitAnd_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildAnd(builder, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_BitAnd_Bool_Int()
        {
            var par = GenHeader(BuiltIns.BitAnd_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildAnd(builder, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_BitAnd_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.BitAnd_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildAnd(builder, ext0, ext1, "");
            var zero = LLVM.ConstInt(LLVM.Int32Type(), 0, false);
            var cmp = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntNE, op, zero, "");
            LLVM.BuildRet(builder, cmp);
        }

        #endregion

        #region __bitor__

        void Gen_BitOr_Int_Int()
        {
            var par = GenHeader(BuiltIns.BitOr_Int_Int);
            var op = LLVM.BuildOr(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_BitOr_Int_Bool()
        {
            var par = GenHeader(BuiltIns.BitOr_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildOr(builder, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_BitOr_Bool_Int()
        {
            var par = GenHeader(BuiltIns.BitOr_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildOr(builder, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_BitOr_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.BitOr_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildOr(builder, ext0, ext1, "");
            var zero = LLVM.ConstInt(LLVM.Int32Type(), 0, false);
            var cmp = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntNE, op, zero, "");
            LLVM.BuildRet(builder, cmp);
        }

        #endregion

        #region __bitxor__

        void Gen_BitXor_Int_Int()
        {
            var par = GenHeader(BuiltIns.BitXor_Int_Int);
            var op = LLVM.BuildXor(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_BitXor_Int_Bool()
        {
            var par = GenHeader(BuiltIns.BitXor_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildXor(builder, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_BitXor_Bool_Int()
        {
            var par = GenHeader(BuiltIns.BitXor_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildXor(builder, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_BitXor_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.BitXor_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildXor(builder, ext0, ext1, "");
            var zero = LLVM.ConstInt(LLVM.Int32Type(), 0, false);
            var cmp = LLVM.BuildICmp(builder, LLVMIntPredicate.LLVMIntNE, op, zero, "");
            LLVM.BuildRet(builder, cmp);
        }

        #endregion

        #region __not__

        void Gen_Invert_Int()
        {
            var par = GenHeader(BuiltIns.Invert_Int);
            var minus1 = LLVM.ConstNeg(LLVM.ConstInt(LLVM.Int32Type(), 1, false));
            var op = LLVM.BuildXor(builder, par[0], minus1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_Invert_Bool()
        {
            var par = GenHeader(BuiltIns.Invert_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var minus1 = LLVM.ConstNeg(LLVM.ConstInt(LLVM.Int32Type(), 1, false));
            var op = LLVM.BuildXor(builder, ext0, minus1, "");
            LLVM.BuildRet(builder, op);
        }

        #endregion

        #region __lshift__

        void Gen_LShift_Int_Int()
        {
            var par = GenHeader(BuiltIns.LShift_Int_Int);
            var op = LLVM.BuildShl(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_LShift_Int_Bool()
        {
            var par = GenHeader(BuiltIns.LShift_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildShl(builder, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_LShift_Bool_Int()
        {
            var par = GenHeader(BuiltIns.LShift_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildShl(builder, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_LShift_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.LShift_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildShl(builder, ext0, ext1, "");
            LLVM.BuildRet(builder, op);
        }

        #endregion

        #region __rshift__

        void Gen_RShift_Int_Int()
        {
            var par = GenHeader(BuiltIns.RShift_Int_Int);
            var op = LLVM.BuildAShr(builder, par[0], par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_RShift_Int_Bool()
        {
            var par = GenHeader(BuiltIns.RShift_Int_Bool);
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildAShr(builder, par[0], ext1, "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_RShift_Bool_Int()
        {
            var par = GenHeader(BuiltIns.RShift_Bool_Int);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var op = LLVM.BuildAShr(builder, ext0, par[1], "");
            LLVM.BuildRet(builder, op);
        }

        void Gen_RShift_Bool_Bool()
        {
            var par = GenHeader(BuiltIns.RShift_Bool_Bool);
            var ext0 = LLVM.BuildZExt(builder, par[0], LLVM.Int32Type(), "");
            var ext1 = LLVM.BuildZExt(builder, par[1], LLVM.Int32Type(), "");
            var op = LLVM.BuildAShr(builder, ext0, ext1, "");
            LLVM.BuildRet(builder, op);
        }

        #endregion
    }
}
