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
    public abstract class BulletModelUI : AssignableUI<BulletModelBase>, ICalculationCallbackHook
    {
        [SerializeField]
        Button change;

        public Button Change { get => change; }

        public Action Calculate { get; set; }
    }

    public abstract class BulletModelUI<T> : BulletModelUI where T : BulletModelBase
    {
        protected new T Model { get; private set; }

        public override sealed void Assign(BulletModelBase model)
        {
            if (!(model is T)) throw new InvalidCastException();
            Assign((T)model);
        }

        public virtual void Assign(T bulletModel)
        {
            base.Assign(bulletModel);
            this.Model = bulletModel;
        }
    }
}
