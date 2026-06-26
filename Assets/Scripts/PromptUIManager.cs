using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class PromptUIManager : MonoBehaviour
{
    public static PromptUIManager Instance;

    [Header("질문 UI")]
    public TMP_Text questionSubText;
    public TMP_Text questionText;
    public TMP_Text inputPlaceholderText;
    public List<TMP_Text> toggleTextList = new();
    public List<GameObject> toggleButton = new();
    public List<Image> toggleImages = new();

    public TMP_Text confirmButtonText;

    [Header("결과 UI")]
    public TMP_Text PromptKr;
    public TMP_Text PromptEn;
    [SerializeField] private TMP_Text textComponent1;
    [SerializeField] private TMP_Text textComponent2;
    [SerializeField] private TMP_Text textComponent3;
    [SerializeField] private TMP_Text textComponent4;

    [Header("진행도 UI")]
    public List<Image> progressDotList = new();

    [Header("진행도 색상")]
    [SerializeField] private string activeColorHex = "#80624C";
    [SerializeField] private string inactiveColorHex = "#EADED5";

    [Header("캔버스 그룹")]
    public CanvasGroup QuestionTab;
    public CanvasGroup PromptTab;

    [DllImport("__Internal")]
    private static extern void CopyToClipboardWebGL(string text);

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
            toggleImages[i].sprite = currentQ.Images[i];
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

    /// <summary>
    /// 딕셔너리 데이터를 조합하여 4개의 텍스트를 생성하고 적용하는 함수
    /// </summary>
    public void GenerateAndApplyTexts()
    {
        var answers = PromptManager.Instance.answers;

        // 안전장치: 필요한 키가 딕셔너리에 없는 경우 에러 방지
        if (!answers.ContainsKey(0) || !answers.ContainsKey(1) || !answers.ContainsKey(2) || !answers.ContainsKey(6))
        {
            Debug.LogError("딕셔너리에 필요한 Key(0, 1, 2, 6) 중 일부가 없습니다!");
            return;
        }

        // 딕셔너리에서 필요한 밸류 꺼내두기
        string val0 = answers[0]; // 포스터 등
        string val1 = answers[1]; // 20대 타겟 등
        string val2 = answers[2]; // 트렌디하고 감각적인 비주얼 / 미니멀&감성 컨셉 등
        string val6 = answers[6]; // 신메뉴 홍보 / 홍보 상품의 종류 등

        // 1. 첫 번째 텍스트 조합 및 적용
        string txt1 = $"{val1} 타겟의 카페 {val6} {val0}를 AI로 제작을 위한 " +
                      $"맞춤형 프롬프트입니다. {val2} 분위기를 " +
                      $"얻을 수 있도록 컨셉별로 나누어 구성했습니다.\n" +
                      $"\n메뉴의 종류({val6})에 맞춰 괄호[ ] 부분을 수정하여 사용해 보세요.";

        // 3. 세 번째 텍스트 조합 및 적용
        string txt2 = $"선택한 소비자층을 위한 제안 ({val2})"; 

        string txt3 = $"{val2}를 타겟으로 한 " +
                      $"{val2} 분위기입니다. 여백의 미와 부드러운 톤으로 제작을 진행 했습니다.";

        // 4. 네 번째 텍스트 조합 및 적용
        string txt4 = $"국문 가이드: {val1} 타겟의 카페 {val6} {val0}";

        // --- 실제 UI 컴포넌트에 텍스트 꽂아넣기 ---
        if (textComponent1 != null) textComponent1.text = txt1;
        if (textComponent2 != null) textComponent2.text = txt2;
        if (textComponent3 != null) textComponent3.text = txt3;
        if (textComponent4 != null) textComponent4.text = txt4;

        Debug.Log("4개의 텍스트가 맞춤형 딕셔너리 값으로 정상 적용되었습니다!");
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
    // 텍스트 복사
    //--------------------------------------------------

    public void CopyToClipboard(bool isKr)
    {
        // 1. 언어에 맞는 인풋필드 선택 및 예외 처리
        TMP_Text targetField = isKr ? PromptKr : PromptEn;

        if (targetField == null)
        {
            Debug.LogWarning($"{(isKr ? "한국어(PromptKr)" : "영어(PromptEn)")} InputField가 연결되지 않았습니다.");
            return;
        }

        // 2. 복사할 텍스트 가져오기
        string textToCopy = targetField.text;

        // 3. 플랫폼별 클립보드 복사 처리
#if UNITY_WEBGL && !UNITY_EDITOR
        // 웹빌드(모바일 브라우저 포함) 환경일 때 실행
        try 
        {
            CopyToClipboardWebGL(textToCopy);
            Debug.Log($"WebGL 클립보드 복사 시도 ({(isKr ? "KR" : "EN")}): " + textToCopy);
        }
        catch (System.Exception e) 
        {
            Debug.LogError("WebGL 복사 실패: " + e.Message);
        }
#else
        // 유니티 에디터나 PC/모바일 네이티브 앱 빌드일 때 실행
        GUIUtility.systemCopyBuffer = textToCopy;
        Debug.Log($"일반 클립보드 복사 완료 ({(isKr ? "KR" : "EN")}): " + textToCopy);
#endif
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