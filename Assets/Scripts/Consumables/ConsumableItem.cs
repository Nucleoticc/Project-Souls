using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class ConsumableItem : Item
    {
        [Header("Item Quantity")]
        public int maxItemAmount;
        public int currentItemAmount;

        [Header("Item Model")]
        public GameObject itemModel;

        [Header("Animations")]
        public string consumableAnimation;
        public bool isInteracting;

        public virtual void AttemptToConsumeItem(PlayerAnimatorHandler playerAnimatorHandler, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            if (currentItemAmount > 0)
            {
                playerAnimatorHandler.PlayTargetAnimation(consumableAnimation, isInteracting, true);
            }
            else
            {
                playerAnimatorHandler.PlayTargetAnimation("Shrug", true);
            }
        }

    }
}

