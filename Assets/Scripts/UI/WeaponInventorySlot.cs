using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Souls
{
    public class WeaponInventorySlot : MonoBehaviour
    {
        PlayerInventory playerInventory;
        PlayerWeaponSlotManager weaponSlotManager;
        UIManager uiManager;

        public Image icon;
        WeaponItem weaponItem;

        void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            weaponSlotManager = FindObjectOfType<PlayerWeaponSlotManager>();
            uiManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(WeaponItem newItem)
        {
            weaponItem = newItem;
            icon.sprite = weaponItem.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            weaponItem = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void EquipThisItem()
        {
            if (uiManager.rightHandSlot01Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[0]);
                playerInventory.weaponsInRightHandSlots[0] = weaponItem;
                playerInventory.weaponsInventory.Remove(weaponItem);
            }
            else if (uiManager.rightHandSlot02Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlots[1]);
                playerInventory.weaponsInRightHandSlots[1] = weaponItem;
                playerInventory.weaponsInventory.Remove(weaponItem);
            }
            else if (uiManager.leftHandSlot01Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[0]);
                playerInventory.weaponsInLeftHandSlots[0] = weaponItem;
                playerInventory.weaponsInventory.Remove(weaponItem);
            }
            else if(uiManager.leftHandSlot02Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlots[1]);
                playerInventory.weaponsInLeftHandSlots[1] = weaponItem;
                playerInventory.weaponsInventory.Remove(weaponItem);
            }

            playerInventory.rightWeapon = playerInventory.weaponsInRightHandSlots[playerInventory.currentRightWeaponIndex];
            playerInventory.leftWeapon = playerInventory.weaponsInLeftHandSlots[playerInventory.currentLeftWeaponIndex];

            weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);

            uiManager.equipmentWindowUI.LoadWeaponOnEquipmentScreen(playerInventory);
            uiManager.ResetAllSelectedSlots();
        }
    }
}
