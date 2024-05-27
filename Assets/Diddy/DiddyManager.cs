using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DiddyManager : MonoBehaviour
{
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("emotion", 0);
        //animator.SetBool("backtoidle", false);
        animator.SetBool("ishide", false);
    }
    public void DiddyShowHide(bool isHide)
    {
        animator.SetBool("ishide", isHide);
        animator.SetInteger("emotion", 0);
    }
    public void DiddyTalk()
    {

    }

    public void DiddyEmotion(int emote_index)
    {
        //animator.SetBool("backtoidle", false);
        animator.SetInteger("emotion", emote_index);
    }

    /*public void DiddySetIdle()
    { 
        animator.SetInteger("emotion", 0);
        //animator.SetBool("backtoidle", true);
    }*/
}
