using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        [Header("Attacking Weapon")]
        public WeaponItem attackingWeapon;

        Animator animator;

        QuickSlotsUI quickSlotsUI;

        PlayerStats playerStats;
        PlayerInventory playerInventory;
        InputHandler inputHandler;
        PlayerManager playerManager;
        PlayerEffectsManager playerEffectsManager;

        void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            playerStats = GetComponent<PlayerStats>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            animator = GetComponent<Animator>();
            LoadWeaponHolderSlots();
        }

        void LoadWeaponHolderSlots()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponHolderSlot in weaponHolderSlots)
            {
                if (weaponHolderSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponHolderSlot;
                }
                else if (weaponHolderSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponHolderSlot;
                }
                else if (weaponHolderSlot.isBackWeaponSlot)
                {
                    backWeaponSlot = weaponHolderSlot;
                }
                else if (weaponHolderSlot.isBackShieldSlot)
                {
                    backShieldSlot = weaponHolderSlot;
                }
            }
        }

        public void LoadRightHandWeaponOnSlot()
        {
            // LoadWeaponOnSlot(playerInventory.leftWeapon, true);
            LoadWeaponOnSlot(playerInventory.rightWeapon, false);
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    if (inputHandler.twoHandFlag)
                    {
                        inputHandler.twoHandFlag = false;
                        backWeaponSlot.UnloadWeaponAndDestroy();
                        backShieldSlot.UnloadWeaponAndDestroy();
                        animator.CrossFade("Both Arms Empty", 0.2f);
                    }
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponItem, true);
                    animator.CrossFade(weaponItem.Left_Hand_Idle, 0.2f);
                }
                else
                {
                    if (inputHandler.twoHandFlag)
                    {
                        if (leftHandSlot.currentWeapon.isShieldWeapon)
                        {
                            backShieldSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        }
                        else
                        {
                            backWeaponSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        }
                        leftHandSlot.UnloadWeaponAndDestroy();
                        animator.CrossFade(weaponItem.TH_Idle, 0.2f);
                    }
                    else
                    {
                        animator.CrossFade("Both Arms Empty", 0.2f);

                        backWeaponSlot.UnloadWeaponAndDestroy();
                        backShieldSlot.UnloadWeaponAndDestroy();
                        animator.CrossFade(weaponItem.Right_Hand_Idle, 0.2f);
                    }

                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponItem, false);
                }
            }
            else
            {
                if (isLeft)
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                    playerInventory.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = unarmedWeapon;
                    leftHandSlot.LoadWeaponModel(unarmedWeapon);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(unarmedWeapon, true);
                }
                else
                {
                    animator.CrossFade("Right Arm Empty", 0.2f);
                    playerInventory.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = unarmedWeapon;
                    rightHandSlot.LoadWeaponModel(unarmedWeapon);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(unarmedWeapon, false);
                }
            }
        }

        #region  Handle Weapon's Damage Collider
        void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
            leftHandDamageCollider.poiseBreak = playerInventory.leftWeapon.poiseBreak;
            leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            playerEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }

        void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = playerInventory.rightWeapon.baseDamage;
            rightHandDamageCollider.poiseBreak = playerInventory.rightWeapon.poiseBreak;
            rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
            playerEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }

        public void OpenDamageCollider()
        {
            if (playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if (playerManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }

        public void CloseDamageCollider()
        {
            if (rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DisableDamageCollider();
            }
            if (leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DisableDamageCollider();
            }
        }
        #endregion

        #region  Handle Weapon's Stamina Drain
        public void DrainStaminaLightAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }
        #endregion

        public void GrantWeaponAttackingPoiseBonus()
        {
            playerStats.totalPoiseDefense += attackingWeapon.offensivePoiseBonus;
        }

        public void ResetWeaponAttackingPoiseBonus()
        {
            playerStats.totalPoiseDefense = playerStats.armorPoiseBonus;
        }
    }
}

