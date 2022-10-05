using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using VisualDanmakuEditor.Models;

namespace VisualDanmakuEditor
{
    public class PredictedBullet : MonoBehaviour
    {
        public PredictableBulletModelBase BulletModel { get; set; } = PredictableBulletModelBase.@default;

        private PredictedBulletVelocityBar arrow;

        SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            arrow = GetComponentInChildren<PredictedBulletVelocityBar>();
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
                BulletPrediction prediction = BulletModel.GetBulletPredictionAt(t);
                if (prediction.ContainsInvalidParameters()) return;
                transform.localPosition = new Vector3(prediction.X, prediction.Y);
                transform.rotation = Quaternion.Euler(0, 0, prediction.Rotation);
                spriteRenderer.sprite = BulletStyleRegistration.Instance.GetCachedSprite(prediction.Style, prediction.Color);
                arrow.gameObject.SetActive(TimeLine.Instance.ShowVelocity);
                if (TimeLine.Instance.ShowVelocity)
                {
                    BulletPrediction predictionDelta = BulletModel.GetBulletPredictionAt(t + 1);
                    float l = 0;
                    if (!predictionDelta.ContainsInvalidParameters())
                    {
                        Vector2 v = new Vector2(predictionDelta.X - prediction.X, predictionDelta.Y - prediction.Y);
                        l = v.magnitude;
                    }
                    arrow.Length = l;
                }
            }
            else
            {
                spriteRenderer.enabled = false;
                arrow.gameObject.SetActive(false);
            }
        }

        public void Hide()
        {
            spriteRenderer.enabled = false;
            arrow.gameObject.SetActive(false);
        }
    }
}