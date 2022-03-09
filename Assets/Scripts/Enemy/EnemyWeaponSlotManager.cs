using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {
        public WeaponItem rightHandWeapon;
        public WeaponItem leftHandWeapon;

        EnemyStats enemyStats;
        EnemyEffectsManager enemyEffectsManager;

        void Awake()
        {
            enemyStats = GetComponent<EnemyStats>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
            LoadWeaponHolderSlots();
        }

        void Start()
        {
            LoadWeaponsOnBothHands();
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
            }
        }

        void LoadWeaponsOnBothHands()
        {
            if (rightHandWeapon != null)
            {
                LoadWeaponOnSlot(rightHandWeapon, false);
            }
            if (leftHandWeapon != null)
            {
                LoadWeaponOnSlot(leftHandWeapon, true);
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadWeaponsDamageCollider(true);
            }
            else
            {
                rightHandSlot.currentWeapon = weaponItem;
                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadWeaponsDamageCollider(false);
            }
        }

        public void LoadWeaponsDamageCollider(bool isLeft)
        {
            if (isLeft)
            {
                leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                leftHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
                enemyEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
            }
            else
            {
                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
                rightHandDamageCollider.characterManager = GetComponentInParent<CharacterManager>();
                enemyEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
            }
        }

        public void OpenDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        public void GrantWeaponAttackingPoiseBonus()
        {
            enemyStats.totalPoiseDefense += enemyStats.offensivePoiseBonus;
        }

        public void ResetWeaponAttackingPoiseBonus()
        {
            enemyStats.totalPoiseDefense = enemyStats.armorPoiseBonus;
        }
    }
}
