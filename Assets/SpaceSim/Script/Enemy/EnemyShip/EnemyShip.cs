using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiryuTK.AsteroidsTopDownController.Enemy
{
    public class EnemyShip : PoolObject, IDamagable
    {
        //Fields
        private PlayerTopDown3DController player;
        Settings settings;

        #region Public
        /// <summary>
        /// Despawn when ship takes damage
        /// </summary>
        /// <param name="amount"> dmg amount</param>
        public void TakeDamage(int amount)
        {
            Despawn();
        }
        #endregion

        #region Object pool
        /// <summary>
        /// Setup references and initializations when first spawned
        /// </summary>
        /// <param name="pool"> object pool </param>
        public override void InitialSpawn(Pool pool)
        {
            base.InitialSpawn(pool);
            settings = Settings.Instance;
            player = PlayerTopDown3DController.Instance;
        }

        /// <summary>
        /// When ship is reactived from pool
        /// </summary>
        /// <param name="p">Position</param>
        /// <param name="r">Rotation</param>
        public override void Activation(Vector2 p, Quaternion r)
        {
            base.Activation(p, r);
            transform.position = p;
            transform.rotation = r;
            //Debug.DrawRay(transform.position, moveDir, Color.yellow, 10f);
        }
        #endregion

        #region Mono
        private void FixedUpdate()
        {
            //Rotate towards player when it is not dead
            if (player != null)
            {
                //Find the direction to it, the rotation to it, then lerp the ship rotation towards that rotation.
                Vector3 dirToPlayer = player.transform.position - transform.position;
                Quaternion targetRot = Quaternion.LookRotation(Vector3.forward, dirToPlayer);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, settings.EnemyRotation * Time.deltaTime);

                //Move forward
                transform.Translate(transform.up * settings.EnemyMove * Time.deltaTime, Space.World);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //When hits the player, tell the player to take damage.
            if (settings.IsTargetOnPlayerLayer(collision.gameObject))
            {
                PlayerTopDown3DController p = collision.GetComponent<PlayerTopDown3DController>();
                if (p != null)
                {
                    p.DamagePlayer(transform.position, 1);
                    Despawn();
                }
            }
        }
        #endregion


    }
}