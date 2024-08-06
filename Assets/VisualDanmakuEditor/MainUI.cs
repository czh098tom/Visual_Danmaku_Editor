using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using UnityEngine;

using VisualDanmakuEditor.Porting;
using VisualDanmakuEditor.IO;

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
            if (GUI.Button(new Rect(200, 0, 100, elementHeight), "Save")) Save();
            if (GUI.Button(new Rect(300, 0, 100, elementHeight), "Load")) Load();
        }

        private void Export()
        {
            string fp = Path.Combine(Application.persistentDataPath, "_generated.lstges");
            FileStream fs = null;
            StreamWriter sw = null;
            try
            {
                fs = new FileStream(fp, FileMode.Create, FileAccess.Write);
                sw = new StreamWriter(fs);
                patternExporter.WriteSharpFile(FindObjectsOfType<BulletCalculator>().Select((a) => a.Model).ToArray()
                    , FindObjectOfType<ObjectUI>().Model, 6, sw);
            }
            finally
            {
                sw?.Close();
                fs?.Close();
            }
            Process.Start(new ProcessStartInfo()
            {
                FileName = Application.persistentDataPath,
                UseShellExecute = true
            });
        }

        private void Save()
        {
            var saveData = new SaveModel(FindObjectOfType<ObjectUI>().Model,
                FindObjectsOfType<BulletCalculator>().Select((a) => a.Model).Reverse());
            IOHelper.Save(saveData, Path.Combine(Application.persistentDataPath, "_saveData"));
        }

        private void Load()
        {
            var saveData = IOHelper.Load(Path.Combine(Application.persistentDataPath, "_saveData"));
            var objUI = FindObjectOfType<ObjectUI>(); 
            objUI.AssignAndUpdateUI(saveData.Object);
            objUI.LoadTasks(saveData.TaskModels.Select(t => t.ToTaskModel()));
        }
    }
}
