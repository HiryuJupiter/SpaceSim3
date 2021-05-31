using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HiryuTK.AsteroidsTopDownController
{
    /// <summary>
    /// Object pool
    /// </summary>
    public class Pool
    {
        //Fields
        private List<PoolObject> active = new List<PoolObject>();
        private List<PoolObject> inactive = new List<PoolObject>();
        private PoolObject prefab;
        private Transform parent;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="prefab"> The prefab that is to be associated with this prefab </param>
        /// <param name="parent"> The parent transform that all prefab will be spawned under </param>
        public Pool(PoolObject prefab, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;
        }

        /// <summary>
        /// Return the prefab either by spawning a new one or pulling from the pool.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public PoolObject Spawn(Vector3 p, Quaternion r)
        {
            PoolObject go;
            if (inactive.Count > 0)
            {
                //If object pool is not empty, then take an object from the pool and make it active
                go = inactive[0];
                go.gameObject.SetActive(true);
                inactive.RemoveAt(0);
                go.transform.position = p;
                go.transform.rotation = r;
            }
            else
            {
                //If object pool is empty, then spawn a new object.
                go = GameObject.Instantiate(prefab, p, r, parent);
                go.GetComponent<PoolObject>().InitialSpawn(this);
            }
            go.GetComponent<PoolObject>().Activation(p, r);
            active.Add(go);
            return go;
        }

        /// <summary>
        /// For returing a poolobject back to its pool.
        /// </summary>
        /// <param name="obj"></param>
        public void Despawn(PoolObject obj)
        {
            if (active.Contains(obj))
            {
                inactive.Add(obj);
                active.Remove(obj);
                obj.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Error, can't find object " + obj.name + "in pool. ");
            }
        }
    }
}