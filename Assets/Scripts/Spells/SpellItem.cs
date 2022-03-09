using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class SpellItem : Item
    {
        public GameObject spellWarmUpFX;
        public GameObject spellCastFX;
        public string spellAnimation;

        [Header("Spell Cost")]
        public int focusPointCost;

        [Header("Spell Type")]
        public bool isFaithSpell;
        public bool isMagicSpell;
        public bool isPyroSpell;

        [Header("Spell Description")]
        [TextArea] public string spellDescription;

        public virtual void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats, PlayerWeaponSlotManager weaponSlot)
        {
            Debug.Log("You attempted to cast a spell");
        }

        public virtual void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats, CameraHandler cameraHandler, PlayerWeaponSlotManager weaponSlot)
        {
            Debug.Log("You successfully cast a spell");
            playerStats.TakeFocusPointsDamage(focusPointCost);
        }
    }
}
