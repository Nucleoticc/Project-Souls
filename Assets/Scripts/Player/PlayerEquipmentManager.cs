using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventory playerInventory;
        public BlockingCollider blockingCollider;

        void Awake()
        {
            playerInventory = GetComponent<PlayerInventory>();
            inputHandler = GetComponent<InputHandler>();
        }

        public void OpenBlockingCollider()
        {
            if (inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventory.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorption(playerInventory.leftWeapon);
            }
            blockingCollider.EnableBlockingCollider();
        }

        public void DisableBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}

