using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class IdleState : State
    {
        public PursueTargetState pursueTargetState;
        public LayerMask detectionLayer;
        public EnemyAnimatorHandler enemyAnimatorHandler;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            if (enemyStats.currentHealth <= 0)
            {
                return this;
            }
            #region Enemy Target Detection
            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    Vector3 targetDirection = characterStats.transform.position - enemyManager.transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maxmimumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                    }
                }
            }
            #endregion

            #region Switching to next State
            if (enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
            #endregion
        }
    }
}

