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

        public Text MoneyAmount;

        /// <summary>
        /// Set the amount to display for money
        /// </summary>
        /// <param name="money"></param>
        public void SetMoney (int money)
        {
            MoneyAmount.text = money.ToString("00");
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}