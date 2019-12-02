﻿using LLVMSharp;
using System;
using System.Collections.Generic;
using Quirk.Helpers;

namespace Quirk.Visitors
{
    public partial class CodeGenVisitor : AST.IVisitor
    {
        BuiltInsLLVM builtIns;

        LLVMBuilderRef builder = LLVM.CreateBuilder();

        LLVMModuleRef moduleLLVM;
        LLVMValueRef mainFuncLLVM;

        Stack<LLVMValueRef> values = new Stack<LLVMValueRef>();
        Stack<LLVMTypeRef> types = new Stack<LLVMTypeRef>();

        Stack<LLVMBasicBlockRef> varBlocks = new Stack<LLVMBasicBlockRef>();
        Stack<LLVMBasicBlockRef> codeBlocks = new Stack<LLVMBasicBlockRef>();

        Dictionary<AST.Function, LLVMValueRef> llvmFuncs = new Dictionary<AST.Function, LLVMValueRef>();
        Dictionary<AST.Variable, LLVMValueRef> llvmVars = new Dictionary<AST.Variable, LLVMValueRef>();
        Dictionary<AST.Parameter, LLVMValueRef> llvmParams = new Dictionary<AST.Parameter, LLVMValueRef>();

        readonly Stack<AST.Function> hierarchy = new Stack<AST.Function>();


               

        public CodeGenVisitor(AST.Module module)
        {
            new TypeVisitor(module);
            module.Accept(this);
        }

        public void Visit(AST.Module module)
        {
            moduleLLVM = LLVM.ModuleCreateWithName(module.Name);

            builtIns = new BuiltInsLLVM(builder, moduleLLVM);

            mainFuncLLVM = LLVM.AddFunction(moduleLLVM, "main", LLVM.FunctionType(LLVM.Int32Type(), new LLVMTypeRef[] { }, false));

            var varBlock = LLVM.AppendBasicBlock(mainFuncLLVM, "var");
            var codeBlock = LLVM.AppendBasicBlock(mainFuncLLVM, "begin");
            varBlocks.Push(varBlock);
            codeBlocks.Push(codeBlock);

            var buildRet = true;
            foreach (var statement in module.Statements) {
                statement.Accept(this);
                if (statement is AST.ReturnStmnt) {
                    buildRet = false;
                    break;
                }
            }
            LLVM.PositionBuilderAtEnd(builder, varBlock);
            LLVM.BuildBr(builder, codeBlock);

            if (buildRet) {
                LLVM.PositionBuilderAtEnd(builder, codeBlocks.Peek());
                LLVM.BuildRet(builder, LLVM.ConstInt(LLVM.Int32Type(), 0, false));
            }

            codeBlocks.Pop();
            varBlocks.Pop();

            LLVM.WriteBitcodeToFile(moduleLLVM, $"{module.Name}.bc");
            LLVM.PrintModuleToFile(moduleLLVM, $"{module.Name}.ll", out var errorMsg);
        }

        public void Visit(AST.Overload overload) { throw new InvalidOperationException(); }

        public void Visit(AST.Function func)
        {
            if (func.BuiltIn) { values.Push(builtIns.Find(func)); return; }

            hierarchy.Push(func);

            if (llvmFuncs.TryGetValue(func, out var funcLLVM)) {
                values.Push(funcLLVM);
            } else {
                var name = DetermineName(func);
                var retType = func.RetType != null ? ((AST.TypeObj)func.RetType).ToLLVM() : LLVM.VoidType();
                var paramTypes = GetTypes(func.Parameters);
                llvmFuncs[func] = funcLLVM = LLVM.AddFunction(moduleLLVM, name, LLVM.FunctionType(retType, paramTypes, false));
                LLVM.SetLinkage(funcLLVM, LLVMLinkage.LLVMExternalLinkage);

                for (var i = 0; i < func.Parameters.Count; i += 1) {
                    var param = LLVM.GetParam(funcLLVM, (uint)i);
                    llvmParams[func.Parameters[i]] = param;
                    LLVM.SetValueName(param, func.Parameters[i].Name);
                }

                var varBlock = LLVM.AppendBasicBlock(funcLLVM, "var");
                var codeBlock = LLVM.AppendBasicBlock(funcLLVM, "begin");
                varBlocks.Push(varBlock);
                codeBlocks.Push(codeBlock);

                var buildRet = true;
                foreach (var statement in func.Statements) {
                    statement.Accept(this);
                    if (statement is AST.ReturnStmnt) {
                        buildRet = false;
                        break;
                    }
                }
                if (func.RetType == null && buildRet) {
                    LLVM.PositionBuilderAtEnd(builder, codeBlocks.Peek());
                    LLVM.BuildRetVoid(builder);
                }
                LLVM.PositionBuilderAtEnd(builder, varBlock);
                LLVM.BuildBr(builder, codeBlock);
                codeBlocks.Pop();
                varBlocks.Pop();

                values.Push(funcLLVM);
            }

            hierarchy.Pop();
        }

        public void Visit(AST.Variable variable)
        {
            LLVM.PositionBuilderAtEnd(builder, codeBlocks.Peek());
            values.Push(LLVM.BuildLoad(builder, llvmVars[variable], ""));

            //var variableLLVM = LLVM.AddGlobal(moduleLLVM, LLVM.Int32Type(), variable.Name);
            //LLVM.SetLinkage(variableLLVM, LLVMLinkage.LLVMCommonLinkage);
            //LLVM.SetInitializer(variableLLVM, LLVM.ConstInt(LLVM.Int32Type(), 0, false));
            //values[variable] = variableLLVM;
        }

        public void Visit(AST.Parameter parameter)
        {
            values.Push(llvmParams[parameter]);
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
            LLVMValueRef left;
            if (assignment.Left is AST.Variable variable) {
                if (llvmVars.TryGetValue(variable, out var variableLLVM) == false) {
                    variable.Type.Accept(this);
                    var typeLLVM = types.Pop();
                    LLVM.PositionBuilderAtEnd(builder, varBlocks.Peek());
                    variableLLVM = LLVM.BuildAlloca(builder, typeLLVM, variable.Name);
                    llvmVars[variable] = variableLLVM;
                }
                left = variableLLVM;
            } else {
                throw new InvalidOperationException();
            }

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

            LLVM.PositionBuilderAtEnd(builder, codeBlocks.Peek());
            var funcCallLLVM = LLVM.BuildCall(builder, funcLLVM, args, "");
            values.Push(funcCallLLVM);
        }

        public void Visit(AST.IfStmnt ifStmnt)
        {
            var funcLLVM = hierarchy.Count > 0 ? llvmFuncs[hierarchy.Peek()] : mainFuncLLVM;

            var thenBlock = LLVM.AppendBasicBlock(funcLLVM, "if_then");
            var elseBlock = LLVM.AppendBasicBlock(funcLLVM, "else");
            var endBlock = LLVM.AppendBasicBlock(funcLLVM, "end_if");

            LLVM.PositionBuilderAtEnd(builder, codeBlocks.Peek());
            ifStmnt.IfThen[0].condition.Accept(this);
            LLVM.BuildCondBr(builder, values.Pop(), thenBlock, elseBlock);

            codeBlocks.Pop();
            codeBlocks.Push(thenBlock);

            var buildBr = true;
            foreach (var stmnt in ifStmnt.IfThen[0].statements) {
                stmnt.Accept(this);
                if (stmnt is AST.ReturnStmnt) {
                    buildBr = false;
                    break;
                }
            }
            if (buildBr) {
                LLVM.PositionBuilderAtEnd(builder, codeBlocks.Peek());
                LLVM.BuildBr(builder, endBlock);
            }

            codeBlocks.Pop();
            codeBlocks.Push(elseBlock);



            LLVM.PositionBuilderAtEnd(builder, codeBlocks.Peek());
            LLVM.BuildBr(builder, endBlock);

            codeBlocks.Pop();
            codeBlocks.Push(endBlock);
        }

        public void Visit(AST.ReturnStmnt returnStmnt)
        {
            var vals = new List<LLVMValueRef>();
            foreach (var val in returnStmnt.Values) {
                val.Accept(this);
                vals.Add(values.Pop());
            }

            LLVM.PositionBuilderAtEnd(builder, codeBlocks.Peek());
            if (vals.Count == 0) {
                values.Push(LLVM.BuildRetVoid(builder));
            } else {
                values.Push(LLVM.BuildRet(builder, vals[0]));
            }
        }

        public void Visit(AST.NameObj nameObj) { throw new InvalidOperationException(); }

        public void Visit(AST.ConstBool constBool)
        {
            values.Push(LLVM.ConstInt(BuiltIns.Bool.ToLLVM(), Convert.ToUInt64(constBool.Value), false));
        }

        public void Visit(AST.ConstInt constInt)
        {
            values.Push(LLVM.ConstInt(BuiltIns.Int.ToLLVM(), Convert.ToUInt64(constInt.Value), false));
        }

        public void Visit(AST.ConstFloat constFloat)
        {
            values.Push(LLVM.ConstReal(BuiltIns.Float.ToLLVM(), Convert.ToDouble(constFloat.Value)));
        }

        public void Visit(AST.TypeObj typeObj)
        {
            types.Push(typeObj.ToLLVM());
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

        LLVMTypeRef[] GetTypes(List<AST.Parameter> parameters)
        {
            var result = new LLVMTypeRef[parameters.Count];
            for (var i = 0; i < parameters.Count; i += 1) {
                parameters[i].Type.Accept(this);
                result[i] = types.Pop();
            }
            return result;
        }
    }
}