using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizResultPopup : MonoBehaviour
{
    // 퀴즈 결과를 표시하고, 정답 여부에 따라 UI
    [SerializeField] private Image resultImage; // Correct or incorrect image
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI explanationText; // Explanation text
    [SerializeField] private Button nextButton; // Button to proceed to the next question
    [SerializeField] private TextMeshProUGUI userStatus; // 이화력


    [SerializeField] private Sprite correctSprite; // Sprite for the correct answer
    [SerializeField] private Sprite incorrectSprite; // Sprite for the incorrect answer

    // Method to show the result popup with the appropriate information
    public void ShowResult(bool isCorrect, string result, string explanation, string status)
    {
        resultImage.sprite = isCorrect ? correctSprite : incorrectSprite;
        resultText.text = result;
        explanationText.text = explanation;
        userStatus.text = status;
        gameObject.SetActive(true);
    }

    // Method to hide the result popup
    public void HideResult()
    {
        gameObject.SetActive(false);
    }


}
