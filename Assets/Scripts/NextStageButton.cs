using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageButton : MonoBehaviour
{
    private int currStage=0;

    public void onClickNextStage() {
        currStage++;
        SceneManager.LoadScene(currStage);
    }

}
