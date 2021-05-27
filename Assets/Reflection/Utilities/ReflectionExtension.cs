using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Latticework.Reflection.Utilities
{
    public static class ReflectionExtension
    {
        public static readonly object[] emptyParameters = Array.Empty<object>();

        public static IEnumerable<Type> IncludeBaseType(this Type type, Type stopFlag = null)
        {
            Type current = type;
            do
            {
                yield return current;
                current = current.BaseType;
            }
            while (current != stopFlag && current != null);
        }

        public static IEnumerable<IMemberWithAccessor> GetAccessorsWithName(this Type type, string name
            , Type stopFlag, BindingFlags bindingFlags = BindingFlags.Default)
        {
            foreach(Type currentType in type.IncludeBaseType(stopFlag))
            {
                FieldInfo fi = currentType.GetField(name, bindingFlags);
                if (fi != null)
                {
                    FieldInfo fTarget = fi.DeclaringType?.GetField(name, bindingFlags);
                    if (fTarget != null)
                    {
                        yield return new FieldAccessor(fTarget);
                    }
                    else
                    {
                        throw new FieldAccessException($"Failed to find field \"{name}\"");
                    }
                }
                else
                {
                    PropertyInfo pi = currentType.GetProperty(name, bindingFlags);
                    if (pi != null)
                    {
                        PropertyInfo pTarget = pi.DeclaringType?.GetProperty(name, bindingFlags);
                        if (pTarget != null)
                        {
                            yield return new PropertyAccessor(pTarget, (bindingFlags & BindingFlags.NonPublic) != 0);
                        }
                        else
                        {
                            throw new FieldAccessException($"Failed to find property \"{name}\"");
                        }
                    }
                }
            }
            throw new FieldAccessException($"Failed to find any field or property with name \"{name}\"");
        }
    }
}
