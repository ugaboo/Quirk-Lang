using System;
using System.Collections.Generic;
using System.Text;

namespace Quirk
{
    public class Parser
    {
        private Scanner scan;

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
            if (Stmnt(module.NameTable, module.Statements)) {
                goto _0;
            }
            return false;
        _end:
            return true;
        }

        bool Stmnt(Dictionary<string, AST.IProgObj> nameTable, List<AST.IProgObj> statements)
        {
            if (SimpleStmnt(statements)) {
                goto _end;
            }
            if (CompoundStmnt(nameTable, statements)) {
                goto _end;
            }
            return false;
        _end:
            return true;
        }

        bool SimpleStmnt(List<AST.IProgObj> statements)
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
            throw new Exception();
        _end:
            return true;
        }

        bool SmallStmnt(List<AST.IProgObj> statements)
        {
            if (ExprStmnt(statements)) {
                goto _end;
            }
            if (PassStmnt()) {
                goto _end;
            }
            return false;
        _end:
            return true;
        }

        bool ExprStmnt(List<AST.IProgObj> statements)
        {
            var rightParts = new List<AST.IProgObj>();

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
                for (var i = rightParts.Count - 1; i >= 0; i -= 1) {
                    statements.Add(new AST.Assignment(left, rightParts[i]));
                }
            } else {
                statements.Add(new AST.Evaluation(left));
            }
            goto _end;
        _4:
            if (TestlistStarExpr(out var right)) {
                rightParts.Add(right);
                goto _3;
            }
            throw new Exception();
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

        bool TestlistStarExpr(out AST.IProgObj expr)
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

        bool Test(out AST.IProgObj expr)
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

        bool OrTest(out AST.IProgObj expr)
        {
            if (AndTest(out expr)) {
                goto _2;
            }
            return false;
        _1:
            if (AndTest(out var right)) {
                expr = new AST.BinaryExpression(AST.BinaryExpressionType.Or, expr, right);
                goto _2;
            }
            throw new Exception();
        _2:
            if (scan.Lexeme == Lexeme.KwOr) {
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool AndTest(out AST.IProgObj expr)
        {
            if (NotTest(out expr)) {
                goto _2;
            }
            return false;
        _1:
            if (NotTest(out var right)) {
                expr = new AST.BinaryExpression(AST.BinaryExpressionType.And, expr, right);
                goto _2;
            }
            throw new Exception();
        _2:
            if (scan.Lexeme == Lexeme.KwAnd) {
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool NotTest(out AST.IProgObj expr)
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
                expr = new AST.UnaryExpression(AST.UnaryExpressionType.Not, right);
                goto _end;
            }
            throw new Exception();
        _end:
            return true;
        }

        bool Comparison(out AST.IProgObj expr)
        {
            AST.BinaryExpressionType type;

            if (Expr(out expr)) {
                goto _2;
            }
            return false;
        _1:
            if (Expr(out var right)) {
                expr = new AST.BinaryExpression(type, expr, right);
                goto _2;
            }
            throw new Exception();
        _2:
            if (CompOp(out type)) {
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool CompOp(out AST.BinaryExpressionType type)
        {
            if (scan.Lexeme == Lexeme.Less) {
                type = AST.BinaryExpressionType.Less;
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.Greater) {
                type = AST.BinaryExpressionType.Greater;
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.Equal) {
                type = AST.BinaryExpressionType.Equal;
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.LessOrEqual) {
                type = AST.BinaryExpressionType.LessOrEqual;
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.GreaterOrEqual) {
                type = AST.BinaryExpressionType.GreaterOrEqual;
                scan.Next();
                goto _end;
            }
            if (scan.Lexeme == Lexeme.NotEqual) {
                type = AST.BinaryExpressionType.NotEqual;
                scan.Next();
                goto _end;
            }
            type = AST.BinaryExpressionType.None;
            return false;
        _end:
            return true;
        }

        bool Expr(out AST.IProgObj expr)
        {
            if (XorExpr(out expr)) {
                goto _2;
            }
            return false;
        _1:
            if (XorExpr(out var right)) {
                expr = new AST.BinaryExpression(AST.BinaryExpressionType.BitOr, expr, right);
                goto _2;
            }
            throw new Exception();
        _2:
            if (scan.Lexeme == Lexeme.BitOr) {
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool XorExpr(out AST.IProgObj expr)
        {
            if (AndExpr(out expr)) {
                goto _2;
            }
            return false;
        _1:
            if (AndExpr(out var right)) {
                expr = new AST.BinaryExpression(AST.BinaryExpressionType.BitXor, expr, right);
                goto _2;
            }
            throw new Exception();
        _2:
            if (scan.Lexeme == Lexeme.BitXor) {
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool AndExpr(out AST.IProgObj expr)
        {
            if (ShiftExpr(out expr)) {
                goto _2;
            }
            return false;
        _1:
            if (ShiftExpr(out var right)) {
                expr = new AST.BinaryExpression(AST.BinaryExpressionType.BitAnd, expr, right);
                goto _2;
            }
            throw new Exception();
        _2:
            if (scan.Lexeme == Lexeme.BitAnd) {
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool ShiftExpr(out AST.IProgObj expr)
        {
            AST.BinaryExpressionType type;

            if (ArithExpr(out expr)) {
                goto _2;
            }
            return false;
        _1:
            if (ArithExpr(out var right)) {
                expr = new AST.BinaryExpression(type, expr, right);
                goto _2;
            }
            throw new Exception();
        _2:
            if (scan.Lexeme == Lexeme.LeftShift) {
                type = AST.BinaryExpressionType.LeftShift;
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.RightShift) {
                type = AST.BinaryExpressionType.RightShift;
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool ArithExpr(out AST.IProgObj expr)
        {
            AST.BinaryExpressionType type;

            if (Term(out expr)) {
                goto _2;
            }
            return false;
        _1:
            if (Term(out var right)) {
                expr = new AST.BinaryExpression(type, expr, right);
                goto _2;
            }
            throw new Exception();
        _2:
            if (scan.Lexeme == Lexeme.Plus) {
                type = AST.BinaryExpressionType.Add;
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.Minus) {
                type = AST.BinaryExpressionType.Sub;
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool Term(out AST.IProgObj expr)
        {
            AST.BinaryExpressionType type;

            if (Factor(out expr)) {
                goto _2;
            }
            expr = null;
            return false;
        _1:
            if (Factor(out var right)) {
                expr = new AST.BinaryExpression(type, expr, right);
                goto _2;
            }
            throw new Exception();
        _2:
            if (scan.Lexeme == Lexeme.Star) {
                type = AST.BinaryExpressionType.Mul;
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.Slash) {
                type = AST.BinaryExpressionType.Div;
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.Percent) {
                type = AST.BinaryExpressionType.Mod;
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.DoubleSlash) {
                type = AST.BinaryExpressionType.FloorDiv;
                scan.Next();
                goto _1;
            }
            goto _end;
        _end:
            return true;
        }

        bool Factor(out AST.IProgObj expr)
        {
            AST.UnaryExpressionType type;

            if (Power(out expr)) {
                goto _end;
            }
            if (scan.Lexeme == Lexeme.Plus) {
                type = AST.UnaryExpressionType.Plus;
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.Minus) {
                type = AST.UnaryExpressionType.Minus;
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.BitNot) {
                type = AST.UnaryExpressionType.BitNot;
                scan.Next();
                goto _1;
            }
            expr = null;
            return false;
        _1:
            if (Factor(out var right)) {
                expr = new AST.UnaryExpression(type, right);
                goto _end;
            }
            throw new Exception();
        _end:
            return true;
        }

        bool Power(out AST.IProgObj expr)
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
                expr = new AST.BinaryExpression(AST.BinaryExpressionType.Power, expr, right);
                goto _end;
            }
            throw new Exception();
        _end:
            return true;
        }

        bool AtomExpr(out AST.IProgObj expr)
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
        //    throw new Exception();
        _2:
            if (Trailer(expr, out var trailer)) {
                expr = trailer;
                goto _2;
            }
            goto _end;
        _end:
            return true;
        }

        bool Atom(out AST.IProgObj obj)
        {
            if (scan.Lexeme == Lexeme.LeftParenthesis) {
                scan.Next();
                goto _1;
            }
            if (scan.Lexeme == Lexeme.Id) {
                obj = new AST.NamedObj(scan.TextValue());
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
            goto _2;
        _2:
            if (scan.Lexeme == Lexeme.RightParenthesis) {
                scan.Next();
                goto _end;
            }
            throw new Exception();
        _end:
            return true;
        }

        bool Trailer(AST.IProgObj atom, out AST.IProgObj expr)
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
            throw new Exception();
        _end:
            return true;
        }

        bool TestlistComp(out AST.IProgObj obj)
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
        //    throw new Exception();
        _end:
            return true;
        }

        bool Arglist(out List<AST.IProgObj> args)
        {
            args = new List<AST.IProgObj>();

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

        bool Argument(out AST.IProgObj expr)
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
        //    throw new Exception();
        _end:
            return true;
        }

        bool CompoundStmnt(Dictionary<string, AST.IProgObj> nameTable, List<AST.IProgObj> statements)
        {
            if (FuncDef(nameTable)) {
                goto _end;
            }
            return false;
        _end:
            return true;
        }

        bool Suite(Dictionary<string, AST.IProgObj> nameTable, List<AST.IProgObj> statements)
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
            throw new Exception();
        _2:
            if (Stmnt(nameTable, statements)) {
                goto _3;
            }
            throw new Exception();
        _3:
            if (Stmnt(nameTable, statements)) {
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
            throw new Exception();
        _end:
            return true;
        }

        bool FuncDef(Dictionary<string, AST.IProgObj> nameTable)
        {
            AST.Func func;

            if (scan.Lexeme == Lexeme.KwDef) {
                scan.Next();
                goto _1;
            }
            return false;
        _1:
            if (scan.Lexeme == Lexeme.Id) {
                func = new AST.Func(scan.TextValue());

                scan.Next();
                goto _2;
            }
            throw new Exception();
        _2:
            if (Parameters(func.Parameters)) {
                goto _3;
            }
            throw new Exception();
        _3:
            goto _5;
        _5:
            if (scan.Lexeme == Lexeme.Colon) {
                scan.Next();
                goto _6;
            }
            throw new Exception();
        _6:
            if (Suite(func.NameTable, func.Statements)) {
                goto _end;
            }
            throw new Exception();
        _end:
            nameTable[func.Name] = func;
            return true;
        }

        bool Parameters(List<AST.IProgObj> parameters)
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
            throw new Exception();
        _end:
            return true;
        }

        bool TypedArgsList(List<AST.IProgObj> parameters)
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

        bool Arg(out AST.Variable argument)
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

        bool TypedFormalParamDef(out AST.Variable param)
        {
            if (scan.Lexeme == Lexeme.Id) {
                param = new AST.Variable(scan.TextValue());

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
            if (Test(out var expr)) {
                goto _end;
            }
            throw new Exception();
        _end:
            return true;
        }
    }
}
