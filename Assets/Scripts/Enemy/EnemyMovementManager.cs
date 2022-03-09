using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class EnemyMovementManager : MonoBehaviour
    {
        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        void Start()
        {
            Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        }
    }
}

