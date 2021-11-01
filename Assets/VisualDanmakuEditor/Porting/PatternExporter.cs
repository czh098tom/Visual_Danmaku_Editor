using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualDanmakuEditor.Models;
using VisualDanmakuEditor.Models.AdvancedRepeat;

namespace VisualDanmakuEditor.Porting
{
    public class PatternExporter
    {
        readonly string head;
        readonly string tail;

        public PatternExporter(StreamReader head, StreamReader tail)
        {
            this.head = head.ReadToEnd();
            this.tail = tail.ReadToEnd();
        }

        public void WriteSharpFile(TaskModel[] tasks, int offset, StreamWriter sw)
        {
            sw.WriteLine(head);
            for (int i = 0; i < tasks.Length; i++)
            {
                int level = offset;
                string[] taskm = ExportingClassMapper.BindExportString(tasks[i]);
                string[] bulletm = ExportingClassMapper.BindExportString(tasks[i].BulletModel);
                sw.WriteLine($"{level},{taskm[0]}");
                level++;
                Stack<AdvancedRepeatModel> advrback = new Stack<AdvancedRepeatModel>();
                foreach(AdvancedRepeatModel advr in tasks[i])
                {
                    advrback.Push(advr);
                    string[] advrm = ExportingClassMapper.BindExportString(advr);
                    sw.WriteLine($"{level},{advrm[0]}");
                    level++;
                    sw.WriteLine($"{level},{advrm[1]}");
                    level++;
                    foreach(VariableModelBase var in advr)
                    {
                        sw.WriteLine($"{level},{ExportingClassMapper.BindExportString(var)[0]}");
                    }
                    level--;
                }
                sw.WriteLine($"{level},{bulletm[0]}");
                while (advrback.Count > 0)
                {
                    AdvancedRepeatModel advr = advrback.Pop();
                    if (advr.Interval != "0")
                    {
                        string[] advrm = ExportingClassMapper.BindExportString(advr);
                        sw.WriteLine($"{level},{advrm[2]}");
                    }
                    level--;
                }
            }
            sw.WriteLine(tail);
        }
    }
}
