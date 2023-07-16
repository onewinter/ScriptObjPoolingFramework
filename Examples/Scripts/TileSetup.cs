using UnityEngine;
using OneWinter.ScriptObjPoolingFramework;

namespace OneWinter.ScriptObjPoolingFrameworkExamples
{
    //[CreateAssetMenu(fileName = "Tile_", menuName = "Tile Setup", order = 51)]
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