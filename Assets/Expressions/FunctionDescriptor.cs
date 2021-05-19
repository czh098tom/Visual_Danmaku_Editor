using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions
{
    public struct FunctionDescriptor
    {
        public string Name { get; set; }
        public Func<IReadOnlyList<float>, float> Function { get; set; }
        public int ParameterCount { get; set; }

        public FunctionDescriptor(string name, Func<IReadOnlyList<float>, float> function, int parameterCount = 1)
        {
            Name = name;
            Function = function;
            ParameterCount = parameterCount;
        }
    }
}
