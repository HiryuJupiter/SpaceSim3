using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HiryuTK.AsteroidsTopDownController
{
    public class WorldSpaceHealthBar : MonoBehaviour
    {
        public Image BarFG;
        public Gradient gradient;

        Transform cam;

        public void SetPercentage(float perc)
        {
            BarFG.fillAmount = perc;
            BarFG.color = gradient.Evaluate(perc);
        }

        private void Awake()
        {
            cam = Camera.main.transform;
            try
            {
                GetComponent<Canvas>().worldCamera = Camera.main;
            }
            catch (System.Exception e)
            {
                Debug.Log("Failed to set camera. Exception: " + e.Message + ". Is canvas null? " + (GetComponent<Canvas>() == null));
                throw;
            }
        }

        void Update()
        {
            //transform.LookAt(cam);
            //transform.Rotate(0, 180, 0); //Or you can just set the fill origin to right. It's a bug with the camera
        }
    }
}
