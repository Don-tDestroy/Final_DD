using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MapSetter : MonoBehaviour
{
    public GameObject locationObj;
    public GameObject arrowObj;

    void Start()
    {
        Scene currScene = SceneManager.GetActiveScene();

        switch (currScene.name)
        {
            case "Scene_1":
            case "Scene_2":
                SetTextAndPinPosition(0);
                break;
            case "Scene_3":
                SetTextAndPinPosition(1);
                break;
            case "Scene_4":
                SetTextAndPinPosition(2);
                break;
            case "Scene_5":
                SetTextAndPinPosition(3);
                break;
            case "Scene_6":
                SetTextAndPinPosition(4);
                break;
        }
    }

    void SetTextAndPinPosition(int childNum)
    {
        Transform currTransform = locationObj.transform.GetChild(childNum);
        TextMeshProUGUI targetText = currTransform.GetComponent<TextMeshProUGUI>();
        targetText.color = new Color32(0, 104, 0, 255);
        targetText.fontStyle = FontStyles.Bold;

        if (childNum != 4) 
        {
            Transform nextTransform = locationObj.transform.GetChild(childNum + 1);

            float midPointY = (nextTransform.position.y + currTransform.position.y) / 2;
            Vector3 arrowPosition = arrowObj.transform.position;
            arrowPosition.y = midPointY;
            arrowObj.transform.position = arrowPosition;
        }
        else // 현 위치가 공과대학
        {
            arrowObj.SetActive(false); // 마지막 목적지에서는 화살표가 필요 없음
        }
    }
}



