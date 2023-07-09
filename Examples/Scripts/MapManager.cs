using UnityEngine;

namespace OneWinter.ScriptObjPoolingFramework
{
    public class MapManager : MonoBehaviour
    {
        public static Vector3 GridCenter = new(50f, 0.5f, 50f);
        [SerializeField] private TileSetup tileSetup;
        public int x, y;

        private void Start()
        {
            for (var i = 0; i < x; i++)
            for (var j = 0; j < y; j++)
                tileSetup.SpawnNewTile(new Vector3(i, 0, j));
        }
    }
}