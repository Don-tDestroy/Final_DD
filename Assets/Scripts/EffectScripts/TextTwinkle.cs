using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TextTwinkle : MonoBehaviour
{
    private TextMeshProUGUI text;
    private FadeInOut fadeInOut;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        fadeInOut = GetComponent<FadeInOut>();
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        while (true)
        {
            fadeInOut.FadeInText(text, 1f);
            yield return new WaitForSeconds(2f);
        }
    }
}