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
    public class VariableUI : AssignableUI<VariableModelBase>, ICalculationCallbackHook
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

        public override void Assign(VariableModelBase model)
        {
            base.Assign(model);
            variableName.Value = model.VariableName;
        }

        public virtual void Start()
        {
            variableName.InputComponent.onValueChanged.AddListener(s => { Model.VariableName = s; Calculate(); });
        }
    }

    public class VariableUI<T> : VariableUI where T : VariableModelBase
    {
        protected new T Model { get; private set; }

        public override sealed void Assign(VariableModelBase model)
        {
            if (!(model is T)) throw new InvalidCastException();
            Assign((T)model);
        }

        public virtual void Assign(T variableModel)
        {
            base.Assign(variableModel);
            this.Model = variableModel;
        }
    }
}
