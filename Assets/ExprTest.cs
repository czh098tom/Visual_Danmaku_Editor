using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Latticework.Expressions;
using VisualDanmakuEditor.Models;

public class ExprTest : MonoBehaviour
{
    Dictionary<string, float> param = new Dictionary<string, float>()
    {
        { "a", 1.2f },
        { "b", 1.2f },
        { "c", 1.2f }
    };

    [SerializeField]
    string expr;

    // Start is called before the first frame update
    void Start()
    {
        LuaSTGFunctionRegistry.Register();
        Expression expr = new Expression(this.expr);
        Debug.Log(expr);
        Debug.Log(expr.Calculate(GetOrDefault));
    }

    private float GetOrDefault(string s)
    {
        if (param.ContainsKey(s))
        {
            return param[s];
        }
        return 0;
    }
}
