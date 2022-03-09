using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Souls
{
    public class UIEnemyHealthBar : MonoBehaviour
    {
        Slider slider;

        float timeUntilBarIsHidden = 0f;
        public bool isLockedOn = false;

        void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }

        void Update()
        {
            timeUntilBarIsHidden -= Time.deltaTime;

            if(!isLockedOn)
            {
                timeUntilBarIsHidden = 0f;
                slider.gameObject.SetActive(false);
            }
            else
            {
                if(!slider.gameObject.activeInHierarchy)
                {
                    slider.gameObject.SetActive(true);
                }
            }

            if(slider.value <= 0)
            {
                Destroy(gameObject);
            }
        }

        public void SetHealth(int health)
        {
            slider.value = health;
            timeUntilBarIsHidden = 3f;
        }

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

    }
}

