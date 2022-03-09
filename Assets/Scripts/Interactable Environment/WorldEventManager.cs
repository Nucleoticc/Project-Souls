using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class WorldEventManager : MonoBehaviour
    {
        public List<FogWall> fogWalls;
        UIBossHealthBar bossHealthBar;
        EnemyBossManager bossManager;

        public bool bossFightIsActive;
        public bool bossHasBeenAwakened;
        public bool bossHasBeenDefeated;

        void Awake()
        {
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        }

        public void ActivateBossFight()
        {
            bossFightIsActive = true;
            bossHasBeenAwakened = true;
            bossHealthBar.SetUIHealthBarToActive();

            foreach (FogWall fogWall in fogWalls)
            {
                fogWall.ActivateFogWall();
            }
        }

        public void DeactivateBossFight()
        {
            bossHasBeenDefeated = true;
            bossFightIsActive = false;
            bossHealthBar.SetUIHealthBarToInactive();
            
            foreach (FogWall fogWall in fogWalls)
            {
                fogWall.DeactivateFogWall();
            }
        }
    }
}

