using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    public class PlayerAnimatorHandler : AnimatorHandler
    {
        InputHandler inputHandler;
        PlayerMovement playerMovement;

        int vertical;
        int horizontal;

        protected override void Awake()
        {
            base.Awake();
            inputHandler = GetComponent<InputHandler>();
            playerMovement = GetComponent<PlayerMovement>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void updateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1f;
            }
            else { v = 0; }
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1f;
            }
            else { h = 0; }
            #endregion

            if (isSprinting)
            {
                v = 2;
            }

            animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void AwardSoulsOnDeath() { }

        public void EnableCollision()
        {
            playerMovement.characterCollider.enabled = true;
            playerMovement.characterCollisionBlockerCollider.enabled = true;
        }

        public void DisableCollision()
        {
            playerMovement.characterCollider.enabled = false;
            playerMovement.characterCollisionBlockerCollider.enabled = false;
        }

        void OnAnimatorMove()
        {
            if (characterManager.isInteracting == false)
            {
                return;
            }

            float delta = Time.deltaTime;
            playerMovement.rigidbody.drag = 0;

            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerMovement.rigidbody.velocity = velocity;
        }
    }
}
