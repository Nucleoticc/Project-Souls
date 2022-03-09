using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class CharacterStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;

        public int focusLevel = 10;
        public float maxFocusPoints;
        public float currentFocusPoints;

        public int soulCount = 0;
        public int soulsAwardedOnDeath;

        [Header("Poise")]
        public float totalPoiseDefense;
        public float offensivePoiseBonus;
        public float armorPoiseBonus;
        public float totalPoiseResetTime = 10f;
        public float poiseResetTimer;

        public bool isDead;

        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }

        void Start()
        {
            totalPoiseDefense = armorPoiseBonus;
        }

        public virtual void TakeDamageNoAnimation(int damage, CharacterStats damagedBy = null) { }

        public virtual void TakeDamage(int damage, string damageAnimation = "Damage_01", CharacterStats damagedBy = null) { }

        public virtual void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer -= Time.deltaTime;
            }
            else
            {
                totalPoiseDefense = armorPoiseBonus;
            }
        }
    }
}

