using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptManager : MonoBehaviour
{
    public static PromptManager Instance;

    [Header("현재 질문")]
    public int currentCount = -1;
    public QuestionData currentQuestion;

    [Header("답변 저장")]
    public Dictionary<int, string> answers = new();

    [Header("인풋 UI 요소")]
    public TMP_InputField inputField;
    public List<Toggle> toggleList = new List<Toggle>();

    [Header("질문 SO")]
    public List<QuestionData> datas = new();

    [Header("프롬프트 템플릿")]
    public PromptTemplateData templateData;

    private void Awake()
    {
        Instance = this;
    }

    //--------------------------------------------------
    // 답변 하나 저장
    //--------------------------------------------------

    public void SaveAnswer()
    {
        answers[currentQuestion.ID] = inputField.text;

        for (int i = 0; i < toggleList.Count; i++)
        {
            if (toggleList[i].isOn)
            {
                if (!string.IsNullOrWhiteSpace(answers[currentQuestion.ID]))
                    answers[currentQuestion.ID] += ", ";

                answers[currentQuestion.ID] += currentQuestion.recommendedAnswers[i];
            }
        }
    }

    //--------------------------------------------------
    // 질문 시작
    //--------------------------------------------------
    public void StartQuestion()
    {
        currentCount = 0;
        currentQuestion = datas[currentCount];
    }

    //--------------------------------------------------
    // 다음 질문
    //--------------------------------------------------

    public void NextQuestion()
    {
        // 마지막 질문이면
        if (currentCount == datas.Count - 1)
        {
            // Prompt 생성
            BuildPrompt();
            return;
        }

        if (currentCount >= datas.Count - 1) return;

        currentCount++;
        currentQuestion = datas[currentCount];
    }

    //--------------------------------------------------
    // Prompt 생성
    //--------------------------------------------------

    public string BuildPrompt()
    {
        return Regex.Replace(templateData.template, @"\{(\d+)\}", match =>
        {
            int id = int.Parse(match.Groups[1].Value);

            return answers.TryGetValue(id, out string value)
                ? value
                : match.Value; // 없으면 {1} 그대로 유지
        });
    }
}