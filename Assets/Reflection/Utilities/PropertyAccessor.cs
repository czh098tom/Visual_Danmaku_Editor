using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Reflection.Utilities
{
    public class PropertyAccessor : IMemberWithAccessor
    {
        private readonly PropertyInfo property;
        public MemberInfo AdapteeMember => property;

        public Type Type { get; }

        public MethodInfo GetMethod { get; private set; } 
        public MethodInfo SetMethod { get; private set; } 

        public PropertyAccessor(PropertyInfo property, bool includeNonPublic = true)
        {
            this.property = property;
            Type = property.PropertyType;
            GetMethod = property.GetGetMethod(includeNonPublic);
            SetMethod = property.GetSetMethod(includeNonPublic);
        }

        public object GetValue(object obj)
        {
            if (GetMethod == null) throw new InvalidOperationException("Cannot find get method in this property.");
            return GetMethod.Invoke(obj, ReflectionExtension.emptyParameters);
        }

        public void SetValue(object obj, object value)
        {
            if (SetMethod == null) throw new InvalidOperationException("Cannot find set method in this property.");
            SetMethod.Invoke(obj, new object[] { value });
        }
    }
}
