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
}
