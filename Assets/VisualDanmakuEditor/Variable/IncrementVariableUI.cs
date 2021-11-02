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
    public class IncrementVariableUI : VariableUI<IncrementVariable>
    {
        [SerializeField]
        LabelledInput begin;
        [SerializeField]
        LabelledInput increment;

        public override void Assign(IncrementVariable model)
        {
            base.Assign(model);
            begin.Value = model.Begin;
            increment.Value = model.Increment;
        }

        public override void Start()
        {
            base.Start();
            begin.InputComponent.onValueChanged.AddListener(s => { Model.Begin = s; Calculate(); });
            increment.InputComponent.onValueChanged.AddListener(s => { Model.Increment = s; Calculate(); });
        }
    }
}
