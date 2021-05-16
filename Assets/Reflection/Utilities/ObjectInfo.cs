using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Latticework.Reflection.Utilities
{
    public static class ObjectInfo
    {
        const BindingFlags anySearchable = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        public static string ToStringEx(this object o, Type exclude = null)
        {
            StringBuilder sb = new StringBuilder();
            Type t = o.GetType();
            sb.Append(t.FullName);
            sb.Append(": {\n");
            while (t != null && t != exclude)
            {
                foreach (FieldInfo fi in t.GetFields(anySearchable))
                {
                    Type tdecl = fi.DeclaringType;
                    if (exclude == null || !tdecl.IsAssignableFrom(exclude))
                    {
                        object value = tdecl.GetField(fi.Name, anySearchable).GetValue(o);
                        sb.AppendFormat("{0} = {1},\n", fi.Name, value);
                    }
                }
                t = t.BaseType;
            }
            sb.Append("}");
            return sb.ToString();
        }
    }
}
