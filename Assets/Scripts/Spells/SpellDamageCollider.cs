using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Souls
{
    public class SpellDamageCollider : DamageCollider
    {
        public GameObject impactParticles;
        public GameObject projectileParticles;
        public GameObject muzzleParticles;

        bool hasCollided = false;

        CharacterStats spellTarget;
        Rigidbody rb;

        Vector3 impactNormal;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Start()
        {
            projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
            projectileParticles.transform.parent = transform;

            if (muzzleParticles)
            {
                muzzleParticles = Instantiate(muzzleParticles, transform.position, transform.rotation);
                Destroy(muzzleParticles, 2f);
            }
        }

        void OnCollisionEnter(Collision other)
        {
            if (!hasCollided)
            {
                spellTarget = other.transform.GetComponent<CharacterStats>();
                if (spellTarget != null)
                {
                    if (other.transform.root.tag == "Enemy")
                    {
                        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
                        spellTarget.TakeDamage(currentWeaponDamage, damagedBy: playerStats);
                    }
                    else if (other.transform.root.tag == "Player")
                    {
                        EnemyStats enemyStats = FindObjectOfType<EnemyStats>();
                        spellTarget.TakeDamage(currentWeaponDamage, damagedBy: enemyStats);
                    }
                }
                hasCollided = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

                Destroy(projectileParticles);
                Destroy(impactParticles, 2f);
                Destroy(gameObject, 2f);
            }
        }
    }
}

