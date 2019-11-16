using System.Collections.Generic;

namespace Quirk.AST
{
    public class Function : ProgObj
    {
        public readonly string Name;
        public readonly List<Variable> Parameters = new List<Variable>();
        public ProgObj RetType;     // TODO: TypeObj
        public readonly List<ProgObj> Statements = new List<ProgObj>();


        public Function(string name)
        {
            Name = name;
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