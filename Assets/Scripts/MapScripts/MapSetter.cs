using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MapSetter : MonoBehaviour
{
    void Start()
    {
        Scene currScene = SceneManager.GetActiveScene();

        switch (currScene.name)
        {
            case "Scene_1":
            case "Scene_2":
                SetTextAndPinPosition("MainGate");
                break;
            case "Scene_3":
                SetTextAndPinPosition("ECC");
                break;
            case "Scene_4":
                SetTextAndPinPosition("Posco");
                break;
            case "Scene_5":
                SetTextAndPinPosition("Science");
                break;
            case "Scene_6":
                SetTextAndPinPosition("Engineering");
                break;
        }
    }

    void SetTextAndPinPosition(string targetName)
    {
        Transform targetTransform = transform.Find(targetName);
        if (targetTransform != null)
        {
            TextMeshProUGUI targetText = targetTransform.GetComponent<TextMeshProUGUI>();
            if (targetText != null)
            {
                targetText.fontSize = 40;
            }

            Transform pinTransform = transform.Find("Pin");
            if (pinTransform != null)
            {
                pinTransform.position = targetTransform.position;
            }
        }
    }
}



