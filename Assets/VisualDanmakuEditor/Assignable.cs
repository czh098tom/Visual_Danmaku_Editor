using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VisualDanmakuEditor
{
    public abstract class Assignable<T> : MonoBehaviour
    {
        public T Model { get; private set; }

        public void AssignAndUpdateUI(T model)
        {
            this.Model = model;
            Assign(model);
            UpdateUI();
        }

        protected virtual void Assign(T model) { }

        public virtual void UpdateUI() { }
    }
}
