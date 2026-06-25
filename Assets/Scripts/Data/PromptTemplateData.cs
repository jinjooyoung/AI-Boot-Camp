using UnityEngine;

[CreateAssetMenu(menuName = "Prompt/Prompt Template")]
public class PromptTemplateData : ScriptableObject
{
    [Header("프롬프트 템플릿")]
    [TextArea(20, 100)]
    public string template;
}