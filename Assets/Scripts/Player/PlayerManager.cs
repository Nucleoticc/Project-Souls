using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator animator;
        PlayerAnimatorHandler playerAnimatorHandler;
        CameraHandler cameraHandler;
        PlayerMovement playerMovement;
        PlayerStats playerStats;

        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;

        public GameObject itemInteractableUIGameObject;


        void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponent<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
            interactableUI = FindObjectOfType<InteractableUI>();
            playerStats = GetComponent<PlayerStats>();
            playerAnimatorHandler = GetComponent<PlayerAnimatorHandler>();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            isUsingRightHand = animator.GetBool("isUsingRightHand");
            isUsingLeftHand = animator.GetBool("isUsingLeftHand");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            animator.SetBool("isInAir", isInAir);
            animator.SetBool("isDead", playerStats.isDead);
            animator.SetBool("isBlocking", isBlocking);

            playerAnimatorHandler.canRotate = animator.GetBool("canRotate");

            inputHandler.TickInput(delta);
            playerMovement.HandleRollingAndSprinting(delta);
            playerMovement.HandleJumping();
            playerStats.RegenerateStamina();



            CheckForInteractableObject();
        }

        void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            playerMovement.HandleMovement(delta);
            playerMovement.HandleFalling(delta, playerMovement.moveDirection);
            playerMovement.HandleRotation(delta);
        }

        void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.lt_Input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;

            float delta = Time.fixedDeltaTime;
            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            if (isInAir)
            {
                playerMovement.inAirTimer += Time.deltaTime;
            }
        }

        #region Player Interactions

        void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactable = hit.collider.GetComponent<Interactable>();

                    if (interactable != null)
                    {
                        string interactableText = interactable.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);

                        if (inputHandler.a_Input)
                        {
                            interactable.Interact(this);
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if (itemInteractableUIGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableUIGameObject.SetActive(false);
                }
            }
        }

        public void OpenChestInteraction(Transform playerStandingPosition)
        {
            playerMovement.rigidbody.velocity = Vector3.zero;
            transform.position = playerStandingPosition.position;
            playerAnimatorHandler.PlayTargetAnimation("Open Chest", true);
        }

        public void PassThroughFogWallInteraction(Transform fogWallEntrance)
        {
            playerMovement.rigidbody.velocity = Vector3.zero;

            Vector3 rotationDirection = fogWallEntrance.transform.forward;
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;

            playerAnimatorHandler.PlayTargetAnimation("Pass Through Fog", true);
        }
        #endregion
    }
}
