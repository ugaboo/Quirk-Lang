﻿using LLVMSharp;
using System;
using System.Collections.Generic;

namespace Quirk.Visitors
{
    public partial class CodeGenVisitor : AST.IVisitor
    {
        LLVMBuilderRef builder = LLVM.CreateBuilder();
        Stack<LLVMValueRef> values = new Stack<LLVMValueRef>();
        Stack<LLVMTypeRef> types = new Stack<LLVMTypeRef>();
        Stack<LLVMBasicBlockRef> blocks = new Stack<LLVMBasicBlockRef>();

        Dictionary<AST.Function, LLVMValueRef> llvmFuncs = new Dictionary<AST.Function, LLVMValueRef>();

        readonly Stack<AST.Function> hierarchy = new Stack<AST.Function>();

        LLVMModuleRef moduleLLVM;
        LLVMValueRef dstr;
               

        public CodeGenVisitor(AST.Module module)
        {
            new TypeVisitor(module);
            module.Accept(this);
        }

        public void Visit(AST.Module module)
        {
            moduleLLVM = LLVM.ModuleCreateWithName(module.Name);

            var mainFunc = LLVM.AddFunction(moduleLLVM, "main", LLVM.FunctionType(LLVM.Int32Type(), new LLVMTypeRef[] { }, false));

            var block = LLVM.AppendBasicBlock(mainFunc, "entry");
            blocks.Push(block);
            foreach (var statement in module.Statements) {
                statement.Accept(this);
            }
            blocks.Pop();

            LLVM.PositionBuilderAtEnd(builder, block);
            LLVM.BuildRet(builder, LLVM.ConstInt(LLVM.Int32Type(), 0, false));

            LLVM.WriteBitcodeToFile(moduleLLVM, $"{module.Name}.bc");
            LLVM.PrintModuleToFile(moduleLLVM, $"{module.Name}.ll", out var errorMsg);
        }

        public void Visit(AST.Overload overload) { throw new InvalidOperationException(); }

        public void Visit(AST.Function func)
        {
            if (GenerateIntrinsic(func)) { return; }

            hierarchy.Push(func);

            if (llvmFuncs.TryGetValue(func, out var funcLLVM)) {
                values.Push(funcLLVM);
            } else {
                var name = DetermineName(func);
                var paramTypes = new LLVMTypeRef[func.Parameters.Count];
                for (var i = 0; i < func.Parameters.Count; i += 1) {
                    func.Parameters[i].Type.Accept(this);
                    paramTypes[i] = types.Pop();
                }
                llvmFuncs[func] = funcLLVM = LLVM.AddFunction(moduleLLVM, name, LLVM.FunctionType(LLVM.VoidType(), paramTypes, false));
                LLVM.SetLinkage(funcLLVM, LLVMLinkage.LLVMExternalLinkage);

                for (var i = 0; i < func.Parameters.Count; i += 1) {
                    var param = LLVM.GetParam(funcLLVM, (uint)i);
                    LLVM.SetValueName(param, func.Parameters[i].Name);
                }

                var block = LLVM.AppendBasicBlock(funcLLVM, "entry");
                blocks.Push(block);
                foreach (var statement in func.Statements) {
                    statement.Accept(this);
                }
                blocks.Pop();

                values.Push(funcLLVM);
            }

            hierarchy.Pop();
        }

        public void Visit(AST.Variable variable)
        {
            //var variableLLVM = LLVM.AddGlobal(moduleLLVM, LLVM.Int32Type(), variable.Name);
            //LLVM.SetLinkage(variableLLVM, LLVMLinkage.LLVMCommonLinkage);
            //LLVM.SetInitializer(variableLLVM, LLVM.ConstInt(LLVM.Int32Type(), 0, false));
            //values[variable] = variableLLVM;
        }

        public void Visit(AST.Tuple tuple) { throw new Exception("Not implemented"); }

        public void Visit(AST.FuncDef funcDef)
        {
            if (funcDef.Func.TemplateParamsCount == 0) {
                funcDef.Func.Accept(this);
            }
        }

        public void Visit(AST.Assignment assignment)
        {
            assignment.Left.Accept(this);
            var left = values.Pop();

            assignment.Right.Accept(this);
            var right = values.Pop();

            values.Push(LLVM.BuildStore(builder, right, left));
        }

        public void Visit(AST.Evaluation evaluation)
        {
            evaluation.Expr.Accept(this);
            values.Pop();
        }

        public void Visit(AST.FuncCall funcCall)
        {
            funcCall.Func.Accept(this);
            var funcLLVM = values.Pop();

            var args = new LLVMValueRef[funcCall.Args.Count];
            for (var i = 0; i < funcCall.Args.Count; i += 1) {
                funcCall.Args[i].Accept(this);
                args[i] = values.Pop();
            }

            LLVM.PositionBuilderAtEnd(builder, blocks.Peek());
            var funcCallLLVM = LLVM.BuildCall(builder, funcLLVM, args, "c");
            values.Push(funcCallLLVM);


            //if (funcCall.Func is AST.Intrinsic intrinsic) {
            //    var args = new LLVMValueRef[funcCall.Args.Count + 1];
            //    args[0] = dstr;
            //    for (var i = 0; i < funcCall.Args.Count; i += 1) {
            //        var arg = funcCall.Args[i];
            //        if (arg is AST.Variable v) {
            //            var load = LLVM.BuildLoad(builder, values[v], "arg");
            //            args[i + 1] = load;
            //        }
            //    }
            //    LLVM.BuildCall(builder, values[intrinsic], args, "printf_result");
            //}
        }

        public void Visit(AST.ReturnStmnt returnStmnt)
        {
        }
               
        public void Visit(AST.NameObj nameObj) { throw new InvalidOperationException(); }

        public void Visit(AST.ConstBool constBool)
        {
            values.Push(LLVM.ConstInt(LLVM.Int8Type(), Convert.ToUInt64(constBool.Value), false));
        }

        public void Visit(AST.ConstInt constInt)
        {
            values.Push(LLVM.ConstInt(LLVM.Int32Type(), Convert.ToUInt64(constInt.Value), false));
        }

        public void Visit(AST.ConstFloat constFloat)
        {
            values.Push(LLVM.ConstReal(LLVM.FloatType(), Convert.ToDouble(constFloat.Value)));
        }

        public void Visit(AST.TypeObj typeObj)
        {
            if (typeObj == AST.TypeObj.Int) {
                types.Push(LLVM.Int32Type());
            } else if (typeObj == AST.TypeObj.Float) {
                types.Push(LLVM.Int32Type());
            } else if (typeObj == AST.TypeObj.Bool) {
                types.Push(LLVM.Int8Type());
            } else {
                throw new Exception("Not implemented");
            }
        }


        string DetermineName(AST.ProgObj obj)
        {
            var name = "";
            foreach (var func in hierarchy) {
                if (name == "") {
                    name = func.Name;
                } else {
                    name = $"{func.Name}.{name}";
                }
            }
            return name;
        }

        bool GenerateIntrinsic(AST.Function func)
        {
            //switch (intrinsic.Name) {
            //    case "print":
            //        dstr = LLVM.BuildGlobalStringPtr(builder, "%d\n", "d");
            //        var printf = LLVM.AddFunction(moduleLLVM, "printf", LLVM.FunctionType(LLVM.Int32Type(), new LLVMTypeRef[] { LLVM.PointerType(LLVM.Int8Type(), 0) }, true));
            //        LLVM.SetLinkage(printf, LLVMLinkage.LLVMExternalLinkage);
            //        values[intrinsic] = printf;
            //        break;
            //    default:
            //        throw new Exception("Not implemented");
            //}

            return false;
        }
    }
}