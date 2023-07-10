using UnityEngine;
using OneWinter.ScriptObjPoolingFramework;

namespace OneWinter.ScriptObjPoolingFramework.Examples
{
    public class Projectile : PooledObjectBase<ProjectileSetup>
    {
        private float damageTimer;
        private bool init;
        private Vector3 projectileStart;
        private Enemy target;
        private Vector3 targetPosition;
        private TrailRenderer trail;

        private float DistanceToTarget => (transform.position - targetPosition).sqrMagnitude;

        private void Awake()
        {
            trail = GetComponent<TrailRenderer>();
            trail.enabled = false;
        }

        private void Update()
        {
            if (!init) return;

            // rotate this projectile end over end (it was for a thrown axe)
            var rot = Time.deltaTime * TypeObject.RotationSpeed;
            transform.Rotate(new Vector3(rot, 0f, 0f));

            // move the projectile in an arc (like a thrown axe) using the TypeObject's Animation Curve
            var newPosition =
                Vector3.Lerp(projectileStart, targetPosition, damageTimer / TypeObject.ProjectileDuration);
            newPosition.y += TypeObject.ShotArc.Evaluate(damageTimer / TypeObject.ProjectileDuration);
            transform.position = newPosition;

            // once we're close enough to the enemy, damage the enemy and release the Projectile
            if (target.IsActive() && DistanceToTarget < .1f) OnImpact();

            damageTimer += Time.deltaTime;

            // if the enemy died, go away too
            if (!target.IsActive()) ReleaseToPool();
        }

        public override void BeforeEnable()
        {
            // reset our firing variables each time
            trail.Clear();
            target = null;
            init = false;
            damageTimer = 0f;
        }

        public void FinalizeObjectSetup(Transform newTarget)
        {
            // set our variables dependent upon spawn position and target each time
            projectileStart = transform.position;
            trail.enabled = true;

            target = newTarget.GetComponent<Enemy>();
            targetPosition = target.transform.position;
            transform.LookAt(targetPosition);

            init = true;
        }

        private void OnImpact()
        {
            if (target.IsActive()) target.ReleaseToPool();

            init = false;
            ReleaseToPool();
        }
    }
}