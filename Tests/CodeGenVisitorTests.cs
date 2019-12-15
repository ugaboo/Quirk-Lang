using System;
using Quirk.Visitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace Quirk.Tests {
    [TestClass()]
    public class CodeGenVisitorTests {
        void Test(string name, string sample = null) {
            new Parser($"Code/CodeGenVisitor/{name}.qk", name, out var module);
            new CodeGenVisitor(module);
            Link(name);
            Execute(name, sample);
        }

        void Link(string name) {
            var clang = new Process();
            clang.StartInfo.FileName = "clang";
            clang.StartInfo.ArgumentList.Add("-w");
            clang.StartInfo.ArgumentList.Add(name + ".ll");
            clang.StartInfo.ArgumentList.Add("-o");
            clang.StartInfo.ArgumentList.Add(name + ".exe");
            clang.StartInfo.RedirectStandardError = true;
            clang.Start();

            var errors = clang.StandardError.ReadToEnd();
            if (!string.IsNullOrWhiteSpace(errors)) {
                throw new Exception(errors);
            }
        }

        void Execute(string name, string sample) {
            var exe = new Process();
            exe.StartInfo.FileName = name + ".exe";
            exe.StartInfo.RedirectStandardOutput = true;
            exe.Start();

            var output = exe.StandardOutput.ReadToEnd().Replace("\r", "");
            if (sample != null && output != sample) {
                throw new Exception(output);
            }
        }


        [TestMethod()] public void Function() { Test("Function"); }
        [TestMethod()] public void Variables() { Test("Variables"); }
        [TestMethod()] public void Return() { Test("Return"); }
        [TestMethod()] public void Add() { Test("Add"); }
        [TestMethod()] public void Sub() { Test("Sub"); }
        [TestMethod()] public void Mul() { Test("Mul"); }
        [TestMethod()] public void TrueDiv() { Test("TrueDiv"); }
        [TestMethod()] public void FloorDiv() { Test("FloorDiv"); }
        [TestMethod()] public void Mod() { Test("Mod"); }
        [TestMethod()] public void Pow() { Test("Pow"); }
        [TestMethod()] public void Pos() { Test("Pos"); }
        [TestMethod()] public void Neg() { Test("Neg"); }
        [TestMethod()] public void Lt() { Test("Lt"); }
        [TestMethod()] public void Gt() { Test("Gt"); }
        [TestMethod()] public void Le() { Test("Le"); }
        [TestMethod()] public void Ge() { Test("Ge"); }
        [TestMethod()] public void Eq() { Test("Eq"); }
        [TestMethod()] public void Ne() { Test("Ne"); }
        [TestMethod()] public void Not() { Test("Not"); }
        [TestMethod()] public void BitAnd() { Test("BitAnd"); }
        [TestMethod()] public void BitOr() { Test("BitOr"); }
        [TestMethod()] public void BitXor() { Test("BitXor"); }
        [TestMethod()] public void Invert() { Test("Invert"); }
        [TestMethod()] public void LShift() { Test("LShift"); }
        [TestMethod()] public void RShift() { Test("RShift"); }
        [TestMethod()] public void Factorial() { Test("Factorial", "120\n"); }
        [TestMethod()] public void Factorial2() { Test("Factorial2", "120\n"); }
        [TestMethod()] public void If1() { Test("If1"); }
        [TestMethod()] public void If2() { Test("If2"); }
        [TestMethod()] public void If3() { Test("If3"); }
        [TestMethod()] public void Fibonacci() { Test("Fibonacci", "55\n"); }
    }
}