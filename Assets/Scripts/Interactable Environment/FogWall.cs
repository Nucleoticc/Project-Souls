using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class FogWall : MonoBehaviour
    {   
        Collider fogWallCollider;

        void Awake()
        {
            fogWallCollider = GetComponent<Collider>();
            DeactivateFogWall();
        }

        public void ActivateFogWall()
        {
            fogWallCollider.enabled = true;
        }

        public void DeactivateFogWall()
        {
            fogWallCollider.enabled = false;
        }
    }
}
