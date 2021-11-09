using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VisualDanmakuEditor;
using VisualDanmakuEditor.Models;
using VisualDanmakuEditor.Models.AdvancedRepeat;
using VisualDanmakuEditor.Models.AdvancedRepeat.Variables;
using VisualDanmakuEditor.Models.Objects;

public class AdvRepeatTest : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;

    void Start()
    {
        TaskModel task = new TaskModel()
        {
            BulletModel = new SimpleBulletModel()
            {
                XExpression = "0",
                YExpression = "120",
                RotationExpression = "0",
                VelocityExpression = "3"
            }
        };

        foreach (PredictableBulletModelBase model in task.GetPredictableBulletModels())
        {
            //Debug.Log(model);
            GameObject go = Instantiate(bullet, transform.parent, true);
            PredictedBullet pred = go.GetComponent<PredictedBullet>();
            pred.BulletModel = model;
            pred.Hide();
        }
    }
}
