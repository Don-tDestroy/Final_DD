using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneQuizManager : MonoBehaviour
{
    // UI 상태 제어

    private DescribeQuiz describeQuiz;
    [SerializeField]
    public QuizResultPopup quizResultPopup; // Reference to the QuizResultPopup script
    [SerializeField]
    private GameObject multipleChoicePanel, oxPanel;
    [SerializeField]
    private QuizStateEffectButton[] multipleChoiceButtons;
    [SerializeField]
    private QuizStateEffectButton[] oxButtons;


    [SerializeField] private GameObject quizMarkObject; // Reference to the QuizMark GameObject
    private ShakerQuizMark shakerQuizMark;

    public int currStep = 0;
    public int currUserAnswerIndex;

    public int currStage = 0;//임시

    void Start()
    {
        describeQuiz = FindObjectOfType<DescribeQuiz>();
        shakerQuizMark = quizMarkObject.GetComponent<ShakerQuizMark>(); // 틀렸을 때 좌우로 흔들리는 효과

        // 객관식 버튼 이벤트 할당
        for (int i = 0; i < multipleChoiceButtons.Length; i++)
        {
            int index = i;
            multipleChoiceButtons[i].GetComponent<Button>().onClick.AddListener(() => describeQuiz.SelectAnswer(index));
        }

        // OX 버튼 이벤트 할당
        for (int i = 0; i < oxButtons.Length; i++)
        {
            int index = i;
            oxButtons[i].GetComponent<Button>().onClick.AddListener(() => describeQuiz.SelectAnswer(index));
        }

        quizResultPopup.HideResult();

    }

    public void ShowMultipleChoicePanel(string[] options, List<int> randomizedOptionIndices)
    {
        multipleChoicePanel.SetActive(true);
        oxPanel.SetActive(false);
        for (int i = 0; i < multipleChoiceButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = multipleChoiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = options[randomizedOptionIndices[i]];
            multipleChoiceButtons[i].SetUnselected();
        }
    }

    public void ShowOXPanel(string[] options)
    {
        multipleChoicePanel.SetActive(false);
        oxPanel.SetActive(true);
        for (int i = 0; i < options.Length; i++)
        {
            TextMeshProUGUI buttonText = oxButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = options[i];
            oxButtons[i].SetUnselected();
        }
    }



    public void HideAllPanels()
    {
        multipleChoicePanel.SetActive(false);
        oxPanel.SetActive(false);
    }

    public void UpdateSelection(int userAnswerIndex)
    {
        DescribeQuiz.Question currentQuestion = describeQuiz.questions[currStep];

        if (currentQuestion.QuestionType == DescribeQuiz.QuestionTypes.MultipleChoice)
        {
            // 다 풀고
            for (int i = 0; i < multipleChoiceButtons.Length; i++)
            {
                multipleChoiceButtons[i].SetUnselected();
            }
            // 선택한 버튼만 색 변경
            multipleChoiceButtons[userAnswerIndex].SetSelected();
        }
        else if (currentQuestion.QuestionType == DescribeQuiz.QuestionTypes.TrueFalse)
        {
            for (int i = 0; i < oxButtons.Length; i++)
            {
                oxButtons[i].SetUnselected();
            }
            oxButtons[userAnswerIndex].SetSelected();
        }
    }
    public void NextStep()
    {
        currStep++;
    }

    public void SubmitAnswer()
    {
        if (currStep < describeQuiz.questions.Count)
        {
            DescribeQuiz.Question currentQuestion = describeQuiz.questions[currStep];
            int correctAnswerIndex = describeQuiz.GetCorrectAnswerIndex();

            NextStep();

            string explanation = currentQuestion.Explanation; // 해설
            if (currUserAnswerIndex == correctAnswerIndex)
            {
                quizResultPopup.ShowResult(true, "정답이에요!", explanation, "+10");
            }
            else
            {
                quizResultPopup.ShowResult(false, "틀렸어요.", explanation, "-10");
                shakerQuizMark.Shake(0.3f, 15f); // Shake the QuizMark for 0.5 seconds with magnitude 10
            }
        }
        else
        {
            Debug.Log("null submit");
        }
    }
}
