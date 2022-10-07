using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Latticework.Expressions.Operator
{
    [String(identifier)]
    internal class FunctionCall : OperatorBase
    {
        public const string identifier = "[call]";

        private readonly Dictionary<string, Func<IReadOnlyList<float>, float>> functions
            = new Dictionary<string, Func<IReadOnlyList<float>, float>>();
        private readonly Dictionary<string, int> parameterCount = new Dictionary<string, int>();

        private readonly List<float> orderedParameters = new List<float>();

        public override int Priority => 11;

        internal void Register(FunctionDescriptor function)
        {
            functions.Add(function.Name, function.Function);
            parameterCount.Add(function.Name, function.ParameterCount);
        }

        internal bool HasFunc(string name)
        {
            return functions.ContainsKey(name);
        }

        public override IEnumerable<ExpressionCalculatingException> OperandCounter(Stack<float> values, Stack<string> identifiers)
        {
            while (!functions.ContainsKey(identifiers.Peek()))
            {
                yield return null;
            }
            yield return null;
        }

        public override void Calculate(IReadOnlyList<float> parameters, IReadOnlyList<string> identifiers)
        {
            orderedParameters.Clear();
            for (int i = parameters.Count - 2; i >= 0; i--)
            {
                orderedParameters.Add(parameters[i]);
            }

            result.Push((float)(functions[identifiers[identifiers.Count - 1]].Invoke(orderedParameters)));
        }
    }
}
