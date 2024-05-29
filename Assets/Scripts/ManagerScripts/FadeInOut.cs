using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class FadeInOut : MonoBehaviour
{
    [SerializeField]
    private List<Image> imageList = new List<Image>();
    [SerializeField]
    private List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();


    public void FadeInAll(float speed = 2f)
    {
        for (int i = 0; i < imageList.Count; i++)
        {
            FadeInImage(imageList[i], speed);
        }
        for (int i = 0; i < textList.Count; i++)
        {
            FadeInText(textList[i], speed);
        }
    }

    public void FadeOutAll(float speed = 2f)
    {
        for (int i = 0; i < imageList.Count; i++)
        {
            FadeOutImage(imageList[i], speed);
        }
        for (int i = 0; i < textList.Count; i++)
        {
            FadeOutText(textList[i], speed);
        }
    }


    public void FadeInImage(Image img, float speed = 2f)
    {
        StartCoroutine(FadeInImageCoroutine(img, speed));
    }

    public void FadeOutImage(Image img, float speed = 2f)
    {
        StartCoroutine(FadeOutImageCoroutine(img, speed));
    }

    private IEnumerator FadeInImageCoroutine(Image img, float speed) // 알파값 0 -> 1
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        while (img.color.a < 1.0f)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a + Time.deltaTime * speed);
            yield return null;
        }
    }

    private IEnumerator FadeOutImageCoroutine(Image img, float speed)  // 알파값 1 -> 0
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);

        while (img.color.a > 0.0f)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - (Time.deltaTime * speed));
            yield return null;
        }
    }

    public void FadeInText(TextMeshProUGUI txt, float speed = 2f)
    {
        StartCoroutine(FadeInTextCoroutine(txt, speed));
    }

    public void FadeOutText(TextMeshProUGUI txt, float speed = 2f)
    {
        StartCoroutine(FadeOutTextCoroutine(txt, speed));
    }

    private IEnumerator FadeInTextCoroutine(TextMeshProUGUI txt, float speed) // 알파값 0 -> 1
    {
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
        while (txt.color.a < 1.0f)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a + Time.deltaTime * speed);
            yield return null;
        }
    }

    private IEnumerator FadeOutTextCoroutine(TextMeshProUGUI txt, float speed)  // 알파값 1 -> 0
    {
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1);

        while (txt.color.a > 0.0f)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a - (Time.deltaTime * speed));
            yield return null;
        }
    }
}
