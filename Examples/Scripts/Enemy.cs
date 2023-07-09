using UnityEngine;

namespace OneWinter.ScriptObjPoolingFramework
{
    public class Enemy : PooledObjectBase<EnemySetup>
    {
        private Vector3 startPosition;
        private float timer;

        private void Update()
        {
            timer += Time.deltaTime;
            if (transform.position != MapManager.GridCenter)
                transform.position =
                    Vector3.Lerp(startPosition, MapManager.GridCenter, timer / TypeObject.TimeToCenter);
            else
                ReleaseToPool();
        }

        public void FinalizeObjectSetup()
        {
            startPosition = transform.position;
            timer = 0;
        }
    }
}