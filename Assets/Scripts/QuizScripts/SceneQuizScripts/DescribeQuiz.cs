using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescribeQuiz : MonoBehaviour
{
    // 퀴즈 데이터 처리
    public static class QuestionTypes
    {
        public const string MultipleChoice = "MULTIPLE";
        public const string TrueFalse = "OX";
    }

    [System.Serializable]
    public class Question
    {
        public string QuestionText;
        public string[] Options;
        public int CorrectAnswerIndex;
        public string QuestionType;

        public Question(string questionText, string[] options, int correctAnswerIndex, string questionType)
        {
            QuestionText = questionText;
            Options = options;
            CorrectAnswerIndex = correctAnswerIndex;
            QuestionType = questionType;
        }
    }

    private SceneQuizManager sceneQuizManager;
    public TextMeshProUGUI questionTxt;
    public List<Question> questions = new List<Question>
    {
        new Question("다음 중 ECC에 있는 것은?", new string[] { "식당", "서점", "영화관", "열람실", "전부" }, 4, QuestionTypes.MultipleChoice),
        new Question("1학년 1학기가 끝났을 때, 채플을 미수료하면 어떻게 될까?", new string[] { "다음 학기에 2개를 들으면 된다.", "채플 F학점을 받게 된다.", "다음 학기 채플에서 장기자랑을 해야 한다.", "다음 학기 채플에서 합창을 해야 한다.", "지도교수님과 면담시간을 가져야 한다." }, 0, QuestionTypes.MultipleChoice),
        new Question("공대에는 식당이 있다", new string[] { "True", "False" }, 0, QuestionTypes.TrueFalse)
    };

    private List<int> randomizedOptionIndices = new List<int>(); // 객관식은 랜덤하게 섞기
    private int correctAnswerIndex;

    void Start()
    {
        sceneQuizManager = FindObjectOfType<SceneQuizManager>();
        sceneQuizManager.currUserAnswerIndex = -1;

        DisplayQuestion();
    }

    public void DisplayQuestion()
    {
        int currStep = sceneQuizManager.currStep;
        if (currStep < questions.Count)
        {
            Question currentQuestion = questions[currStep];
            questionTxt.text = currentQuestion.QuestionText;


            if (currentQuestion.QuestionType == QuestionTypes.MultipleChoice)
            {
                RandomizeOptions();
                sceneQuizManager.ShowMultipleChoicePanel(currentQuestion.Options, randomizedOptionIndices);
            }
            else if (currentQuestion.QuestionType == QuestionTypes.TrueFalse)
            {
                sceneQuizManager.ShowOXPanel(currentQuestion.Options);
            }
        }
        else
        {
            questionTxt.text = "퀴즈가 끝났습니다!";
            sceneQuizManager.HideAllPanels();
        }
    }

    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }

    private void RandomizeOptions()
    {
        int currStep = sceneQuizManager.currStep;

        randomizedOptionIndices.Clear();
        for (int i = 0; i < questions[currStep].Options.Length; i++)
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
        correctAnswerIndex = randomizedOptionIndices.IndexOf(questions[currStep].CorrectAnswerIndex);
    }

    public void SelectAnswer(int userAnswerIndex)
    {
        sceneQuizManager.UpdateSelection(userAnswerIndex);
        sceneQuizManager.currUserAnswerIndex = userAnswerIndex;
    }
}