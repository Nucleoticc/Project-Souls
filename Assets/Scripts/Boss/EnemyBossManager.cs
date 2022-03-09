using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class EnemyBossManager : MonoBehaviour
    {
        public string bossName;
        UIBossHealthBar bossHealthBar;
        EnemyStats enemyStats;
        EnemyAnimatorHandler enemyAnimatorHandler;
        BossCombatStanceState bossCombatStanceState;

        [Header("Second Phase FX")]
        public GameObject weaponParticleFx;
        public GameObject bossParticleFx;

        void Awake()
        {
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
            enemyStats = GetComponent<EnemyStats>();
            enemyAnimatorHandler = GetComponentInChildren<EnemyAnimatorHandler>();
            bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
        }

        void Start()
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
        }

        public void UpdateBossHealth(int currentHealth, int maxHealth)
        {
            bossHealthBar.SetBossHealth(currentHealth);

            if (currentHealth <= maxHealth * 0.5 && !bossCombatStanceState.hasPhaseShifted)
            {
                ShiftTwoSecondPhase();
            }
        }

        public void ShiftTwoSecondPhase()
        {
            enemyAnimatorHandler.animator.SetBool("isInvulnerable", true);
            enemyAnimatorHandler.animator.SetBool("isPhaseShifting", true);
            enemyAnimatorHandler.PlayTargetAnimation("Phase Shift", true);
            bossCombatStanceState.hasPhaseShifted = true;
        }
    }
}

