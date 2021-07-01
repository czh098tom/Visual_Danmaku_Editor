using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using VisualDanmakuEditor.IMGUI;
using VisualDanmakuEditor.Models;
using VisualDanmakuEditor.Models.AdvancedRepeat;
using VisualDanmakuEditor.Models.AdvancedRepeat.Variables;

using static VisualDanmakuEditor.IMGUI.IMGUIHelper;

namespace VisualDanmakuEditor
{
    public class AdvancedRepeatUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject bullet;

        private BulletObjectPool objectPool;

        private Rect editWindowSize = new Rect(0, 20, windowWidth, elementHeight * (elementCount + 3));

        public TaskModel Task { get; } = new TaskModel();

        private AdvancedRepeatUIBuilder builder;

        private void Awake()
        {
            LuaSTGFunctionRegistry.Register();
            builder = new AdvancedRepeatUIBuilder(Task);
            objectPool = FindObjectOfType<BulletObjectPool>();
        }

        private void Start()
        {
            AdvancedRepeatModel advr = new AdvancedRepeatModel
            {
                Times = "10"
            };
            advr.AddFirst(new LinearVariable() { VariableName = "ir", Begin = "0", End = "360", IsPrecisely = false });
            Task.AddFirst(advr);
            Task.RotationExpression = "ir";
        }

        private void Update()
        {
            if (builder.IsDirty)
            {
                Calculate();
                builder.IsDirty = false;
            }
        }

        private void OnGUI()
        {
            builder.PredictHeight();
            editWindowSize = GUI.Window(advancedRepeatWindowID, new Rect(editWindowSize.x, editWindowSize.y, windowWidth, windowHeight)
                , OnMainWindow, "Advanced Repeat Group");
            builder.ManageModalWindows();
        }

        private void OnMainWindow(int id)
        {
            builder.BuildUI(editWindowSize);
        }

        private void Calculate()
        {
            try
            {
                objectPool.DeactivateAll();
                int i = 0;
                foreach (PredictableBulletModel model in Task.GetPredictableBulletModels())
                {
                    PredictedBullet pred = objectPool.AllocateObject().PredictedBullet;
                    pred.BulletModel = model;
                    i++;
                    if (i > 10000) throw new Exception("10000+");
                }
            }
#if !UNITY_EDITOR
            catch (Exception e) 
            {
                Debug.LogError(e);
#else
            finally {
#endif
            }
        }
    }
}
