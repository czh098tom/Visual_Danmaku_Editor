﻿using System;
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
    public class ObjectUI : Assignable<FixedObject>
    {
        [SerializeField]
        Button add;
        [SerializeField]
        Button remove;
        [SerializeField]
        LabelledInput x;
        [SerializeField]
        LabelledInput y;
        [SerializeField]
        ToggleGroup taskGroup;
        [SerializeField]
        VerticalLayoutGroup taskRoot;

        RectTransform rect;

        readonly List<TaskItemUI> tasks = new List<TaskItemUI>();
        readonly Dictionary<TaskItemUI, BulletCalculator> taskCalcMapping = new Dictionary<TaskItemUI, BulletCalculator>();

        BulletCalculatorAllocator bulletAllocator;
        TaskUIAllocator taskAllocator;

        public PredictedObject Boss { get; private set; }

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            bulletAllocator = FindObjectOfType<BulletCalculatorAllocator>();
            taskAllocator = FindObjectOfType<TaskUIAllocator>();
            Boss = FindObjectOfType<PredictedObject>();
        }

        public override void UpdateUI()
        {
            base.UpdateUI();

            x.Value = Model.X.ToString();
            y.Value = Model.Y.ToString();
        }

        public void LoadTasks(IEnumerable<TaskModel> tasks)
        {
            while (this.tasks.Count > 0) RemoveTaskImpl(this.tasks[0]);
            foreach (var task in tasks)
            {
                AddTask(task);
            }
        }

        private void Start()
        {
            x.InputComponent.onValueChanged.AddListener(s => { Model.X = int.TryParse(s, out int v) ? v : 0; CalculateAllRelative(); });
            y.InputComponent.onValueChanged.AddListener(s => { Model.Y = int.TryParse(s, out int v) ? v : 0; CalculateAllRelative(); });
            add.onClick.AddListener(() => { AddTask(); });
            remove.onClick.AddListener(RemoveTask);

            AssignAndUpdateUI(new FixedObject() { X = 0, Y = 100 });
        }

        private void CalculateAllRelative()
        {
            Boss.ObjectModel = Model.BuildModelInContext(s => 0, TimeLine.Instance.CurrentTime);
            foreach (BulletCalculator cal in FindObjectsOfType<BulletCalculator>().Where(c => !c.Model.BulletModel.IsGlobalCoord))
            {
                cal.Calculate();
            }
        }

        private void AddTask(TaskModel task = null)
        {
            BulletCalculator calc = bulletAllocator.Add(task);
            TaskModel taskModel = calc.Model;
            taskModel.Shooter = Model;
            TaskItemUI taskItem = Instantiate(UIPrototypes.Instance.TaskItemUI).GetComponent<TaskItemUI>();
            taskItem.transform.SetParent(taskRoot.transform, false);
            taskItem.transform.SetAsLastSibling();
            int index = tasks.Count;
            tasks.Add(taskItem);
            taskItem.GetComponent<Toggle>().group = taskGroup;
            taskCalcMapping.Add(taskItem, calc);
            taskItem.Toggle.onValueChanged.AddListener(b => { if (b) SwitchTo(index); });
            taskItem.AssignAndUpdateUI(taskModel);
            SwitchTo(index);
        }

        private void RemoveTask()
        {
            TaskItemUI target = tasks.Find(t => t.Toggle == taskGroup.GetFirstActiveToggle());
            RemoveTaskImpl(target);
        }

        private void RemoveTaskImpl(TaskItemUI target)
        {
            tasks.Remove(target);
            Destroy(BulletCalculator.GetCalculatorFor(target.Model).gameObject);
            Destroy(target.gameObject);
            if (tasks.Count == 0) taskAllocator.Set(null);
        }

        private void SwitchTo(int index)
        {
            if (index >= tasks.Count) return;
            TaskItemUI target = tasks[index];
            taskAllocator.Set(target.Model);
        }
    }
}
