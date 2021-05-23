using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace HiryuTK.AsteroidsTopDownController
{
    /// <summary>
    /// This class controls the player's ship
    /// </summary>
    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerMiningBeam))]
    [RequireComponent(typeof(PlayerShootingModule))]
    public class PlayerTopDown3DController : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Transform shootPoint;
        [SerializeField] private LineRenderer lineRenderer;

        //Class and components
        private Settings settings;
        private UIManager uiManager;

        private Rigidbody2D rb;
        private PlayerMiningBeam miningBeam;
        private PlayerShootingModule gun;

        //Status 
        private int Money;
        private Vector2 velocity;
        private float rotDelta;

        //Properties
        public static PlayerTopDown3DController Instance { get; private set; }
        public Transform ShootPoint => shootPoint;
        public LineRenderer LineRenderer => lineRenderer;
        #endregion

        #region Public
        /// <summary>
        /// Adds money to the player and the update the ui display
        /// </summary>
        /// <param name="newAmount"></param>
        public void AddMoney(int newAmount)
        {
            Money = Money + newAmount;
            uiManager.SetMoney(Money);
        }

        /// <summary>
        /// Damages the player, in this case just restart the level
        /// </summary>
        /// <param name="enemyPos"> For doing potential knock backs and pfx explosions </param>
        /// <param name="damage"> For counting damage in a bigger game </param>
        public void DamagePlayer(Vector2 enemyPos, int damage)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        #endregion

        #region MonoBehiavor
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            //Reference and initialize
            settings = Settings.Instance;
            uiManager = UIManager.Instance;

            rb = GetComponent<Rigidbody2D>();
            gun = GetComponent<PlayerShootingModule>();
            miningBeam = GetComponent<PlayerMiningBeam>();

            gun.Setup(this); //Injection
            miningBeam.Setup(this);
        }

        private void Update()
        {
            //Every frame, update gun and mining beam
            ScreenWarp();
            gun.TickUpdate();
            miningBeam.TickUpdate();
        }

        private void FixedUpdate()
        {
            //Movement updates
            UpdateMovement();
            UpdateRotation();
            ApplyRigidbodyVelocity();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //When htting enemy or asteroid...
            if (Settings.Instance.IsTargetOnEnemyLayer(collision.gameObject) || 
                Settings.Instance.IsTargetOnGroundLayer(collision.gameObject))
            {
                //Damge the other object and damage the player
                Debug.Log("player hits damaging object");
                collision.gameObject.GetComponent<IDamagable>().TakeDamage(1);
                DamagePlayer(collision.gameObject.transform.position, 999);
            }
        }
        #endregion

        #region Movement
        private void UpdateMovement()
        {
            //The ship can only move forward
            float drive = Mathf.Clamp(GameInput.MoveY, 0f, 1f);

            //lerp the move speed so it's smooth
            velocity.y = Mathf.Lerp(velocity.y, drive * settings.MoveSpeed,
                Time.deltaTime * settings.MoveAcceleration);
        }

        private void UpdateRotation()
        {
            //Rotational changes should be applied slowly
            rotDelta = Mathf.Lerp(rotDelta, GameInput.MoveX, settings.RotationAccleration * Time.deltaTime);
            rb.rotation -= rotDelta * settings.RotationSpeed * Time.deltaTime;
        }

        private void ApplyRigidbodyVelocity()
        {
            //Movement is relative to ship's facing
            rb.velocity = transform.TransformDirection(velocity);
        }

        /// <summary>
        /// Screenwarp when hitting screen boarder
        /// </summary>
        private void ScreenWarp ()
        {
            if (settings.TryGetScreenWarpPosition(transform.position, out Vector2 warpPosition))
            {
                transform.position = warpPosition;
            }
        }
        #endregion
    }
}