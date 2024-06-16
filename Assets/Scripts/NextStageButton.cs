using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageButton : MonoBehaviour
{
    [SerializeField]
    private float remitTime;

    private float time = 0f; // 시간 재기

    public GameObject warningPopup; // 4층 도착했을 때 누르라는 경고 팝업

    private void Awake()
    {
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        while (true)
        {
            time += Time.deltaTime;
            Debug.Log("스톱워치: " + time);
            yield return null;
        }
    }

    public void onClickNextStage() {
        if (time < remitTime)
        {
            warningPopup.SetActive(true);
            Time.timeScale = 0; // 시간 멈추기
            return;
        }

        int currStage = GameManager.Instance.GetStageNumber();
        string nextStage = "";

        switch (currStage)
        {
            case 4:
                nextStage = "Scene_4_After";
                break;
            case 5:
                nextStage = "Scene_5_After";
                break;
        }
        SceneManager.LoadScene(nextStage);
    }

    public void OnClicOkButton()
    {
        Time.timeScale = 1; //시간 흐르기
    }

}
