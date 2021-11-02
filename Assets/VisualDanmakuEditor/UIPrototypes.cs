using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Latticework.UnityEngine.Utilities;
using VisualDanmakuEditor.Variable;
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
        [SerializeField]
        GameObject incrementVariableUI;
        [SerializeField]
        GameObject reboundingVariableUI;
        [Header("Models")]
        [SerializeField]
        GameObject defaultModelUI;

        public GameObject IteratorUI { get => iteratorUI; }
        public GameObject DefaultModelUI { get => defaultModelUI; }

        public Dictionary<Type, GameObject> model2PrototypeMappping = new Dictionary<Type, GameObject>();
        public Dictionary<Type, Type> model2BehaviorTypeMappping = new Dictionary<Type, Type>();

        protected override void Awake()
        {
            base.Awake();
            model2PrototypeMappping.Add(typeof(LinearVariable), linearVariableUI);
            model2PrototypeMappping.Add(typeof(IncrementVariable), incrementVariableUI);
            model2PrototypeMappping.Add(typeof(ReboundingVariable), reboundingVariableUI);

            model2BehaviorTypeMappping.Add(typeof(LinearVariable), typeof(LinearVariableUI));
            model2BehaviorTypeMappping.Add(typeof(IncrementVariable), typeof(IncrementVariableUI));
            model2BehaviorTypeMappping.Add(typeof(ReboundingVariable), typeof(ReboundingVariableUI));
        }

        public GameObject SelectVariableUIObject(Type t)
        {
            return model2PrototypeMappping[t];
        }

        public Type SelectVariableUIBehavior(Type t)
        {
            return model2BehaviorTypeMappping[t];
        }
    }
}
