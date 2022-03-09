using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class EnemyStats : CharacterStats
    {
        EnemyManager enemyManager;
        WorldEventManager worldEventManager;
        EnemyAnimatorHandler enemyAnimatorHandler;
        EnemyBossManager enemyBossManager;
        CameraHandler cameraHandler;
        PursueTargetState pursueTargetState;
        public UIEnemyHealthBar uiEnemyHealthBar;
        public bool isBoss;

        void Awake()
        {
            enemyAnimatorHandler = GetComponent<EnemyAnimatorHandler>();
            worldEventManager = FindObjectOfType<WorldEventManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            enemyManager = GetComponent<EnemyManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            pursueTargetState = GetComponentInChildren<PursueTargetState>();
            SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        void Start()
        {
            if (!isBoss)
            {
                uiEnemyHealthBar.SetMaxHealth(maxHealth);
            }
        }

        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer -= Time.deltaTime;
            }
            else if (poiseResetTimer <= 0 && !enemyManager.isInteracting)
            {
                totalPoiseDefense = armorPoiseBonus;
            }
        }

        public void BreakGuard()
        {
            enemyAnimatorHandler.PlayTargetAnimation("Break Guard", true);
        }

        void SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
        }

        public override void TakeDamageNoAnimation(int damage, CharacterStats damagedBy = null)
        {
            if (isDead) { return; }
            currentHealth -= damage;

            if (isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealth(currentHealth, maxHealth);
            }
            else if (!isBoss)
            {
                uiEnemyHealthBar.SetHealth(currentHealth);
            }

            if (enemyManager.currentTarget == null)
            {
                enemyManager.currentTarget = damagedBy;
                enemyManager.currentState = pursueTargetState;
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                cameraHandler.ClearLockOnTargets();
                isDead = true;
                if (isBoss)
                {
                    worldEventManager.DeactivateBossFight();
                }
            }
        }

        public override void TakeDamage(int damage, string damageAnimation = "Damage_01", CharacterStats damagedBy = null)
        {
            if (isDead) { return; }
            currentHealth -= damage;

            if (isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealth(currentHealth, maxHealth);
            }
            else if (!isBoss)
            {
                uiEnemyHealthBar.SetHealth(currentHealth);
            }

            if (enemyManager.currentTarget == null)
            {
                enemyManager.currentTarget = damagedBy;
                enemyManager.currentState = pursueTargetState;
            }

            enemyAnimatorHandler.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                cameraHandler.ClearLockOnTargets();
                enemyAnimatorHandler.PlayTargetAnimation("Dead_01", true);
                isDead = true;
                if (isBoss)
                {
                    worldEventManager.DeactivateBossFight();
                }
            }
        }
    }
}

