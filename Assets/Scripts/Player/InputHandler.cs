using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool a_Input;
        public bool y_Input;
        public bool x_Input;

        public bool rb_Input;
        public bool lb_Input;
        public bool rt_Input;
        public bool lt_Input;
        public bool critical_Attack_Input;

        public bool jump_Input;

        public bool inventory_Input;

        public bool lockOn_Input;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

        public bool rollFlag;
        public bool sprintFlag;
        public bool twoHandFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool inventoryFlag;
        public float rollInputTimer;

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerWeaponSlotManager weaponSlotManager;
        PlayerEffectsManager playerEffectsManager;
        PlayerAnimatorHandler playerAnimatorHandler;
        PlayerManager playerManager;
        PlayerStats playerStats;
        BlockingCollider blockingCollider;
        CameraHandler cameraHandler;
        AnimatorHandler animatorHandler;
        UIManager uiManager;

        Vector2 movementInput;
        Vector2 cameraInput;

        public Transform criticalAttackRayCastStartPoint;

        void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.RB.performed += i => rb_Input = true;
                inputActions.PlayerActions.LB.performed += i => lb_Input = true;
                inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
                inputActions.PlayerActions.RT.performed += i => rt_Input = true;
                inputActions.PlayerActions.LT.performed += i => lt_Input = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
                inputActions.PlayerActions.A.performed += i => a_Input = true;
                inputActions.PlayerActions.X.performed += i => x_Input = true;
                inputActions.PlayerActions.Roll.performed += i => b_Input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerActions.Y.performed += i => y_Input = true;
                inputActions.PlayerActions.CriticalAttack.performed += i => critical_Attack_Input = true;
            }

            inputActions.Enable();
        }

        void OnDisable()
        {
            inputActions.Disable();
        }

        void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            blockingCollider = GetComponentInChildren<BlockingCollider>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            uiManager = FindObjectOfType<UIManager>();
            playerAnimatorHandler = GetComponent<PlayerAnimatorHandler>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
        }

        public void TickInput(float delta)
        {
            HandleMoveInput(delta);
            HandleRollInput(delta);
            HandleCombatInput(delta);
            HandleQuickSlotInput();
            HandleInventoryInput();
            HandleUseConsumableInput();
            HandleLockOnInput();
            HandleTwoHandInput();
            HandleCriticalAttackInput();
        }

        void HandleMoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        void HandleRollInput(float delta)
        {
            if (b_Input)
            {
                rollInputTimer += delta;

                if (playerStats.currentStamina <= 0)
                {
                    b_Input = false;
                    sprintFlag = false;
                }

                if (moveAmount > 0.5f && playerStats.currentStamina > 0)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                sprintFlag = false;
                if (rollInputTimer > 0 && rollInputTimer < 0.5)
                {
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        void HandleCombatInput(float delta)
        {
            if (inventoryFlag)
            {
                return;
            }
            
            if (rb_Input)
            {
                playerAttacker.HandleRBAction();
            }

            if (lb_Input)
            {
                playerAttacker.HandleLBAction();
            }
            else
            {
                playerManager.isBlocking = false;

                if (blockingCollider.blockingCollider.enabled)
                {
                    blockingCollider.blockingCollider.enabled = false;
                }
            }

            if (rt_Input)
            {
                playerAttacker.HandleRTAction();
            }

            if (lt_Input)
            {
                if (twoHandFlag)
                {

                }
                else
                {
                    playerAttacker.HandleLTAction();
                }
            }
        }

        void HandleQuickSlotInput()
        {
            if (d_Pad_Right)
            {
                playerInventory.ChangeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }

        void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;
                if (inventoryFlag)
                {
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                    uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.hudWindow.SetActive(true);
                    uiManager.CloseAllInventoryWindows();
                }
            }
        }

        void HandleUseConsumableInput()
        {
            if (x_Input)
            {
                x_Input = false;
                playerInventory.currentConsumableItem.AttemptToConsumeItem(playerAnimatorHandler, weaponSlotManager, playerEffectsManager);
            }
        }

        public void HandleLockOnInput()
        {
            if (lockOn_Input && !lockOnFlag)
            {
                lockOn_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.SetCameraHeight();
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    cameraHandler.currentLockOnTarget.DisplayHealthBar(true);
                    lockOnFlag = true;
                }
            }
            else if (lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                cameraHandler.SetCameraHeight();
                cameraHandler.currentLockOnTarget.DisplayHealthBar(false);
                cameraHandler.ClearLockOnTargets();
            }

            if (lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget.DisplayHealthBar(false);
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                    cameraHandler.currentLockOnTarget.DisplayHealthBar(true);
                }
            }
            else if (lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget.DisplayHealthBar(false);
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                    cameraHandler.currentLockOnTarget.DisplayHealthBar(true);
                }
            }
        }

        void HandleTwoHandInput()
        {
            if (y_Input)
            {
                y_Input = false;
                twoHandFlag = !twoHandFlag;

                if (twoHandFlag)
                {
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                }
                else
                {
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                }
            }
        }

        void HandleCriticalAttackInput()
        {
            if (critical_Attack_Input)
            {
                critical_Attack_Input = false;
                playerAttacker.AttemptBackstapOrRiposte();
            }
        }
    }
}
