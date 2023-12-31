using UnityEngine;
using OneWinter.ScriptObjPoolingFramework;

namespace OneWinter.ScriptObjPoolingFrameworkExamples
{


    public class SpawnManager : MonoBehaviour
    {

        [SerializeField] private EnemySetup enemySetup;
        [SerializeField] private int maxActiveSpawn;
        [SerializeField] private int maxSpawnAtOnce = 50;
        //[SerializeField] private float spawnDelay;

        
        //private float timer;

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < maxSpawnAtOnce; i++)
            {
                if (enemySetup.ActiveObjects.Count >= maxActiveSpawn) break;

                var newRandom = Random.insideUnitCircle;
                var newSpawn =
                    new Vector3(newRandom.x * MapManager.GridCenter.x, 0.5f, newRandom.y * MapManager.GridCenter.z) +
                    new Vector3(MapManager.GridCenter.x, 0f, MapManager.GridCenter.z);
                enemySetup.SpawnNewEnemy(newSpawn);
            }
        }
    }
}