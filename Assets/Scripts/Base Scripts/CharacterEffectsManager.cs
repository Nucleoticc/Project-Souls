using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        [Header("Damage Effects")]
        public GameObject bloodSplatterFX;

        [Header("Weapon Effects")]
        public WeaponFX rightWeaponFX;
        public WeaponFX leftWeaponFX;

        public virtual void PlayWeaponFX(bool isLeft)
        {
            if (isLeft)
            {
                if (leftWeaponFX != null)
                {
                    rightWeaponFX.PlayWeaponFX();
                }
            }
            else
            {
                if (rightWeaponFX != null)
                {
                    rightWeaponFX.PlayWeaponFX();
                }
            }
        }

        public virtual void PlayBloodSplatterEffects(Vector3 bloodSplatterLocation)
        {
            GameObject bloodSplatter = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
            Destroy(bloodSplatter, 2.0f);
        }
    }
}

