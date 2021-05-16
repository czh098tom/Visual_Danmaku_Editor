using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions
{
    public class OperatorNotFoundException : Exception
    {
        public OperatorNotFoundException(string operatorName, KeyNotFoundException innerException) 
            : base($"Operator \"{operatorName}\" does not exist.", innerException) { }
    }
}
