using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions
{
    class StringAttribute : Attribute
    {
        public string StringForm { get; }
        public StringAttribute(string stringForm) { StringForm = stringForm; }
    }
}
