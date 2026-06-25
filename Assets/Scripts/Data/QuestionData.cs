using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prompt/Question Data")]
public class QuestionData : ScriptableObject
{
    [Header("ID")]
    public int ID;

    [Header("질문 보충")]
    public string variableName;

    [Header("질문")]
    public string title;

    [TextArea]
    public string tip;

    [Header("토글 추천 답변")]
    public List<string> recommendedAnswers = new();
}