using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescribeQuiz : MonoBehaviour
{
    // 퀴즈 데이터 처리

    public TextMeshProUGUI questionTxt;

    private List<int> randomizedOptionIndices = new List<int>(); // 객관식은 랜덤하게 섞기
    private int correctAnswerIndex;

  
    public void DisplayQuestion()
    {
        int currStep = SceneQuizManager.Instance.currStep;
        if (currStep < QuestionClass.questions.Count)
        {
            Question currentQuestion = QuestionClass.questions[currStep];
            questionTxt.text = currentQuestion.QuestionText;

            
            if (currentQuestion.QuestionType == QuestionTypes.MultipleChoice) // 객관식
            {
                RandomizeOptions();
                SceneQuizManager.Instance.ShowMultipleChoicePanel(currentQuestion.Options, randomizedOptionIndices);
            }
            else if (currentQuestion.QuestionType == QuestionTypes.TrueFalse) // OX
            {
                SceneQuizManager.Instance.ShowOXPanel(currentQuestion.Options);
            }
        }
        else
        {
            questionTxt.text = "null quiz";
            SceneQuizManager.Instance.HideAllPanels();
        }
    }

    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }

    // 객관식 선택지 랜덤하게 섞기
    private void RandomizeOptions()
    {
        int currStep = SceneQuizManager.Instance.currStep;

        randomizedOptionIndices.Clear();
        for (int i = 0; i < QuestionClass.questions[currStep].Options.Length; i++)
        {
            randomizedOptionIndices.Add(i);
        }
        for (int i = 0; i < randomizedOptionIndices.Count; i++)
        {
            int temp = randomizedOptionIndices[i];
            int randomIndex = Random.Range(i, randomizedOptionIndices.Count);
            randomizedOptionIndices[i] = randomizedOptionIndices[randomIndex];
            randomizedOptionIndices[randomIndex] = temp;
        }
        correctAnswerIndex = randomizedOptionIndices.IndexOf(QuestionClass.questions[currStep].CorrectAnswerIndex);
    }

    public void SelectAnswer(int userAnswerIndex)
    {
        Debug.Log(userAnswerIndex);
        SceneQuizManager.Instance.UpdateSelection(userAnswerIndex);
        SceneQuizManager.Instance.currUserAnswerIndex = userAnswerIndex;
    }
}