using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class RotateTowardsTargetState : State
    {
        public CombatStanceState combatStanceState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            if (enemyStats.currentHealth <= 0)
            {
                return this;
            }
            enemyAnimatorHandler.animator.SetFloat("Vertical", 0);
            enemyAnimatorHandler.animator.SetFloat("Horizontal", 0);

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

            if (enemyManager.isInteracting)
            {
                return this;
            }

            if (viewableAngle >= 100 && viewableAngle <= 180 && !enemyManager.isInteracting)
            {
                enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("Turn_Behind", true);
                return combatStanceState;
            }
            else if (viewableAngle <= -101 && viewableAngle >= -180 && !enemyManager.isInteracting)
            {
                enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("Turn_Behind", true);
                return combatStanceState;
            }
            else if (viewableAngle <= -45 && viewableAngle >= -100 && !enemyManager.isInteracting)
            {
                enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("Turn_Right", true);
                return combatStanceState;
            }
            else if (viewableAngle >= 45 && viewableAngle <= 100 && !enemyManager.isInteracting)
            {
                enemyAnimatorHandler.PlayTargetAnimationWithRootRotation("Turn_Left", true);
                return combatStanceState;
            }

            return combatStanceState;
        }
    }
}

