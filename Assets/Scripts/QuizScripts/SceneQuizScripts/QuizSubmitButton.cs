using UnityEngine;
using UnityEngine.UI;

public class QuizSubmitButton : MonoBehaviour
{
    private Button submitButton;
    private SceneQuizManager quizManager;

    void Start()
    {
        submitButton = GetComponent<Button>();
        quizManager = FindObjectOfType<SceneQuizManager>();

        submitButton.onClick.AddListener(SubmitAnswer);
    }

    void Update()
    {
        // 선택한 답이 없으면 버튼 비활성화
        if (quizManager.currUserAnswerIndex == -1)
        {
            submitButton.interactable = false;
        }
        else
        {
            submitButton.interactable = true;
        }
    }

    void SubmitAnswer()
    {
        quizManager.SubmitAnswer();
    }
}
