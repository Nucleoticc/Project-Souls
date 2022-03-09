using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class EventColliderBeginBossFight : MonoBehaviour
    {
        WorldEventManager worldEventManager;

        void Awake()
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                worldEventManager.ActivateBossFight();
            }
        }
    }
}

