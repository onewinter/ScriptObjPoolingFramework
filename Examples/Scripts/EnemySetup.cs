using UnityEngine;
using OneWinter.ScriptObjPoolingFramework;

namespace OneWinter.ScriptObjPoolingFramework.Examples
{
    [CreateAssetMenu(fileName = "Enemy_", menuName = "Enemy Setup", order = 51)]
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