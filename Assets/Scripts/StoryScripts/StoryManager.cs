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
    public TextMeshProUGUI name;
    public int StoryIndex;
    bool isSkip;
    bool isPrintingLines;
    int curDialogueIndex;
    public bool isDiddyVisible;

    List<string> myLines;
    List<int> myEmotions;
    List<int> myNames;
    void Awake()
    {
        myEmotionManager = DiddyAnimated.GetComponent<DiddyEmotionManager>(); 
        myStoryScript = GetComponent<StoryScripts>();
        isSkip = false;
        isPrintingLines = false;

        if (!isDiddyVisible)
        {
            DiddyAnimated.SetActive(false);
        }
        else
        {
            DiddyAnimated.SetActive(true);
        }

        myLines = myStoryScript.Lines[StoryIndex];
        myEmotions = myStoryScript.Emotions[StoryIndex];
        myNames = myStoryScript.Names[StoryIndex];

        StartCoroutine(printDialogue(curDialogueIndex));
    }

    IEnumerator printDialogue(int index)
    {
        dialogue.text = "";
        if (myNames[index] == 0)
        {
            name.text = "화연";
        }
        else if (myNames[index] == 1)
        {
            name.text = "디디";
        }

        float txtdelay = 0.1f;
        int count = 0;
        isPrintingLines = true;
        if (isDiddyVisible)
        {
            myEmotionManager.DiddyChangeEmotion(myEmotions[index]);
        }

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
