using System;
using UnityEngine;
using UnityEngine.Pool;

namespace OneWinter.ScriptObjPoolingFramework
{
    /// <summary>
    ///     Create your Pooled Object MonoBehaviour classes from this.
    /// </summary>
    public abstract class PooledObjectBase : MonoBehaviour
    {
        private IObjectPool<PooledObjectBase> objectPool;
        protected PooledObjectSetup ObjectSetup;

        /// <summary>
        ///     Called every time object returned to pool
        /// </summary>
        public virtual void BeforeDisable()
        {
        }

        /// <summary>
        ///     Called only once, when object is deleted from pool
        /// </summary>
        public virtual void BeforeDestroy()
        {
        }

        /// <summary>
        ///     Called only once, when object is first created in pool
        /// </summary>
        public void AssignObjectSetup(PooledObjectSetup newSetup, IObjectPool<PooledObjectBase> newPool)
        {
            ObjectSetup = newSetup;
            objectPool = newPool;
        }

        /// <summary>
        ///     Called only once, when object is first created in pool--after AssignObjectSetup()
        /// </summary>
        public virtual void InitializeObjectSetup()
        {
        }

        /// <summary>
        ///     Called every time object is taken from the pool
        /// </summary>
        public virtual void BeforeEnable()
        {
        }

        /// <summary>
        ///     Called to return object to pool
        /// </summary>
        public void ReleaseToPool()
        {
            // try/catch in case we try to recycle the same enemy twice
            try
            {
                objectPool.Release(this);
            }
            catch (Exception e)
            {
                /* ignored */
            }

            // in case we accidentally use this class outside of pooling
            if (objectPool != null) return;

            Destroy(gameObject);
        }

        // exec order: Awake() 1x => Assign/InitializeObjectSetup() 1x => BeforeEnable() eachX => GetPooledObject/set position eachX => Finalize() eachX => Start() 1x
    }

    /// <summary>
    ///     Base your Pooled Object Base Monobehaviour classes off of this
    /// </summary>
    /// <typeparam name="T">The PooledObjectSetup inheritor that will spawn this</typeparam>
    public class PooledObjectBase<T> : PooledObjectBase where T : PooledObjectSetup
    {
        public virtual T TypeObject => (T)ObjectSetup;
    }
}