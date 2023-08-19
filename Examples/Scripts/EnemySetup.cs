using UnityEngine;
using OneWinter.ScriptObjPoolingFramework;

namespace OneWinter.ScriptObjPoolingFrameworkExamples
{
    [CreateAssetMenu(menuName = "*ScriptObjPooling Examples/Enemy Setup", order = 999)]
    public class EnemySetup : PooledObjectSetup<Enemy>
    {
        public float TimeToCenter;

        public Enemy SpawnNewEnemy(Vector3 position)
        {
            var newEnemy = SpawnNewPooledObject(position + Vector3.up * .5f);
            newEnemy.FinalizeObjectSetup();
            return newEnemy;
        }
    }
}