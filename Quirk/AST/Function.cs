using System.Collections.Generic;

namespace Quirk.AST
{
    public class Function : ProgObj
    {
        public readonly string Name;
        public readonly bool BuiltIn;     // will be removed after implementation of the import system

        public readonly List<Parameter> Parameters = new List<Parameter>();
        public ProgObj RetType;     // TODO: TypeObj

        public readonly List<ProgObj> Statements = new List<ProgObj>();



        public Function(string name)
        {
            Name = name;
        }

        public Function(TypeObj retType, string name, params TypeObj[] paramTypes)
        {
            Name = name;
            BuiltIn = true;
            RetType = retType;
            for (var i = 0; i < paramTypes.Length; i += 1) {
                Parameters.Add(new Parameter(i, paramTypes[i]));
            }
        }

        public int TemplateParamsCount {
            get {
                var count = 0;
                foreach (var param in Parameters) {
                    if (param.Type == null) {
                        count += 1;
                    }
                }
                return count;
            }
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}