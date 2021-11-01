using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Latticework.UnityEngine.Utilities;
using VisualDanmakuEditor.Models.AdvancedRepeat.Variables;

namespace VisualDanmakuEditor
{
    public class UIPrototypes : Singleton<UIPrototypes>
    {
        [SerializeField]
        GameObject iteratorUI;
        [Header("Variables")]
        [SerializeField]
        GameObject linearVariableUI;
        [Header("Models")]
        [SerializeField]
        GameObject defaultModelUI;

        public GameObject IteratorUI { get => iteratorUI; }
        public GameObject LinearVariableUI { get => linearVariableUI; }
        public GameObject DefaultModelUI { get => defaultModelUI; }

        public Dictionary<Type, GameObject> modelDict = new Dictionary<Type, GameObject>();

        protected override void Awake()
        {
            base.Awake();
            modelDict.Add(typeof(LinearVariable), linearVariableUI);
        }

        public GameObject SelectVariableUI(Type t)
        {
            return modelDict[t];
        }
    }
}
