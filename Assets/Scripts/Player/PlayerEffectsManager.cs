using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        PlayerStats playerStats;
        PlayerWeaponSlotManager weaponSlotManager;
        public GameObject currentParticleFx;
        public GameObject instantiatedFXModel;
        public int amountToBeHealed;

        void Awake() {
            playerStats = GetComponent<PlayerStats>();    
            weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        }

        public void HealPlayerFromEffect()
        {
            playerStats.HealPlayer(amountToBeHealed);
            GameObject healParticles = Instantiate(currentParticleFx, playerStats.transform);
            Destroy(instantiatedFXModel.gameObject, 2f);
            weaponSlotManager.LoadRightHandWeaponOnSlot();
            Destroy(healParticles, 2f);
        }
    }
}
