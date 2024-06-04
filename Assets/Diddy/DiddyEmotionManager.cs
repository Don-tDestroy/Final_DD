using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DiddyEmotionManager : MonoBehaviour
{
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetInteger("emotion", 0);
    }

    public void DiddyChangeEmotion(int emote_index)
    {
        animator.SetInteger("emotion", 0);
        StartCoroutine(GiveDelay(0.5f));
        animator.SetInteger("emotion", emote_index);
    }

    IEnumerator GiveDelay(float time)
    {
        yield return new WaitForSeconds(time);
    }

}
