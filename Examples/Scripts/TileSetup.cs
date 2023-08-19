using UnityEngine;
using OneWinter.ScriptObjPoolingFramework;

namespace OneWinter.ScriptObjPoolingFrameworkExamples
{
    [CreateAssetMenu(menuName = "*ScriptObjPooling Examples/Tile Setup", order = 999)]
    public class TileSetup : PooledObjectSetup<Tile>
    {
        public Tile SpawnNewTile(Vector3 position)
        {
            var newTile = SpawnNewPooledObject(position);
            newTile.FinalizeObjectSetup();
            return newTile;
        }
    }
}