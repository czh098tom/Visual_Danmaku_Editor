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

        private Rect editWindowSize = new Rect(0, 0, windowWidth, elementHeight * (elementCount + 3));
        private Vector2 scrollPosition = Vector2.zero;

        float height;

        bool isDirty = true;
        readonly HashSet<PredictedBullet> predictedBullets = new HashSet<PredictedBullet>();

        readonly TaskModel task = new TaskModel();

        private AdvancedRepeatUIBuilder builder;

        private void Awake()
        {
            builder = new AdvancedRepeatUIBuilder(task);
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
#if !UNITY_EDITOR
            try
            {
#endif
                foreach (PredictedBullet pred in predictedBullets) Destroy(pred.gameObject);
                predictedBullets.Clear();
                foreach (PredictableBulletModel model in task.GetPredictableBulletModels())
                {
                    GameObject go = Instantiate(bullet, transform.parent, true);
                    PredictedBullet pred = go.GetComponent<PredictedBullet>();
                    pred.BulletModel = model;
                    pred.Hide();
                    predictedBullets.Add(pred);
                }
#if !UNITY_EDITOR
            }
            catch (Exception e) 
            {
                Debug.LogError(e);
            }
#endif
        }
    }
}
