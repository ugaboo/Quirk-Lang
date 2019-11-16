using System;
using System.Collections.Generic;
using Quirk.AST;

namespace Quirk.Visitors
{
    public class SpecFuncs
    {
        Dictionary<Function, List<Function>> specs = new Dictionary<Function, List<Function>>();


        public void Add(Function template, Function spec)
        {
            if (specs.TryGetValue(template, out var list) == false) {
                specs[template] = list = new List<Function>();
            }
            list.Add(spec);
        }

        public Function Find(Function template, List<TypeObj> types)
        {
            if (specs.TryGetValue(template, out var list)) {
                foreach (var spec in list) {
                    if (spec.Parameters.Count == types.Count) {
                        int i;
                        for (i = 0; i < types.Count; i += 1) {
                            if (spec.Parameters[i].Type != types[i]) {
                                break;
                            }
                        }
                        if (i >= types.Count) {
                            return spec;
                        }
                    }
                }
            }
            return null;
        }
    }
}
