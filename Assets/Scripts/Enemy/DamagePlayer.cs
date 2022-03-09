using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class DamagePlayer : MonoBehaviour
    {
        private int damage = 25;

        void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }
        }
    }
}

