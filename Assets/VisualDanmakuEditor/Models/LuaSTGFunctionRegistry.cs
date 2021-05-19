using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Latticework.Expressions;

namespace VisualDanmakuEditor.Models
{
    public static class LuaSTGFunctionRegistry
    {
        private static bool isRegistered = false;
        public static void Register()
        {
            if (isRegistered) return;
            OperatorBase.RegisterFunction(
                new FunctionDescriptor("sin", new Func<IReadOnlyList<float>, float>((f) 
                => Convert.ToSingle(Math.Sin(f[0] * Math.PI / 180)))));
            OperatorBase.RegisterFunction(
                new FunctionDescriptor("cos", new Func<IReadOnlyList<float>, float>((f) 
                => Convert.ToSingle(Math.Cos(f[0] * Math.PI / 180)))));
            OperatorBase.RegisterFunction(
                new FunctionDescriptor("tan", new Func<IReadOnlyList<float>, float>((f) 
                => Convert.ToSingle(Math.Tan(f[0] * Math.PI / 180)))));
            OperatorBase.RegisterFunction(
                new FunctionDescriptor("asin", new Func<IReadOnlyList<float>, float>((f) 
                => Convert.ToSingle(Math.Asin(f[0]) / Math.PI * 180))));
            OperatorBase.RegisterFunction(
                new FunctionDescriptor("acos", new Func<IReadOnlyList<float>, float>((f) 
                => Convert.ToSingle(Math.Acos(f[0]) / Math.PI * 180))));
            OperatorBase.RegisterFunction(
                new FunctionDescriptor("atan", new Func<IReadOnlyList<float>, float>((f) 
                => Convert.ToSingle(Math.Atan(f[0]) / Math.PI * 180))));
            OperatorBase.RegisterFunction(
                new FunctionDescriptor("abs", new Func<IReadOnlyList<float>, float>((f) 
                => Convert.ToSingle(Math.Abs(f[0])))));
            OperatorBase.RegisterFunction(
                new FunctionDescriptor("sign", new Func<IReadOnlyList<float>, float>((f) 
                => Convert.ToSingle(Math.Sign(f[0])))));
            OperatorBase.RegisterFunction(
                new FunctionDescriptor("sqrt", new Func<IReadOnlyList<float>, float>((f) 
                => Convert.ToSingle(Math.Sqrt(f[0])))));
            OperatorBase.RegisterFunction(
                new FunctionDescriptor("int", new Func<IReadOnlyList<float>, float>((f) 
                => Convert.ToSingle(Convert.ToInt64(f[0])))));

            OperatorBase.RegisterFunction(
                new FunctionDescriptor("atan2", new Func<IReadOnlyList<float>, float>((f) 
                => Convert.ToSingle(Math.Atan2(f[0], f[1]) / Math.PI * 180))));
            OperatorBase.RegisterFunction(
                new FunctionDescriptor("hypot", new Func<IReadOnlyList<float>, float>((f) 
                => Convert.ToSingle(Math.Sqrt(f[0] * f[0] + f[1] * f[1])))));
            isRegistered = true;
        }
    }
}
