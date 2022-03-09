using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class PursueTargetState : State
    {
        public CombatStanceState combatStanceState;
        public RotateTowardsTargetState rotateTowardsTargetState;
        public IdleState idleState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorHandler enemyAnimatorHandler)
        {
            if (enemyStats.currentHealth <= 0)
            {
                return this;
            }
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

            if (enemyManager.isInteracting) { return this; }

            HandleRotationTowardsTarget(enemyManager);


            if (enemyStats.currentHealth <= 0)
            {
                return idleState;
            }
            
            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorHandler.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                enemyAnimatorHandler.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            if (distanceFromTarget <= enemyManager.maximumAggroRadius)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }
        }

        private void HandleRotationTowardsTarget(EnemyManager enemyManager)
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
    }
}
