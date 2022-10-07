using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using Latticework.UnityEngine.UI;
using VisualDanmakuEditor.Models;

namespace VisualDanmakuEditor
{
    public abstract class BulletModelUI : Assignable<BulletModelBase>, ICalculationCallbackHook
    {
        [SerializeField]
        Button change;
        [SerializeField]
        Toggle useGlobalCoord;

        public Button Change { get => change; }

        public Action Calculate { get; set; }

        public override void UpdateUI()
        {
            base.UpdateUI();
            useGlobalCoord.isOn = Model.IsGlobalCoord;
        }

        protected virtual void Start()
        {
            useGlobalCoord.onValueChanged.AddListener(b => { Model.IsGlobalCoord = b; Calculate(); });
        }
    }

    public abstract class BulletModelUI<T> : BulletModelUI where T : BulletModelBase
    {
        protected new T Model { get; private set; }

        protected override sealed void Assign(BulletModelBase model)
        {
            if (!(model is T)) throw new InvalidCastException();
            Assign((T)model);
        }

        protected virtual void Assign(T bulletModel)
        {
            base.Assign(bulletModel);
            Model = bulletModel;
        }
    }
}
