using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    [CreateAssetMenu(menuName = "Spells/Projectile Spell")]
    public class ProjectileSpell : SpellItem
    {
        [Header("Projectile Damage")]
        public float baseDamage;

        [Header("Projectile Physics")]
        public float projectileForwardVelocity;
        public float projectileUpwardVelocity;
        public float projectileMass;
        public bool isEffectedByGravity;

        Rigidbody rigidbody;

        public override void AttemptToCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats, PlayerWeaponSlotManager weaponSlot)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats, weaponSlot);
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlot.rightHandSlot.transform);
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
        }

        public override void SuccessfullyCastSpell(AnimatorHandler animatorHandler, PlayerStats playerStats, CameraHandler cameraHandler, PlayerWeaponSlotManager weaponSlot)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats, cameraHandler, weaponSlot);
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, weaponSlot.rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
            rigidbody = instantiatedSpellFX.GetComponent<Rigidbody>();

            if (cameraHandler.currentLockOnTarget != null)
            {
                instantiatedSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
            }
            else
            {
                instantiatedSpellFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStats.transform.eulerAngles.y, 0);
            }

            rigidbody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
            rigidbody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
            rigidbody.useGravity = isEffectedByGravity;
            rigidbody.mass = projectileMass;
            instantiatedSpellFX.transform.parent = null;
        }
    }
}