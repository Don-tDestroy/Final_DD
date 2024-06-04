using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DiddyONOFFManager : MonoBehaviour
{
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("ishide", false);
    }
    public void DiddyShowHide(bool isHide)
    {
        animator.SetBool("ishide", isHide);
    }
}
