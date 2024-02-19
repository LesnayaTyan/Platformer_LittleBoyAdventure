using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float leftAngle;
    [SerializeField] private float rightAngle;

    bool movingClockwise;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movingClockwise = true;
    }

    void Update()
    {
        Move();
    }

    public void ChangeDirection()
    {
        if(transform.rotation.z > rightAngle)
        {
            movingClockwise = false;
        }
        if(transform.rotation.z < leftAngle)
        {
            movingClockwise = true;
        }
    }

    public void Move()
    {
        ChangeDirection();

        if (movingClockwise)
        {
            rb.angularVelocity = moveSpeed;
        }
        if(!movingClockwise)
        {
            rb.angularVelocity = -1*moveSpeed;
        }
    }
}
