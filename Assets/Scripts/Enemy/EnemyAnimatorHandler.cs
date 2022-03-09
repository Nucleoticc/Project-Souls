using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class EnemyAnimatorHandler : AnimatorHandler
    {
        EnemyBossManager enemyBossManager;
        EnemyEffectsManager enemyEffectsManager;
        EnemyManager enemyManager;

        protected override void Awake()
        {
            base.Awake();
            enemyManager = GetComponent<EnemyManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
        }

        public void AwardSoulsOnDeath()
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            SoulCountUI soulCountUI = FindObjectOfType<SoulCountUI>();

            if (playerStats != null)
            {
                playerStats.AddSouls(characterStats.soulsAwardedOnDeath);
                if (soulCountUI != null)
                {
                    soulCountUI.SetSoulCountText(playerStats.soulCount);
                }
            }

        }

        public void InstantiateWeaponParticleFx()
        {
            BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();

            GameObject particleFxInstance = Instantiate(enemyBossManager.weaponParticleFx, bossFXTransform.transform);
        }

        public void InstantiateBossParticleFx()
        {
            GameObject particleFxInstance = Instantiate(enemyBossManager.bossParticleFx, transform);
            Destroy(particleFxInstance, 5f);
        }

        public void PlayWeaponTrailFX()
        {
            enemyEffectsManager.PlayWeaponFX(false);
        }

        public override void TakeCriticalDamageAnimationEvent()
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            characterStats.TakeDamageNoAnimation(characterManager.pendingCriticalDamage, playerStats);
            characterManager.pendingCriticalDamage = 0;
        }

        void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.rb.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;

            enemyManager.rb.velocity = velocity;

            if (enemyManager.isRotatingWithRootMotion)
            {
                enemyManager.transform.rotation *= animator.deltaRotation;
            }
        }
    }
}

