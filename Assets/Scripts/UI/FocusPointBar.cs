using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Souls
{
    public class FocusPointBar : MonoBehaviour
    {
        Slider slider;

        void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public void SetMaxFocusPoints(float maxFP)
        {
            slider.maxValue = maxFP;
            slider.value = maxFP;
        }

        public void SetCurrentFocusPoints(float currentFP)
        {
            slider.value = currentFP;
        }
    }
}