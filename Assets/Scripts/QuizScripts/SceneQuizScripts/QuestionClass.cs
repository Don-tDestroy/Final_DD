using System.Collections.Generic;
using UnityEngine;

public class QuestionTypes
{
    public const string MultipleChoice = "MULTIPLE";
    public const string TrueFalse = "OX";
}

public class Question
{
    public string QuestionText;
    public string[] Options;
    public int CorrectAnswerIndex;
    public string QuestionType;
    public string Explanation;

    public Question(string questionText, string[] options, int correctAnswerIndex, string questionType, string explanation)
    {
        QuestionText = questionText;
        Options = options;
        CorrectAnswerIndex = correctAnswerIndex;
        QuestionType = questionType;
        Explanation = explanation;
    }
}

public static class QuestionClass
{

    static public List<Question> questions = new List<Question>
    {
        new Question("다음 중 ECC에 있는 것은?", new string[] { "식당", "서점", "영화관", "열람실", "전부" }, 4, QuestionTypes.MultipleChoice, "ECC에는\n식당, 서점, 영화관, 열람실 등\n다양한 시설이 존재합니다."),
        new Question("1학년 1학기가 끝났을 때,\n채플을 미수료하면 어떻게 될까?", new string[] { "다음 학기에 2개를 들으면 된다.", "채플 F학점을 받게 된다.", "다음 학기 채플에서 장기자랑을 해야 한다.", "다음 학기 채플에서 합창을 해야 한다.", "지도교수님과 면담시간을 가져야 한다." }, 0, QuestionTypes.MultipleChoice, "채플을 미수료하면\n다음 학기에 2개를 들으면 됩니다."),
        new Question("공대에는 식당이 있다.", new string[] { "O", "X" }, 0, QuestionTypes.TrueFalse, "공대에는 식당이 있습니다.")
    };

}