using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    DiddyEmotionManager myEmotionManager;
    StoryScripts myStoryScript;
    public TextMeshProUGUI dialogue;
    public int StoryIndex;
    bool isSkip;
    int curDialogueIndex;

    void Awake()
    {
        myEmotionManager = GetComponent<DiddyEmotionManager>(); 
        myStoryScript = GetComponent<StoryScripts>();
        isSkip = false;
        curDialogueIndex = 0;
        //printDialogue();
    }

    void printDialogue(string curDialogue)
    {
        float txtdelay = 0.1f;
        int count = 0;
        while (count < curDialogue.Length)
        {
            dialogue.text += curDialogue[count].ToString();
            count++;
            if (isSkip)
            {
                txtdelay = 0f;
            }
            StartCoroutine(giveTextDelay(txtdelay));
        }
    }

    public void onClickDialogueButton()
    {
        isSkip = true;
        curDialogueIndex++;
        //printDialogue()
    }

    IEnumerator giveTextDelay(float time)
    {
        yield return new WaitForSeconds(time);
    }

}
