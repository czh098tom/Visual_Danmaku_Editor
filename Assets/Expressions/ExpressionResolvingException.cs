using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions
{
    public class ExpressionResolvingException : Exception
    {
        public ExpressionResolvingException(string message, Exception inner) : base(message, inner) { }
    }
}
