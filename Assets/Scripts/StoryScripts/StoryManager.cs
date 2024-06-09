using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryManager : MonoBehaviour
{
    public GameObject DiddyAnimated;
    DiddyEmotionManager myEmotionManager;
    StoryScripts myStoryScript;
    public TextMeshProUGUI dialogue;
    public int StoryIndex;
    bool isSkip;
    bool isPrintingLines;
    int curDialogueIndex;

    List<string> myLines;
    List<int> myEmotions;
    void Awake()
    {
        myEmotionManager = DiddyAnimated.GetComponent<DiddyEmotionManager>(); 
        myStoryScript = GetComponent<StoryScripts>();
        isSkip = false;
        isPrintingLines = false;

        myLines = myStoryScript.Lines[curDialogueIndex];
        myEmotions = myStoryScript.Emotions[curDialogueIndex];

        StartCoroutine(printDialogue(curDialogueIndex));
    }

    IEnumerator printDialogue(int index)
    {
        dialogue.text = "";
        float txtdelay = 0.1f;
        int count = 0;
        isPrintingLines = true;
        while (count < myLines[index].Length)
        {
            dialogue.text += myLines[index][count].ToString();
            count++;
            if (!isSkip)
            {
                yield return new WaitForSeconds(txtdelay);
            }
            
        }
        isPrintingLines = false;
        isSkip = false;
        myEmotionManager.DiddyChangeEmotion(myEmotions[index]);
    }

    public void onClickDialogueButton()
    {
        if (isPrintingLines)
        {
            isSkip = true;
        }

        if (curDialogueIndex < myLines.Count - 1) // 마지막 대사 직전까지
        {

            if (!isPrintingLines)
            {
                curDialogueIndex++;
                StartCoroutine(printDialogue(curDialogueIndex));
            }
        }
        else if(curDialogueIndex == myLines.Count - 1) // 마지막 대사일 때
        {

        }
    }


}
