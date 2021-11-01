using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using Latticework.UnityEngine.UI;
using VisualDanmakuEditor.Models.AdvancedRepeat.Variables;

namespace VisualDanmakuEditor.Variable
{
    public class LinearVariableUI : VariableUI<LinearVariable>
    {
        [SerializeField]
        LabelledInput begin;
        [SerializeField]
        LabelledInput end;
        [SerializeField]
        Toggle includeEndPoint;

        public override void Assign(LinearVariable model)
        {
            base.Assign(model);
            begin.Value = model.Begin;
            end.Value = model.End;
            includeEndPoint.isOn = model.IsPrecisely;
        }

        public override void Start()
        {
            base.Start();
            begin.InputComponent.onValueChanged.AddListener(s => { Model.Begin = s; Calculate(); });
            end.InputComponent.onValueChanged.AddListener(s => { Model.End = s; Calculate(); });
            includeEndPoint.onValueChanged.AddListener(b => { Model.IsPrecisely = b; Calculate(); });
        }
    }
}
