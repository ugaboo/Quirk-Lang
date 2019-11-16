using System.Collections.Generic;

namespace Quirk.AST
{
    public class Overload : ProgObj
    {
        public readonly string Name;
        public readonly List<Function> Funcs = new List<Function>();


        public Overload(string name)
        {
            Name = name;
        }

        public Overload(Overload other)
        {
            Name = other.Name;
            Funcs.AddRange(other.Funcs);
        }

        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Function Find(List<TypeObj> args)
        {
            Function result = null;

            foreach (var func in Funcs) {
                if (func.Parameters.Count == args.Count) {
                    var matched = true;
                    for (var i = 0; i < args.Count; i += 1) {
                        var type = func.Parameters[i].Type;
                        if (type != null && type != args[i]) {
                            matched = false;
                            break;
                        }
                    }
                    if (matched) {
                        if (result == null || func.TemplateParamsCount <= result.TemplateParamsCount) {
                            result = func;
                        }
                    }
                }
            }

            if (result == null) {
                // search for function with default parameters
                // ...
            }

            return result;
        }

        public Function Find(params TypeObj[] paramTypes)
        {
            return Find(new List<TypeObj>(paramTypes));
        }
    }
}