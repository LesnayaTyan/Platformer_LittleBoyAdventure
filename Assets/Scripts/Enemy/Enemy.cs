using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected private Animator anim;
    protected private Rigidbody2D rb;
    protected private AudioSource deathSound;

    protected private Health playerHealth; // check that in unity it didnt change to slime health
    [SerializeField] private int damage;


    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        deathSound = GetComponent<AudioSource>();

        //playerHealth = FindObjectOfType<Health>();
        //if (playerHealth == null)
        //{
        //    Debug.LogError("playerHealth == null");            
        //}
    }
    //player jump on slime and killing it
    public void JumpedOn()
    {
        //DamagePlayer();
        //Die animation.
        anim.SetTrigger("Death");
        deathSound.Play();
        //Disable the enemy
        rb.velocity = new Vector2(0, 0);
        GetComponent<Collider2D>().enabled = false;
        //rb.bodyType = RigidbodyType2D.Kinematic;
        this.enabled = false;
        Destroy(this.gameObject);
    }
    private void Death()
    {
        Destroy(this.gameObject);
    }

    //public void DamagePlayer()
    //{
    //    playerHealth.TakeDamage(damage);
    //}
}
