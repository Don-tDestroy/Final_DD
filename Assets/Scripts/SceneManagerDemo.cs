using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerDemo : MonoBehaviour
{
    public string sceneName;
    public void LoadSceneName()
    {
        SceneManager.LoadScene(sceneName);

    }
}
