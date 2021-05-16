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
            float currOffset = 0;
            float currIndention = 10;
            float currWidth = windowWidth - 20;

            HashSet<AdvancedRepeatModel> repToBeDestroyed = new HashSet<AdvancedRepeatModel>();
            Dictionary<VariableModelBase, AdvancedRepeatModel> varToBeDestroyed = new Dictionary<VariableModelBase, AdvancedRepeatModel>();

            foreach (AdvancedRepeatModel rep in task)
            {
                rep.Times = TryConvertInt(LabelledTextField(new Rect(currIndention, currOffset, currWidth - shortLabelWidth, elementHeight)
                    , "Times", rep.Times.ToString()));
                if (GUI.Button(new Rect(currIndention + currWidth - shortLabelWidth, currOffset, shortLabelWidth, elementHeight), "-"))
                {
                    repToBeDestroyed.Add(rep);
                }
                currOffset += elementHeight;

                currIndention += levelIndention;
                currWidth -= levelIndention;

                foreach (VariableModelBase var in rep)
                {
                    var.VariableName = LabelledTextField(new Rect(currIndention, currOffset, currWidth - shortLabelWidth, elementHeight)
                        , "Name", var.VariableName);
                    if (GUI.Button(new Rect(currIndention + currWidth - shortLabelWidth, currOffset, shortLabelWidth, elementHeight), "-"))
                    {
                        varToBeDestroyed.Add(var, rep);
                    }
                    currOffset += elementHeight;
                    if (var is LinearVariable lv)
                    {
                        lv.Begin = LabelledTextField(new Rect(currIndention, currOffset, currWidth, elementHeight)
                            , "Begin", lv.Begin);
                        currOffset += elementHeight;
                        lv.End = LabelledTextField(new Rect(currIndention, currOffset, currWidth, elementHeight)
                            , "End", lv.End);
                        currOffset += elementHeight;
                        lv.IsPrecisely = GUI.Toggle(new Rect(currIndention, currOffset, currWidth, elementHeight)
                            , lv.IsPrecisely, "Is Precisely");
                        currOffset += elementHeight;
                        currOffset += elementHeight * 0.5f;
                    }
                }
                if (GUI.Button(new Rect(currIndention, currOffset, currWidth, elementHeight), "+ Variable"))
                {
                    rep.AddLast(new LinearVariable());
                }
                currOffset += elementHeight;
            }

            if (GUI.Button(new Rect(currIndention, currOffset, currWidth, elementHeight), "+ Repeat"))
            {
                task.AddLast(new AdvancedRepeatModel());
            }
            currOffset += elementHeight;

            currIndention = 10;
            currWidth = windowWidth - 20;
            task.XExpression = LabelledTextField(new Rect(currIndention, currOffset, currWidth, elementHeight)
                , "X", task.XExpression);
            currOffset += elementHeight;
            task.YExpression = LabelledTextField(new Rect(currIndention, currOffset, currWidth, elementHeight)
                , "Y", task.YExpression);
            currOffset += elementHeight;
            task.RotationExpression = LabelledTextField(new Rect(currIndention, currOffset, currWidth, elementHeight)
                , "Rotation", task.RotationExpression);
            currOffset += elementHeight;
            task.VelocityExpression = LabelledTextField(new Rect(currIndention, currOffset, currWidth, elementHeight)
                , "Velocity", task.VelocityExpression);
            currOffset += elementHeight;
            task.Interval = TryConvertInt(LabelledTextField(new Rect(currIndention, currOffset, currWidth, elementHeight)
                , "Interval", task.Interval.ToString()));
            currOffset += elementHeight;

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
    }
}
