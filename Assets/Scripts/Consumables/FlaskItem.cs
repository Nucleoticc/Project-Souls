using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    [CreateAssetMenu(menuName = "Items/Consumables/Flask")]
    public class FlaskItem : ConsumableItem
    {
        [Header("Flask Type")]
        public bool estusFlask;
        public bool ashenFlask;

        [Header("Recovery Amount")]
        public int healthRecoveryAmount;
        public int focusPointsRecoveryAmount;

        [Header("Recovery FX")]
        public GameObject recoveryFX;

        public override void AttemptToConsumeItem(PlayerAnimatorHandler playerAnimatorHandler, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            base.AttemptToConsumeItem(playerAnimatorHandler, weaponSlotManager, playerEffectsManager);
            GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
            playerEffectsManager.currentParticleFx = recoveryFX;
            playerEffectsManager.amountToBeHealed = healthRecoveryAmount;
            playerEffectsManager.instantiatedFXModel = flask;
            weaponSlotManager.rightHandSlot.UnloadWeapon();
        }
    }
}

