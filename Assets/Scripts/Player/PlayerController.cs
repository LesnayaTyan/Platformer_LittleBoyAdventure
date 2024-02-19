using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.U2D;

public class PlayerController : MonoBehaviour
{
    //Start() varables
    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D coll;

    //FSM - Finite State Machine
    private enum State { idle, running, jumping, falling, hurt, climb, die, attack };
    private State state = State.idle;

    //LadderVariables
    //[HideInInspector] public bool canClimb = false;
    //[HideInInspector] public bool bottomLadder = false;
    //[HideInInspector] public bool topLadder = false;
    //[HideInInspector] public Ladder ladder;
    private float naturalGravity;
   // [SerializeField] float climbSpeed = 3f;

    //Inspector variables
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource pickedItemSound;
    [SerializeField] private AudioSource footStep;
    private float horizontalDirection;

    [SerializeField] public int cherries = 0;
    [SerializeField] private TextMeshProUGUI cherryText;
    [SerializeField] private AudioSource jumpSoundEffect;

    private float dirX = 0f;
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 10f);

    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingPower = 24f; //dashingVelocity
    //[SerializeField] private float dashingVelocity = 14f; //dashingVelocity
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer trailRenderer;
    private Vector2 dashingDirection;

    public bool IsDashing { get { return isDashing; } }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
        naturalGravity = rb.gravityScale;
        //Cursor.visible = false;
    }

    private void Update()
    {
        //if (state == State.climb)
        //{
        //    Climb();
        //}
        if (isDashing)
        {
            return;
        }

        Movemenent();

        WallSlide();
        WallJump();

        AnimationStateSwitch();
        anim.SetInteger("state", (int)state); //set animation based on Enumerator state

        //ChangeScene();        
    }

    private void Movemenent()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            jumpSoundEffect.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        AnimationStateSwitch();
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        canDash = false;
        trailRenderer.emitting = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.velocity = new Vector2(transform.localScale.x * dashingPower, Input.GetAxisRaw("Vertical") * dashingPower); 
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;

        trailRenderer.emitting = false;
        isDashing = false;
        canDash = true;
        yield return new WaitForSeconds(dashingCooldown);
    }

    private void AnimationStateSwitch()
    {
        if (dirX > 0f)
        {
            state = State.running;
            transform.localScale = new Vector2(1, 1);
        }
        else if (dirX < 0f)
        {
            state = State.running;
            transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            state = State.idle;
        }

        if (rb.velocity.y > .1f)
        {
            state = State.jumping;
        }
        else if (state == State.hurt)
        {
            if (Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if (rb.velocity.y < -.1f)       //another way
        {
            state = State.falling;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            Destroy(collision.gameObject);
            cherries += 1;
            cherryText.text = cherries.ToString();
            pickedItemSound.Play();
        }

        if (collision.tag == "PowerUp")
        {
            Destroy(collision.gameObject);
            jumpForce = 15f;
            speed = 9f;
            GetComponent<SpriteRenderer>().color = Color.yellow;
            StartCoroutine(ResetPower());
        }
    }

    private void OnCollisionEnter2D(Collision2D collisionEnemy)
    {
        if (collisionEnemy.gameObject.tag == "Enemy")
        {
            state = State.hurt;
            //hurtSound.Play();
            if (collisionEnemy.gameObject.transform.position.x > transform.position.x)
            {
                //enemy is to my right therefore i should be damaged and move left
                rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
            }
            else
            {
                //enemy is to my left therefore i should be damaged and move right
                rb.velocity = new Vector2(hurtForce, rb.velocity.y);

            }            
        }
        else if (collisionEnemy.gameObject.tag == "SlimeEnemy")
        {
            Enemy enemy = collisionEnemy.gameObject.GetComponent<Enemy>();
            if (state == State.falling) 
            {
                //Debug.Log("JumpedOn slime");
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                if (collisionEnemy.gameObject.transform.position.x > transform.position.x)
                {
                    //enemy is to my right therefore i should be damaged and move left
                    rb.velocity = new Vector2(-hurtForce, rb.velocity.y);
                }
                else
                {
                    //enemy is to my left therefore i should be damaged and move right
                    rb.velocity = new Vector2(hurtForce, rb.velocity.y);

                }
            }
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jumping;
    }

    private void FootStepPlaying()
    {
        footStep.Play();
    }


    private IEnumerator ResetPower()
    {
        yield return new WaitForSeconds(5);
        jumpForce = 11f;
        speed = 4.5f;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    //private void Climb()
    //{
    //    if (Input.GetButtonDown("Jump"))
    //    {
    //        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    //       // canClimb = false;
    //        rb.gravityScale = naturalGravity;
    //        anim.speed = 1f;
    //        Jump();
    //        return;
    //    }

    //    //float vDirection = Input.GetAxis("Vertical");

    //    //Climbing up
    //    //if (vDirection > .1f && !topLadder)
    //    //{
    //    //    rb.velocity = new Vector2(0f, vDirection * climbSpeed);
    //    //    anim.speed = 1f;
    //    //}
    //    ////CLimbing down
    //    //else if (vDirection < -.1f && !bottomLadder)
    //    //{
    //    //    rb.velocity = new Vector2(0f, vDirection * climbSpeed);
    //    //    anim.speed = 1f;
    //    //}
    //    //Still
    //    else
    //    {
    //        rb.velocity = Vector2.zero;
    //        anim.speed = 0f;
    //    }
    //}

    public bool canAttack()
    {
        return horizontalDirection == 0 /*&& coll.IsTouchingLayers(ground)*/;
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !coll.IsTouchingLayers(ground) /*&& horizontalDirection != 0f*/)
        {
            isWallSliding = true;
            state = State.falling;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                //isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}