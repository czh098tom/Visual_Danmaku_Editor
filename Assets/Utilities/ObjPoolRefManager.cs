using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Latticework.UnityEngine.Utilities
{
    /// <summary>
    /// Generic type of object pool references with no parameter on creation. Must be used with <see cref="ObjPool{T, U}"/>.
    /// </summary>
    /// <typeparam name="T">Must be setted to new <see cref="ObjPool{T, U}"/> when inheriting from this.</typeparam>
    /// <typeparam name="U">Must be setted to new type when inheriting from this.</typeparam>
    public abstract class ObjPoolRefManager<T, U>
        where T : ObjPool<T, U>
        where U : ObjPoolRefManager<T, U>, new()
    {
        /// <summary>
        /// Store the <see cref="GameObject"/> referenced by this object.
        /// </summary>
        public GameObject Instance { get; private set; } = null;
        /// <summary>
        /// Store whether the object in pool is activated.
        /// </summary>
        public bool IsActive { get; private set; } = false;

        //public event Action<U> OnDisableInPool;

        /// <summary>
        /// Disable the instance in pool.
        /// </summary>
        public void Disable()
        {
            //OnDisableInPool?.Invoke(this as U);
            IsActive = false;
            Sleep();
        }

        /// <summary>
        /// Activate the instance in pool.
        /// </summary>
        public void Active()
        {
            IsActive = true;
            Awake();
        }

        /// <summary>
        /// Clone prefab from a given source and assign this to <see cref="Instance"/> if Instance is not null.
        /// </summary>
        /// <param name="source">An object typed <see cref="GameObject"/>, indicating the prefab.</param>
        public void ClonePrefabIfNull(GameObject source)
        {
            Instance = Instance == null ? global::UnityEngine.Object.Instantiate(source) : Instance;
        }

        /// <summary>
        /// Action after the object is indicated as disabled.
        /// </summary>
        protected virtual void Sleep()
        {
            if (Instance != null) Instance.SetActive(false);
        }

        /// <summary>
        /// Action after the object is indicated as actived.
        /// </summary>
        protected virtual void Awake()
        {
            if (Instance != null) Instance.SetActive(true);
        }
    }
}
