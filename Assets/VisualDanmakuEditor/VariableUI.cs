using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using Latticework.UnityEngine.UI;
using VisualDanmakuEditor.Models.AdvancedRepeat;

namespace VisualDanmakuEditor
{
    public abstract class VariableUI : Assignable<VariableModelBase>, ICalculationCallbackHook
    {
        [SerializeField]
        Button change;
        [SerializeField]
        Button remove;
        [SerializeField]
        protected LabelledInput variableName;

        public Button Change { get => change; }
        public Button Remove { get => remove; }

        public Action Calculate { get; set; }

        public override void UpdateUI()
        {
            base.UpdateUI();
            variableName.Value = Model.VariableName;
        }

        public virtual void Start()
        {
            variableName.InputComponent.onValueChanged.AddListener(s => { Model.VariableName = s; Calculate(); });
        }
    }

    public abstract class VariableUI<T> : VariableUI where T : VariableModelBase
    {
        protected new T Model { get; private set; }

        protected override sealed void Assign(VariableModelBase model)
        {
            if (!(model is T)) throw new InvalidCastException();
            Assign((T)model);
        }

        protected virtual void Assign(T variableModel)
        {
            base.Assign(variableModel);
            this.Model = variableModel;
        }
    }
}
