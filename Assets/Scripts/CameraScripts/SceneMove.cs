using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public string prevSceneName; // 현재 씬 이름
    public string destSceneName; // 다음 씬 이름

    public void GoToScene()
    {
        SceneManager.LoadScene(destSceneName); // 목적지 씬으로 이동
    }

    public void GoToCameraScene()
    {
        SceneManager.LoadScene("TestCameraScene"); // 카메라 씬으로 이동
    }

    public void ReturnAfterCameraScene()
    {
        SceneManager.LoadScene(prevSceneName); // 이전 씬으로 이동
    }
}