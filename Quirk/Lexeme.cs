namespace Quirk
{
    public enum Lexeme
    {
        Error,

        Ignore,
        NewLine,
        EndMarker,
        Indent,
        Dedent,

        NotEqual,               // != or <>
        Percent,                // %
        //ModAssign,              // %=
        BitAnd,                 // &
        //BitAndAssign,           // &=
        LeftParenthesis,        // (
        RightParenthesis,       // )
        Star,                   // *
        Power,                  // **
        //PowerAssign,            // **=
        //MulAssign,              // *=
        Plus,                   // +
        //AddAssign,              // +=
        Comma,                  // ,
        Minus,                  // -
        //SubAssign,              // -=
        //RightArrow,             // ->
        FullStop,               // .
        //Ellipsis,               // ...
        Slash,                  // /
        DoubleSlash,            // //
        //IntDivAssign,           // //=
        //DivAssign,              // /=
        Colon,                  // :
        Semicolon,              // ;
        Less,                   // <
        LeftShift,              // <<
        //LeftShiftAssign,        // <<=
        LessOrEqual,            // <=
        Assignment,             // =
        Equal,                  // ==
        Greater,                // >
        GreaterOrEqual,         // >=
        RightShift,             // >>
        //RightShiftAssign,       // >>=
        //At,                     // @
        //LeftBracket,            // [
        //RightBracket,           // ]
        BitXor,                 // ^
        //BitXorAssign,           // ^=
        //LeftBrace,              // {
        BitOr,                  // |
        //BitOrAssign,            // |=
        //RightBrace,             // }
        BitNot,                 // ~

        Int,                    // 42
        Float,                  // 42.0

        Id,

        KwAnd,
        //KwAs,
        //KwAssert,
        //KwAsync,
        //KwAwait,
        //KwBreak,
        //KwContinue,
        KwDef,
        //KwDel,
        //KwElif,
        //KwElse,
        //KwExcept,
        KwFalse,
        //KwFinally,
        //KwFor,
        //KwFrom,
        //KwGlobal,
        //KwIf,
        //KwImport,
        //KwIn,
        //KwIs,
        //KwLambda,
        //KwNonLocal,
        //KwNone,
        KwNot,
        KwOr,
        KwPass,
        //KwRaise,
        //KwReturn,
        KwTrue,
        //KwTry,
        //KwType,
        //KwValue,
        //KwWhile,
        //KwWith,
        //KwYield,
    }
}