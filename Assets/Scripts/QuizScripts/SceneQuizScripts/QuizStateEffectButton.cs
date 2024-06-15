using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizStateEffectButton : MonoBehaviour
{
    public Button button;
    public Image checkImage; // Reference to the child Image component for the checkmark
    public TextMeshProUGUI buttonText; // Reference to the child TextMeshProUGUI component for the button text
    private Color originalButtonColor;
    private Color originalTextColor;

    private Color selectedButtonColor = new Color(0f / 255f, 82f / 255f, 66f / 255f); // 선택된 버튼의 색상

    void Start()
    {
        button = GetComponent<Button>();
        originalButtonColor = button.image.color; // 초기 버튼 색상 저장
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        originalTextColor = buttonText.color; // 초기 텍스트 색상 저장
        checkImage.gameObject.SetActive(false); // 처음에는 Check 이미지 비활성화
    }

    // 선택된 버튼의 색상을 변경하고 Check 이미지를 활성화
    public void SetSelected()
    {
        button.image.color = selectedButtonColor; // 버튼 색상을 선택된 색상으로 변경
        buttonText.color = Color.white; // 텍스트 색상을 하얀색으로 변경
        checkImage.gameObject.SetActive(true); // Check 이미지 활성화
    }

    // 선택되지 않은 버튼의 색상을 원래대로 되돌리고 Check 이미지를 비활성화
    public void SetUnselected()
    {
        button.image.color = originalButtonColor; // 원래 버튼 색상으로 복원
        buttonText.color = originalTextColor; // 원래 텍스트 색상으로 복원
        checkImage.gameObject.SetActive(false); // Check 이미지 비활성화
    }
}