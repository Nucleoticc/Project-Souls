using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        [Header("Weapon Slots")]
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        protected WeaponHolderSlot backWeaponSlot;
        protected WeaponHolderSlot backShieldSlot;

        [Header("Weapong Colliders")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        [Header("Unarmed Weapon")]
        public WeaponItem unarmedWeapon;
    }
}
