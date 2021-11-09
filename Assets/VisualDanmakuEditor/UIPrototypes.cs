using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Latticework.UnityEngine.Utilities;

using VisualDanmakuEditor.Variable;
using VisualDanmakuEditor.BulletModel;
using VisualDanmakuEditor.Models.AdvancedRepeat.Variables;
using VisualDanmakuEditor.Models.Objects;

namespace VisualDanmakuEditor
{
    public class UIPrototypes : Singleton<UIPrototypes>
    {
        [SerializeField]
        GameObject emptyContent;
        [SerializeField]
        GameObject taskUI;
        [SerializeField]
        GameObject taskItemUI;
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
        GameObject simpleModelUI;
        [SerializeField]
        GameObject twoSegmentModelUI;

        public GameObject EmptyContent { get => emptyContent; }
        public GameObject TaskUI { get => taskUI; }
        public GameObject IteratorUI { get => iteratorUI; }
        public GameObject TaskItemUI { get => taskItemUI; }

        private Dictionary<Type, GameObject> var2PrototypeMappping = new Dictionary<Type, GameObject>();
        private Dictionary<Type, Type> var2BehaviorTypeMappping = new Dictionary<Type, Type>();

        private Dictionary<Type, GameObject> bullet2PrototypeMappping = new Dictionary<Type, GameObject>();
        private Dictionary<Type, Type> bullet2BehaviorTypeMappping = new Dictionary<Type, Type>();

        protected override void Awake()
        {
            base.Awake();
            var2PrototypeMappping.Add(typeof(LinearVariable), linearVariableUI);
            var2PrototypeMappping.Add(typeof(IncrementVariable), incrementVariableUI);
            var2PrototypeMappping.Add(typeof(ReboundingVariable), reboundingVariableUI);

            var2BehaviorTypeMappping.Add(typeof(LinearVariable), typeof(LinearVariableUI));
            var2BehaviorTypeMappping.Add(typeof(IncrementVariable), typeof(IncrementVariableUI));
            var2BehaviorTypeMappping.Add(typeof(ReboundingVariable), typeof(ReboundingVariableUI));

            bullet2PrototypeMappping.Add(typeof(SimpleBulletModel), simpleModelUI);
            bullet2PrototypeMappping.Add(typeof(TwoSegmentModel), twoSegmentModelUI);

            bullet2BehaviorTypeMappping.Add(typeof(SimpleBulletModel), typeof(SimpleModelUI));
            bullet2BehaviorTypeMappping.Add(typeof(TwoSegmentModel), typeof(TwoSegmentModelUI));
        }

        public GameObject SelectVariableUIObject(Type t)
        {
            return var2PrototypeMappping[t];
        }

        public Type SelectVariableUIBehavior(Type t)
        {
            return var2BehaviorTypeMappping[t];
        }

        public GameObject SelectBulletUIObject(Type t)
        {
            return bullet2PrototypeMappping[t];
        }

        public Type SelectBulletUIBehavior(Type t)
        {
            return bullet2BehaviorTypeMappping[t];
        }
    }
}
