using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Souls
{
    public class HandEquipmentSlotUI : MonoBehaviour
    {
        UIManager uiManager;

        public Image icon;
        WeaponItem weaponItem;

        public bool rightHandSlot01;
        public bool rightHandSlot02;

        public bool leftHandSlot01;
        public bool leftHandSlot02;

        void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
        }

        public void AddItem(WeaponItem newWeapon)
        {
            weaponItem = newWeapon;
            icon.sprite = weaponItem.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearItem()
        {
            weaponItem = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void SelectThisSlot()
        {
            if (rightHandSlot01)
            {
                uiManager.rightHandSlot01Selected = true;
            }
            else if (rightHandSlot02)
            {
                uiManager.rightHandSlot02Selected = true;
            }
            else if (leftHandSlot01)
            {
                uiManager.leftHandSlot01Selected = true;
            }
            else
            {
                uiManager.leftHandSlot02Selected = true;
            }
        }
    }
}

