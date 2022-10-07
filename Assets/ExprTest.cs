using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Latticework.Expressions;
using VisualDanmakuEditor.Models;
using VisualDanmakuEditor.Models.AdvancedRepeat;

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
    [SerializeField]
    float prop = 1;

    // Start is called before the first frame update
    void Start()
    {
        LuaSTGFunctionRegistry.Register();
        Expression expr = new Expression(this.expr);
        Expression exprp = new Expression(VariableModelBase.Scale(expr.Original, prop));
        Debug.Log(expr.ToReversedPolandExprString());
        Debug.Log(expr.Calculate(GetOrDefault));
        Debug.Log(expr.IsConstant);
        Debug.Log(exprp.Original);
        Debug.Log(exprp.ToReversedPolandExprString());
        Debug.Log(exprp.Calculate(GetOrDefault));
        Debug.Log(exprp.IsConstant);
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
