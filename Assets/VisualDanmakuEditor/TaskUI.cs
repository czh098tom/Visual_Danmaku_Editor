using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using Latticework.UnityEngine.UI;
using VisualDanmakuEditor.Models;
using VisualDanmakuEditor.Models.AdvancedRepeat;
using VisualDanmakuEditor.Models.Objects;

namespace VisualDanmakuEditor
{
    public class TaskUI : Assignable<TaskModel>, ICalculationCallbackHook
    {
        [SerializeField]
        LabelledInput taskName;
        [SerializeField]
        LabelledInput beginTime;
        [SerializeField]
        Button addIterator;
        [SerializeField]
        VerticalLayoutGroup firstIterator;
        [SerializeField]
        VerticalLayoutGroup result;

        LinkedList<IteratorUI> iteratorUIs = new LinkedList<IteratorUI>();
        BulletModelUI bulletModelUI;

        RectTransform rect;

        public Action Calculate { get; set; }

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            Calculate = () => BulletCalculator.GetCalculatorFor(Model).Calculate();
            foreach (ICalculationCallbackHook cal in GetComponentsInChildren<MonoBehaviour>().OfType<ICalculationCallbackHook>())
            {
                cal.Calculate = Calculate;
            }
        }

        public override void Assign(TaskModel model)
        {
            base.Assign(model);

            taskName.Value = model.Name;
            beginTime.Value = model.BeginTime.ToString();
            IteratorUI parent = null;
            foreach (AdvancedRepeatModel advr in model)
            {
                if (parent == null)
                {
                    parent = AddIterator(advr, firstIterator, 0);
                }
                else
                {
                    parent = AddIterator(advr, parent.IteratorContainer);
                }
            }
            SetBulletModel();
        }

        private void Start()
        {
            taskName.InputComponent.onValueChanged.AddListener(s => Model.Name = s);
            beginTime.InputComponent.onValueChanged.AddListener(s => { Model.BeginTime = int.TryParse(s, out int v) ? v : 0; Calculate(); });
            addIterator.onClick.AddListener(() => { AddNewIterator(); Calculate(); });
        }

        private void Update()
        {
            LayoutRebuilder.MarkLayoutForRebuild(rect);
        }

        public void AddNewIterator()
        {
            AdvancedRepeatModel model = new AdvancedRepeatModel();
            if (iteratorUIs.Count == 0)
            {
                AddIterator(model, firstIterator, 0);
            }
            else
            {
                AddIterator(model, iteratorUIs.Last.Value.IteratorContainer);
            }
            Model.AddLast(model);
        }

        public IteratorUI AddIterator(AdvancedRepeatModel model, VerticalLayoutGroup parent, int idOffset = -1)
        {
            IteratorUI ui = Instantiate(UIPrototypes.Instance.IteratorUI).GetComponent<IteratorUI>();
            ui.Calculate = Calculate;
            RectTransform rect = ui.GetComponent<RectTransform>();
            rect.SetParent(parent.transform, false);
            rect.SetSiblingIndex(parent.transform.childCount - 1 + idOffset);
            iteratorUIs.AddLast(ui);
            ui.Assign(model);
            ui.Remove.onClick.AddListener(() => { RemoveIterator(ui); Calculate(); });
            return ui;
        }

        public void RemoveIterator(IteratorUI ui)
        {
            LinkedListNode<AdvancedRepeatModel> modelNode = Model.First;
            LinkedListNode<IteratorUI> uiNode = iteratorUIs.First;
            while (modelNode.Value != ui.Model)
            {
                modelNode = modelNode.Next;
                uiNode = uiNode.Next;
            }
            if (uiNode.Next != null)
            {
                if (iteratorUIs.First.Value == ui)
                {
                    uiNode.Next.Value.transform.SetParent(firstIterator.transform, false);
                }
                else
                {
                    uiNode.Next.Value.transform.SetParent(uiNode.Previous.Value.IteratorContainer.transform, false);
                }
            }
            Destroy(ui.gameObject);
            Model.Remove(modelNode);
            iteratorUIs.Remove(uiNode);
        }

        private void ChangeModel()
        {
            BulletModelBase model = Model.BulletModel;
            Model.BulletModel = model switch
            {
                SimpleBulletModel m => new TwoSegmentModel() 
                { 
                    Style = m.Style, 
                    Color = m.Color, 
                    XExpression = m.XExpression, 
                    YExpression = m.YExpression, 
                    IsGlobalCoord = m.IsGlobalCoord 
                },
                TwoSegmentModel m => new SimpleBulletModel()
                {
                    Style = m.Style,
                    Color = m.Color,
                    XExpression = m.XExpression,
                    YExpression = m.YExpression,
                    IsGlobalCoord = m.IsGlobalCoord
                },
                _ => new SimpleBulletModel()
            };
            Destroy(bulletModelUI.gameObject);
            SetBulletModel();
        }

        private void SetBulletModel()
        {
            bulletModelUI = (BulletModelUI)Instantiate(UIPrototypes.Instance.SelectBulletUIObject(Model.BulletModel.GetType()))
                .GetComponent(UIPrototypes.Instance.SelectBulletUIBehavior(Model.BulletModel.GetType()));
            bulletModelUI.Calculate = Calculate;
            bulletModelUI.transform.SetParent(result.transform, false);
            bulletModelUI.transform.SetAsLastSibling();
            bulletModelUI.Assign(Model.BulletModel);
            bulletModelUI.Change.onClick.AddListener(() => { ChangeModel(); Calculate(); });
        }
    }
}
