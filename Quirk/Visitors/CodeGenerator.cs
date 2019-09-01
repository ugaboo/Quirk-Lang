using LLVMSharp;
using System;
using System.Collections.Generic;

namespace Quirk.Visitors
{
    public partial class CodeGenerator : AST.IVisitor
    {
        LLVMBuilderRef builder = LLVM.CreateBuilder();

        LLVMModuleRef moduleLLVM;

        LLVMValueRef d;

        Dictionary<AST.IProgObj, LLVMValueRef> registers = new Dictionary<AST.IProgObj, LLVMValueRef>();


        public void Visit(AST.Module module)
        {
            moduleLLVM = LLVM.ModuleCreateWithName(module.Name);

            var main_ = LLVM.AddFunction(moduleLLVM, "main", LLVM.FunctionType(LLVM.Int32Type(), new LLVMTypeRef[] { }, false));
            var entry_ = LLVM.AppendBasicBlock(main_, "entry");
            LLVM.PositionBuilderAtEnd(builder, entry_);

            foreach (var pair in module.NameTable) {
                pair.Value.Accept(this);
            }
            foreach (var statement in module.Statements) {
                statement.Accept(this);
            }
            LLVM.BuildRet(builder, LLVM.ConstInt(LLVM.Int32Type(), 0, false));

            LLVM.WriteBitcodeToFile(moduleLLVM, $"{module.Name}.bc");
            LLVM.PrintModuleToFile(moduleLLVM, $"{module.Name}.ll", out var errorMsg);
        }

        public void Visit(AST.Func func)
        {
        }

        public void Visit(AST.Variable variable)
        {
            var variableLLVM = LLVM.AddGlobal(moduleLLVM, LLVM.Int32Type(), variable.Name);
            LLVM.SetLinkage(variableLLVM, LLVMLinkage.LLVMCommonLinkage);
            LLVM.SetInitializer(variableLLVM, LLVM.ConstInt(LLVM.Int32Type(), 0, false));
            registers[variable] = variableLLVM;
        }

        public void Visit(AST.Intrinsic intrinsic)
        {
            switch (intrinsic.Type) {
                case AST.IntrinsicType.Print:
                    d = LLVM.BuildGlobalStringPtr(builder, "%d\n", "d");
                    var printf = LLVM.AddFunction(moduleLLVM, "printf", LLVM.FunctionType(LLVM.Int32Type(), new LLVMTypeRef[] { LLVM.PointerType(LLVM.Int8Type(), 0) }, true));
                    LLVM.SetLinkage(printf, LLVMLinkage.LLVMExternalLinkage);
                    registers[intrinsic] = printf;
                    break;
                default:
                    throw new Exception("Not implemented");
            }
        }


        public void Visit(AST.Assignment assignment)
        {
            assignment.Left.Accept(this);
            assignment.Right.Accept(this);

            registers[assignment] = LLVM.BuildStore(builder, registers[assignment.Right], registers[assignment.Left]);
        }

        public void Visit(AST.Evaluation evaluation)
        {
            evaluation.Expr.Accept(this);
        }


        public void Visit(AST.BinaryExpression expression)
        {
            expression.Left.Accept(this);
            expression.Right.Accept(this);

            switch (expression.Type) {
                case AST.BinaryExpressionType.Add:
                    registers[expression] = LLVM.BuildAdd(builder, registers[expression.Left], registers[expression.Right], "add");
                    break;
                default:
                    throw new Exception("Not implemented");
            }
        }

        public void Visit(AST.UnaryExpression expression)
        {
        }

        public void Visit(AST.FuncCall funcCall)
        {
            if (funcCall.Func is AST.Intrinsic intrinsic) {
                var args = new LLVMValueRef[funcCall.Args.Count + 1];
                args[0] = d;
                for (var i = 0; i < funcCall.Args.Count; i += 1) {
                    var arg = funcCall.Args[i];
                    if (arg is AST.Variable v) {
                        var load = LLVM.BuildLoad(builder, registers[v], "arg");
                        args[i + 1] = load;
                    }
                }
                LLVM.BuildCall(builder, registers[intrinsic], args, "printf_result");
            }
        }

        public void Visit(AST.NamedObj namedObj)
        {
            throw new Exception();
        }

        public void Visit(AST.ConstBool constBool) { }

        public void Visit(AST.ConstInt constInt)
        {
            registers[constInt] = LLVM.ConstInt(LLVM.Int32Type(), (ulong)constInt.Value, false);
        }

        public void Visit(AST.ConstFloat constFloat) { }
    }
}