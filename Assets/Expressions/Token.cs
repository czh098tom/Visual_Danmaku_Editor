using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions
{
    public static class Token
    {
        public static bool IsAsciiSymbol(char c)
        {
            return ((c > 0x20 && c < 0x30) || (c > 0x39 && c < 0x41) || (c > 0x5a && c < 0x61) || (c > 0x7a && c < 0x7f))
                && c != '.';
        }

        public static bool IsAsciiNonBracketSymbol(char c)
        {
            return IsAsciiSymbol(c) && c != '(' && c != ')';
        }

        public static bool IsIdentifierOrNumber(char c)
        {
            return !char.IsWhiteSpace(c) && (!IsAsciiSymbol(c) || c == '_');
        }

        public static bool IsOperator(char c)
        {
            return !char.IsWhiteSpace(c) && (IsAsciiNonBracketSymbol(c) && c != '_');
        }
    }
}
