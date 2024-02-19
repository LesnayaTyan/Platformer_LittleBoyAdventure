using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    [SerializeField] private AudioSource hurtSound;


    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    //[SerializeField] private Behaviour[] components;
    private bool invulnerable;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            hurtSound.Play();
            //StartCoroutine(Invunerability());
        }
        else
        {

            if (!dead)
            {
                anim.SetTrigger("die");

                if(GetComponent<PlayerController>() != null)
                {
                    GetComponent<PlayerController>().enabled= false;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }

                if (GetComponent<EnemyPatrol>() != null)
                    GetComponent<EnemyPatrol>().enabled = false;

                if (GetComponent<MeleeEnemyCave>() != null)
                {
                    GetComponentInParent<MeleeEnemyCave>().enabled= false;
                }

                dead = true;

            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
    //private IEnumerator Invunerability()
    //{
    //    invulnerable = true;
    //    Physics2D.IgnoreLayerCollision(10, 11, true);
    //    for (int i = 0; i < numberOfFlashes; i++)
    //    {
    //        spriteRend.color = new Color(1, 0, 0, 0.5f);
    //        yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
    //        spriteRend.color = Color.white;
    //        yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
    //    }
    //    Physics2D.IgnoreLayerCollision(10, 11, false);
    //    invulnerable = false;
    //}
}