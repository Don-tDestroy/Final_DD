using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraStageMove : MonoBehaviour
{
    int currStage;

    void Start()
    {
        currStage = GameManager.Instance.GetStageNumber();
    }

    public void goToNextStage()
    {
        switch (currStage)
        {
            case 2: // stage 2 -> 3
                SceneManager.LoadScene("Scene_3");
                break;
            case 3: // stage 3 -> 4
                SceneManager.LoadScene("Scene_4_Before");
                break;
            case 4: // stage 4 -> 5
                SceneManager.LoadScene("Scene_5_Before");
                break;
            case 5: // stage 5 -> 6
                SceneManager.LoadScene("Scene_6_GPS");
                break;
            default:
                break;
        }
    }
}