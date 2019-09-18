using Quirk.AST;

namespace Quirk.Visitors
{
    public partial class FuncParamsProcessor : Visitor
    {
        public override void Visit(Module module)
        {
            foreach (var pair in module.NameTable) {
                pair.Value.Accept(this);
            }
        }

        public override void Visit(Overload overload)
        {
            foreach (var func in overload.Funcs) {
                func.Accept(this);
            }
        }

        public override void Visit(Func func)
        {
            foreach (var obj in func.Parameters) {
                var param = (Variable)obj;
                if (func.NameTable.TryGetValue(param.Name, out var inner)) {
                    if (inner is Overload overload) {
                        // TODO: параметр param должен быть обобщен с делегатом типа overload, результат должен быть добавлен в таблицу имен функции func
                    } else {
                        throw new CompilationError(ErrorType.DuplicateParameter);
                    }
                } else {
                    func.NameTable[param.Name] = param;
                }
            }
        }
    }
}