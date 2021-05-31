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

        private void Awake()
        {
            Instance = this;
        }
    }
}