using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizManager : MonoBehaviour
{
    private static QuizManager _instance; // 싱글톤 객체
    private int answerCnt = 0; // 정답 개수
    private int currentCnt = 0; // 버튼 채워져있는 개수
    private readonly int targetCnt = 5;

    [SerializeField]
    private GameObject answerPopup, noAnswerPopup;
    private Transform originParent;
    private List<Transform> sourceTrList = new List<Transform>(5);
    private List<Vector3> sourceTrPosList = new List<Vector3>(5);

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        originParent = GameObject.Find("Quiz_Panel").transform;
        List<string> names = new List<string> { "Front", "ECC", "Posco", "Science", "Engineering" };
        for(int i = 0; i < names.Count; i++)
        {
            GameObject temp = GameObject.Find(names[i]);
            sourceTrList.Add(temp.transform);
            sourceTrPosList.Add(temp.transform.localPosition);
        }
    }

    public static QuizManager Instance
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
        if (currentCnt == targetCnt)
        {
            FinalAnswerCheck();
        }
    }

    public void CurrentCntDown()
    {
        currentCnt--;
    }

    private void FinalAnswerCheck()
    {
        if (answerCnt == targetCnt)
        {
            answerPopup.SetActive(true);
        }
        else
        {
            noAnswerPopup.SetActive(true);
        }
    }

    public void OnClickOk()
    {
        for (int i = 0; i < sourceTrList.Count; i++)
        {
            sourceTrList[i].SetParent(originParent);
            sourceTrList[i].SetAsFirstSibling();
            sourceTrList[i].localPosition = sourceTrPosList[i];
            sourceTrList[i].gameObject.GetComponent<ButtonDrag>().ObjInit();
        }
        currentCnt = 0;
        answerCnt = 0;
    }

    private void Update()
    {
        Debug.Log("answer " + answerCnt);
        Debug.Log("current " + currentCnt);
    }
}