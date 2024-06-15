using UnityEngine;
using UnityEngine.UI;

public class QuizStateEffectButton : MonoBehaviour
{
    public Button button;
    private Color originalColor;

    void Start()
    {
        button = GetComponent<Button>();
        originalColor = button.image.color;
    }

    // 선택된 버튼의 색상을 변경
    public void SetSelected()
    {
        button.image.color = Color.yellow;
    }

    // 선택되지 않은 버튼의 색상을 원래대로 되돌림
    public void SetUnselected()
    {
        button.image.color = originalColor;
    }
}