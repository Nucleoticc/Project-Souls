using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;
        public PursueTargetState pursueTargetState;
        public EnemyAttackAction currentAttack;
        public RotateTowardsTargetState rotateTowardsTargetState;

        public bool willDoComboOnNextAttack = false;
        public bool hasPerformedAttack = false;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            if (enemyStats.currentHealth <= 0)
            {
                return this;
            }
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            RotateTowardsTargetWhilstAttacking(enemyManager);

            if (currentAttack == null)
            {
                return combatStanceState;
            }

            if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if (willDoComboOnNextAttack && enemyManager.canDoCombo)
            {
                AttackTargetWithCombo(enemyAnimatorHandler, enemyManager);
            }
            if (!hasPerformedAttack)
            {
                AttackTarget(enemyAnimatorHandler, enemyManager);
                RollForComboChance(enemyManager);
            }

            if (willDoComboOnNextAttack && hasPerformedAttack)
            {
                return this;
            }

            return rotateTowardsTargetState;
        }

        void AttackTarget(EnemyAnimatorHandler enemyAnimatorHandler, EnemyManager enemyManager)
        {
            if (currentAttack != null)
            {
                enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
                enemyAnimatorHandler.PlayWeaponTrailFX();
                enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                hasPerformedAttack = true;
            }
        }

        void AttackTargetWithCombo(EnemyAnimatorHandler enemyAnimatorHandler, EnemyManager enemyManager)
        {
            if (currentAttack == null)
            {
                return;
            }
            willDoComboOnNextAttack = false;
            enemyAnimatorHandler.PlayTargetAnimation(currentAttack.actionAnimation, true);
            enemyAnimatorHandler.PlayWeaponTrailFX();
            enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
            currentAttack = null;
        }

        void RotateTowardsTargetWhilstAttacking(EnemyManager enemyManager)
        {
            //Rotate manually if performing action
            if (enemyManager.canRotate && enemyManager.isInteracting)
            {
                Vector3 targetDirection = (enemyManager.currentTarget.transform.position - enemyManager.transform.position).normalized;
                targetDirection.y = 0;

                if (targetDirection == Vector3.zero)
                {
                    targetDirection = enemyManager.transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
            }

        }

        void RollForComboChance(EnemyManager enemyManager)
        {
            if (currentAttack == null)
            {
                return;
            }
            if (enemyManager.allowAIToPerformCombo && Random.Range(0, 100) <= enemyManager.comboLikelyhood)
            {
                if (currentAttack.comboAction != null)
                {
                    willDoComboOnNextAttack = true;
                    currentAttack = currentAttack.comboAction;
                }
                else
                {
                    willDoComboOnNextAttack = false;
                    currentAttack = null;
                }
            }
        }
    }
}
