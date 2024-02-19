using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] private Transform leftCap;  
    [SerializeField] private Transform rightCap;  

    [SerializeField] private float jumpLength = 8f;
    [SerializeField] private float jumpHeight = 8f;
    [SerializeField] private LayerMask ground;
    private Collider2D coll;

    private bool facingLeft = true;         


    protected override void Start()
    {
        base.Start();
        coll = GetComponent<Collider2D>();
    }

    public void Update()
    {
        //Transition from jump to fall
        if (anim.GetBool("IsJumping"))
        {
            if (rb.velocity.y <= .1)
            {
                anim.SetBool("IsFalling", true);
                anim.SetBool("IsJumping", false);
            }
        }

        //Transition from fall to idle
        if (coll.IsTouchingLayers(ground) && anim.GetBool("IsFalling"))
        {
            anim.SetBool("IsFalling", false);
        }

        //Move(); // im call this method with animation event
    }

    private void Move()
    {
        if (facingLeft)
        {
            //test to see if we are beyond the leftCap
            if (transform.position.x > leftCap.position.x)
            {
                //Make sure that sprite is facing right direction and if its not, then face the right direction
                if (transform.localScale.x != 1)
                {
                    transform.localScale = new Vector3(1, 1);
                }

                if (coll.IsTouchingLayers(ground))
                {
                    //jump
                    rb.velocity = new Vector2(-jumpLength, jumpHeight);
                    anim.SetBool("IsJumping", true);
                }
            }
            else
            {
                facingLeft = false;
            }
        }
        //if we are not, we are going to face right
        else
        {
            if (transform.position.x < rightCap.position.x)
            {
                if (transform.localScale.x != -1)
                {
                    transform.localScale = new Vector3(-1, 1);
                }

                if (coll.IsTouchingLayers(ground))
                {
                    rb.velocity = new Vector2(jumpLength, jumpHeight);
                    anim.SetBool("IsJumping", true);
                }
            }
            else
            {
                facingLeft = true;
            }
        }
    }

}