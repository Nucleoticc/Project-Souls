using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class PlayerInventory : MonoBehaviour
    {
        PlayerWeaponSlotManager weaponSlotManager;

        public SpellItem currentSpell;
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public ConsumableItem currentConsumableItem;

        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

        public int currentRightWeaponIndex = 0;
        public int currentLeftWeaponIndex = 0;

        public List<WeaponItem> weaponsInventory;

        void Awake()
        {
            weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        }

        void Start()
        {
            rightWeapon = weaponsInRightHandSlots[0];
            leftWeapon = weaponsInLeftHandSlots[0];
            
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }

        public void ChangeRightWeapon()
        {
            currentRightWeaponIndex += 1;

            if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            }
            else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] == null)
            {
                currentRightWeaponIndex += 1;
            }
            else if (currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            }
            else
            {
                currentRightWeaponIndex += 1;
            }

            if (currentRightWeaponIndex > weaponsInRightHandSlots.Length - 1)
            {
                currentRightWeaponIndex = -1;
                rightWeapon = weaponSlotManager.unarmedWeapon;
                weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            }
        }

        public void ChangeLeftWeapon()
        {
            currentLeftWeaponIndex += 1;

            if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] != null)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            }
            else if (currentLeftWeaponIndex == 0 && weaponsInLeftHandSlots[0] == null)
            {
                currentLeftWeaponIndex += 1;
            }
            else if (currentLeftWeaponIndex == 1 && weaponsInLeftHandSlots[1] != null)
            {
                leftWeapon = weaponsInLeftHandSlots[currentLeftWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            }
            else
            {
                currentLeftWeaponIndex += 1;
            }

            if (currentLeftWeaponIndex > weaponsInLeftHandSlots.Length - 1)
            {
                currentLeftWeaponIndex = -1;
                leftWeapon = weaponSlotManager.unarmedWeapon;
                weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            }
        }
    }
}
