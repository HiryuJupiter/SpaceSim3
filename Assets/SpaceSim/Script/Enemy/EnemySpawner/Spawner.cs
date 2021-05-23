using System.Collections;
using UnityEngine;

namespace HiryuTK.AsteroidsTopDownController.Enemy
{
    /// <summary>
    /// A spawner for spawning asteroids and enemy ships
    /// </summary>
    public class Spawner : MonoBehaviour
    {
        //Fields
        [SerializeField] private float spawnIntervalMin = 2f;
        [SerializeField] private float spawnIntervalMax = 10f;

        private Settings settings;
        private ObjectPoolManager poolM;

        private float timer = 0f;
        private float speedUpMod = 1f;
        private float speedUpSpeed = 0.01f;

        private void Start()
        {
            //Reference then start spawning
            settings    = Settings.Instance;
            poolM       = ObjectPoolManager.Instance;
            Spawn();
        }

        private void Spawn()
        {
            StartCoroutine(DoSpawn());
        }

        /// <summary>
        /// Coroutine that goes on forever to spawn the environmental objects
        /// </summary>
        private IEnumerator DoSpawn()
        {
            //Infinite loop
            while (true)
            {
                //Ticks timer when it is above zero
                if (timer > 0f)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    //50% chance to spawn a ship or an asteroid
                    if (Random.Range(0, 2) == 0)
                        SpawnAsteroid();
                    else
                        SpawnEnemyShip();
                    RefreshTimer();
                }
                yield return null;
            }
        }

        /// <summary>
        /// Reset timer to a regular interval
        /// </summary>
        void RefreshTimer()
        {
            timer = Random.Range(spawnIntervalMin, spawnIntervalMax);
            //Gradually shorten the spawn interval
            while (speedUpMod> 0.1f)
            {
                speedUpMod -= speedUpSpeed;
            }
            timer *= speedUpMod;
        }

        /// <summary>
        /// Method for spawning an asteroid
        /// </summary>
        private void SpawnAsteroid()
        {
            Vector3 p = settings.RandomSpawnPoint();
            Quaternion r = settings.RandomSpawnRotation(p);
            Debug.DrawRay(p, r * Vector3.up * 10f, Color.red, 10f);
            poolM.SpawnAsteroid(p, r);
        }

        /// <summary>
        /// Method for spawning an enemy ship
        /// </summary>
        private void SpawnEnemyShip ()
        {
            Vector3 p = settings.RandomSpawnPoint();
            Quaternion r = settings.RandomSpawnRotation(p);
            Debug.DrawRay(p, r * Vector3.up * 10f, Color.red, 10f);
            poolM.SpawnEnemyShip(p, r);
        }
    }
}