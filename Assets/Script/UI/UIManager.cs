using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HiryuTK.AsteroidsTopDownController
{
    /// <summary>
    /// For displaying UI elements
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        //Fields
        public static UIManager Instance;

        public Text Text_MoneyAmount;
        public Text Text_MouseHoverInfo;
        public RenderTexture existingRenderTexture;
        public Material renderTextureMaterial;

        Camera cam;
        /// <summary>
        /// Set the amount to display for money
        /// </summary>
        /// <param name="money"></param>
        public void SetMoney (int money)
        {
            Text_MoneyAmount.text = money.ToString("00");
        }

        public void SetHovingInfoText (string text)
        {
            Text_MouseHoverInfo.text = text;
        }

        #region Settings
        //Resolution
        public void SetResolution1 ()
        {
            Screen.SetResolution(1920, 1080, true);
        }

        public void SetResolution2()
        {
            Screen.SetResolution(1600, 900, false);
        }

        //Depth buffer
        public void SetDepthBuffer1()
        {
            RenderTexture newTexture = new RenderTexture(existingRenderTexture);
            newTexture.depth = 24;
            Camera.main.targetTexture = newTexture;
            renderTextureMaterial.mainTexture = newTexture;
        }

        public void SetDepthBuffer2()
        {
            RenderTexture newTexture = new RenderTexture(existingRenderTexture);
            newTexture.depth = 0;
            Camera.main.targetTexture = newTexture;
            renderTextureMaterial.mainTexture = newTexture;
        }

        //Anti aliasing
        public void SetAntiAliasing1()
        {
            QualitySettings.antiAliasing = 2;
        }

        public void SetAntiAliasing2()
        {
            QualitySettings.antiAliasing = 0;
        }

        //Anisotropic filter
        public void SetAnisotropicFilter1()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
        }

        public void SetAnisotropicFilter2()
        {
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        }

        //FrameRate Limiter
        public void SetFrameRateLimiter1()
        {
            QualitySettings.vSyncCount = 2;

            //QualitySettings.vSyncCount = 0;  // VSync must be disabled
            //Application.targetFrameRate = 45;
        }

        public void SetFrameRateLimiter2()
        {
            QualitySettings.vSyncCount = 0;
        }
        #endregion

        private void Awake()
        {
            Instance = this;
            cam = Camera.main;
            SetDepthBuffer1();
        }

        private void OnDisable()
        {
            Camera.main.targetTexture = existingRenderTexture;
            renderTextureMaterial.mainTexture = existingRenderTexture;
        }
    }
}