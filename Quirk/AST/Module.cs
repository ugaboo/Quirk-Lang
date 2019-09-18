using System.Collections.Generic;

namespace Quirk.AST
{
    public class Module : ProgObj
    {
        public readonly Dictionary<string, ProgObj> NameTable = new Dictionary<string, ProgObj>();
        public readonly List<ProgObj> Statements = new List<ProgObj>();

        public readonly string Name;


        public Module(string name)
        {
            Name = name;

            NameTable["print"] = new Intrinsic("print", IntrinsicType.Print);
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}