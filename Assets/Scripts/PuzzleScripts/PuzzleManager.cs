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


    public PuzzleButtonDragGroup[] stagePullyGroup;

    public Animator fixSpaceShipAnim;


    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        originParent = GameObject.Find("Puzzle").transform;
        UpdateCompletionText();

        for (int i = 0; i < stagePullyGroup.Length; i++) {
            stagePullyGroup[i].SetPullyInitText();
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
        StartCoroutine(PlayFixAnim());

    }

    private IEnumerator PlayFixAnim()
    {
        fixSpaceShipAnim.SetBool("isFix", true);

        yield return new WaitForSeconds(1f);

        fixSpaceShipAnim.SetBool("isFix", false);

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
    private void UpdateCompletionText()
    {
        int completionPercentage = (currentCnt * 100) / targetCnt;
        completionText.text = $"{completionPercentage}%";
    }

    public void UpdateStagePullyCnt(int stageIdx) {
        stagePullyGroup[stageIdx].SetPullySuccessText();
    }


}
