using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    public static string prevSceneName; // 이전 씬 이름
    public static string nextSceneName; // 다음 씬 이름

    public static void GoToCameraScene(string prev, string next)
    {
        prevSceneName = prev;
        nextSceneName = next;
        SceneManager.LoadScene("TestCameraScene"); // 카메라 씬으로 이동
    }

    public static void GoToNextScene()
    {
        SceneManager.LoadScene(nextSceneName); // 다음 씬으로 이동
    }
   
    public static void ReturnAfterCameraScene()
    {
        SceneManager.LoadScene(prevSceneName); // 이전 씬으로 이동
    }
}