using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using Latticework.UnityEngine.UI;
using Latticework.UnityEngine.Utilities;
using VisualDanmakuEditor.Variable;
using VisualDanmakuEditor.Models.AdvancedRepeat;
using VisualDanmakuEditor.Models.AdvancedRepeat.Variables;

namespace VisualDanmakuEditor
{
    public class IteratorUI : AssignableUI<AdvancedRepeatModel>, ICalculationCallbackHook
    {
        [SerializeField]
        Button remove;
        [SerializeField]
        Button addVariable;
        [SerializeField]
        LabelledInput times;
        [SerializeField]
        LabelledInput interval;
        [SerializeField]
        VerticalLayoutGroup variableContainer;

        VerticalLayoutGroup iteratorContainer;

        public Button Remove { get => remove; }
        public VerticalLayoutGroup VariableContainer { get => variableContainer; }
        public VerticalLayoutGroup IteratorContainer { get => iteratorContainer; }

        List<VariableUI> variables = new List<VariableUI>();

        public Action Calculate { get; set; }

        private void Awake()
        {
            iteratorContainer = GetComponent<VerticalLayoutGroup>();
        }

        public override void Assign(AdvancedRepeatModel model)
        {
            base.Assign(model);
            times.Value = model.Times;
            interval.Value = model.Interval;
            foreach (VariableModelBase var in model)
            {
                AddVariable(var);
            }
        }

        private void Start()
        {
            times.InputComponent.onValueChanged.AddListener(s => { Model.Times = s; Calculate(); });
            interval.InputComponent.onValueChanged.AddListener(s => { Model.Interval = s; Calculate(); });
            addVariable.onClick.AddListener(AddNewVariable);
        }

        public void AddNewVariable()
        {
            LinearVariable var = new LinearVariable();
            Model.AddLast(var);
            AddVariable(var);
            Calculate();
        }

        private void AddVariable(VariableModelBase var)
        {
            VariableUI ui = Instantiate(UIPrototypes.Instance.SelectVariableUI(var.GetType())).GetComponent<VariableUI>();
            ui.Calculate = Calculate;
            RectTransform rect = ui.GetComponent<RectTransform>();
            rect.SetParent(variableContainer.transform, false);
            rect.SetSiblingIndex(variableContainer.transform.childCount - 1);
            variables.Add(ui);
            ui.Assign(var);
            ui.Remove.onClick.AddListener(() => { RemoveVariable(ui); Calculate(); });
        }

        public void RemoveVariable(VariableUI ui)
        {
            Destroy(ui.gameObject);
            variables.Remove(ui);
            LinkedListNode<VariableModelBase> node = Model.First;
            while (node.Value != ui.Model)
            {
                node = node.Next;
            }
            Model.Remove(node);
        }
    }
}
