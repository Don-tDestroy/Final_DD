using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescribeQuiz : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string QuestionText;
        public string[] Options;
        public int CorrectAnswerIndex;

        public Question(string questionText, string[] options, int correctAnswerIndex)
        {
            QuestionText = questionText;
            Options = options;
            CorrectAnswerIndex = correctAnswerIndex;
        }
    }

    // 현재 어디까지 왔는 지에 따라 문제 달라짐
    public int currStep = 0;

    public TextMeshProUGUI describeTxt; // 문제
    public TextMeshProUGUI[] optionTxts; // 선택지


    public List<Question> questions = new List<Question>
    {
        // 객관식
        new Question("다음 중 ECC에 있는 것은?",
            new string[] { "식당",
                "서점",
                "영화관",
                "열람실",
                "전부" }, 4),
        new Question("1학년 1학기가 끝났을 때, 채플을 미수료하면 어떻게 될까?",
                     new string[] { "다음 학기에 2개를 들으면 된다.",
                         "채플 F학점을 받게 된다.",
                         "다음 학기 채플에서 장기자랑을 해야 한다.",
                         "다음 학기 채플에서 합창을 해야 한다.",
                         "지도교수님과 면담시간을 가져야 한다." }, 0),
        // OX
        new Question("공대에는 식당이 있다", new string[] { "True", "False" }, 0)
    };
    private List<int> randomizedOptionIndices = new List<int>(); // 랜덤화된 선택지 인덱스 리스트
    private int correctAnswerIndex; // 랜덤화된 선택지의 정답 인덱스

    void Start()
    {
        DisplayQuestion();
    }

    public void DisplayQuestion()
    {
        if (currStep < questions.Count)
        {
            describeTxt.text = questions[currStep].QuestionText;
            RandomizeOptions();
            for (int i = 0; i < optionTxts.Length; i++)
            {
                optionTxts[i].text = questions[currStep].Options[randomizedOptionIndices[i]];
            }
        }
        else
        {
            describeTxt.text = "퀴즈 끝";
        }
    }

    public void NextStep()
    {
        currStep++;
        DisplayQuestion();
    }

    public int GetCorrectAnswerIndex()
    {
        return correctAnswerIndex;
    }

    // 문제 랜덤하게 섞기
    private void RandomizeOptions()
    {
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
}