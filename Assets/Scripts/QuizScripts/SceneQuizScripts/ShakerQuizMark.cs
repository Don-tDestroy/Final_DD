using System.Collections;
using UnityEngine;

public class ShakerQuizMark : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 originalPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // Get the RectTransform component
        originalPosition = rectTransform.localPosition; // Store the original position
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(ShakeAnimation(duration, magnitude));
    }

    private IEnumerator ShakeAnimation(float duration, float magnitude)
    {
        float elapsed = 0f;
        Vector3 originalPos = rectTransform.localPosition;

        while (elapsed < duration)
        {
            float x = originalPos.x + Mathf.Sin(elapsed * 40f) * magnitude; // Adjust the speed and magnitude as needed
            rectTransform.localPosition = new Vector3(x, originalPos.y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.localPosition = originalPos; // Reset to the original position after shaking
    }
}