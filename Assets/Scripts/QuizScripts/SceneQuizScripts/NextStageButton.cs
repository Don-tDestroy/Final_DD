using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageButton : MonoBehaviour
{
    private SceneQuizManager sceneQuizManager;

    void Start()
    {
        sceneQuizManager = FindObjectOfType<SceneQuizManager>();
    }

    public void onClickNextStage() {
        SceneManager.LoadScene(sceneQuizManager.currStage+1);
    }

}
