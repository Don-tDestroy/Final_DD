using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultPopupSetter : MonoBehaviour
{
    public GameObject introObj;
    public TextMeshProUGUI ewhaText;

    void Start()
    {
        ewhaText.text = "+" + GameManager.Instance.GetEwhaPower();
    }

    public void OnClickOkButton()
    {
        // 수정수정~~ TestCameraScene->CameraScene
        if (GameManager.Instance.GetStageNumber() == 2) { SceneManager.LoadScene("TestCameraScene"); } // 예외처리) 튜토리얼 스테이지의 경우 바로 카메라 씬
        else { SceneManager.LoadScene("Scene_Quiz"); }

        introObj.SetActive(false);
        gameObject.SetActive(false);
    }
}
