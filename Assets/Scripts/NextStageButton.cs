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
            SoundEffectManager.Instance.Play(3);
            warningPopup.SetActive(true);
            warningPopup.transform.GetChild(0).GetComponent<ShakerQuizMark>().Shake(0.3f, 15f);
            Invoke(nameof(StopTime), 0.4f);
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
            default:
                break;
        }
        SceneManager.LoadScene(nextStage);
    }

    private void StopTime()
    {
        Time.timeScale = 0; // 시간 멈추기
    }

    public void OnClicOkButton()
    {
        SoundEffectManager.Instance.Play(0);
        Time.timeScale = 1; //시간 흐르기
    }

}
