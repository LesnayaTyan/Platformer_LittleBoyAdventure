using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestructibleWall : MonoBehaviour
{
    PlayerController playerController;
    ////public GameObject destroyEffect;

    [SerializeField] private UnityEvent hit;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && playerController != null && playerController.IsDashing)
        {
            hit?.Invoke();
        }
        if(collision.contacts[0].normal.y > 0)       //ломание головой
        {
            hit?.Invoke();
        }
    }
}
