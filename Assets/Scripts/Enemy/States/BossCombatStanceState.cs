using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class BossCombatStanceState : CombatStanceState
    {
        [Header("Second Phase Attacks")]
        public bool hasPhaseShifted;
        public EnemyAttackAction[] secondPhaseAttacks;

        protected override void GetNewAttack(EnemyManager enemyManager)
        {
            if (hasPhaseShifted)
            {
                Vector3 targetsDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);
                float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

                int maxScore = 0;

                for (int i = 0; i < secondPhaseAttacks.Length; i++)
                {
                    if (distanceFromTarget <= secondPhaseAttacks[i].maximumDistanceNeededToAttack
                        && distanceFromTarget >= secondPhaseAttacks[i].minimumDistanceNeededToAttack)
                    {
                        if (viewableAngle <= secondPhaseAttacks[i].maximumAttackAngle
                            && viewableAngle >= secondPhaseAttacks[i].minimumAttackAngle)
                        {
                            maxScore += secondPhaseAttacks[i].attackScore;
                        }
                    }
                }

                int randomValue = Random.Range(0, maxScore);
                int temporaryScore = 0;

                for (int i = 0; i < secondPhaseAttacks.Length; i++)
                {
                    if (distanceFromTarget <= secondPhaseAttacks[i].maximumDistanceNeededToAttack
                         && distanceFromTarget >= secondPhaseAttacks[i].minimumDistanceNeededToAttack)
                    {
                        if (viewableAngle <= secondPhaseAttacks[i].maximumAttackAngle
                            && viewableAngle >= secondPhaseAttacks[i].minimumAttackAngle)
                        {
                            if (attackState.currentAttack != null) { return; }

                            temporaryScore += secondPhaseAttacks[i].attackScore;

                            if (temporaryScore > randomValue)
                            {
                                attackState.currentAttack = secondPhaseAttacks[i];
                            }
                        }
                    }
                }
            }
            else
            {
                base.GetNewAttack(enemyManager);
            }
        }
    }

}
