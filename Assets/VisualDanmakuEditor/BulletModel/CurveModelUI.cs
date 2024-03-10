using Latticework.UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

using VisualDanmakuEditor.Models.Curve;
using VisualDanmakuEditor.Models.Objects;

namespace VisualDanmakuEditor.BulletModel
{
    public class CurveModelUI : BulletModelUI<CurveBulletModel>
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
        VerticalLayoutGroup velocityContainer;
        [SerializeField]
        VerticalLayoutGroup rotationContainer;

        [SerializeField]
        Button addVelocityPoint;
        [SerializeField]
        Button addRotationPoint;

        readonly List<CurvePointUI> velocityPoints = new List<CurvePointUI>();
        readonly List<CurvePointUI> rotationPoints = new List<CurvePointUI>();
        public VerticalLayoutGroup VelocityContainer { get => velocityContainer; }
        public VerticalLayoutGroup RotationContainer { get => rotationContainer; }

        protected override void Assign(CurveBulletModel model)
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
            foreach (var cp in model.TVExpression)
            {
                AddVelocityPoint(cp);
            }
            foreach (var cp in model.TRExpression)
            {
                AddRotationPoint(cp);
            }
        }

        public override void UpdateUI()
        {
            base.UpdateUI();
            BulletStyleRegistration reg = BulletStyleRegistration.Instance;
            color.value = reg.GetColorIdOfName(Model.Color);
            style.value = reg.GetStyleIdOfName(Model.Style);
            x.Value = Model.XExpression;
            y.Value = Model.YExpression;
            foreach (var vui in velocityPoints)
            {
                vui.UpdateUI();
            }
            foreach (var rui in rotationPoints)
            {
                rui.UpdateUI();
            }
        }

        protected override void Start()
        {
            base.Start();
            color.onValueChanged.AddListener(SetColor);
            style.onValueChanged.AddListener(SetStyle);
            x.InputComponent.onValueChanged.AddListener(s => { Model.XExpression = s; Calculate(); });
            y.InputComponent.onValueChanged.AddListener(s => { Model.YExpression = s; Calculate(); });
            addVelocityPoint.onClick.AddListener(() => { AddNewVelocityPoint(); Calculate(); });
            addRotationPoint.onClick.AddListener(() => { AddNewRotationPoint(); Calculate(); });
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

        public void AddNewVelocityPoint()
        {
            CurvePoint p = new CurvePoint();
            Model.TVExpression.Add(p);
            AddVelocityPoint(p);
        }

        public void AddNewRotationPoint()
        {
            CurvePoint p = new CurvePoint();
            Model.TRExpression.Add(p);
            AddRotationPoint(p);
        }

        public void AddVelocityPoint(CurvePoint p)
        {
            AddCurvePoint(p, velocityContainer, velocityPoints, Model.TVExpression);
        }

        private void AddRotationPoint(CurvePoint p)
        {
            AddCurvePoint(p, rotationContainer, rotationPoints, Model.TRExpression);
        }

        private void AddCurvePoint(CurvePoint p, 
            VerticalLayoutGroup container, 
            List<CurvePointUI> uiList,
            List<CurvePoint> modelList
            )
        {
            CurvePointUI ui = (CurvePointUI)Instantiate(UIPrototypes.Instance.CurvePointUI)
                .GetComponent(typeof(CurvePointUI));
            ui.Calculate = Calculate;
            if (uiList.Count == 0)
            {
                ui.SetAsFirst();
            }
            RectTransform rect = ui.GetComponent<RectTransform>();
            rect.SetParent(container.transform, false);
            rect.SetAsLastSibling();
            uiList.Add(ui);
            ui.AssignAndUpdateUI(p);
            ui.Remove.onClick.AddListener(() => { RemovePoint(ui, uiList, modelList); Calculate(); });
        }

        private void RemovePoint(CurvePointUI ui,
            List<CurvePointUI> uiList,
            List<CurvePoint> modelList)
        {
            Destroy(ui.gameObject);
            uiList.Remove(ui);
            modelList.Remove(ui.Model);
        }
    }
}
