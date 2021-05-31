using System.Collections;
using UnityEngine;

namespace HiryuTK.AsteroidsTopDownController
{
    /// <summary>
    /// We will use this base class instead an interface (IPoolable) for the poolable
    /// prefabs, as the inspector cannot reference prefabs (via drag and drop) 
    /// via its interface type and I want to avoid referencing a prefab by 
    /// GameObject type. The downside of doing this is that the prefab must 
    /// inherit from this base class instead of MonoBehaviour. 
    /// </summary>
    public abstract class PoolObject : MonoBehaviour
    {
        protected Pool pool;
        private bool isActive;

        /// <summary>
        /// Called when the object is first created in the pool
        /// </summary>
        /// <param name="pool"> Assign the pool that this object belongs to </param>
        public virtual void InitialSpawn(Pool pool)
        {
            this.pool = pool;
        }

        /// <summary>
        /// Called when the object is activated inside the world, either after it is 
        /// spawned or after reactivated from pool.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="r"></param>
        public virtual void Activation(Vector2 p, Quaternion r)
        {
            isActive = true;
            transform.position = p;
            transform.rotation = r;
        }

        protected virtual void Despawn()
        {
            if (isActive) //Prevent situations where object despawns multiple times.
            {
                pool.Despawn(this);
                isActive = false;
            }
        }
    }
}