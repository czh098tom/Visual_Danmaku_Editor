using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VisualDanmakuEditor.Models;

namespace VisualDanmakuEditor
{
    public class PredictedBullet : MonoBehaviour
    {
        public PredictableBulletModel BulletModel { get; set; } = PredictableBulletModel.@default;

        SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            ShowPrediction();
        }

        private void Update()
        {
            ShowPrediction();
        }

        private void ShowPrediction()
        {
            int t = TimeLine.Instance.CurrentTime;
            if (t >= BulletModel.LifeTimeBegin && t < BulletModel.LifeTimeEnd)
            {
                spriteRenderer.enabled = true;
                BulletPrediction prediction = BulletModel.GetPredictionAt(t);
                if (prediction.ContainsInvalidParameters()) return;
                transform.localPosition = new Vector3(prediction.X, prediction.Y);
                transform.rotation = Quaternion.Euler(0, 0, prediction.Rotation);
            }
            else
            {
                spriteRenderer.enabled = false;
            }
        }

        public void Hide()
        {
            spriteRenderer.enabled = false;
        }
    }
}