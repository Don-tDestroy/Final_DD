using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneQuizManager : MonoBehaviour
{
    // UI 상태 제어

    private DescribeQuiz describeQuiz;
    [SerializeField]
    private GameObject answerPopup, noAnswerPopup;
    [SerializeField]
    private GameObject multipleChoicePanel, oxPanel;
    [SerializeField]
    private QuizStateEffectButton[] multipleChoiceButtons;
    [SerializeField]
    private QuizStateEffectButton[] oxButtons;

    public int currStep = 0;
    public int currUserAnswerIndex;

    void Start()
    {
        describeQuiz = FindObjectOfType<DescribeQuiz>();

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
    }

    public void ShowMultipleChoicePanel(string[] options, List<int> randomizedOptionIndices)
    {
        multipleChoicePanel.SetActive(true);
        oxPanel.SetActive(false);
        for (int i = 0; i < options.Length; i++)
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
            for (int i = 0; i < multipleChoiceButtons.Length; i++)
            {
                multipleChoiceButtons[i].SetUnselected();
            }
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
            int correctAnswerIndex = describeQuiz.GetCorrectAnswerIndex();
            if (currUserAnswerIndex == correctAnswerIndex)
            {
                answerPopup.SetActive(true);
                NextStep();
            }
            else
            {
                noAnswerPopup.SetActive(true);
            }
        }
        else
        {
            Debug.Log("null submit");
        }
    }
}
