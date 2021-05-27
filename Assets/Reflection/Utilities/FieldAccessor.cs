using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Latticework.Reflection.Utilities
{
    public class FieldAccessor : IMemberWithAccessor
    {
        private readonly FieldInfo field;
        public MemberInfo AdapteeMember => field;

        public Type Type { get; }

        public FieldAccessor(FieldInfo field)
        {
            this.field = field;
            Type = field.FieldType;
        }

        public object GetValue(object obj)
        {
            return field.GetValue(obj);
        }

        public void SetValue(object obj, object value)
        {
            field.SetValue(obj, value);
        }
    }
}
