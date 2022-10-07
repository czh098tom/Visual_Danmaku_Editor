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
    public class ReboundingVariableUI : VariableUI<ReboundingVariable>
    {
        [SerializeField]
        LabelledInput first;
        [SerializeField]
        LabelledInput another;

        public override void UpdateUI()
        {
            base.UpdateUI();
            first.Value = Model.First;
            another.Value = Model.Another;
        }

        public override void Start()
        {
            base.Start();
            first.InputComponent.onValueChanged.AddListener(s => { Model.First = s; Calculate(); });
            another.InputComponent.onValueChanged.AddListener(s => { Model.Another = s; Calculate(); });
        }
    }
}
