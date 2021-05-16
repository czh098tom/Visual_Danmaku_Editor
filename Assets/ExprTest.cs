using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Latticework.Expressions;

public class ExprTest : MonoBehaviour
{
    Dictionary<string, float> param = new Dictionary<string, float>()
    {
        { "a", 1.2f }
    };

    // Start is called before the first frame update
    void Start()
    {
        Expression expr = new Expression("a*1.2");
        Debug.Log(expr.Calculate((s) => param[s]));
    }
}
