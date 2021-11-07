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
using VisualDanmakuEditor.Models.Bullet;

namespace VisualDanmakuEditor
{
    public class BulletCalculator : MonoBehaviour
    {
        private static Dictionary<TaskModel, BulletObjectPool> modelPoolMapping = new Dictionary<TaskModel, BulletObjectPool>();

        private BulletObjectPool objectPool;
        public TaskModel Model { get; private set; }

        private void Awake()
        {
            LuaSTGFunctionRegistry.Register();
            objectPool = FindObjectOfType<BulletObjectPool>();
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
            Model = task;
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
    }
}
