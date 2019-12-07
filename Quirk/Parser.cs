using System;
using System.Collections.Generic;
using System.Text;

namespace Quirk
{
    public class Parser
    {
        Scanner scan;


        public Parser(string filename, string name, out AST.Module module)
        {
            scan = new Scanner(filename);
            Module(name, out module);
        }

        bool Module(string name, out AST.Module module)
        {
            module = new AST.Module(name);
        _0:
            if (scan.Lexeme == Lexeme.EndMarker) {
                goto _end;
            }
            if (Stmnt(module.Statements)) {
                goto _0;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _end:
            return true;
        }

        bool Stmnt(List<AST.ProgObj> statements)
        {
            if (SimpleStmnt(statements)) {
                goto _end;
            }
            if (CompoundStmnt(statements)) {
                goto _end;
            }
            return false;
        _end:
            return true;
        }

        bool SimpleStmnt(List<AST.ProgObj> statements)
        {
            if (SmallStmnt(statements)) {
                goto _1;
            }
            return false;
        _1:
            if (scan.Lexeme == Lexeme.Semicolon) {
                scan.Next();
                goto _2;
            }
            goto _3;
        _2:
            if (SmallStmnt(statements)) {
                goto _1;
            }
            goto _3;
        _3:
            if (scan.Lexeme == Lexeme.NewLine) {
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.EndMarker) {
                scan.Next();
                goto _end;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _end:
            return true;
        }

        bool SmallStmnt(List<AST.ProgObj> statements)
        {
            if (ExprStmnt(statements)) {
                goto _end;
            }
            if (PassStmnt()) {
                goto _end;
            }
            if (FlowStmnt(statements)) {
                goto _end;
            }
            return false;
        _end:
            return true;
        }

        bool ExprStmnt(List<AST.ProgObj> statements)
        {
            var rightParts = new List<AST.ProgObj>();

            if (TestlistStarExpr(out var left)) {
                goto _1;
            }
            return false;
        _1:
            goto _3;
        _3:
            if (scan.Lexeme == Lexeme.Assignment) {
                scan.Next();
                goto _4;
            }
            if (rightParts.Count > 0) {
                for (var i = rightParts.Count - 1; i >= 1; i -= 1) {
                    statements.Add(new AST.Assignment(rightParts[i - 1], rightParts[i]));
                }
                statements.Add(new AST.Assignment(left, rightParts[0]));
            } else {
                statements.Add(new AST.Evaluation(left));
            }
            goto _end;
        _4:
            if (TestlistStarExpr(out var right)) {
                rightParts.Add(right);
                goto _3;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _end:
            return true;
        }

        bool PassStmnt()
        {
            if (scan.Lexeme == Lexeme.KwPass) {
                scan.Next();
                goto _end;
            }
            return false;
        _end:
            return true;
        }

        bool TestlistStarExpr(out AST.ProgObj expr)
        {
            if (Test(out expr)) {
                goto _1;
            }
            return false;
        _1:
            //if (scan.Lexeme == Lexeme.Comma) {
            //    scan.Next();
            //    goto _2;
            //}
            goto _end;
        //_2:
        //    if (Test()) {
        //        goto _1;
        //    }
        //    goto _end;
        _end:
            return true;
        }

        bool Test(out AST.ProgObj expr)
        {
            if (OrTest(out expr)) {
                goto _1;
            }
            return false;
        _1:
            goto _end;
        _end:
            return true;
        }

        bool OrTest(out AST.ProgObj expr)
        {
            if (AndTest(out expr)) {
                goto _2;
            }
            return false;
        _1:
            if (AndTest(out var right)) {
                expr = new AST.FuncCall("__or__", expr, right);
                goto _2;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _2:
            if (scan.Lexeme == Lexeme.KwOr) {
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool AndTest(out AST.ProgObj expr)
        {
            if (NotTest(out expr)) {
                goto _2;
            }
            return false;
        _1:
            if (NotTest(out var right)) {
                expr = new AST.FuncCall("__and__", expr, right);
                goto _2;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _2:
            if (scan.Lexeme == Lexeme.KwAnd) {
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool NotTest(out AST.ProgObj expr)
        {
            if (scan.Lexeme == Lexeme.KwNot) {
                scan.Next();
                goto _1;
            }
            if (Comparison(out expr)) {
                goto _end;
            }
            return false;
        _1:
            if (NotTest(out var right)) {
                expr = new AST.FuncCall("__not__", right);
                goto _end;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _end:
            return true;
        }

        bool Comparison(out AST.ProgObj expr)
        {
            string name;
            AST.ProgObj left;
            var chained = false;

            if (Expr(out expr)) {
                left = expr;
                goto _2;
            }
            return false;
        _1:
            if (Expr(out var right)) {
                var comp = new AST.FuncCall(name, left, right);
                left = right;
                if (chained) {
                    expr = new AST.FuncCall("__and__", expr, comp);
                } else {
                    expr = comp;
                    chained = true;
                }
                goto _2;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _2:
            if (CompOp(out name)) {
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool CompOp(out string name)
        {
            if (scan.Lexeme == Lexeme.Less) {
                name = "__lt__";
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.Greater) {
                name = "__gt__";
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.Equal) {
                name = "__eq__";
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.LessOrEqual) {
                name = "__le__";
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.GreaterOrEqual) {
                name = "__ge__";
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.NotEqual) {
                name = "__ne__";
                scan.Next();
                goto _end;
            }
            name = null;
            return false;
        _end:
            return true;
        }

        bool Expr(out AST.ProgObj expr)
        {
            if (XorExpr(out expr)) {
                goto _2;
            }
            return false;
        _1:
            if (XorExpr(out var right)) {
                expr = new AST.FuncCall("__bitor__", expr, right);
                goto _2;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _2:
            if (scan.Lexeme == Lexeme.BitOr) {
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool XorExpr(out AST.ProgObj expr)
        {
            if (AndExpr(out expr)) {
                goto _2;
            }
            return false;
        _1:
            if (AndExpr(out var right)) {
                expr = new AST.FuncCall("__bitxor__", expr, right);
                goto _2;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _2:
            if (scan.Lexeme == Lexeme.BitXor) {
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool AndExpr(out AST.ProgObj expr)
        {
            if (ShiftExpr(out expr)) {
                goto _2;
            }
            return false;
        _1:
            if (ShiftExpr(out var right)) {
                expr = new AST.FuncCall("__bitand__", expr, right);
                goto _2;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _2:
            if (scan.Lexeme == Lexeme.BitAnd) {
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool ShiftExpr(out AST.ProgObj expr)
        {
            string name;

            if (ArithExpr(out expr)) {
                goto _2;
            }
            return false;
        _1:
            if (ArithExpr(out var right)) {
                expr = new AST.FuncCall(name, expr, right);
                goto _2;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _2:
            if (scan.Lexeme == Lexeme.LeftShift) {
                name = "__lshift__";
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.RightShift) {
                name = "__rshift__";
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool ArithExpr(out AST.ProgObj expr)
        {
            string name;

            if (Term(out expr)) {
                goto _2;
            }
            return false;
        _1:
            if (Term(out var right)) {
                expr = new AST.FuncCall(name, expr, right);
                goto _2;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _2:
            if (scan.Lexeme == Lexeme.Plus) {
                name = "__add__";
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.Minus) {
                name = "__sub__";
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool Term(out AST.ProgObj expr)
        {
            string name;

            if (Factor(out expr)) {
                goto _2;
            }
            expr = null;
            return false;
        _1:
            if (Factor(out var right)) {
                expr = new AST.FuncCall(name, expr, right);
                goto _2;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _2:
            if (scan.Lexeme == Lexeme.Star) {
                name = "__mul__";
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.Slash) {
                name = "__truediv__";
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.Percent) {
                name = "__mod__";
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.DoubleSlash) {
                name = "__floordiv__";
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool Factor(out AST.ProgObj expr)
        {
            string name;

            if (Power(out expr)) {
                goto _end;
            }
            if (scan.Lexeme == Lexeme.Plus) {
                name = "__pos__";
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.Minus) {
                name = "__neg__";
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.BitNot) {
                name = "__invert__";
                scan.Next();
                goto _1;
            }
            expr = null;
            return false;
        _1:
            if (Factor(out var right)) {
                expr = new AST.FuncCall(name, right);
                goto _end;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _end:
            return true;
        }

        bool Power(out AST.ProgObj expr)
        {
            if (AtomExpr(out expr)) {
                goto _1;
            }
            expr = null;
            return false;
        _1:
            if (scan.Lexeme == Lexeme.Power) {
                scan.Next();
                goto _2;
            }
            goto _end;
        _2:
            if (Factor(out var right)) {
                expr = new AST.FuncCall("__pow__", expr, right);
                goto _end;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _end:
            return true;
        }

        bool AtomExpr(out AST.ProgObj expr)
        {
            if (Atom(out expr)) {
                goto _2;
            }
            expr = null;
            return false;

        //_1:
        //    if (Atom()) {
        //        goto _2;
        //    }
        //    throw new CompilationError(ErrorType.InvalidSyntax);
        _2:
            if (Trailer(expr, out var trailer)) {
                expr = trailer;
                goto _2;
            }
            goto _end;
        _end:
            return true;
        }

        bool Atom(out AST.ProgObj obj)
        {
            if (scan.Lexeme == Lexeme.LeftParenthesis) {
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.Id) {
                obj = new AST.NameObj(scan.TextValue());
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.Int) {
                obj = new AST.ConstInt(scan.ToInt());
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.Float) {
                obj = new AST.ConstFloat(scan.ToSingle());
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.KwTrue) {
                obj = new AST.ConstBool(true);
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.KwFalse) {
                obj = new AST.ConstBool(false);
                scan.Next();
                goto _end;
            }
            obj = null;
            return false;
        _1:
            if (TestlistComp(out obj)) {
                goto _2;
            }
            obj = new AST.Tuple();
            goto _2;
        _2:
            if (scan.Lexeme == Lexeme.RightParenthesis) {
                scan.Next();
                goto _end;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _end:
            return true;
        }

        bool Trailer(AST.ProgObj atom, out AST.ProgObj expr)
        {
            if (scan.Lexeme == Lexeme.LeftParenthesis) {
                scan.Next();
                goto _1;
            }
            expr = null;
            return false;
        _1:
            if (Arglist(out var args)) {
                expr = new AST.FuncCall(atom, args);
                goto _2;
            }
            expr = new AST.FuncCall(atom);
            goto _2;
        _2:
            if (scan.Lexeme == Lexeme.RightParenthesis) {
                scan.Next();
                goto _end;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _end:
            return true;
        }

        bool TestlistComp(out AST.ProgObj obj)
        {
            if (Test(out obj)) {
                goto _1;
            }
            obj = null;
            return false;
        _1:
            //if (scan.Lexeme == Lexeme.Comma) {
            //    scan.Next();
            //    goto _2;
            //}
            goto _end;
        //_2:
        //    if (Test()) {
        //        goto _3;
        //    }
        //    goto _end;
        //_3:
        //    if (scan.Lexeme == Lexeme.Comma) {
        //        scan.Next();
        //        goto _2;
        //    }
        //    CompilationError(ErrorType.InvalidSyntax);
        _end:
            return true;
        }

        bool Arglist(out List<AST.ProgObj> args)
        {
            args = new List<AST.ProgObj>();

            if (Argument(out var expr)) {
                args.Add(expr);
                goto _1;
            }
            return false;
        _1:
            if (scan.Lexeme == Lexeme.Comma) {
                scan.Next();
                goto _2;
            }
            goto _end;
        _2:
            if (Argument(out expr)) {
                args.Add(expr);
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool Argument(out AST.ProgObj expr)
        {
            if (Test(out expr)) {
                goto _1;
            }
            return false;
        _1:
            //if (scan.Lexeme == Lexeme.Assignment) {
            //    scan.Next();
            //    goto _2;
            //}
            goto _end;
        //_2:
        //    if (Test()) {
        //        goto _end;
        //    }
        //    CompilationError(ErrorType.InvalidSyntax);
        _end:
            return true;
        }

        bool CompoundStmnt(List<AST.ProgObj> statements)
        {
            if (IfStmnt(statements)) {
                goto _end;
            }
            if (FuncDef(statements)) {
                goto _end;
            }
            return false;
        _end:
            return true;
        }

        bool IfStmnt(List<AST.ProgObj> statements)
        {
            AST.IfStmnt result, ifStmnt;

            if (scan.Lexeme == Lexeme.KwIf) {
                result = ifStmnt = new AST.IfStmnt();

                scan.Next();
                goto _1;
            }
            return false;
        _1:
            if (Test(out ifStmnt.Condition)) {
                goto _2;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _2:
            if (scan.Lexeme == Lexeme.Colon) {
                scan.Next();
                goto _3;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _3:
            if (Suite(ifStmnt.Then)) {
                goto _4;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _4:
            if (scan.Lexeme == Lexeme.KwElif) {
                var elif = new AST.IfStmnt();
                ifStmnt.Else.Add(elif);
                ifStmnt = elif;

                scan.Next();
                goto _5;
            }
            if (scan.Lexeme == Lexeme.KwElse) {
                scan.Next();
                goto _8;
            }
            goto _end;
        _5:
            if (Test(out ifStmnt.Condition)) {
                goto _6;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _6:
            if (scan.Lexeme == Lexeme.Colon) {
                scan.Next();
                goto _7;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _7:
            if (Suite(ifStmnt.Then)) {
                goto _4;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _8:
            if (scan.Lexeme == Lexeme.Colon) {
                scan.Next();
                goto _9;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _9:
            if (Suite(ifStmnt.Else)) {
                goto _end;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _end:
            statements.Add(result);
            return true;
        }

        bool Suite(List<AST.ProgObj> statements)
        {
            if (SimpleStmnt(statements)) {
                goto _end;
            }
            if (scan.Lexeme == Lexeme.NewLine) {
                scan.Next();
                goto _1;
            }
            return false;
        _1:
            if (scan.Lexeme == Lexeme.Indent) {
                scan.Next();
                goto _2;
            }
            throw new CompilationError(ErrorType.ExpectedAnIndentedBlock);
        _2:
            if (Stmnt(statements)) {
                goto _3;
            }
            throw new InvalidOperationException();
        _3:
            if (Stmnt(statements)) {
                goto _3;
            }
            if (scan.Lexeme == Lexeme.Dedent) {
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.EndMarker) {
                scan.Next();
                goto _end;
            }
            throw new InvalidOperationException();
        _end:
            return true;
        }

        bool FuncDef(List<AST.ProgObj> statements)
        {
            string name;
            AST.Function func;

            if (scan.Lexeme == Lexeme.KwDef) {
                scan.Next();
                goto _1;
            }
            return false;
        _1:
            if (scan.Lexeme == Lexeme.Id) {
                name = scan.TextValue();
                func = new AST.Function(name);

                scan.Next();
                goto _2;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _2:
            if (Parameters(func.Parameters)) {
                goto _3;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _3:
            if (scan.Lexeme == Lexeme.RightArrow) {
                scan.Next();
                goto _4;
            }
            goto _5;
        _4:
            if (Test(out func.RetType)) {
                goto _5;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _5:
            if (scan.Lexeme == Lexeme.Colon) {
                scan.Next();
                goto _6;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _6:
            if (Suite(func.Statements)) {
                goto _end;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _end:
            statements.Add(new AST.FuncDef(func));
            return true;
        }

        bool Parameters(List<AST.Parameter> parameters)
        {
            if (scan.Lexeme == Lexeme.LeftParenthesis) {
                scan.Next();
                goto _1;
            }
            return false;
        _1:
            if (TypedArgsList(parameters)) {
                goto _2;
            }
            goto _2;
        _2:
            if (scan.Lexeme == Lexeme.RightParenthesis) {
                scan.Next();
                goto _end;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _end:
            return true;
        }

        bool TypedArgsList(List<AST.Parameter> parameters)
        {
            if (Arg(out var argument)) {
                parameters.Add(argument);

                goto _1;
            }
            return false;
        _1:
            if (scan.Lexeme == Lexeme.Comma) {
                scan.Next();
                goto _2;
            }
            goto _end;
        _2:
            if (Arg(out argument)) {
                parameters.Add(argument);

                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool Arg(out AST.Parameter argument)
        {
            if (TypedFormalParamDef(out argument)) {
                goto _1;
            }
            return false;
        _1:
            goto _end;
        _end:
            return true;
        }

        bool TypedFormalParamDef(out AST.Parameter param)
        {
            if (scan.Lexeme == Lexeme.Id) {
                param = new AST.Parameter(scan.TextValue());

                scan.Next();
                goto _1;
            }
            param = null;
            return false;
        _1:
            if (scan.Lexeme == Lexeme.Colon) {
                scan.Next();
                goto _2;
            }
            goto _end;
        _2:
            if (Test(out param.Type)) {
                goto _end;
            }
            throw new CompilationError(ErrorType.InvalidSyntax);
        _end:
            return true;
        }

        bool FlowStmnt(List<AST.ProgObj> statements)
        {
            if (ReturnStmnt(out var stmnt)) {
                statements.Add(stmnt);
                goto _end;
            }
            return false;
        _end:
            return true;
        }

        bool ReturnStmnt(out AST.ReturnStmnt stmnt)
        {
            if (scan.Lexeme == Lexeme.KwReturn) {
                stmnt = new AST.ReturnStmnt();

                scan.Next();
                goto _1;
            }
            stmnt = null;
            return false;
        _1:
            if (TestList(stmnt.Values)) {
                goto _end;
            }
            goto _end;
        _end:
            return true;
        }

        bool TestList(List<AST.ProgObj> expressions)
        {
            if (Test(out var expr)) {
                expressions.Add(expr);
                goto _1;
            }
            return false;
        _1:
            if (scan.Lexeme == Lexeme.Comma) {
                scan.Next();
                goto _2;
            }
            goto _end;
        _2:
            if (Test(out expr)) {
                expressions.Add(expr);
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }
    }
}
