using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using VisualDanmakuEditor.IMGUI;
using VisualDanmakuEditor.Models;

using static VisualDanmakuEditor.IMGUI.IMGUIHelper;

namespace VisualDanmakuEditor
{
    public class AdvancedRepeatUI : MonoBehaviour
    {
        [SerializeField]
        private GameObject bullet;

        private BulletObjectPool objectPool;

        private Rect editWindowSize = new Rect(100, 0, windowWidth, elementHeight * (elementCount + 3));
        private Vector2 scrollPosition = Vector2.zero;

        float height;

        bool isDirty = true;

        readonly TaskModel task = new TaskModel();

        private AdvancedRepeatUIBuilder builder;

        private void Awake()
        {
            LuaSTGFunctionRegistry.Register();
            builder = new AdvancedRepeatUIBuilder(task);
            objectPool = FindObjectOfType<BulletObjectPool>();
        }

        private void Update()
        {
            if (isDirty)
            {
                Calculate();
                isDirty = false;
            }
        }

        private void OnGUI()
        {
            height = builder.PredictHeight();
            editWindowSize = GUI.Window(0, new Rect(editWindowSize.x, editWindowSize.y, windowWidth, windowHeight)
                , OnMainWindow, "Advanced Repeat Group");
        }

        private void OnMainWindow(int id)
        {
            scrollPosition = GUI.BeginScrollView(new Rect(0, elementHeight, windowWidth, editWindowSize.height), scrollPosition,
                new Rect(0, 0, windowWidth, height));
            isDirty = builder.BuildUI(isDirty);
        }

        private void Calculate()
        {
            try
            {
                objectPool.DeactivateAll();
                int i = 0;
                foreach (PredictableBulletModel model in task.GetPredictableBulletModels())
                {
                    PredictedBullet pred = objectPool.AllocateObject().PredictedBullet;
                    pred.BulletModel = model;
                    i++;
                    if (i > 4000) throw new Exception("4000+");
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
