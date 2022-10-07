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
    public class TaskItemUI : Assignable<TaskModel>
    {
        [SerializeField]
        Text taskName;

        public Toggle Toggle { get; private set; }

        private void Awake()
        {
            Toggle = GetComponent<Toggle>();
        }

        public override void UpdateUI()
        {
            taskName.text = Model.Name;
        }

        private void Update()
        {
            UpdateUI();
        }
    }
}
