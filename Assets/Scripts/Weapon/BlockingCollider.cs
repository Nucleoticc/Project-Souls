using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class BlockingCollider : MonoBehaviour
    {
        public BoxCollider blockingCollider;

        public float blockingPhysicalDamageAbsorption;

        void Awake()
        {
            blockingCollider = GetComponent<BoxCollider>();
        }

        public void SetColliderDamageAbsorption(WeaponItem weaponItem)
        {
            if (weaponItem != null)
            {
                blockingPhysicalDamageAbsorption = weaponItem.physicalDamageAbsorption;
            }
        }

        public void EnableBlockingCollider()
        {
            blockingCollider.enabled = true;
        }

        public void DisableBlockingCollider()
        {
            blockingCollider.enabled = false;
        }
    }
}
