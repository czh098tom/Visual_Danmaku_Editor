using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using UnityEngine;

using VisualDanmakuEditor.IMGUI;
using VisualDanmakuEditor.Porting;

using static VisualDanmakuEditor.IMGUI.IMGUIHelper;

namespace VisualDanmakuEditor
{
    public class MainUI : MonoBehaviour
    {
        PatternExporter patternExporter;

        private void Start()
        {
            StreamReader head = new StreamReader(Path.Combine(Application.streamingAssetsPath, "SharpHead.lstges"));
            StreamReader tail = new StreamReader(Path.Combine(Application.streamingAssetsPath, "SharpTail.lstges"));
            patternExporter = new PatternExporter(head, tail);
            head.Close();
            tail.Close();
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 100, elementHeight), "Export")) Export();
            TimeLine.Instance.MaxTime = TryConvertInt(LabelledTextField(new Rect(100, 0, 100, elementHeight), "frames"
                , TimeLine.Instance.MaxTime.ToString()), 400);
        }

        private void Export()
        {
            string fp = Path.Combine(Application.persistentDataPath, "_generated.lstges");
            FileStream fs = new FileStream(fp, FileMode.Truncate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            patternExporter.WriteSharpFile(FindObjectsOfType<AdvancedRepeatUI>().Select((a) => a.Task).ToArray(), 6, sw);
            sw.Close();
            fs.Close();
            Process.Start(new ProcessStartInfo()
            {
                FileName = Application.persistentDataPath,
                UseShellExecute = true
            });
        }
    }
}
