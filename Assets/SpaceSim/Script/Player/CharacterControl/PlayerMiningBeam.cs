using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiryuTK.AsteroidsTopDownController
{
    public class PlayerMiningBeam : MonoBehaviour
    {
        //Fields
        private float miningCooldownTimer;
        private PlayerTopDown3DController player;
        private Settings settings;
        private bool isSetup;

        private bool MiningCooldownReady => miningCooldownTimer <= 0f;

        //This has to be set up before it can be used
        public void Setup(PlayerTopDown3DController player)
        {
            this.player = player;
            settings = Settings.Instance;
            isSetup = true;
        }

        //Similar to update but has to be called from the outside
        public void TickUpdate()
        {
            //Guard statement
            if (!isSetup)
                return;

            //Beam controls based on input
            if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.K))
            {
                ShootMiningBeam();
            }
            else if (Input.GetMouseButtonUp(1) || Input.GetKeyUp(KeyCode.K) || Input.GetKeyUp(KeyCode.Escape))
            {
                TurnOffMiningBeam();
            }

            //Mining timer needs to have a cooldown or it'll go too fast.
            if (miningCooldownTimer > 0)
            {
                miningCooldownTimer -= Time.deltaTime;
                if (miningCooldownTimer <= 0f)
                {
                    TurnOffMiningBeam();
                }
            }
        }

        /// <summary>
        /// Shoot the mining beam using a raycast towards the mouse location
        /// </summary>
        private void ShootMiningBeam()
        {
            //Convert mouse pos from screen to world pos, then get the direction from player to mouse, then do raycast in that direction
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 shootDir = mousePos - (Vector2)player.ShootPoint.position;
            RaycastHit2D hit = Physics2D.Raycast(player.ShootPoint.position, shootDir, settings.MiningDistance, settings.GroundLayer);

            //Enable line renderer
            player.LineRenderer.enabled = true;

            //If we hit something
            if (hit.collider != null) 
            {
                Debug.DrawLine(player.ShootPoint.position, hit.point, Color.red);
                //Make the line renderer more visible
                player.LineRenderer.startColor = Color.yellow;
                player.LineRenderer.endColor = Color.yellow;
                player.LineRenderer.SetPositions(new Vector3[] { player.ShootPoint.position, hit.point });
                player.LineRenderer.widthMultiplier = 0.1f;

                //If mining beam is ready, shoot the laser and reset the cooldown to max.
                if (MiningCooldownReady)
                {
                    IMineable asteroid = hit.collider.GetComponent<IMineable>();
                    if (asteroid != null)
                    {
                        MineAsteroid(asteroid);
                        miningCooldownTimer = settings.MiningCD;
                    }
                }
            }
            else
            {
                //If there isn't a valid object to mine, then make it less apparent.
                Debug.DrawLine(player.ShootPoint.position, mousePos, Color.blue, 1f);
                player.LineRenderer.SetPositions(new Vector3[] { player.ShootPoint.position, mousePos });
                player.LineRenderer.widthMultiplier = 0.05f;
                player.LineRenderer.startColor = Color.grey;
                player.LineRenderer.endColor = Color.grey;
            }
        }

        //Turn off mining beam
        private void TurnOffMiningBeam()
        {
            player.LineRenderer.enabled = false;
        }

        /// <summary>
        /// Mine the asteroid passed in
        /// </summary>
        /// <param name="asteroid"> The asteroid that is to be mined </param>
        private void MineAsteroid(IMineable asteroid)
        {
            asteroid.Mine(settings.MiningPower);
            player.AddMoney(10);
        }
    }
}