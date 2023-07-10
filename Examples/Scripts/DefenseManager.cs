using System;
using UnityEngine;
using OneWinter.ScriptObjPoolingFramework;

namespace OneWinter.ScriptObjPoolingFrameworkExamples
{
    public class DefenseManager : MonoBehaviour
    {
        [SerializeField] private int MaxActive = 100;
        [SerializeField] private ProjectileSetup projectileSetup;
        [SerializeField] private float projectileRange;
        [SerializeField] private LayerMask enemyLayer;

        private Collider[] collidersInRange;


        private void Start()
        {
            transform.position = MapManager.GridCenter;
            collidersInRange = new Collider[25];
        }

        private void Update()
        {
            if (projectileSetup.CountActive >= MaxActive) return;

            Array.Clear(collidersInRange, 0, collidersInRange.Length);
            var size = Physics.OverlapSphereNonAlloc(transform.position, projectileRange / 2f, collidersInRange,
                enemyLayer);
            if (size < 1) return;

            for (var i = 0; i < size; i++)
            {
                projectileSetup.SpawnNewProjectile(transform.position, collidersInRange[i].transform);
                if (projectileSetup.CountActive >= MaxActive) break;
            }
        }
    }
}