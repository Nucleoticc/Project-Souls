using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class IllusionaryWall : MonoBehaviour
    {
        Renderer rend;

        public bool wallHasBeenHit;
        // public Material illusionaryWallMaterial;
        float alpha;
        public float fadeTimer = 2.5f;
        public BoxCollider wallCollider;

        void Awake()
        {
            rend = GetComponent<Renderer>();
        }

        void Update()
        {
            if (wallHasBeenHit)
            {
                FadeIllusionaryWall();
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                wallHasBeenHit = true;
            }
        }

        public void FadeIllusionaryWall()
        {
            alpha = rend.material.color.a;
            alpha -= Time.deltaTime / fadeTimer;
            Color fadedWallColor = new Color(1, 1, 1, alpha);
            rend.material.color = fadedWallColor;

            if (wallCollider.enabled == true)
            {
                wallCollider.enabled = false;
            }

            if (alpha <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}

