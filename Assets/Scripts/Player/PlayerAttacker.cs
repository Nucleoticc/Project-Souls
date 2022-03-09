using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class PlayerAttacker : MonoBehaviour
    {
        PlayerAnimatorHandler animatorHandler;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerManager playerManager;
        InputHandler inputHandler;
        PlayerInventory playerInventory;
        PlayerWeaponSlotManager weaponSlotManager;
        PlayerStats playerStats;
        CameraHandler cameraHandler;
        PlayerEffectsManager playerEffectsManager;

        public string lastAttack;

        LayerMask backStabLayer = 1 << 9;
        LayerMask riposteLayer = 1 << 10;

        void Awake()
        {
            animatorHandler = GetComponent<PlayerAnimatorHandler>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            inputHandler = GetComponent<InputHandler>();
            playerManager = GetComponent<PlayerManager>();
            playerInventory = GetComponent<PlayerInventory>();
            playerStats = GetComponent<PlayerStats>();
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weaponItem)
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }
            if (inputHandler.comboFlag)
            {
                animatorHandler.animator.SetBool("canDoCombo", false);
                if (lastAttack == weaponItem.OH_Light_Attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weaponItem.OH_Light_Attack_02, true);
                }
                else if (lastAttack == weaponItem.TH_Light_Attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weaponItem.TH_Light_Attack_02, true);
                }
                else
                {
                    HandleLightAttack(weaponItem);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weaponItem)
        {
            if (animatorHandler.animator.GetBool("isInteracting")) { return; }
            if (weaponItem.isUnarmed) { return; }
            if (playerStats.currentStamina <= 0)
            {
                return;
            }

            weaponSlotManager.attackingWeapon = weaponItem;
            playerEffectsManager.PlayWeaponFX(false);

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weaponItem.TH_Light_Attack_01, true);
                lastAttack = weaponItem.TH_Light_Attack_01;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weaponItem.OH_Light_Attack_01, true);
                lastAttack = weaponItem.OH_Light_Attack_01;
            }
        }

        public void HandleHeavyAttack(WeaponItem weaponItem)
        {
            if (animatorHandler.animator.GetBool("isInteracting")) { return; }
            if (weaponItem.isUnarmed) { return; }
            if (playerStats.currentStamina <= 0)
            {
                return;
            }

            weaponSlotManager.attackingWeapon = weaponItem;
            animatorHandler.animator.SetBool("isUsingRightHand", true);

            playerEffectsManager.PlayWeaponFX(false);

            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weaponItem.TH_Heavy_Attack_01, true);
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weaponItem.OH_Heavy_Attack_01, true);
            }
        }

        public void HandleRBAction()
        {
            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                PerformRBMeleeAction();
            }
            else if (playerInventory.rightWeapon.isMagicCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
            {
                PerformRBCasterAction(playerInventory.rightWeapon);
            }
        }

        public void HandleRTAction()
        {
            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }

        public void HandleLBAction()
        {
            if (playerInventory.leftWeapon.isShieldWeapon && !inputHandler.twoHandFlag)
            {
                PerformLBBlockingAction();
            }
        }

        public void HandleLTAction()
        {
            if (playerInventory.leftWeapon.isShieldWeapon)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);
            }
            else if (playerInventory.leftWeapon.isMeleeWeapon)
            {

            }
        }
        void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventory.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting) { return; }

                animatorHandler.animator.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventory.rightWeapon);
            }
        }

        void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting) { return; }

            if (isTwoHanding)
            {
            }
            else
            {
                animatorHandler.PlayTargetAnimation(playerInventory.leftWeapon.weapon_art, true);
            }
        }

        public void AttemptBackstapOrRiposte()
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }
            RaycastHit hit;

            if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                if (enemyCharacterManager != null)
                {
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamagerStandPosition.position;

                    Vector3 rotationDirection;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    animatorHandler.PlayTargetAnimation("Back Stab", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorHandler>().PlayTargetAnimation("Back Stabbed", true);
                }
            }

            else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
                {
                    playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamagerStandPosition.position;

                    Vector3 rotationDirection;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    // Multiplying criticalDamageMultiplier by 2 because the damage is doubled when the enemy is riposted
                    int criticalDamage = (playerInventory.rightWeapon.criticalDamageMultiplier * 2) * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    animatorHandler.PlayTargetAnimation("Riposte", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorHandler>().PlayTargetAnimation("Riposted", true);
                    enemyCharacterManager.canBeRiposted = false;
                }
            }
        }

        void PerformRBCasterAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting)
            {
                return;
            }

            if (weapon.isMagicCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isMagicSpell)
                {
                    // Check for FP
                    // Attempt to Cast
                }
            }
            else if (weapon.isFaithCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    if (playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);
                    }
                }
            }
            else if (weapon.isPyroCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isPyroSpell)
                {
                    if (playerStats.currentFocusPoints >= playerInventory.currentSpell.focusPointCost)
                    {
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);
                    }
                }
            }
        }

        void PerformLBBlockingAction()
        {
            if (playerManager.isInteracting)
            {
                return;
            }

            if (playerManager.isBlocking)
            {
                return;
            }

            animatorHandler.PlayTargetAnimation("Block_Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }

        void SuccessfullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats, cameraHandler, weaponSlotManager);
            animatorHandler.animator.SetBool("isFiringSpell", true);
        }
    }
}
