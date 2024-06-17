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
        SceneManager.LoadScene(currStage + 1);
    }
}