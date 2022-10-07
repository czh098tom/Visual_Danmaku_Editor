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
    public class IteratorUI : Assignable<AdvancedRepeatModel>, ICalculationCallbackHook
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
        [SerializeField]
        Button densityDown;
        [SerializeField]
        Button densityUp;
        [SerializeField]
        Button lengthDown;
        [SerializeField]
        Button lengthUp;

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

        protected override void Assign(AdvancedRepeatModel model)
        {
            base.Assign(model);
            foreach (VariableModelBase var in model)
            {
                AddVariable(var);
            }
        }

        public override void UpdateUI()
        {
            times.Value = Model.Times;
            interval.Value = Model.Interval;
            foreach(VariableUI vui in variables)
            {
                vui.UpdateUI();
            }
        }

        private void Start()
        {
            times.InputComponent.onValueChanged.AddListener(s => { Model.Times = s; Calculate(); });
            interval.InputComponent.onValueChanged.AddListener(s => { Model.Interval = s; Calculate(); });
            addVariable.onClick.AddListener(() => { AddNewVariable(); Calculate(); });
            densityDown.onClick.AddListener(() => { Model.AdjustDensity(-1); UpdateUI(); Calculate(); });
            densityUp.onClick.AddListener(() => { Model.AdjustDensity(+1); UpdateUI(); Calculate(); });
            lengthDown.onClick.AddListener(() => { Model.AdjustLength(-1); UpdateUI(); Calculate(); });
            lengthUp.onClick.AddListener(() => { Model.AdjustLength(+1); UpdateUI(); Calculate(); });
        }

        public void AddNewVariable()
        {
            LinearVariable var = new LinearVariable();
            Model.AddLast(var);
            AddVariable(var);
        }

        private void AddVariable(VariableModelBase var)
        {
            VariableUI ui = (VariableUI)Instantiate(UIPrototypes.Instance.SelectVariableUIObject(var.GetType()))
                .GetComponent(UIPrototypes.Instance.SelectVariableUIBehavior(var.GetType()));
            ui.Calculate = Calculate;
            RectTransform rect = ui.GetComponent<RectTransform>();
            rect.SetParent(variableContainer.transform, false);
            rect.SetAsLastSibling();
            variables.Add(ui);
            ui.AssignAndUpdateUI(var);
            ui.Remove.onClick.AddListener(() => { RemoveVariable(ui); Calculate(); });
            ui.Change.onClick.AddListener(() => { ChangeVariable(ui); Calculate(); });
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

        public void ChangeVariable(VariableUI ui)
        {
            string name = ui.Model.VariableName;
            int id = variables.IndexOf(ui);
            int count = variables.Count;

            Destroy(ui.gameObject);
            variables.Remove(ui);
            LinkedListNode<VariableModelBase> node = Model.First;
            while (node.Value != ui.Model)
            {
                node = node.Next;
            }
            VariableModelBase old = node.Value;
            LinkedListNode<VariableModelBase> beforeNode = node.Previous;
            Model.Remove(node);

            VariableModelBase var = null;
            var = old switch
            {
                LinearVariable => new IncrementVariable(),
                IncrementVariable => new ReboundingVariable(),
                ReboundingVariable => new LinearVariable(),
                _ => new LinearVariable()
            };
            var.VariableName = name;

            if (beforeNode != null)
            {
                Model.AddAfter(beforeNode, var);
            }
            else
            {
                Model.AddFirst(var);
            }

            VariableUI newUI = (VariableUI)Instantiate(UIPrototypes.Instance.SelectVariableUIObject(var.GetType()))
                .GetComponent(UIPrototypes.Instance.SelectVariableUIBehavior(var.GetType()));
            newUI.Calculate = Calculate;
            RectTransform rect = newUI.GetComponent<RectTransform>();
            rect.SetParent(variableContainer.transform, false);
            rect.SetSiblingIndex(variableContainer.transform.childCount - count + id);
            variables.Insert(id , newUI);
            newUI.AssignAndUpdateUI(var);
            newUI.Remove.onClick.AddListener(() => { RemoveVariable(newUI); Calculate(); });
            newUI.Change.onClick.AddListener(() => { ChangeVariable(newUI); Calculate(); });
        }
    }
}
