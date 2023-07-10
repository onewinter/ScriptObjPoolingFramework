using UnityEngine;
using OneWinter.ScriptObjPoolingFramework;

namespace OneWinter.ScriptObjPoolingFrameworkExamples
{
    public class Tile : PooledObjectBase<TileSetup>
    {
        private Renderer tileRenderer;

        // called each time the object is placed in the scene
        public void FinalizeObjectSetup()
        {
            tileRenderer ??= GetComponent<Renderer>();
            tileRenderer.material.color = Color.Lerp(Color.green, Color.white, Random.Range(0.2f, 0.8f));
        }
    }
}