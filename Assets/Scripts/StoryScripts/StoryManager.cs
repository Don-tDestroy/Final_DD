using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public GameObject DiddyAnimated;
    DiddyEmotionManager myEmotionManager;
    StoryScripts myStoryScript;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    public GameObject finishButton;

    public int StoryIndex;
    bool isEnding = false;
    bool isSkip;
    bool isPrintingLines;
    int curDialogueIndex;
    public bool isDiddyVisible;
    public bool isFinished;

    List<string> myLines;
    List<int> myEmotions;
    List<int> myNames;
    GameManager gameManager;
    
    void Awake()
    {
        myEmotionManager = DiddyAnimated.GetComponent<DiddyEmotionManager>(); 
        myStoryScript = GetComponent<StoryScripts>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        isSkip = false;
        isPrintingLines = false;
        isFinished = false;

        InitializeStoryIndex();


        myLines = myStoryScript.Lines[StoryIndex];
        myEmotions = myStoryScript.Emotions[StoryIndex];
        myNames = myStoryScript.Names[StoryIndex];

        StartCoroutine(printDialogue(curDialogueIndex));
    }

    private void InitializeStoryIndex()
    {
        int curEwhaPower = gameManager.GetEwhaPower();

        if(curEwhaPower <= 20)
        {
            StoryIndex = 2;
        }
        else if(curEwhaPower <= 40)
        {
            StoryIndex = 3;
        }
        else
        {
            StoryIndex = 4;
        }

        if (StoryIndex == 0 || StoryIndex == 1) { isEnding = false; }
        else { isEnding = true; }

        if (!isDiddyVisible)
        {
            DiddyAnimated.SetActive(false);
        }
        else
        {
            DiddyAnimated.SetActive(true);
        }
    }

    IEnumerator printDialogue(int index)
    {
        dialogueText.text = "";
        if (myNames[index] == 0)
        {
            nameText.text = "화연";
        }
        else if (myNames[index] == 1)
        {
            nameText.text = "디디";
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
            dialogueText.text += myLines[index][count].ToString();
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

        if (curDialogueIndex < myLines.Count-1) // 마지막 대사 직전까지
        {

            if (!isPrintingLines)
            {
                curDialogueIndex++;
                StartCoroutine(printDialogue(curDialogueIndex));
            }
        }
        else if(curDialogueIndex == myLines.Count-1) // 마지막 대사일 때
        {
            finishButton.SetActive(true);
        }
    }

    public void onClickFinishButton()
    {
        if (isEnding)
        {
            SceneManager.LoadScene("Scene0");
        }
        if (!isPrintingLines)
        {
            StartCoroutine(printDialogue(curDialogueIndex));
            isFinished = true;
        }

        
        
    }


}
