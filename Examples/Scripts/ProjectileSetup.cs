using UnityEngine;
using OneWinter.ScriptObjPoolingFramework;

namespace OneWinter.ScriptObjPoolingFramework.Examples
{
    [CreateAssetMenu(fileName = "Projectile_", menuName = "Projectile Setup", order = 51)]
    public class ProjectileSetup : PooledObjectSetup<Projectile>
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