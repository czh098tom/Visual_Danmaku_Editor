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
                sw.WriteLine($"{level},{taskm[0]}");
                level++;
                foreach(AdvancedRepeatModel advr in tasks[i])
                {
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
                sw.WriteLine($"{level},{taskm[1]}");
                level = offset + 3;
                if (tasks[i].Count > 1)
                {
                    sw.WriteLine($"{level},{taskm[2]}");
                }
                level = offset + 2;
                if (tasks[i].Count > 0)
                {
                    sw.WriteLine($"{level},{taskm[3]}");
                }
            }
            sw.WriteLine(tail);
        }
    }
}
