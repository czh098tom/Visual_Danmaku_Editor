using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions
{
    public class ExpressionCalculatingException : Exception
    {
        public ExpressionCalculatingException(string message, Exception inner) : base(message, inner) { }
    }
}
