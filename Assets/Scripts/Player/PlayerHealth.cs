using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int health = 1;
    public float deathExplosionForce = 5f;
    public float deathExplosionRadius = 10f;
    public float deathExposionUpwardModifier = 5f;

    public AudioSource deathSound;

    Rigidbody[] rigibodies;
    // Start is called before the first frame update
    void Start()
    {
        rigibodies = GetComponentsInChildren<Rigidbody>();
        deathSound = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        health--;
        if (health <= 0)
        {
            this.Die();
        }
    }
    void Die()
    {
        foreach (Rigidbody rb in rigibodies)
        {
            rb.isKinematic = false;
            rb.AddExplosionForce(
                deathExplosionForce,
                this.gameObject.transform.position, 
                deathExplosionRadius, 
                deathExposionUpwardModifier);
        }

        if (deathSound != null)
        {
            deathSound.Play();
        }
    }
}
