using UnityEngine;
using System.Collections;

namespace HiryuTK.AsteroidsTopDownController
{
    /// <summary>
    /// For setting the game's settings all in one place
    /// </summary>
    [DefaultExecutionOrder(-90000000)]
    public class Settings : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private int playerHealth = 3;
        public LayerMask PlayerMaxHealth => playerHealth;

        [Header("Abilities")]
        [SerializeField] private float basicBulletSeed = 20f;
        [SerializeField] private float basicAttackCD = 1f;
        [SerializeField] private float miningCD = 0.1f;
        [SerializeField] private float miningPower = 5f;
        [SerializeField] private float miningDistance = 50f;
        public float BasicBulletSpeed => basicBulletSeed;
        public float BasicAttackCD => basicAttackCD;
        public float MiningCD => miningCD;
        public float MiningPower => miningPower;
        public float MiningDistance => miningDistance;

        [Header("Layers")]
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask enemyLayer;
        public LayerMask PlayerLayer => playerLayer;
        public LayerMask GroundLayer => groundLayer;
        public LayerMask EnemyLayer => enemyLayer;

        [Header("Player Movement")]
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float moveAcceleration = 1f;
        [SerializeField] private float rotationSpeed = 20f; //50f
        [SerializeField] private float rotationAccleration = 1f; //50f
        public float MoveSpeed => moveSpeed;
        public float MoveAcceleration => moveAcceleration;
        public float RotationSpeed => rotationSpeed;
        public float RotationAccleration => rotationAccleration;

        [Header("Enemy Movement")]
        [SerializeField] private float enemyMove = 2f;
        [SerializeField] private float enemyRotation = 100f;
        public float EnemyMove => enemyMove;
        public float EnemyRotation => enemyRotation;

        [Header("Asteroid Movement")]
        [SerializeField] private float asteroidMove = 15f;
        [SerializeField] private float asteroidRotation = 10f;
        public float AsteroidMove => asteroidMove;
        public float AsteroidRotation => asteroidRotation;

        [SerializeField] Collider2D spawnBound;

        //Properties
        public static Settings Instance { get; private set; }
        private bool RandomBool => Random.Range(0, 2) == 0;

        float top, bot, left, right;


        private void Awake()
        {
            //Lazy singlton
            Instance = this;

            //Calculate and cache screen bound locations
            top = spawnBound.bounds.max.y;
            bot = -top;
            right = spawnBound.bounds.max.x;
            left = -right;
        }

        //For doing layer checks
        public bool IsTargetOnPlayerLayer(GameObject go) => PlayerLayer == (PlayerLayer | 1 << go.layer);
        public bool IsTargetOnEnemyLayer(GameObject go) => EnemyLayer == (EnemyLayer | 1 << go.layer);
        public bool IsTargetOnGroundLayer(GameObject go) => GroundLayer == (GroundLayer | 1 << go.layer);

        /// <summary>
        /// Get a random spawn point
        /// </summary>
        /// <returns></returns>
        public Vector2 RandomSpawnPoint()
        {
            if (RandomBool)
            {
                //Spawn top and bottom
                float x = Random.Range(left, right);
                float y = RandomBool ? bot : top;
                return new Vector2(x, y);
            }
            else
            {
                //Spawn left and right edge
                float y = Random.Range(bot, top);
                float x = RandomBool ? left : right;
                return new Vector2(x, y);
            }
        }

        /// <summary>
        /// Get a random rotation
        /// </summary>
        /// <param name="spawnPoint"></param>
        /// <returns></returns>
        public Quaternion RandomSpawnRotation(Vector2 spawnPoint)
        {
            Vector2 aim = Random.insideUnitCircle * 5f; 
            return Quaternion.LookRotation(Vector3.forward, aim - spawnPoint);
        }

        /// <summary>
        /// Get screen warp location if the current position parameter is indeed out of bounds
        /// </summary>
        /// <param name="currentPos"></param>
        /// <param name="warpPos"></param>
        /// <returns></returns>
        public bool TryGetScreenWarpPosition(Vector2 currentPos, out Vector2 warpPos)
        {
            warpPos = currentPos;
            //Warp Left >>> Right
            if (currentPos.x < left)
            {
                warpPos.x = right- .01F;
                return true;
            }
            //Warp Right >>> Left
            if (currentPos.x > right)
            {
                warpPos.x = left + .01F;
                return true;
            }

            //Warp Bot >>> Top
            if (currentPos.y < bot)
            {
                warpPos.y = top - .01F;
                return true;
            }

            //Warp Top >>> Bot
            if (currentPos.y > top)
            {
                warpPos.y = bot + .01F;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check if a position is ount of bounds
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsOutOfBounds(Vector2 position)
        {
            if (position.x < left || position.x > right ||
                position.y < bot || position.y > top)
            {
                return true;
            }
            return false;
        }

        //Get a random true false

    }
}