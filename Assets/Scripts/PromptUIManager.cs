using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptUIManager : MonoBehaviour
{
    public static PromptUIManager Instance;

    [Header("질문 UI")]
    public TMP_Text questionSubText;
    public TMP_Text questionText;
    public TMP_Text inputPlaceholderText;
    public List<TMP_Text> toggleTextList = new();
    public List<GameObject> toggleButton = new();
    public TMP_Text confirmButtonText;

    [Header("결과 UI")]
    public TMP_InputField PromptKr;
    public TMP_InputField PromptEn;

    [Header("진행도 UI")]
    public List<Image> progressDotList = new();

    [Header("진행도 색상")]
    [SerializeField] private string activeColorHex = "#80624C";
    [SerializeField] private string inactiveColorHex = "#EADED5";

    [Header("캔버스 그룹")]
    public CanvasGroup QuestionTab;
    public CanvasGroup PromptTab;

    private void Awake()
    {
        Instance = this;
    }

    //--------------------------------------------------
    // 페이지 UI 업데이트
    //--------------------------------------------------

    public void UpdateUI()
    {
        QuestionData currentQ = PromptManager.Instance.currentQuestion;

        if (currentQ == null) return;

        questionSubText.text = currentQ.questionSubText;
        questionText.text = currentQ.title;

        for (int i = 0; i < toggleButton.Count; i++)
        {
            toggleButton[i].SetActive(false);
        }

        for (int i = 0; i < currentQ.recommendedAnswers.Count; i++)
        {
            toggleButton[i].SetActive(true);
            toggleTextList[i].text = currentQ.recommendedAnswers[i];
        }

        // 마지막 질문이면
        if (PromptManager.Instance.currentCount == PromptManager.Instance.datas.Count - 1)
        {
            confirmButtonText.text = "완료";
        }
        else
        {
            confirmButtonText.text = "다음";
        }
    }

    public void ToggleQuestionTab()
    {
        if (QuestionTab.alpha == 0)
        {
            QuestionTab.alpha = 1;
            QuestionTab.interactable = true;
            QuestionTab.blocksRaycasts = true;
        }
        else
        {
            QuestionTab.alpha = 0;
            QuestionTab.interactable = false;
            QuestionTab.blocksRaycasts = false;
        }
    }

    public void TogglePromptTab()
    {
        if (PromptTab.alpha == 0)
        {
            PromptTab.alpha = 1;
            PromptTab.interactable = true;
            PromptTab.blocksRaycasts = true;
        }
        else
        {
            PromptTab.alpha = 0;
            PromptTab.interactable = false;
            PromptTab.blocksRaycasts = false;
        }
    }

    //--------------------------------------------------
    // 현재 페이지 표시
    //--------------------------------------------------

    /// <summary>
    /// 현재 질문에 맞게 진행도 점 색상 갱신
    /// </summary>
    public void RefreshProgressDots()
    {
        if (PromptManager.Instance.currentQuestion == null)
            return;

        Color activeColor = HexToColor(activeColorHex);
        Color inactiveColor = HexToColor(inactiveColorHex);
        inactiveColor.a = 0.6f;

        int currentId = PromptManager.Instance.currentQuestion.ID;

        for (int i = 0; i < progressDotList.Count; i++)
        {
            progressDotList[i].color =
                (i == currentId) ? activeColor : inactiveColor;

            progressDotList[i].transform.localScale =
                (i == currentId) ? new Vector3(1.2f, 1.2f, 1.2f) : new Vector3(1, 1, 1);
        }
    }

    private Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out Color color))
            return color;

        return Color.white;
    }
}