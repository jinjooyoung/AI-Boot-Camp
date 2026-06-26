using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Prompt/Question Data")]
public class QuestionData : ScriptableObject
{
    [Header("ID")]
    public int ID;

    [Header("질문 보충")]
    public string questionSubText;

    [Header("질문")]
    public string title;

    [Header("토글 추천 답변")]
    public List<string> recommendedAnswers = new();

    [Header("버튼 이미지")]
    public List<Sprite> Images = new();
}