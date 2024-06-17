using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;


public class SceneQuizManager : MonoBehaviour
{
    // UI 상태 제어

    private static SceneQuizManager _instance;
    public static SceneQuizManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<SceneQuizManager>();

                if (_instance == null)
                {
                    GameObject go = new GameObject("SceneQuizManager");
                    _instance = go.AddComponent<SceneQuizManager>();
                }
            }
            return _instance;
        }
    }

    private DescribeQuiz describeQuiz;

    [SerializeField]
    public QuizResultPopup quizResultPopup; // 퀴즈 끝나고, 결과 팝업
    [SerializeField]
    private GameObject multipleChoicePanel, oxPanel; // 객관식, OX 문제 패널
    [SerializeField]
    private QuizStateEffectButton[] multipleChoiceButtons; // 객관식 선택지 버튼
    [SerializeField]
    private QuizStateEffectButton[] oxButtons; // OX 선택지 버튼


    [SerializeField] private GameObject quizMarkObject; // 퀴즈 오답 시, 결과 팝업 흔들림
    private ShakerQuizMark shakerQuizMark;

    public int currStep = 0;
    public int currUserAnswerIndex; // 사용자가 선택한 선택지 상태

    Question currentQuestion; // 현재 스테이지에 따라 문제 바뀜

    private bool isAnswer = false; // 정답 여부
    public TextMeshProUGUI quizTitleTxt;
    public GameObject ewhaZeroPopup;

    void Start()
    {
        currStep = GameManager.Instance.GetStageNumber() - 3; // 게임 매니저에서 현재 스테이지 정보 가져옴 (0: stage 3, 1: stage 4, 2: stage 5)
        if (currStep < 0) currStep = 0; // 테스트용 예외처리
        quizTitleTxt.text = $"Quiz #{currStep + 1}";

        currentQuestion = QuestionClass.questions[currStep];
        currUserAnswerIndex = -1;
        quizResultPopup.HideResult();

        if (currentQuestion.QuestionType == QuestionTypes.MultipleChoice) // 객관식
        {
            describeQuiz = multipleChoicePanel.GetComponent<DescribeQuiz>();

            // 객관식 버튼 이벤트 할당
            for (int i = 0; i < multipleChoiceButtons.Length; i++)
            {
                int index = i;
                multipleChoiceButtons[i].GetComponent<Button>().onClick.AddListener(() => describeQuiz.SelectAnswer(index));
            }
        }
        else if (currentQuestion.QuestionType == QuestionTypes.TrueFalse) // OX
        {

            describeQuiz = oxPanel.GetComponent<DescribeQuiz>();

            // OX 버튼 이벤트 할당
            for (int i = 0; i < oxButtons.Length; i++)
            {
                int index = i;
                oxButtons[i].GetComponent<Button>().onClick.AddListener(() => describeQuiz.SelectAnswer(index));
            }
        }

        describeQuiz.DisplayQuestion();


    }

    public void ShowMultipleChoicePanel(string[] options, List<int> randomizedOptionIndices)
    {
        for (int i = 0; i < multipleChoiceButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = multipleChoiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = options[randomizedOptionIndices[i]];
            multipleChoiceButtons[i].SetUnselected();
        }
        multipleChoicePanel.SetActive(true);
        oxPanel.SetActive(false);
    }

    public void ShowOXPanel(string[] options)
    {
        for (int i = 0; i < oxButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = oxButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = options[i];
            oxButtons[i].SetUnselected();
        }
        multipleChoicePanel.SetActive(false);
        oxPanel.SetActive(true);
    }

    public void HideAllPanels()
    {
        multipleChoicePanel.SetActive(false);
        oxPanel.SetActive(false);
    }

    public void UpdateSelection(int userAnswerIndex)
    {
        if (currentQuestion.QuestionType == QuestionTypes.MultipleChoice) // 객관식
        {
            // 선택 시, 모든 선택지의 색 다 풀고
            for (int i = 0; i < multipleChoiceButtons.Length; i++)
            {
                multipleChoiceButtons[i].SetUnselected();
            }
            // 선택한 버튼만 색 변경
            multipleChoiceButtons[userAnswerIndex].SetSelected();
        }
        else if (currentQuestion.QuestionType == QuestionTypes.TrueFalse) // OX
        {
            for (int i = 0; i < oxButtons.Length; i++)
            {
                oxButtons[i].SetUnselected();
            }
            oxButtons[userAnswerIndex].SetSelected();
        }
    }

    public void SubmitAnswer()
    {
        if (currStep < QuestionClass.questions.Count)
        {
            int correctAnswerIndex = describeQuiz.GetCorrectAnswerIndex();

            string explanation = currentQuestion.Explanation; // 해설
            if (currUserAnswerIndex == correctAnswerIndex)
            {
                quizResultPopup.ShowResult(true, "정답이에요!", explanation, "+10");
                isAnswer = true;
            }
            else
            {
                quizResultPopup.ShowResult(false, "틀렸어요.", explanation, "-10");
                shakerQuizMark = quizMarkObject.GetComponent<ShakerQuizMark>(); // 틀렸을 때 좌우로 흔들리는 효과
                shakerQuizMark.Shake(0.3f, 15f);
                isAnswer = false;
            }
        }
        else
        {
            Debug.Log("null submit");
        }
    }

    public void OnClickNextButton()
    {
        // 정답이라면 카메라 씬으로 넘어가기
        if (isAnswer)
        {
            SceneManager.LoadScene("TestCameraScene");
        }
        else
        {
            Debug.Log(currStep);
            // 정답 아니라면 다음 씬으로 넘어가기
            switch (currStep)
            {
                case 0: // stage 3 -> 4
                    SceneManager.LoadScene("Scene_4_Before");
                    break;
                case 1: // stage 4 -> 5
                    SceneManager.LoadScene("Scene_5_Before");
                    break;
                case 2:
                    // stage 5
                    // 이화력 확인해서 엔딩 or stage 6
                    Debug.Log("scene 5 quiz");
                    if (GameManager.Instance.GetEwhaPower() == 0)
                    {
                        Debug.Log("이화력 0 엔딩 !!");
                        // 엔딩씬 로드 전 팝업 안내
                        ewhaZeroPopup.SetActive(true);
                    }
                    else
                    {
                        Debug.Log("scene 6 load");
                        SceneManager.LoadScene("Scene_6_GPS");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
