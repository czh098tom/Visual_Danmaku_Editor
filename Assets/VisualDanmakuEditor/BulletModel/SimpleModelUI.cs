﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using Latticework.UnityEngine.UI;
using VisualDanmakuEditor.Models;
using VisualDanmakuEditor.Models.Objects;

namespace VisualDanmakuEditor.BulletModel
{
    public class SimpleModelUI : BulletModelUI<SimpleBulletModel>
    {
        [SerializeField]
        Image bulletExample;
        [SerializeField]
        Dropdown color;
        [SerializeField]
        Dropdown style;
        [SerializeField]
        LabelledInput x;
        [SerializeField]
        LabelledInput y;
        [SerializeField]
        LabelledInput rotation;
        [SerializeField]
        LabelledInput velocity;

        protected override void Assign(SimpleBulletModel model)
        {
            base.Assign(model);

            BulletStyleRegistration reg = BulletStyleRegistration.Instance;
            bulletExample.sprite = reg.GetCachedSprite(model.Style, model.Color);
            color.options = reg.colorNames
                .Select(s => new Dropdown.OptionData() { text = s, image = reg.GetCachedSprite(model.Style, s) })
                .ToList();
            style.options = reg.styleNames
                .Select(s => new Dropdown.OptionData() { text = s, image = reg.GetCachedSprite(s, model.Color) })
                .ToList();
        }

        public override void UpdateUI()
        {
            base.UpdateUI();
            BulletStyleRegistration reg = BulletStyleRegistration.Instance;
            color.value = reg.GetColorIdOfName(Model.Color);
            style.value = reg.GetStyleIdOfName(Model.Style);
            x.Value = Model.XExpression;
            y.Value = Model.YExpression;
            velocity.Value = Model.VelocityExpression;
            rotation.Value = Model.RotationExpression;
        }

        protected override void Start()
        {
            base.Start();
            color.onValueChanged.AddListener(SetColor);
            style.onValueChanged.AddListener(SetStyle);
            x.InputComponent.onValueChanged.AddListener(s => { Model.XExpression = s; Calculate(); });
            y.InputComponent.onValueChanged.AddListener(s => { Model.YExpression = s; Calculate(); });
            velocity.InputComponent.onValueChanged.AddListener(s => { Model.VelocityExpression = s; Calculate(); });
            rotation.InputComponent.onValueChanged.AddListener(s => { Model.RotationExpression = s; Calculate(); });
        }

        public void SetColor(int id)
        {
            BulletStyleRegistration reg = BulletStyleRegistration.Instance;
            style.options = reg.styleNames
                .Select(s => new Dropdown.OptionData() { text = s, image = reg.GetCachedSprite(s, reg.GetColorName(color.value)) })
                .ToList();
            bulletExample.sprite = reg.GetCachedSprite(style.value, color.value);
            Model.Color = reg.GetColorName(color.value);
            Calculate();
        }

        public void SetStyle(int id)
        {
            BulletStyleRegistration reg = BulletStyleRegistration.Instance;
            color.options = reg.colorNames
                .Select(s => new Dropdown.OptionData() { text = s, image = reg.GetCachedSprite(reg.GetStyleName(style.value), s) })
                .ToList();
            bulletExample.sprite = reg.GetCachedSprite(style.value, color.value);
            Model.Style = reg.GetStyleName(style.value);
            bulletExample.SetNativeSize();
            Calculate();
        }
    }
}
