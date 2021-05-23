using UnityEngine;
using System.Collections;

namespace HiryuTK.AsteroidsTopDownController
{
    /// <summary>
    /// Object pool for prefabs used in this scene
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class ObjectPoolManager : MonoBehaviour
    {
        //Fields
        public static ObjectPoolManager Instance;

        [Header("Pf")]
        [SerializeField] private PoolObject pf_playerBullet;
        [SerializeField] private PoolObject pf_enemyShip;
        [SerializeField] private PoolObject pf_Asteroid;
        private Pool playerBullet;
        private Pool enemyShip;
        private Pool asteroids;

        void Awake()
        {
            //Lazy singleton
            Instance = this;

            //Initialize pool for each prefab
            playerBullet = new Pool(pf_playerBullet, transform);
            enemyShip = new Pool(pf_enemyShip, transform);
            asteroids = new Pool(pf_Asteroid, transform);
        }

        //Public 
        public PoolObject SpawnPlayerBullet(Vector3 p, Quaternion r) => playerBullet.Spawn(p, r);
        public PoolObject SpawnEnemyShip(Vector3 p, Quaternion r) => enemyShip.Spawn(p, r);
        public PoolObject SpawnAsteroid(Vector3 p, Quaternion r) => asteroids.Spawn(p, r);
    }
}