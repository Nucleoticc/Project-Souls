using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Souls
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Damage")]
        public int baseDamage = 25;
        public int criticalDamageMultiplier = 3;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Absorption")]
        public float physicalDamageAbsorption;

        [Header("Idle Animations")]
        public string Right_Hand_Idle;
        public string Left_Hand_Idle;
        public string TH_Idle;

        [Header("One Handed Animations")]
        public string OH_Light_Attack_01;
        public string OH_Light_Attack_02;
        public string OH_Heavy_Attack_01;

        [Header("Two Handed Animations")]
        public string TH_Light_Attack_01;
        public string TH_Light_Attack_02;
        public string TH_Heavy_Attack_01;

        [Header("Weapon Art")]
        public string weapon_art;

        [Header("Stamina Costs")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;

        [Header("Weapon Type")]
        public bool isMagicCaster;
        public bool isFaithCaster;
        public bool isPyroCaster;
        public bool isMeleeWeapon;
        public bool isShieldWeapon;
    }
}

