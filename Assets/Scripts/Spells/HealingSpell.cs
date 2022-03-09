using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    [CreateAssetMenu(menuName = "Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;
        GameObject instantiatedWarmUpSpellSFX;
        GameObject instantiatedSpellSFX;

        public override void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats, PlayerWeaponSlotManager weaponSlot)
        {
            if (spellWarmUpFX != null)
                instantiatedWarmUpSpellSFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
            base.AttemptToCastSpell(animatorHandler, playerStats, weaponSlot);
            Destroy(instantiatedWarmUpSpellSFX, 3f);
        }

        public override void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats, CameraHandler cameraHandler, PlayerWeaponSlotManager weaponSlot)
        {
            if (spellCastFX != null)
                instantiatedSpellSFX = Instantiate(spellCastFX, animatorHandler.transform);
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
            playerStats.HealPlayer(healAmount);
            base.SuccessfullyCastSpell(animatorHandler, playerStats, cameraHandler, weaponSlot);
            Destroy(instantiatedSpellSFX, 3f);
        }
    }
}

