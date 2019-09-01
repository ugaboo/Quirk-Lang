using System.Collections.Generic;

namespace Quirk.AST
{
    public class Module : IProgObj
    {
        public readonly Dictionary<string, IProgObj> NameTable = new Dictionary<string, IProgObj>();
        public readonly List<IProgObj> Statements = new List<IProgObj>();

        public readonly string Name;


        public Module(string name)
        {
            Name = name;

            NameTable["print"] = new Intrinsic("print", IntrinsicType.Print);
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}