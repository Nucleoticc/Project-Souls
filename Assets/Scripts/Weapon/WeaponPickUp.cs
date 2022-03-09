using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Souls
{
    public class WeaponPickUp : Interactable
    {
        public WeaponItem weaponItem;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            PickUpItem(playerManager);
        }

        void PickUpItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory = playerManager.GetComponent<PlayerInventory>();
            PlayerMovement playerMovement = playerManager.GetComponent<PlayerMovement>();
            PlayerAnimatorHandler animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorHandler>();

            playerMovement.rigidbody.velocity = Vector3.zero;
            animatorHandler.PlayTargetAnimation("Pick Up Item", true);
            playerInventory.weaponsInventory.Add(weaponItem);
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<Text>().text = weaponItem.itemName;
            playerManager.itemInteractableUIGameObject.GetComponentInChildren<RawImage>().texture = weaponItem.itemIcon.texture;
            playerManager.itemInteractableUIGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}

