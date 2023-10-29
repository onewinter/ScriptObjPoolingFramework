using UnityEngine;
using OneWinter.ScriptObjPoolingFramework;

namespace OneWinter.ScriptObjPoolingFrameworkExamples
{
    [CreateAssetMenu(menuName = "*ScriptObjPooling Examples/Projectile Setup", order = 999)]
    public class ProjectileSetup : ListPooledObjectSetup<Projectile>
    {
        public AnimationCurve ShotArc;
        public float RotationSpeed = 750f;
        public float ProjectileDuration = .3f;

        public Projectile SpawnNewProjectile(Vector3 startPosition, Transform target)
        {
            var newProjectile = SpawnNewPooledObject(startPosition);
            newProjectile.FinalizeObjectSetup(target);
            return newProjectile;
        }
    }
}