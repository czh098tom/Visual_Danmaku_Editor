using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Latticework.UnityEngine.Utilities;
using VisualDanmakuEditor.Models;
using VisualDanmakuEditor.Models.AdvancedRepeat;
using VisualDanmakuEditor.Models.AdvancedRepeat.Variables;
using VisualDanmakuEditor.Models.Objects;

namespace VisualDanmakuEditor
{
    public class BulletCalculator : Assignable<TaskModel>
    {
        private static Dictionary<BulletCalculator, TaskModel> calcTaskMapping = new Dictionary<BulletCalculator, TaskModel>();
        private static Dictionary<TaskModel, BulletCalculator> taskCalcMapping = new Dictionary<TaskModel, BulletCalculator>();

        public static BulletCalculator GetCalculatorFor(TaskModel model) => taskCalcMapping[model];

        private BulletObjectPool objectPool;

        private void Awake()
        {
            LuaSTGFunctionRegistry.Register();
            objectPool = GetComponent<BulletObjectPool>();
            CreateDefaultModel();
        }

        private void Start()
        {
            Calculate();
        }

        private void CreateDefaultModel()
        {
            TaskModel task = new TaskModel();
            AdvancedRepeatModel advr = new AdvancedRepeatModel
            {
                Times = "10"
            };
            advr.AddFirst(new LinearVariable() { VariableName = "ir", Begin = "0", End = "360", IsPrecisely = false });
            task.AddFirst(advr);
            SimpleBulletModel simpleBullet = new SimpleBulletModel
            {
                RotationExpression = "ir"
            };
            task.BulletModel = simpleBullet;
            base.AssignAndUpdateUI(task);
        }

        protected override void Assign(TaskModel model)
        {
            if (calcTaskMapping.ContainsKey(this)) calcTaskMapping.Remove(this);
            if (taskCalcMapping.ContainsKey(Model)) taskCalcMapping.Remove(Model);
            base.Assign(model);
            calcTaskMapping.Add(this, Model);
            taskCalcMapping.Add(Model, this);
        }

        public void Calculate()
        {
            try
            {
                objectPool.DeactivateAll();
                int i = 0;
                foreach (PredictableBulletModelBase model in Model.GetPredictableBulletModels())
                {
                    PredictedBullet pred = objectPool.AllocateObject().PredictedBullet;
                    pred.BulletModel = model;
                    i++;
                    if (i > 10000) throw new Exception("10000+");
                }
            }
#if !UNITY_EDITOR
            catch (Exception e) 
            {
                Debug.LogError(e);
#else
            finally
            {
#endif
            }
        }

        private void OnDestroy()
        {
            calcTaskMapping.Remove(this);
            taskCalcMapping.Remove(Model);
        }
    }
}
