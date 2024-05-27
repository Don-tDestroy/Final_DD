using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuzzleManager : MonoBehaviour
{
    private static PuzzleManager _instance; // 싱글톤 객체
    private int answerCnt = 0; // 정답 개수
    private int currentCnt = 0; // 버튼 채워져있는 개수
    private readonly int targetCnt = 3;
    public TextMeshProUGUI completionText; // 완성도


    [SerializeField]
    private GameObject answerPopup;
    private Transform originParent;


    public int[] stagePully = { 64, 1, 24 };
    public PuzzleButtonDragGroup[] stagePullyGroup;


    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        originParent = GameObject.Find("Puzzle").transform;
        UpdateCompletionText();

        for (int i = 0; i < stagePullyGroup.Length; i++) {
            stagePullyGroup[i].setPullyCntText(stagePully[i]);
        }
    }

    public static PuzzleManager Instance
    {
        get
        {
            if (_instance == null)
            {
                return null;
            }
            return _instance;
        }
    }

    public int GetAnswerCnt()
    {
        return answerCnt;
    }

    public void AnswerCntUp()
    {
        answerCnt++;
    }

    public void AnswerCntDown()
    {
        answerCnt--;
    }

    public void CurrentCntUp()
    {
        currentCnt++;
        UpdateCompletionText();
        // 100% 완성된 모습은 보여주고 끝내기?
        // 아님 팝업에서 완성된 우주선 보여주기?
        if (currentCnt == targetCnt)
        {
            FinalAnswerCheck();
        }
    }

    public void CurrentCntDown()
    {
        if (currentCnt > 0)
        {
            currentCnt--;
        }
    }

    private void FinalAnswerCheck()
    {
        if (answerCnt == targetCnt)
        {
            answerPopup.SetActive(true);
        }
    }

    //public void OnClickOk()
    //{
    //    for (int i = 0; i < sourceTrList.Count; i++)
    //    {
    //        sourceTrList[i].SetParent(originParent);
    //        sourceTrList[i].SetAsLastSibling();
    //        sourceTrList[i].localPosition = sourceTrPosList[i];
    //        sourceTrList[i].gameObject.GetComponent<ButtonDrag>().ObjInit();
    //    }
    //    currentCnt = 0;
    //    answerCnt = 0;
    //}

    private void Update()
    {
        Debug.Log("answer " + answerCnt);
        Debug.Log("current " + currentCnt);
    }

    private void UpdateCompletionText()
    {
        int completionPercentage = (currentCnt * 100) / targetCnt;
        //completionText.text = $"Completion: {completionPercentage}% ({currentCnt}/{targetCnt} parts)";
        completionText.text = $"{completionPercentage}%";
    }

    public void UpdateStagePullyCnt(int stageIdx) {
        stagePullyGroup[stageIdx].SetPullySuccessText();
    }
}
