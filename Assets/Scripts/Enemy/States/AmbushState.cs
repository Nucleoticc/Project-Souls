using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class AmbushState : State
    {
        public bool isSleeping = true;
        public float detectionRadius = 2f;
        public string sleepAnimation;
        public string wakeAnimation;
        public LayerMask detectionLayer;

        public PursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            if(enemyStats.currentHealth <= 0)
            {
                return this;
            }

            if (isSleeping && !enemyManager.isInteracting)
            {
                enemyAnimatorHandler.PlayTargetAnimation(sleepAnimation, true);
            }

            #region Handle Target Detection
            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    Vector3 targetsDirection = characterStats.transform.position - enemyManager.transform.position;
                    float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle
                        && viewableAngle < enemyManager.maxmimumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                        isSleeping = false;
                        enemyAnimatorHandler.PlayTargetAnimation(wakeAnimation, true);
                    }
                }
            }
            #endregion

            #region Handle State Change
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

