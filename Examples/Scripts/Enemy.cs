using UnityEngine;
using OneWinter.ScriptObjPoolingFramework;

namespace OneWinter.ScriptObjPoolingFrameworkExamples
{
    public class Enemy : PooledObjectBase<EnemySetup>
    {
        private Vector3 startPosition;
        private float timer;

        public override void BeforeEnable()
        {
            base.BeforeEnable();
            timer = 0;
        }

        private void Update()
        {
            timer += Time.deltaTime;
            
            if (timer <= TypeObject.TimeToCenter)
                transform.position =
                    Vector3.Lerp(startPosition, MapManager.GridCenter, timer / TypeObject.TimeToCenter);
            else
                ReleaseToPool();
        }

        public void FinalizeObjectSetup()
        {
            startPosition = transform.position;
        }
    }
}