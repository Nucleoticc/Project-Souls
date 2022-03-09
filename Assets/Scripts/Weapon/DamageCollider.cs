using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        Collider damageCollider;
        public bool enabledOnStartup = false;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Damage")]
        public int currentWeaponDamage = 25;

        void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = enabledOnStartup;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Player")
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                EnemyStats enemyStats = FindObjectOfType<EnemyStats>();
                CharacterManager playerCharacterManager = collision.GetComponent<CharacterManager>();
                CharacterEffectsManager playerEffectsManager = collision.GetComponent<CharacterEffectsManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (playerCharacterManager != null)
                {
                    if (playerCharacterManager.isParrying)
                    {
                        characterManager.GetComponentInChildren<AnimatorHandler>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                    else if (shield != null)
                    {
                        if (playerCharacterManager.isBlocking)
                        {
                            float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                            if (playerStats != null)
                            {
                                playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard", enemyStats);
                                return;
                            }
                        }
                    }
                }

                if (playerStats != null)
                {
                    playerStats.poiseResetTimer = playerStats.totalPoiseResetTime;
                    playerStats.totalPoiseDefense = playerStats.totalPoiseDefense - poiseBreak;
                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    playerEffectsManager.PlayBloodSplatterEffects(contactPoint);

                    if (playerStats.totalPoiseDefense > poiseBreak)
                    {
                        playerStats.TakeDamageNoAnimation(currentWeaponDamage, enemyStats);
                    }
                    else
                    {
                        playerStats.TakeDamage(currentWeaponDamage, damagedBy: enemyStats);
                    }
                }
            }

            if (collision.tag == "Enemy" && transform.root.tag != "Enemy")
            {
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
                PlayerStats playerStats = FindObjectOfType<PlayerStats>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                EnemyEffectsManager enemyEffectsManager = collision.GetComponent<EnemyEffectsManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        characterManager.GetComponentInChildren<AnimatorHandler>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                    else if (shield != null)
                    {
                        if (enemyCharacterManager.isBlocking)
                        {
                            float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                            if (enemyStats != null)
                            {
                                enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard", playerStats);
                                return;
                            }
                        }
                    }
                }

                if (enemyStats != null)
                {
                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                    enemyStats.totalPoiseDefense = enemyStats.totalPoiseDefense - poiseBreak;
                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    enemyEffectsManager.PlayBloodSplatterEffects(contactPoint);

                    if (enemyStats.isBoss)
                    {
                        if (enemyStats.totalPoiseDefense > poiseBreak)
                        {
                            enemyStats.TakeDamageNoAnimation(currentWeaponDamage, playerStats);
                        }
                        else
                        {
                            enemyStats.TakeDamageNoAnimation(currentWeaponDamage, playerStats);
                            enemyStats.BreakGuard();
                        }
                    }
                    else
                    {
                        if (enemyStats.totalPoiseDefense > poiseBreak)
                        {
                            enemyStats.TakeDamageNoAnimation(currentWeaponDamage, playerStats);

                        }
                        else
                        {
                            enemyStats.TakeDamage(currentWeaponDamage, damagedBy: playerStats);
                        }
                    }
                }
            }

            if (collision.tag == "Illusionary wall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();

                illusionaryWall.wallHasBeenHit = true;
            }
        }
    }
}

