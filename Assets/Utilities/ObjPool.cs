using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEditor;

namespace Latticework.UnityEngine.Utilities
{
    /// <summary>
    /// Generic type of object pool with no parameter on creation. Must be used with <see cref="ObjPoolRefManager{T, U}"/>.
    /// </summary>
    /// <typeparam name="T">Must be setted to new type when inheriting from this.</typeparam>
    /// <typeparam name="U">Must be setted to new <see cref="ObjPoolRefManager{T, U}"/> when inheriting from this.</typeparam>
    public abstract class ObjPool<T, U> : MonoBehaviour
        where T : ObjPool<T, U>
        where U : ObjPoolRefManager<T, U>, new()
    {
        /// <summary>
        /// The default prefab when creating new objects.
        /// </summary>
        [Tooltip("The default prefab when creating new objects.")]
        public GameObject prefab;

        /// <summary>
        /// Store the reference in format of <see cref="ObjPoolRefManager{T, U}"/>.
        /// </summary>
        [SerializeField]
        private readonly LinkedList<U> objPoolRefs = new LinkedList<U>();

        /// <summary>
        /// Store the number of the objects activated in this pool.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Allocate a new object and returns it.
        /// </summary>
        /// <returns>An object typed <see cref="U"/>.</returns>
        public virtual U AllocateObject()
        {
            U newObj = GetDisabledOrNew();
            SetActive(newObj);
            Count++;
            return newObj;
        }

        /// <summary>
        /// Execute actions after allocating objects.
        /// </summary>
        /// <param name="newObj">An object typed <see cref="U"/>, must be not null.</param>
        protected void SetActive(U newObj)
        {
            newObj.ClonePrefabIfNull(prefab);
            newObj.Active();
            newObj.Instance.transform.SetParent(transform);
        }

        /// <summary>
        /// Find unused objects in the pool, otherwise create one and add to pool.
        /// </summary>
        /// <returns>A new object in pool.</returns>
        protected U GetDisabledOrNew()
        {
            U newObj = null;
            foreach (U u in objPoolRefs)
            {
                if (!u.IsActive)
                {
                    newObj = u;
                    break;
                }
            }
            if (newObj == null)
            {
                newObj = new U();
                objPoolRefs.AddLast(newObj);
            }
            return newObj;
        }

        /// <summary>
        /// Enumerate all actived <see cref="ObjPoolRefManager{T, U}"/>.
        /// </summary>
        /// <returns>A <see cref="IEnumerable{U}"/>.</returns>
        public IEnumerable<U> GetAllActive()
        {
            foreach (U u in objPoolRefs)
            {
                if (u.IsActive)
                {
                    yield return u;
                }
            }
        }

        /// <summary>
        /// Deactive all objects in this pool.
        /// </summary>
        public void DeactivateAll()
        {
            foreach (U u in GetAllActive())
            {
                u.Disable();
            }
            Count = 0;
        }

        public U AllocateObjectAt(int id)
        {
            id = GetIDByActivedID(id);
            U newObj = GetDisabledOrNew();
            SetActive(newObj);
            newObj.Instance.transform.SetSiblingIndex(id);

            int i = 0;
            LinkedListNode<U> next = null;
            for (LinkedListNode<U> curr = objPoolRefs.First; curr != null; curr = curr.Next)
            {
                if (i == id) next = curr;
                i++;
            }

            if (next != null)
            {
                objPoolRefs.Remove(newObj);
                objPoolRefs.AddBefore(next, new LinkedListNode<U>(newObj));
            }

            Count++;
            return newObj;
        }

        public U DeactivateObjectAt(int id)
        {
            id = GetIDByActivedID(id);

            int i = 0;
            for (LinkedListNode<U> curr = objPoolRefs.First; curr != null; curr = curr.Next)
            {
                if (i == id)
                {
                    curr.Value.Disable();
                    return curr.Value;
                }
                i++;
            }

            Count--;
            throw new IndexOutOfRangeException();
        }

        private int GetIDByActivedID(int id)
        {
            int i = 0, iid = 0;
            foreach (U u in objPoolRefs)
            {
                if (u.IsActive)
                {
                    if (iid == id)
                    {
                        return i;
                    }
                    iid++;
                }
                i++;
            }
            return i - 1;
        }
    }
}
