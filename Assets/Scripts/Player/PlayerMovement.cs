using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class PlayerMovement : MonoBehaviour
    {
        CameraHandler cameraHandler;
        Transform cameraObject;
        InputHandler inputHandler;

        PlayerManager playerManager;
        PlayerStats playerStats;

        public Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public PlayerAnimatorHandler playerAnimatorHandler;

        public new Rigidbody rigidbody;
        GameObject normalCamera;

        [Header("Ground and Air Detection Stats")]
        [SerializeField] float groundDetectionRayStartPoint = 0.5f;
        [SerializeField] float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField] float groundDirectionRayDistance = 0.2f;
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField] float movementSpeed = 5f;
        [SerializeField] float rotationSpeed = 10f;
        [SerializeField] float sprintSpeed = 7f;
        [SerializeField] float fallingSpeed = 45f;

        [Header("Stamina Costs")]
        [SerializeField] float rollStaminaCost = 15f;
        [SerializeField] float sprintStaminaCost = 1f;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        [Header("Movement Vectors")]
        Vector3 normalVector;
        Vector3 targetPosition;

        void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            playerAnimatorHandler = GetComponent<PlayerAnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
        }

        void Start()
        {
            cameraObject = Camera.main.transform;

            myTransform = transform;

            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 11);

            Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        }

        #region Movement
        public void HandleRotation(float delta)
        {
            if (playerAnimatorHandler.canRotate)
            {
                if (inputHandler.lockOnFlag)
                {
                    if (inputHandler.sprintFlag || inputHandler.rollFlag)
                    {
                        Vector3 targetDir = Vector3.zero;
                        targetDir = cameraHandler.cameraTransform.forward * inputHandler.vertical + cameraHandler.cameraTransform.right * inputHandler.horizontal;
                        targetDir.Normalize();
                        targetDir.y = 0;

                        if (targetDir == Vector3.zero)
                        {
                            targetDir = myTransform.forward;
                        }

                        Quaternion tr = Quaternion.LookRotation(targetDir);
                        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rotationSpeed * delta);

                        myTransform.rotation = targetRotation;
                    }
                    else
                    {
                        Vector3 rotationDirection = moveDirection;
                        rotationDirection = cameraHandler.currentLockOnTarget.transform.position - myTransform.position;
                        rotationDirection.y = 0;
                        rotationDirection.Normalize();

                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rotationSpeed * delta);
                        transform.rotation = targetRotation;
                    }
                }
                else
                {
                    Vector3 targetDir = Vector3.zero;
                    float moveOverride = inputHandler.moveAmount;

                    targetDir = cameraObject.forward * inputHandler.vertical + cameraObject.right * inputHandler.horizontal;
                    targetDir.Normalize();
                    targetDir.y = 0;

                    if (targetDir == Vector3.zero)
                    {
                        targetDir = myTransform.forward;
                    }

                    float rs = rotationSpeed;
                    Quaternion tr = Quaternion.LookRotation(targetDir);
                    Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rotationSpeed * delta);

                    myTransform.rotation = targetRotation;
                }
            }
        }

        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag)
            {
                return;
            }

            if (playerManager.isInteracting)
            {
                return;
            }

            moveDirection = cameraObject.forward * inputHandler.vertical + cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
            {
                speed = sprintSpeed;
                playerManager.isSprinting = true;
                moveDirection *= speed;
                playerStats.TakeStaminaDamage(Mathf.RoundToInt(sprintStaminaCost));
            }
            else
            {
                moveDirection *= speed;
                playerManager.isSprinting = false;
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            if (inputHandler.lockOnFlag && !inputHandler.sprintFlag)
            {
                playerAnimatorHandler.updateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
            }
            else
            {
                playerAnimatorHandler.updateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (playerAnimatorHandler.animator.GetBool("isInteracting"))
            {
                return;
            }

            if (playerStats.currentStamina <= 0)
            {
                return;
            }

            if (inputHandler.rollFlag)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical + cameraObject.right * inputHandler.horizontal;
                moveDirection.Normalize();

                if (inputHandler.moveAmount > 0)
                {
                    playerAnimatorHandler.PlayTargetAnimation("Rolling", true);
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                    playerStats.TakeStaminaDamage(Mathf.RoundToInt(rollStaminaCost));
                }
                else
                {
                    playerAnimatorHandler.PlayTargetAnimation("BackStep", true);
                    playerStats.TakeStaminaDamage(Mathf.RoundToInt(rollStaminaCost));
                }
            }
        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if (playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
            }

            Vector3 dir = moveDirection.normalized;
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;

            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        Debug.Log($"Your were in air for {inAirTimer}");
                        playerAnimatorHandler.PlayTargetAnimation("Landing", true);
                        inAirTimer = 0;
                    }
                    else
                    {
                        playerAnimatorHandler.PlayTargetAnimation("Empty", true);
                        inAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }
            }
            else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                }

                if (!playerManager.isInAir)
                {
                    if (!playerManager.isInteracting)
                    {
                        playerAnimatorHandler.PlayTargetAnimation("Falling", true);
                    }

                    Vector3 vel = rigidbody.velocity.normalized;
                    rigidbody.velocity = vel * (movementSpeed / 2);
                    playerManager.isInAir = true;
                }
            }

            if (playerManager.isGrounded)
            {
                if (playerManager.isInteracting || inputHandler.moveAmount > 0f)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
                }
                else
                {
                    myTransform.position = targetPosition;
                }
            }
        }

        public void HandleJumping()
        {
            if (playerManager.isInteracting) { return; }

            if (playerStats.currentStamina <= 0)
            {
                return;
            }

            if (inputHandler.jump_Input)
            {
                if (inputHandler.moveAmount > 0)
                {
                    moveDirection = cameraObject.forward * inputHandler.vertical + cameraObject.right * inputHandler.horizontal;

                    playerAnimatorHandler.PlayTargetAnimation("Jump", true);
                    moveDirection.y = 0;
                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = jumpRotation;
                }
            }
        }
        #endregion
    }
}
