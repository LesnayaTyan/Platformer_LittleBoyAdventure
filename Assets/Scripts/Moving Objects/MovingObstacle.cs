using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingOb : MonoBehaviour
{
    [Range(0,5)]
    public float speed;

    [Range(0,2)]
    public float waitDuration;
    Vector3 targetPosition;

    public GameObject ways;
    public Transform[] wayPoints;
    int pointIndex;
    int pointCount;
    int direction;
    int speedMultiplier = 1;
    [SerializeField] private float damage;


    private void Awake()
    {
        wayPoints = new Transform[ways.transform.childCount];
        for(int i = 0; i < ways.gameObject.transform.childCount; i++)
        {
            wayPoints[i] = ways.transform.GetChild(i).gameObject.transform;
        }
    }

    private void Start()
    {
        pointCount = wayPoints.Length;
        pointIndex = 1;
        targetPosition = wayPoints[pointIndex].transform.position;
    }

    private void Update()
    {
        var step = speedMultiplier * speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if(transform.position == targetPosition)
        {
            NextPoint();
        }
    }

    void NextPoint()
    {
        if(pointIndex == pointCount - 1) // Arrived last point
        {
            direction = -1;
        }
        if(pointIndex == 0) //Arrived first point
        {
            direction = 1;
        }

        pointIndex += direction;
        targetPosition = wayPoints[pointIndex].transform.position;

        StartCoroutine(WaitNextPoint());
    }

    IEnumerator WaitNextPoint()
    {
        speedMultiplier = 0;
        yield return new WaitForSeconds(waitDuration);
        speedMultiplier = 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
