using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Souls
{
    public class QuickSlotsUI : MonoBehaviour
    {
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;

        public void UpdateWeaponQuickSlotsUI(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                if (weaponItem.itemIcon != null)
                {
                    leftWeaponIcon.sprite = weaponItem.itemIcon;
                    leftWeaponIcon.enabled = true;
                }
                else
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
            }
            else
            {
                if (weaponItem.itemIcon != null)
                {
                    rightWeaponIcon.sprite = weaponItem.itemIcon;
                    rightWeaponIcon.enabled = true; 
                }
                else
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }
            }
        }
    }
}

