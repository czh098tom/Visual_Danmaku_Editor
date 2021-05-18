using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Latticework.Expressions;

public class ExprTest : MonoBehaviour
{
    Dictionary<string, float> param = new Dictionary<string, float>()
    {
        { "a", 1.2f },
        { "b", 1.2f },
        { "c", 1.2f }
    };

    // Start is called before the first frame update
    void Start()
    {
        Expression expr = new Expression("1+sin(45+45*1)/2");
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
