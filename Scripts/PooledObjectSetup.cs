using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace OneWinter.ScriptObjPoolingFramework
{
    /// <summary>
    ///     Abstract base class for the Pooled Object Setup Scriptable Objects.
    /// </summary>
    public abstract class PooledObjectSetup : ScriptableObject
    {
        [SerializeField] private PooledObjectBase basePrefab;
        [SerializeField] private List<GameObject> modelPrefabs;
        private GameObject masterParent;
        private ObjectPool<PooledObjectBase> objectPool;
        private Transform poolOrganizer;

        public int CountActive => objectPool.CountActive;
        public int CountTotal => objectPool.CountAll;

        private void OnEnable()
        {
            // create a new pool
            objectPool = new ObjectPool<PooledObjectBase>(CreatePooledObject, OnTakeObjectFromPool,
                OnObjectReturnedToPool, OnDestroyObject);
        }

        private void OnTakeObjectFromPool(PooledObjectBase pooledObject)
        {
            if (!pooledObject) return; // sometimes the pool destroys an object before we can grab it... 

            pooledObject.BeforeEnable();
            pooledObject.gameObject.SetActive(true);

            // then OnEnable() runs
        }

        private void OnObjectReturnedToPool(PooledObjectBase pooledObject)
        {
            if (!pooledObject) return; // sometimes the pool destroys an object before we can return it...

            pooledObject.BeforeDisable();

            // reset the parent transform (in case we parented to a non-pool object that may be destroyed)
            poolOrganizer.transform.SetParent(poolOrganizer, true);
            pooledObject.gameObject.SetActive(false);

            // then OnDisable() runs
        }

        private void OnDestroyObject(PooledObjectBase pooledObject)
        {
            if (!pooledObject) return; // sometimes the pool destroys an object before we can return it...

            pooledObject.BeforeDestroy();
            Destroy(pooledObject.gameObject);

            // then OnDestroy() runs
        }

        /// <summary>
        ///     Setup empty GameObjects in the scene to organize our Pooled Objects
        /// </summary>
        private void CheckPoolOrganizer()
        {
            if (poolOrganizer) return;

            var newPoolOrganizer = new GameObject("Pool: " + name);
            poolOrganizer = newPoolOrganizer.transform;

            if (!masterParent) masterParent = GameObject.Find("Pools");
            if (!masterParent) masterParent = new GameObject("Pools");
            poolOrganizer.SetParent(masterParent.transform, true);
        }

        private void AddModelToObject(Component newObject)
        {
            if (modelPrefabs.Count <= 0) return;
            // bring in a random model from our list
            var model = Instantiate(modelPrefabs.RandomItem(), newObject.transform);
            model.name = "Model";
        }

        private PooledObjectBase CreatePooledObject()
        {
            CheckPoolOrganizer(); // ensure our pool organizer is good to go
            var newObject = Instantiate(basePrefab, poolOrganizer, true);

            newObject.AssignObjectSetup(this, objectPool); // assign our type object setup
            AddModelToObject(newObject); // check if we're adding a model

            newObject.InitializeObjectSetup(); // run whatever init code this object has using its setup
            newObject.name = name;

            return newObject;
        }

        /// <summary>
        ///     Call this from inside your own SpawnObject(vars) method to get your Pooled Object.
        /// </summary>
        /// <param name="position">The Vector3 to spawn the Object at</param>
        /// <param name="parent">(Optional) The transform to parent the Object to</param>
        /// <returns></returns>
        protected PooledObjectBase GetPooledObject(Vector3 position, Transform parent = null)
        {
            PooledObjectBase newObject;
            do
            {
                newObject = objectPool.Get();
            } while (!newObject); // ensure we actually get an object from the pool

            newObject.transform.position = position;
            if (parent) newObject.transform.SetParent(parent, true);

            //newObject.FinalizeObjectSetup(vars) => call finalize here inside SpawnObject() wrapper
            return newObject;
        }

        // exec order: Awake() 1x => Assign->InitializeObjectSetup() 1x => BeforeEnable() eachX => GetPooledObject/set position eachX => Finalize() eachX => Start() 1x
    }


    /// <summary>
    ///     Base your Pooled Object Setup ScriptableObject classes off of this
    /// </summary>
    /// <typeparam name="T">The PooledObjectBase inheritor this will spawn</typeparam>
    public class PooledObjectSetup<T> : PooledObjectSetup where T : PooledObjectBase
    {
        /// <summary>
        ///     Call this from inside your own SpawnObject(vars) method to get your Pooled Object.
        /// </summary>
        /// <param name="position">The Vector3 to spawn the Object at</param>
        /// <param name="parent">(Optional) The transform to parent the Object to</param>
        /// <returns></returns>
        public T SpawnNewPooledObject(Vector3 position, Transform parent = null)
        {
            var newObject = (T)GetPooledObject(position, parent);
            return newObject;
        }
    }
}