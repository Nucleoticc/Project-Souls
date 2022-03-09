using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Souls
{
    public class SoulCountUI : MonoBehaviour
    {
        public Text soulCountText;

        public void SetSoulCountText(int soulCount)
        {
            soulCountText.text = soulCount.ToString();
        }
    }
}
