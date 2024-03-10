using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VisualDanmakuEditor.Models;
using VisualDanmakuEditor.Models.AdvancedRepeat;
using VisualDanmakuEditor.Models.Objects;

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

        public void WriteSharpFile(TaskModel[] tasks, ObjectModelBase boss, int offset, StreamWriter sw)
        {
            sw.WriteLine(head);
            sw.WriteLine($"{offset},{ExportingClassMapperBase.BindExportString(boss)[0]}");
            for (int i = 0; i < tasks.Length; i++)
            {
                int level = offset;
                string[] taskm = ExportingClassMapperBase.BindExportString(tasks[i]);
                string[] bulletm = ExportingClassMapperBase.BindExportString(tasks[i].BulletModel);
                sw.WriteLine($"{level},{taskm[0]}");
                level++;
                sw.WriteLine($"{level},{taskm[1]}");
                Stack<AdvancedRepeatModel> advrback = new Stack<AdvancedRepeatModel>();
                foreach(AdvancedRepeatModel advr in tasks[i])
                {
                    advrback.Push(advr);
                    string[] advrm = ExportingClassMapperBase.BindExportString(advr);
                    sw.WriteLine($"{level},{advrm[0]}");
                    level++;
                    sw.WriteLine($"{level},{advrm[1]}");
                    level++;
                    foreach(VariableModelBase var in advr)
                    {
                        sw.WriteLine($"{level},{ExportingClassMapperBase.BindExportString(var)[0]}");
                    }
                    level--;
                }
                if (tasks[i].BulletModel is CurveBulletModel cbm)
                {
                    var variables = string.Join(", ", cbm.GetVariables());
                    sw.WriteLine($"{level},{string.Format(bulletm[0], variables)}");
                    sw.WriteLine($"{level + 1},{bulletm[1]}");
                    sw.WriteLine($"{level + 2},{bulletm[2]}");
                    sw.WriteLine($"{level + 3},{bulletm[3]}");
                    foreach (var cp in cbm.TVExpression)
                    {
                        sw.WriteLine($"{level + 4},{ExportingClassMapperBase.BindExportString(cp)[0]}");
                    }
                    sw.WriteLine($"{level + 3},{bulletm[3]}");
                    foreach (var cp in cbm.TRExpression)
                    {
                        sw.WriteLine($"{level + 4},{ExportingClassMapperBase.BindExportString(cp)[0]}");
                    }
                }
                else
                {
                    sw.WriteLine($"{level},{bulletm[0]}");
                }
                while (advrback.Count > 0)
                {
                    AdvancedRepeatModel advr = advrback.Pop();
                    if (advr.Interval != "0")
                    {
                        string[] advrm = ExportingClassMapperBase.BindExportString(advr);
                        sw.WriteLine($"{level},{advrm[2]}");
                    }
                    level--;
                }
            }
            sw.WriteLine(tail);
        }
    }
}
