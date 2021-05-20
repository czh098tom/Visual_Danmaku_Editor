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

        public AdvancedRepeatUIBuilder(TaskModel task)
        {
            this.task = task;
        }

        public float PredictHeight()
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
            return height;
        }

        public bool BuildUI(bool isDirty)
        {
            currOffset = 0;
            currIndention = 10;
            currWidth = windowWidth - 20;

            HashSet<AdvancedRepeatModel> repToBeDestroyed = new HashSet<AdvancedRepeatModel>();
            Dictionary<VariableModelBase, AdvancedRepeatModel> varToBeDestroyed = new Dictionary<VariableModelBase, AdvancedRepeatModel>();

            foreach (AdvancedRepeatModel rep in task)
            {
                BuildLabelledItem("Times", GUI.TextField, (v) => rep.Times = TryConvertInt(v), () => rep.Times.ToString()
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

            return isDirty || GUI.changed;
        }

        private void BuildLabelledItem<TAccepts>(string label, Func<Rect, TAccepts, TAccepts> imgui
            , Action<TAccepts> setter, Func<TAccepts> getter, float spacing = 0, bool finished = true)
        {
            GUI.Label(new Rect(currIndention, currOffset, labelWidth, elementHeight), label);
            setter(imgui(new Rect(currIndention + labelWidth, currOffset, currWidth - labelWidth - spacing, elementHeight), getter()));
            if (finished) MakeOffset();
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
