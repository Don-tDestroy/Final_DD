using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ProgressTitle : MonoBehaviour
{
    public string status;

    public TextMeshProUGUI statusText;
    public float updateInterval = 0.5f; // ������Ʈ ����

    private string[] statusStrings = { ".", "..", "..." };
    private int currentIndex = 0;

    void Start()
    {
        // Coroutine ����
        StartCoroutine(UpdateStatus());
    }

    IEnumerator UpdateStatus()
    {
        while (true)
        {
            statusText.text = $"{status}{statusStrings[currentIndex]}";

            currentIndex = (currentIndex + 1) % statusStrings.Length;

            yield return new WaitForSeconds(updateInterval);
        }
    }

}
