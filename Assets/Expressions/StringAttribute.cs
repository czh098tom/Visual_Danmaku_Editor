using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions
{
    class StringAttribute : Attribute
    {
        public string String { get; }

        public string IdentifierKey { get; }

        public StringAttribute(string midString) { String = midString; }
    }
}
