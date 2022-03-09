using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("LockOn Transform")]
        public Transform lockOnTransform;

        [Header("Combat Colliders")]
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider riposteCollider;

        [Header("Combat Flags")]
        public bool canBeRiposted;
        public bool canbeParried;
        public bool isParrying;
        public bool isBlocking;
        public bool isInvulnerable;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool canDoCombo;
        public bool isInteracting;

        [Header("Movement Flags")]
        public bool isRotatingWithRootMotion;
        public bool canRotate;
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;

        [Header("Spell Flags")]
        public bool isFiringSpell;

        //Damage will be inflicted during an animation event
        //Used in backstab or riposte animations
        public int pendingCriticalDamage;

        public virtual void DisplayHealthBar(bool status) { }
    }
}
