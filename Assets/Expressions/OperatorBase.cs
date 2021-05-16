using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Latticework.Expressions
{
    internal abstract class OperatorBase
    {
        private static Dictionary<string, ConstructorInfo> operatorCtor = null;
        private static readonly Dictionary<string, OperatorBase> cached = new Dictionary<string, OperatorBase>();
        private static readonly HashSet<string> unary = new HashSet<string>();
        private readonly static object[] nullParam = new object[] { };
        private readonly static Type[] nullType = new Type[] { };

        public static OperatorBase GetOperator(string s)
        {
            if (operatorCtor == null)
            {
                operatorCtor = new Dictionary<string, ConstructorInfo>();
                foreach (Type t in typeof(OperatorBase).Assembly.GetTypes())
                {
                    StringAttribute sa = t.GetCustomAttribute<StringAttribute>();
                    if (t.IsSubclassOf(typeof(OperatorBase)) && sa != null)
                    {
                        operatorCtor.Add(sa.StringForm, t.GetConstructor(nullType));
                        cached.Add(sa.StringForm, operatorCtor[sa.StringForm].Invoke(nullParam) as OperatorBase);
                        if (cached[sa.StringForm].NumOfOprands == 1) unary.Add(s);
                    }
                }
            }
            try
            {
                return cached[s];
            }
            catch (KeyNotFoundException knf)
            {
                throw new OperatorNotFoundException(s, knf);
            }
        }

        public virtual int NumOfOprands { get => 2; }
        /// <summary>
        /// Store priority of an operator. Operator with smaller priority is calculated first.
        /// </summary>
        public virtual int Priority { get => 0; }

        public abstract float Calculate(params float[] parameters);
    }
}
