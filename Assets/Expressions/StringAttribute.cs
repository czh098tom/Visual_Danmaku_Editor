using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions
{
    class StringAttribute : Attribute
    {
        public string HeadString { get; }
        public string HeadSeperatorString { get; }
        public string MidString { get; }
        public string TailString { get; }

        public string IdentifierKey { get; }

        public StringAttribute(string midString) : this(null, null, midString, null) { }

        public StringAttribute(string headString, string headSeperatorString, string midString, string tailString) 
        {
            HeadString = headString;
            HeadSeperatorString = headSeperatorString;
            MidString = midString;
            TailString = tailString;
            IdentifierKey = $"{headString} {headSeperatorString} {midString} {tailString}";
        }
    }
}
