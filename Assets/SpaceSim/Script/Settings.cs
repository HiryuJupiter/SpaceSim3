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

        //Private field
        private float[] xDivisionPoints;
        private float[] yDivisionPoints;

        private float screenBound_Top;
        private float screenBound_Bot;
        private float screenBound_Left;
        private float screenBound_Right;
        private float innerSideline_Top;
        private float innerSideline_Bot;
        private float innerSideline_Left;
        private float innerSideline_Right;
        private float outerSideline_Top;
        private float outerSideline_Bot;
        private float outerSideline_Left;
        private float outerSideline_Right;

        //Properties
        public static Settings Instance { get; private set; }
        private bool RandomBool => Random.Range(0, 2) == 0;

        private void Awake()
        {
            //Lazy singlton
            Instance = this;

            //Calculate and cache screen bound locations
            Vector2 lowerLeft = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f));
            Vector2 upperRight = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

            screenBound_Top = upperRight.y;
            screenBound_Bot = lowerLeft.y;
            screenBound_Left = lowerLeft.x;
            screenBound_Right = upperRight.x;

            float offset = .4f;
            innerSideline_Top = screenBound_Top + offset;
            innerSideline_Bot = screenBound_Bot - offset;
            innerSideline_Left = screenBound_Left - offset;
            innerSideline_Right = screenBound_Right + offset;

            float offsetSmall = 1f;
            outerSideline_Top = innerSideline_Top + offsetSmall;
            outerSideline_Bot = innerSideline_Bot - offsetSmall;
            outerSideline_Left = innerSideline_Left - offsetSmall;
            outerSideline_Right = innerSideline_Right + offsetSmall;

            //Visualize the edge of the edge bounds
            Debug.DrawLine(new Vector2(outerSideline_Left, outerSideline_Top), new Vector2(outerSideline_Right, outerSideline_Top), Color.red, 10f);
            Debug.DrawLine(new Vector2(outerSideline_Left, outerSideline_Top), new Vector2(outerSideline_Left, outerSideline_Bot), Color.red, 10f);
            Debug.DrawLine(new Vector2(outerSideline_Right, outerSideline_Bot), new Vector2(outerSideline_Left, outerSideline_Bot), Color.red, 10f);
            Debug.DrawLine(new Vector2(outerSideline_Right, outerSideline_Bot), new Vector2(outerSideline_Right, outerSideline_Top), Color.red, 10f);

            Debug.DrawLine(new Vector2(innerSideline_Left, innerSideline_Top), new Vector2(innerSideline_Right, innerSideline_Top), Color.white, 10f);
            Debug.DrawLine(new Vector2(innerSideline_Left, innerSideline_Top), new Vector2(innerSideline_Left, innerSideline_Bot), Color.white, 10f);
            Debug.DrawLine(new Vector2(innerSideline_Right, innerSideline_Bot), new Vector2(innerSideline_Left, innerSideline_Bot), Color.white, 10f);
            Debug.DrawLine(new Vector2(innerSideline_Right, innerSideline_Bot), new Vector2(innerSideline_Right, innerSideline_Top), Color.white, 10f);

            //Initialize the subdivision points in the middle of the zone.
            int xDivisions = 4;
            float xdivisionDist = (screenBound_Right - screenBound_Left) / xDivisions;
            xDivisionPoints = new float[xDivisions - 1];
            for (int i = 0; i < xDivisions - 1; i++)
            {
                xDivisionPoints[i] = screenBound_Left + xdivisionDist * (1 + i);
            }

            int yDivisions = 4;
            float ydivisionDist = (screenBound_Top - screenBound_Bot) / yDivisions;
            yDivisionPoints = new float[yDivisions - 1];
            for (int i = 0; i < yDivisions - 1; i++)
            {
                yDivisionPoints[i] = screenBound_Bot + ydivisionDist * (1 + i);
            }

            //Debug 
            for (int x = 0; x < xDivisionPoints.Length; x++)
            {
                for (int z = 0; z < yDivisionPoints.Length; z++)
                {
                    Vector2 p = new Vector2(xDivisionPoints[x], yDivisionPoints[z]);
                    Debug.DrawLine(p, p + Vector2.right, Color.magenta, 10f);
                }
            }
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
                float x = Random.Range(screenBound_Left, screenBound_Right);
                float y = RandomBool ? innerSideline_Bot : innerSideline_Top;
                return new Vector2(x, y);
            }
            else
            {
                //Spawn left and right edge
                float y = Random.Range(screenBound_Top, screenBound_Bot);
                float x = RandomBool ? innerSideline_Left : innerSideline_Right;
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
            Vector2 aim = new Vector2(
                xDivisionPoints[Random.Range(0, xDivisionPoints.Length)],
                yDivisionPoints[Random.Range(0, yDivisionPoints.Length)]);
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
            if (currentPos.x < innerSideline_Left)
            {
                warpPos.x = innerSideline_Right - .01F;
                return true;
            }
            //Warp Right >>> Left
            if (currentPos.x > innerSideline_Right)
            {
                warpPos.x = innerSideline_Left + .01F;
                return true;
            }

            //Warp Bot >>> Top
            if (currentPos.y < innerSideline_Bot)
            {
                warpPos.y = innerSideline_Top - .01F;
                return true;
            }

            //Warp Top >>> Bot
            if (currentPos.y > innerSideline_Top)
            {
                warpPos.y = innerSideline_Bot + .01F;
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
            if (position.x < outerSideline_Left || position.x > outerSideline_Right ||
                position.y < outerSideline_Bot || position.y > outerSideline_Top)
            {
                return true;
            }
            return false;
        }

        //Get a random true false

    }
}