using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimator : MonoBehaviour
{
    public Animator startAnim;
    public DialogueManager dialogueManager;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        startAnim.SetBool("IsStartOpen", true);
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        startAnim.SetBool("IsStartOpen", false);
    }
}
