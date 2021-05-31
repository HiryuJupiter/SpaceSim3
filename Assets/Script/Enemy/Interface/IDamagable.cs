using System.Collections;
using UnityEngine;

namespace HiryuTK.AsteroidsTopDownController
{
    /// <summary>
    /// Interface for objects that can be damaged
    /// </summary>
    public interface IDamagable
    {
        void TakeDamage(int amount);
    }
}