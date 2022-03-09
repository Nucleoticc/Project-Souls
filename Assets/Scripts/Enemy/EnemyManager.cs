using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace Souls
{
    public class EnemyManager : CharacterManager
    {
        EnemyAnimatorHandler enemyAnimatorHandler;
        EnemyStats enemyStats;

        public NavMeshAgent navMeshAgent;
        public Rigidbody rb;

        public State currentState;
        public CharacterStats currentTarget;

        public bool isPerformingAction;
        public float rotationSpeed = 25f;
        public float maximumAggroRadius = 1.5f;

        [Header("AI Settings")]
        public float detectionRadius = 20f;
        public float minimumDetectionAngle = -50f;
        public float maxmimumDetectionAngle = 50f;
        public float currentRecoveryTime = 0f;

        [Header("AI Combat Settings")]
        public bool allowAIToPerformCombo;
        public bool isPhaseShifting;
        public float comboLikelyhood;

        void Awake()
        {
            enemyAnimatorHandler = GetComponent<EnemyAnimatorHandler>();
            enemyStats = GetComponent<EnemyStats>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            rb = GetComponent<Rigidbody>();
        }

        void Start()
        {
            navMeshAgent.enabled = false;
            rb.isKinematic = false;
        }

        void Update()
        {
            HandleRecoveryTimer();
            HandleStateMachine();

            isRotatingWithRootMotion = enemyAnimatorHandler.animator.GetBool("isRotatingWithRootMotion");
            isInteracting = enemyAnimatorHandler.animator.GetBool("isInteracting");
            isPhaseShifting = enemyAnimatorHandler.animator.GetBool("isPhaseShifting");
            isInvulnerable = enemyAnimatorHandler.animator.GetBool("isInvulnerable");
            // canDoCombo = enemyAnimatorHandler.animator.GetBool("canDoCombo");
            canRotate = enemyAnimatorHandler.animator.GetBool("canRotate");
            enemyAnimatorHandler.animator.SetBool("isDead", enemyStats.isDead);
        }

        void LateUpdate()
        {
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorHandler);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        void SwitchToNextState(State nextState)
        {
            currentState = nextState;
        }

        void HandleRecoveryTimer()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }

        public override void DisplayHealthBar(bool status)
        {
            if (!enemyStats.isBoss)
            {
                enemyStats.uiEnemyHealthBar.isLockedOn = status;
            }
        }
    }
}

