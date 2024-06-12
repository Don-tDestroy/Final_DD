using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneQuizManager : MonoBehaviour
{
    private DescribeQuiz describeQuiz;

    void Start()
    {
        describeQuiz = FindObjectOfType<DescribeQuiz>();
    }

    public void SubmitAnswer(int userAnswerIndex)
    {
        int step = describeQuiz.currStep;

        if (step < describeQuiz.questions.Count)
        {
            int correctAnswerIndex = describeQuiz.GetCorrectAnswerIndex();
            if (userAnswerIndex == correctAnswerIndex)
            {
                Debug.Log("정답입니다!");
                // 정답 처리 로직
            }
            else
            {
                Debug.Log("오답입니다.");
                // 오답 처리 로직
            }

            describeQuiz.NextStep();
        }
        else
        {
            Debug.Log("퀴즈 끝");
        }
    }
}
