using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Graph
{
    public interface INamed
    {
        string TypeName { get; }
    }

    public static class NamedExtension
    {
        public static T ReferredDefinition<T>(this INamed obj) where T : INamed
        {
            return Definition<T>.Get(obj.TypeName);
        }
    }
}
