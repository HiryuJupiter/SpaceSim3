using System.Collections;
using UnityEngine;

namespace HiryuTK.AsteroidsTopDownController
{
    /// <summary>
    /// Class for controlling the player's bullet
    /// </summary>
    public class PlayerBullet : PoolObject
    {
        //References
        private Settings settings;
        private Rigidbody2D rb;

        #region Object pool
        public override void InitialSpawn(Pool pool)
        {
            //Set the object's pool reference, then reference classes and components
            base.InitialSpawn(pool);
            settings = Settings.Instance;
            rb = GetComponent<Rigidbody2D>();
        }

        public override void Activation(Vector2 p, Quaternion r)
        {
            //When this object is spawned, give it the proper velocity towards its up direction
            base.Activation(p, r);
            rb.velocity = transform.up * settings.BasicBulletSpeed;
        }
        #endregion

        private void FixedUpdate()
        {
            //Despawn this object when it goes outside the screen
            if (settings.IsOutOfBounds(transform.position))
            {
                Despawn();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Make this able to damage enemies and asteroids upon collision
            if (Settings.Instance.IsTargetOnEnemyLayer(collision.gameObject) ||
                Settings.Instance.IsTargetOnGroundLayer(collision.gameObject))
            {
                collision.gameObject.GetComponent<IDamagable>().TakeDamage(1);
                Despawn();
            }
        }
    }
}