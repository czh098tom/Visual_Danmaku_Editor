using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using Latticework.UnityEngine.UI;
using VisualDanmakuEditor.Models.Curve;

namespace VisualDanmakuEditor
{
    public class CurvePointUI : Assignable<CurvePoint>, ICalculationCallbackHook
    {
        [SerializeField]
        private Button remove;
        [SerializeField]
        private LabelledInput time;
        [SerializeField]
        private LabelledInput value;

        public Button Remove => remove;
        public Action Calculate { get; set; }

        protected virtual void Start()
        {
            time.InputComponent.onValueChanged.AddListener(s =>
            {
                Model.Time = s;
                Calculate();
            });
            value.InputComponent.onValueChanged.AddListener(s =>
            {
                Model.Value = s;
                Calculate();
            });
        }

        public override void UpdateUI()
        {
            base.UpdateUI();

            time.Value = Model.Time;
            value.Value = Model.Value;
        }

        public void SetAsFirst()
        {
            time.InputComponent.interactable = false;
            remove.gameObject.SetActive(false);
        }
    }
}
