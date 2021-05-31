using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiryuTK.AsteroidsTopDownController.Enemy
{
    /// <summary>
    /// The class for managing the Asteroids that can fly around and hit the player
    /// </summary>
    public class Asteroid : PoolObject, IDamagable, IMineable
    {
        //Field
        private const float ScaleMax = 1f;
        private const float ScaleMin = .4f;

        private  bool alive;
        private  float moveSpeed;
        private  float rotationSpeed;
        /// <summary>
        /// The base forward moving force prior to modification
        /// </summary>
        private  Vector3 rawForward;
        private  Vector3 initalScale;
        private  Settings settings;

        #region Interface
        /// <summary>
        /// Despawn when damaged
        /// </summary>
        /// <param name="amount"> Damage amount </param>
        public void TakeDamage(int amount)
        {
            Despawn();
        }

        /// <summary>
        /// Mine the asteroid with certain amount of force
        /// </summary>
        /// <param name="force"> The mining force </param>
        public void Mine(float force)
        {
            //Each value will shrink the asteroid by 1%
            float x = transform.localScale.x;
            x -= .01f * force;

            if (x > 0.4f)
            {
                //Shrink
                Vector3 scale = new Vector3(x, x, x);
                transform.localScale = scale;
            }
            else
            {
                //Despawn when too small
                Despawn();
            }
        }
        #endregion

        #region Base class
        /// <summary>
        /// When the asteroid initially spawns
        /// </summary>
        /// <param name="pool"> Object pool </param>
        public override void InitialSpawn(Pool pool)
        {
            base.InitialSpawn(pool);
            //Reference
            settings = Settings.Instance;
            initalScale = transform.localScale;
        }

        /// <summary>
        /// When the asteroid is activated
        /// </summary>
        /// <param name="p"> Position </param>
        /// <param name="r"> Rotation </param>
        public override void Activation(Vector2 p, Quaternion r)
        {
            base.Activation(p, r);

            //Initialize scale, speed, and rotation
            float s = Random.Range(ScaleMin, ScaleMax);
            transform.localScale = initalScale * s;

            float percentage = (s - ScaleMin) / (ScaleMax - ScaleMin);
            rotationSpeed = settings.AsteroidRotation * (1 - percentage);
            moveSpeed = settings.AsteroidMove * (1 - percentage * .5f);

            rawForward = transform.up;

            //Start checking when it exits the screen
            StartCoroutine(DetectOutOfBounds());
        }

        /// <summary>
        /// When the object despawns and return to pool
        /// </summary>
        protected override void Despawn()
        {
            alive = false;
            base.Despawn();
        }
        #endregion

        /// <summary>
        /// Check if object has gone outside of screen bounds
        /// </summary>
        private IEnumerator DetectOutOfBounds()
        {
            //Let the aseteroid live for a while before checking, as they can be moving very slowly
            alive = true;
            yield return new WaitForSeconds(30f);
            while (alive)
            {
                if (settings.IsOutOfBounds(transform.position))
                {
                    Despawn();
                }
                yield return null;
            }
        }

        void FixedUpdate()
        {
            //Move forward and rotate
            transform.Translate(rawForward * Time.deltaTime * moveSpeed, Space.World);
            transform.Rotate(new Vector3(0f, 0f, 1f), rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}