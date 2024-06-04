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
    int curDialogueIndex;

    List<string> testLines = new List<string>()
    {
        "테스트 스크립트!",
        "디디의 대사야.",
        "안녕! 난 외계인 디디."
    };

    List<int> testEmotions = new List<int>()
    {
        1, 2, 3
    };
    void Awake()
    {
        myEmotionManager = DiddyAnimated.GetComponent<DiddyEmotionManager>(); 
        myStoryScript = GetComponent<StoryScripts>();
        isSkip = false;
        curDialogueIndex = 0;
        StartCoroutine(printDialogue(curDialogueIndex));
    }

    IEnumerator printDialogue(int index)
    {
        dialogue.text = "";
        float txtdelay = 0.1f;
        int count = 0;
        while (count < testLines[index].Length)
        {
            dialogue.text += testLines[index][count].ToString();
            count++;
            if (isSkip)
            {
                txtdelay = 0f;
            }
            yield return new WaitForSeconds(txtdelay);
        }
        myEmotionManager.DiddyChangeEmotion(testEmotions[index]);
    }

    public void onClickDialogueButton()
    {
        isSkip = true;
        curDialogueIndex++;
        StartCoroutine(printDialogue(curDialogueIndex));
    }


}
