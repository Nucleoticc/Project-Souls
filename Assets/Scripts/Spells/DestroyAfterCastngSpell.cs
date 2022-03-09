using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class DestroyAfterCastngSpell : MonoBehaviour
    {
        CharacterManager characterCastingSpell;

        void Awake()
        {
            characterCastingSpell = GetComponentInParent<CharacterManager>();
        }

        void Update()
        {
            if (characterCastingSpell.isFiringSpell)
            {
                Destroy(gameObject);
            }
        }
    }
}
