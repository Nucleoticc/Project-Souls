using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class AnimatorHandler : MonoBehaviour
    {
        public Animator animator;
        protected CharacterManager characterManager;
        protected CharacterStats characterStats;
        public bool canRotate;

        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            characterManager = GetComponent<CharacterManager>();
            characterStats = GetComponent<CharacterStats>();
        }

        public void PlayTargetAnimation(string targetAnim, bool isInteracting, bool canRotate = false)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("canRotate", canRotate);
            animator.SetBool("isInteracting", isInteracting);

            animator.CrossFade(targetAnim, 0.2f);
        }

        public void PlayTargetAnimationWithRootRotation(string targetAnim, bool isInteracting)
        {
            animator.applyRootMotion = isInteracting;
            animator.SetBool("isRotatingWithRootMotion", true);
            animator.SetBool("isInteracting", isInteracting);

            animator.CrossFade(targetAnim, 0.2f);
        }

        public virtual void CanRotate()
        {
            animator.SetBool("canRotate", true);
        }

        public virtual void StopRotation()
        {
            animator.SetBool("canRotate", false);
        }

        public virtual void EnableCombo()
        {
            animator.SetBool("canDoCombo", true);
        }

        public virtual void DisableCombo()
        {
            animator.SetBool("canDoCombo", false);
        }

        public virtual void EnableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", true);
        }

        public virtual void DisableIsInvulnerable()
        {
            animator.SetBool("isInvulnerable", false);
        }

        public virtual void EnableIsParrying()
        {
            characterManager.isParrying = true;
        }

        public virtual void DisableIsParrying()
        {
            characterManager.isParrying = false;
        }

        public virtual void EnableCanbeRiposted()
        {
            characterManager.canBeRiposted = true;
        }

        public virtual void DisableCanbeRiposted()
        {
            characterManager.canBeRiposted = false;
        }

        public virtual void TakeCriticalDamageAnimationEvent()
        {
            characterStats.TakeDamageNoAnimation(characterManager.pendingCriticalDamage);
            characterManager.pendingCriticalDamage = 0;
        }
    }

}
