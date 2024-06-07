using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroManager : MonoBehaviour
{
    string temp = "Scene_5"; // 임시 변수. Intro Scene 분리 시: 이전 씬 이름, 분리 안할 시: 현재 씬 이름

    public GameObject introDataObj;
    public TextMeshProUGUI introText;

    string[] data; // 특정 씬에 대한 intro data 저장
    bool isSkip = false;
    int curDialogueIndex = 0;

    // 목적지에 따라 추가되는 이화력 데이터
    Dictionary<string, int> ewhaPoint = new Dictionary<string, int> {
    { "Scene_2", 0 },
    { "Scene_3", 5 },
    { "Scene_4", 5 },
    { "Scene_5", 10 }
    };

    void Start()
    {
        data = introDataObj.GetComponent<IntroDataScript>().GetData(temp);
        StartCoroutine(printDialogue());
    }

    IEnumerator printDialogue()
    {
        introText.text = "";
        float txtdelay = 0.1f;
        int count = 0;
        while (count < data[curDialogueIndex].Length)
        {
            introText.text += data[curDialogueIndex][count].ToString();
            count++;
            if (isSkip)
            {
                txtdelay = 0f;
            }
            yield return new WaitForSeconds(txtdelay);
        }
    }

    public void onClickDialogueButton()
    {
        isSkip = true;
        curDialogueIndex++;

        if(data.Length == curDialogueIndex)
        {

            Debug.Log("+" + ewhaPoint[temp] + " 이화력 상승!"); // TODO: 이화력 전역 변수 생기면 값 올리기
            // TODO: 퀴즈 씬으로 이동
            return;
        }
        StartCoroutine(printDialogue());
    }
}
