using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public string sceneName;
    public string cameraSceneName;

    private bool isCameraScene;

    public void Start()
    {
        if (sceneName != cameraSceneName) ;

    }
    public void GoToCameraScene()
    {
        // 이전 씬이 어디였는지 저장하고 그런 절차가 필요함
        SceneManager.LoadScene("TestCameraScene"); // 카메라 씬으로 이동
    }

    public void ReturnToOriginScene()
    {

    }
}