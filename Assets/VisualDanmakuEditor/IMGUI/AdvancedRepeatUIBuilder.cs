using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using VisualDanmakuEditor.Models;
using VisualDanmakuEditor.Models.AdvancedRepeat;
using VisualDanmakuEditor.Models.AdvancedRepeat.Variables;

using static VisualDanmakuEditor.IMGUI.IMGUIHelper;

namespace VisualDanmakuEditor.IMGUI
{
    public class AdvancedRepeatUIBuilder
    {
        TaskModel task;

        float height;

        float currOffset = 0;
        float currIndention = 10;
        float currWidth = windowWidth - 20;

        private Rect subWindowPosition;

        private Vector2 mainScrollPosition = Vector2.zero;
        private Vector2 subScrollPosition = Vector2.zero;

        private float currentOffsetWhenClicking;

        private bool subStyleWindowShown = false;
        private bool subColorWindowShown = false;

        public bool IsDirty { get; set; } = true;

        public AdvancedRepeatUIBuilder(TaskModel task)
        {
            this.task = task;
        }

        public void PredictHeight()
        {
            float nElements = 0;
            foreach (AdvancedRepeatModel rep in task)
            {
                //button to add var, adv repeat info, vars(each 3 && 0.5 indention)
                nElements += 2 + rep.Count * 4.5f;
            }
            //window header, elemCount, button to add adv repeat
            nElements += 2 + elementCount;
            height = nElements * elementHeight;
        }

        public void ManageModalWindows()
        {
            if (subStyleWindowShown)
            {
                subWindowPosition = GUI.ModalWindow(bulletStyleWindowID, subWindowPosition, OnBulletStyleWindow, "Select Bullet Style");
            }
            if (subColorWindowShown)
            {
                subWindowPosition = GUI.ModalWindow(bulletColorWindowID, subWindowPosition, OnBulletColorWindow, "Select Bullet Color");
            }
        }

        public void BuildUI(Rect parentWindow)
        {
            currOffset = 0;
            currIndention = 10;
            currWidth = windowWidth - 20;

            HashSet<AdvancedRepeatModel> repToBeDestroyed = new HashSet<AdvancedRepeatModel>();
            Dictionary<VariableModelBase, AdvancedRepeatModel> varToBeDestroyed = new Dictionary<VariableModelBase, AdvancedRepeatModel>();

            mainScrollPosition = GUI.BeginScrollView(new Rect(0, elementHeight, windowWidth, windowHeight), mainScrollPosition,
                new Rect(0, 0, windowWidth, height));
            foreach (AdvancedRepeatModel rep in task)
            {
                BuildLabelledItem("Times", GUI.TextField, (v) => rep.Times = v, () => rep.Times
                    , shortLabelWidth, false);
                if (GUI.Button(new Rect(currIndention + currWidth - shortLabelWidth, currOffset, shortLabelWidth, elementHeight), "-"))
                {
                    repToBeDestroyed.Add(rep);
                }
                MakeOffset();
                MakeIndention();

                foreach (VariableModelBase var in rep)
                {
                    BuildLabelledItem("Name", GUI.TextField, (v) => var.VariableName = v, () => var.VariableName, shortLabelWidth, false);
                    if (GUI.Button(new Rect(currIndention + currWidth - shortLabelWidth, currOffset, shortLabelWidth, elementHeight), "-"))
                    {
                        varToBeDestroyed.Add(var, rep);
                    }
                    MakeOffset();
                    if (var is LinearVariable lv)
                    {
                        BuildLabelledItem("Begin", GUI.TextField, (v) => lv.Begin = v, () => lv.Begin);
                        BuildLabelledItem("End", GUI.TextField, (v) => lv.End = v, () => lv.End);
                        BuildLabelledItem("", (r, b) => GUI.Toggle(r, b, "Include Endpoint"), (v) => lv.IsPrecisely = v, () => lv.IsPrecisely);
                        currOffset += elementHeight * 0.5f;
                    }
                }
                if (GUI.Button(new Rect(currIndention, currOffset, currWidth, elementHeight), "+ Variable"))
                {
                    rep.AddLast(new LinearVariable());
                }
                MakeOffset();
            }

            if (GUI.Button(new Rect(currIndention, currOffset, currWidth, elementHeight), "+ Repeat"))
            {
                task.AddLast(new AdvancedRepeatModel());
            }
            MakeOffset();

            ResetIndention();
            BuildLabelledItem("X", GUI.TextField, (v) => task.XExpression = v, () => task.XExpression);
            BuildLabelledItem("Y", GUI.TextField, (v) => task.YExpression = v, () => task.YExpression);
            BuildLabelledItem("Rotation", GUI.TextField, (v) => task.RotationExpression = v, () => task.RotationExpression);
            BuildLabelledItem("Velocity", GUI.TextField, (v) => task.VelocityExpression = v, () => task.VelocityExpression);
            BuildLabelledItem("Interval", GUI.TextField, (v) => task.Interval = TryConvertInt(v), () => task.Interval.ToString());
            BuildLabelledItem("Interval2", GUI.TextField, (v) => task.Interval2 = TryConvertInt(v), () => task.Interval2.ToString());

            if (GUI.Button(new Rect(currIndention, currOffset, currWidth, elementHeight), task.Style))
            {
                subStyleWindowShown = true;
                PredictPositionForModalWindow(parentWindow);
                currentOffsetWhenClicking = currOffset;
            }
            MakeOffset();
            if (GUI.Button(new Rect(currIndention, currOffset, currWidth, elementHeight), task.Color))
            {
                subColorWindowShown = true;
                PredictPositionForModalWindow(parentWindow);
                currentOffsetWhenClicking = currOffset;
            }

            GUI.EndScrollView();

            GUI.DragWindow(Screen.safeArea);

            foreach (KeyValuePair<VariableModelBase, AdvancedRepeatModel> tup in varToBeDestroyed)
            {
                tup.Value.Remove(tup.Key);
            }

            foreach (AdvancedRepeatModel rep in repToBeDestroyed)
            {
                task.Remove(rep);
            }

            IsDirty = IsDirty || GUI.changed;
        }

        private void OnBulletStyleWindow(int id)
        {
            string[] styleNames = BulletStyleRegistration.Instance.styleNames;
            float height = ((styleNames.Length + 3) / 4) * windowWidth / 4;
            Texture[] stylePic = new Texture[styleNames.Length];
            for (int i = 0; i < stylePic.Length; i++)
            {
                stylePic[i] = BulletStyleRegistration.Instance.GetCachedTexture(styleNames[i], task.Color);
            }

            if (GUI.Button(new Rect(0, elementHeight, windowWidth, elementHeight), "Confirm")) subStyleWindowShown = false;
            subScrollPosition = GUI.BeginScrollView(new Rect(0, 2 * elementHeight, windowWidth, windowWidth), subScrollPosition
                , new Rect(0, 0, windowWidth, height));
            task.Style = BulletStyleRegistration.Instance.GetStyleName(GUI.SelectionGrid(new Rect(0, 0, windowWidth, height)
                , BulletStyleRegistration.Instance.GetStyleIdOfName(task.Style), stylePic, 4));
            GUI.EndScrollView();

            GUI.DragWindow(Screen.safeArea);

            IsDirty = IsDirty || GUI.changed;
        }

        private void OnBulletColorWindow(int id)
        {
            string[] colorNames = BulletStyleRegistration.Instance.colorNames;
            float height = ((colorNames.Length + 3) / 4) * windowWidth / 4;
            Texture[] colorPic = new Texture[colorNames.Length];
            for (int i = 0; i < colorPic.Length; i++)
            {
                colorPic[i] = BulletStyleRegistration.Instance.GetCachedTexture(task.Style, colorNames[i]);
            }

            if (GUI.Button(new Rect(0, elementHeight, windowWidth, elementHeight), "Confirm")) subColorWindowShown = false;
            subScrollPosition = GUI.BeginScrollView(new Rect(0, 2 * elementHeight, windowWidth, windowWidth), subScrollPosition
                , new Rect(0, 0, windowWidth, height));
            task.Color = BulletStyleRegistration.Instance.GetColorName(GUI.SelectionGrid(new Rect(0, 0, windowWidth, height)
                , BulletStyleRegistration.Instance.GetColorIdOfName(task.Color), colorPic, 4));
            GUI.EndScrollView();

            GUI.DragWindow(Screen.safeArea);

            IsDirty = IsDirty || GUI.changed;
        }

        private void BuildLabelledItem<TAccepts>(string label, Func<Rect, TAccepts, TAccepts> imgui
            , Action<TAccepts> setter, Func<TAccepts> getter, float spacing = 0, bool finished = true)
        {
            GUI.Label(new Rect(currIndention, currOffset, labelWidth, elementHeight), label);
            setter(imgui(new Rect(currIndention + labelWidth, currOffset, currWidth - labelWidth - spacing, elementHeight), getter()));
            if (finished) MakeOffset();
        }

        private void PredictPositionForModalWindow(Rect parentWindow)
        {
            if (parentWindow.xMax < Screen.safeArea.xMax)
            {
                subWindowPosition = new Rect(parentWindow.x + parentWindow.width
                    , parentWindow.y + currentOffsetWhenClicking - mainScrollPosition.y
                    , windowWidth, windowWidth + 2 * elementHeight);
            }
            else
            {
                subWindowPosition = new Rect(parentWindow.x - windowWidth + 2 * elementHeight
                    , parentWindow.y + currentOffsetWhenClicking - mainScrollPosition.y
                    , windowWidth, windowWidth + 2 * elementHeight);
            }
        }

        private void MakeOffset()
        {
            currOffset += elementHeight;
        }

        private void ResetIndention()
        {
            currIndention = 10;
            currWidth = windowWidth - 20;
        }

        private void MakeIndention()
        {
            currIndention += levelIndention;
            currWidth -= levelIndention;
        }
    }
}
