using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupManager : MonoBehaviour
{
    // 테스트용
    public TextMeshProUGUI screenPosTxt; // (테스트용) 스크린 클릭 position
    public TextMeshProUGUI partInfoTxt; // (테스트용) 부품 생성 확인
    public TextMeshProUGUI partCntTxt; // (테스트용) 주운 부품 개수 확인
    public TextMeshProUGUI distancePart; // (테스트용) 클릭한 부품과의 거리
    public GameObject debuggingPanel; // 디버깅 패널

    // 실제 UI
    public TextMeshProUGUI ewhaLevelTxt;
    public TextMeshProUGUI partTxt;
    public TextMeshProUGUI hintTxt;

    // 팝업창 & 스낵바
    public GameObject firstPartPopup; // 처음 부품 주운 후 나오는 팝업
    public GameObject partGuideCanavas; // 씬 시작할 때 나오는 부품 줍기 가이드 팝업
    public GameObject planeSnackbar; // 바닥 인식 중에 뜨는 스낵바
    public GameObject partSnackbar; // 부품 줍기 중에 뜨는 스낵바
    public GameObject pickPartSnackbar; // 부품 주운 후 뜨는 스낵바

    // 가이드 팝업 뜰 때 비활성화 할  UI
    public List<GameObject> inactiveWhileGuide = new List<GameObject>();

    // 힌트 팝업창
    public GameObject hintPopup;
    // 힌트 가이드 텍스트창
    public TextMeshProUGUI hintGuideTxt;


    public void SetDistancePart(float distance)
    {
        distancePart.text = "부품 거리 " + distance;
    }

    public void SetScreenPosTxt(Vector3 rayPos)
    {
        screenPosTxt.text = "현재 클릭 위치\n" + rayPos;
    }

    public void SetPartInfoTxt(int currPathIdx)
    {
        partInfoTxt.text = (currPathIdx + 1).ToString() + "번째 반경 ";
    }

    public void SetDebuggingPartTxt(int partCnt)
    {
        partCntTxt.text = "part count : " + partCnt;
    }

    public void SetPartCntTxt(int partCnt, int totalPartCnt)
    {
        partTxt.text = $"{partCnt}/{totalPartCnt}";
    }

    public void SetHintCntTxt(int hintCnt, int totalHintCnt)
    {
        hintTxt.text = $"{hintCnt}/{totalHintCnt}";
    }

   
    public void SetDebuggingScreenPosTxt(int index, Vector2 createdPos)
    {
        screenPosTxt.text = $"s_p {index}: {createdPos} \n";
    }

    public void OpenFirstPartPopup()
    {
        firstPartPopup.SetActive(true);
        Time.timeScale = 0; // 시간 멈추기 (그 다음 부품 생성되는 시간 맞추기 위해)
    }

    public void OpenPartGuide(float interval)
    {
        StartCoroutine(OpenPartGuideCoroutine(interval));
    }

    private IEnumerator OpenPartGuideCoroutine(float interval)
    {
        // 가이드 캔버스
        SoundEffectManager.Instance.Play(1);
        partGuideCanavas.SetActive(true);

        foreach (GameObject obj in inactiveWhileGuide)
        {
            obj.SetActive(false);
        }

        // 기다리기
        yield return new WaitForSeconds(interval);

        // 가이드 캔버스 지우기
        partGuideCanavas.SetActive(false);
        foreach (GameObject obj in inactiveWhileGuide)
        {
            obj.SetActive(true);
        }
    }

    public void OpenPartScnackbar()
    {
        StartCoroutine(OpenPartScnackbarCoroutine());
    }

    private IEnumerator OpenPartScnackbarCoroutine()
    {
        partSnackbar.SetActive(true);
        partSnackbar.GetComponent<FadeInOut>().FadeInAll();

        yield return new WaitForSeconds(2f);

        if (partSnackbar.activeSelf == true)
        {
            partSnackbar.GetComponent<FadeInOut>().FadeOutAll();
            yield return new WaitForSeconds(1f);
            partSnackbar.SetActive(false);
        }
    }

    public void OpenPickPartSnackbar()
    {
        StartCoroutine(OpenPickPartSnackbarCoroutine());
    }


    private IEnumerator OpenPickPartSnackbarCoroutine()
    {
        // 부품 주운 경우 기존 스낵바 inactivate
        partSnackbar.SetActive(false);
        planeSnackbar.SetActive(false);

        // 부품 주운 스낵바 띄우기
        pickPartSnackbar.SetActive(true);
        pickPartSnackbar.GetComponent<FadeInOut>().FadeInAll();

        yield return new WaitForSeconds(2f);

        if (pickPartSnackbar.activeSelf == true)
        {
            pickPartSnackbar.GetComponent<FadeInOut>().FadeOutAll();
            yield return new WaitForSeconds(1f);
            pickPartSnackbar.SetActive(false);

        }
    }

    public void OpenHint(string txt)
    {
        Time.timeScale = 0; // 시간 멈추기 (그 다음 부품 생성되는 시간 맞추기 위해)
        hintTxt.text = txt;
        hintPopup.SetActive(true);
    }

    public void PopupOkButton()
    {
        Time.timeScale = 1; // 시간 다시 흐르기
    }

    public void PopupOpen()
    {
        Time.timeScale = 0; // 시간 멈추기
    }

    private void HideShowDebuggingPanel(bool isOn)
    {
        debuggingPanel.SetActive(isOn);
    }

    public void OnClickDebuggingPanel(bool isOn)
    {
        HideShowDebuggingPanel(isOn);
    }

}
