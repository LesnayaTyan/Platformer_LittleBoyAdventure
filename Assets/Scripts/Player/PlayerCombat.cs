using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 10;

    public float attackRate = 2f;
    float nextAttackTime = 0f;
    [SerializeField] private AudioSource attackSound;

    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }           
    }

    private void Attack()
    {
        //Play attack animations
        animator.SetTrigger("attack");
        attackSound.Play();
        //Detect enemies in range of attack
        Collider2D[] hitEmemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //Damage them
        foreach(Collider2D enemy in hitEmemies)
        {
            enemy.GetComponent<Health>().TakeDamage(attackDamage);
        }        
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
