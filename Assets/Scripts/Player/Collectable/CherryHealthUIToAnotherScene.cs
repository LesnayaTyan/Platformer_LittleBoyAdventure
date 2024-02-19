using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CherryHealthUIToAnotherScene : MonoBehaviour
{
    public int cherries = 0;
    public Text cherryText;
    //public int health;
    public Text healthCount;
    //private Health health;

    public static CherryHealthUIToAnotherScene perm;

    //Singleton for next scene
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (!perm)
        {
            perm = this;
        }
        else
            Destroy(gameObject);
    }

    public void Reset()
    {
        cherries = 0;
        //health = 5;
        cherryText.text = cherries.ToString();
        // healthCount.text = health.ToString();

    }
}
