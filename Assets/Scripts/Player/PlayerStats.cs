using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class PlayerStats : CharacterStats
    {
        public HealthBar healthBar;
        public StaminaBar staminaBar;
        public FocusPointBar focusPointBar;

        PlayerAnimatorHandler playerAnimatorHandler;
        PlayerManager playerManager;

        public float staminaRegenerationAmount = 20f;
        float staminaRegenerationTimer = 0f;

        void Awake()
        {
            playerAnimatorHandler = GetComponent<PlayerAnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
        }

        void Start()
        {
            SetMaxHealthFromHealthLevel();
            SetMaxStaminaFromStaminaLevel();
            SetMaxFocusPointsFromFocusPointsLevel();
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            currentFocusPoints = maxFocusPoints;
            healthBar.SetMaxHealth(maxHealth);
            staminaBar.SetMaxStamina(Mathf.RoundToInt(maxStamina));
            focusPointBar.SetMaxFocusPoints(Mathf.RoundToInt(maxFocusPoints));
        }

        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer -= Time.deltaTime;
            }
            else if (poiseResetTimer <= 0 && !playerManager.isInteracting)
            {
                totalPoiseDefense = armorPoiseBonus;
            }
        }

        void SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
        }

        public override void TakeDamageNoAnimation(int damage, CharacterStats damagedBy = null)
        {
            if (playerManager.isInvulnerable) { return; }
            if (isDead) { return; }

            currentHealth -= damage;

            healthBar.SetCurrentHealth(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
                DisableCollidersAfterDeath();
            }
        }

        public override void TakeDamage(int damage, string damageAnimation = "Damage_01", CharacterStats damagedBy = null)
        {
            if (playerManager.isInvulnerable) { return; }
            if (isDead) { return; }
            currentHealth -= damage;

            healthBar.SetCurrentHealth(currentHealth);

            playerAnimatorHandler.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                playerAnimatorHandler.PlayTargetAnimation("Dead_01", true);
                isDead = true;
                DisableCollidersAfterDeath();
            }
        }

        public void DisableCollidersAfterDeath()
        {
            Collider bodyCollider = GetComponent<Collider>();
            Collider[] colliders = GetComponentsInChildren<Collider>();
            bodyCollider.enabled = false;
            foreach(Collider collider in colliders)
            {
                collider.enabled = false;
            }
        }

        public void HealPlayer(int healAmount)
        {
            currentHealth += healAmount;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            healthBar.SetCurrentHealth(currentHealth);
        }

        void SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina -= damage;

            staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
        }

        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenerationTimer = 0;
                return;
            }

            staminaRegenerationTimer += Time.deltaTime;
            if (currentStamina < maxStamina && staminaRegenerationTimer > 0.75f)
            {
                currentStamina += staminaRegenerationAmount * Time.deltaTime;
                staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
            }
        }

        void SetMaxFocusPointsFromFocusPointsLevel()
        {
            maxFocusPoints = focusLevel * 10;
        }

        public void TakeFocusPointsDamage(int damage)
        {
            currentFocusPoints -= damage;

            if (currentFocusPoints < 0)
            {
                currentFocusPoints = 0;
            }

            focusPointBar.SetCurrentFocusPoints(Mathf.RoundToInt(currentFocusPoints));
        }

        public void AddSouls(int amount)
        {
            soulCount += amount;
        }
    }
}

