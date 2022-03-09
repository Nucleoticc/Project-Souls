using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public EnemyAttackAction[] enemyAttacks;
        public PursueTargetState pursueTargetState;

        protected bool randomDestinationSet = false;
        protected float verticalMovementValue = 0;
        protected float horizontalMovementValue = 0;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            if (enemyStats.currentHealth <= 0)
            {
                return this;
            }
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            enemyAnimatorHandler.animator.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemyAnimatorHandler.animator.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            attackState.hasPerformedAttack = false;

            if (enemyManager.isInteracting)
            {
                enemyAnimatorHandler.animator.SetFloat("Vertical", 0);
                enemyAnimatorHandler.animator.SetFloat("Horizontal", 0);

                return this;
            }

            if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(enemyAnimatorHandler);
            }

            HandleRotationTowardsTarget(enemyManager);

            if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                randomDestinationSet = false;
                return attackState;
            }
            else
            {
                GetNewAttack(enemyManager);
            }
            return this;
        }

        protected void HandleRotationTowardsTarget(EnemyManager enemyManager)
        {
            //Rotate manually if performing action
            if (enemyManager.isPerformingAction)
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
            else
            {
                Vector3 relativeDirection = enemyManager.transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.rb.velocity;

                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.rb.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }

        protected void DecideCirclingAction(EnemyAnimatorHandler enemyAnimatorHandler)
        {
            WalkAroundTarget(enemyAnimatorHandler);
        }

        protected void WalkAroundTarget(EnemyAnimatorHandler enemyAnimatorHandler)
        {
            verticalMovementValue = 0.5f;

            horizontalMovementValue = Random.Range(-1, 1);

            if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }
        }

        protected virtual void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                if (distanceFromTarget <= enemyAttacks[i].maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttacks[i].minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttacks[i].maximumAttackAngle
                        && viewableAngle >= enemyAttacks[i].minimumAttackAngle)
                    {
                        maxScore += enemyAttacks[i].attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                if (distanceFromTarget <= enemyAttacks[i].maximumDistanceNeededToAttack
                     && distanceFromTarget >= enemyAttacks[i].minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttacks[i].maximumAttackAngle
                        && viewableAngle >= enemyAttacks[i].minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null) { return; }

                        temporaryScore += enemyAttacks[i].attackScore;

                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttacks[i];
                        }
                    }
                }
            }
        }
    }
}

