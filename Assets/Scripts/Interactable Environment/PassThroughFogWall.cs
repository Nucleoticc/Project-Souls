using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class PassThroughFogWall : Interactable
    {
        Collider wallCollider;
        WorldEventManager worldEventManager;

        void Awake()
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();
            wallCollider = GetComponent<Collider>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            playerManager.PassThroughFogWallInteraction(transform);
            wallCollider.enabled = false;
        }
    }
}

