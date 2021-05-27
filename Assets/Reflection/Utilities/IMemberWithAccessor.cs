using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Latticework.Reflection.Utilities
{
    public interface IMemberWithAccessor
    {
        MemberInfo AdapteeMember { get; }
        Type Type { get; }
        object GetValue(object obj);
        void SetValue(object obj, object value);
    }
}
