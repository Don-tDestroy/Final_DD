using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MapSetter : MonoBehaviour
{
    public GameObject locationObj;
    public GameObject arrowObj;

    private RectTransform arrowRectTransform;
    private Vector2 initialPos;
    private Vector2 targetPos;

    void Start()
    {
        int currStage = GameManager.Instance.GetStageNumber();

        arrowRectTransform = arrowObj.GetComponent<RectTransform>();

        switch (currStage)
        {
            case 2:
                SetTextAndPinPosition(0);
                break;
            case 3:
                SetTextAndPinPosition(1);
                break;
            case 4:
                SetTextAndPinPosition(2);
                break;
            case 5:
                SetTextAndPinPosition(3);
                break;
            case 6:
                SetTextAndPinPosition(4);
                break;
        }
    }

    void SetTextAndPinPosition(int childNum)
    {
        RectTransform currRectTransform = locationObj.transform.GetChild(childNum).GetComponent<RectTransform>();
        TextMeshProUGUI targetText = currRectTransform.GetComponent<TextMeshProUGUI>();
        targetText.color = new Color32(0, 104, 0, 255);
        targetText.fontStyle = FontStyles.Bold;

        if (childNum != 4) 
        {
            RectTransform nextRectTransform = locationObj.transform.GetChild(childNum + 1).GetComponent<RectTransform>();
            float midPointY = (nextRectTransform.anchoredPosition.y + currRectTransform.anchoredPosition.y) / 2;

            Vector3 tempPosition = arrowRectTransform.anchoredPosition;
            tempPosition.y = midPointY;
            arrowRectTransform.anchoredPosition = tempPosition;

            initialPos = arrowRectTransform.anchoredPosition; // 현재 위치 
            targetPos = initialPos + Vector2.up * 1f; // 목표 위치

            StartCoroutine(UpAnimation());
        }
        else // 현 위치가 공과대학
        {
            arrowObj.SetActive(false); // 마지막 목적지에서는 화살표가 필요 없음
        }
    }

    private IEnumerator UpAnimation()
    {
        while (true)
        {
            // 위로 이동
            arrowRectTransform.anchoredPosition = targetPos;
            yield return new WaitForSeconds(0.8f);

            // 원위치로 돌아가기
            arrowRectTransform.anchoredPosition = initialPos;
            yield return new WaitForSeconds(0.8f);
        }
    }
}



