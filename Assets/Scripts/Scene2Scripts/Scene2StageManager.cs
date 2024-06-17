using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene2StageManager : MonoBehaviour
{
    void Start()
    {
        int currStage = GameManager.Instance.GetStageNumber() + 1;
        GameManager.Instance.SetStageNumber(2);
    }
}