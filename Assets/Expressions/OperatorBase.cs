﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Latticework.Expressions
{
    public abstract class OperatorBase
    {
        private static Dictionary<string, ConstructorInfo> operatorCtor = null;
        private static readonly Dictionary<string, OperatorBase> cached = new Dictionary<string, OperatorBase>();
        private static Operator.FunctionCall functionCall;

        private readonly static object[] nullParam = new object[] { };
        private readonly static Type[] nullType = new Type[] { };

        public static OperatorBase GetOperator(string s)
        {
            InitOperators();
            try
            {
                return cached[s];
            }
            catch (KeyNotFoundException knf)
            {
                throw new OperatorNotFoundException(s, knf);
            }
        }

        private static void InitOperators()
        {
            if (operatorCtor == null)
            {
                operatorCtor = new Dictionary<string, ConstructorInfo>();
                foreach (Type t in typeof(OperatorBase).Assembly.GetTypes())
                {
                    StringAttribute sa = t.GetCustomAttribute<StringAttribute>();
                    if (typeof(OperatorBase).IsAssignableFrom(t) && sa != null)
                    {
                        operatorCtor.Add(sa.String, t.GetConstructor(nullType));
                        cached.Add(sa.String, operatorCtor[sa.String].Invoke(nullParam) as OperatorBase);
                    }
                }
                functionCall = cached[Operator.FunctionCall.identifier] as Operator.FunctionCall;
            }
        }

        public static bool HasOperator(string name)
        {
            InitOperators();
            return cached.ContainsKey(name);
        } 

        public static void RegisterFunction(FunctionDescriptor function)
        {
            GetOperator(Operator.FunctionCall.identifier);
            functionCall.Register(function);
        }

        public static bool HasFunction(string name)
        {
            GetOperator(Operator.FunctionCall.identifier);
            return functionCall.HasFunc(name);
        }

        protected readonly Stack<float> result = new Stack<float>();

        /// <summary>
        /// Calculate the result of this operator.
        /// </summary>
        /// <param name="parameters">Inversely ordered list of parameters.</param>
        /// <param name="identifiers">Inversely ordered list of identifier of parameters if it is not calculated.</param>
        /// <returns>The stack of the results. Should not be modified by other method.</returns>
        public Stack<float> CalculateWithIdentifiers(IReadOnlyList<float> parameters, IReadOnlyList<string> identifiers)
        {
            result.Clear();
            Calculate(parameters, identifiers);
            return result;
        }

        /// <summary>
        /// Store priority of an operator. Operator with smaller priority is calculated first.
        /// </summary>
        public virtual int Priority { get => 0; }

        public abstract IEnumerable<ExpressionCalculatingException> OperandCounter(Stack<float> values, Stack<string> identifiers);

        /// <summary>
        /// Calculate the result of this operator.
        /// </summary>
        /// <param name="parameters">Inversely ordered list of parameters.</param>
        /// <param name="identifiers">Inversely ordered list of identifier of parameters if it is not calculated.</param>
        /// <returns></returns>
        public abstract void Calculate(IReadOnlyList<float> parameters, IReadOnlyList<string> identifiers);
    }
}
