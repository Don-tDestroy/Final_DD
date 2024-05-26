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


    public void FadeInAll()
    {
        for (int i = 0; i < imageList.Count; i++)
        {
            FadeInImage(imageList[i]);
        }
        for (int i = 0; i < textList.Count; i++)
        {
            FadeInText(textList[i]);
        }
    }

    public void FadeOutAll()
    {
        for (int i = 0; i < imageList.Count; i++)
        {
            FadeOutImage(imageList[i]);
        }
        for (int i = 0; i < textList.Count; i++)
        {
            FadeOutText(textList[i]);
        }
    }


    public void FadeInImage(Image img)
    {
        StartCoroutine(FadeInImageCoroutine(img));
    }

    public void FadeOutImage(Image img)
    {
        StartCoroutine(FadeOutImageCoroutine(img));
    }

    private IEnumerator FadeInImageCoroutine(Image img) // 알파값 0 -> 1
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        while (img.color.a < 1.0f)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a + Time.deltaTime * 2f);
            yield return null;
        }
    }

    private IEnumerator FadeOutImageCoroutine(Image img)  // 알파값 1 -> 0
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, 1);

        while (img.color.a > 0.0f)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a - (Time.deltaTime * 2f));
            yield return null;
        }
    }

    public void FadeInText(TextMeshProUGUI txt)
    {
        StartCoroutine(FadeInTextCoroutine(txt));
    }

    public void FadeOutText(TextMeshProUGUI txt)
    {
        StartCoroutine(FadeOutTextCoroutine(txt));
    }

    private IEnumerator FadeInTextCoroutine(TextMeshProUGUI txt) // 알파값 0 -> 1
    {
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
        while (txt.color.a < 1.0f)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a + Time.deltaTime * 2f);
            yield return null;
        }
    }

    private IEnumerator FadeOutTextCoroutine(TextMeshProUGUI txt)  // 알파값 1 -> 0
    {
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1);

        while (txt.color.a > 0.0f)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a - (Time.deltaTime * 2f));
            yield return null;
        }
    }
}
