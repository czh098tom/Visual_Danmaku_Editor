using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VisualDanmakuEditor.IMGUI
{
    public class IMGUIHelper
    {
        public const int advancedRepeatWindowID = 0;
        public const int bulletColorWindowID = 1;
        public const int bulletStyleWindowID = 2;

        public const float windowWidth = 400f;
        public const float windowHeight = 400f;
        public const float elementHeight = 20f;
        public const float levelIndention = 20f;

        public const int elementCount = 8;

        public const float labelWidth = 50f;
        public const float shortLabelWidth = 20f;

        public static string LabelledTextField(Rect position, string label, string text, bool useShort = false)
        {
            float currLabelWidth = useShort ? shortLabelWidth : labelWidth;
            GUI.Label(new Rect(position.x, position.y, currLabelWidth, position.height), label);
            return GUI.TextField(new Rect(position.x + currLabelWidth, position.y, position.width - currLabelWidth, position.height), text);
        }

        public static int TryConvertInt(string str, int @default = default)
        {
            if (int.TryParse(str, out int v))
            {
                return v;
            }
            return @default;
        }
    }
}
