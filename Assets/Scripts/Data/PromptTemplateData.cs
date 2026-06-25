using UnityEngine;

[CreateAssetMenu(menuName = "Prompt/Prompt Template")]
public class PromptTemplateData : ScriptableObject
{
    [Header("ÄÜÅÙÃ÷ Å¸ÀÔ")]
    public ContentType contentType;

    [Header("ÇÁ·ÒÇÁÆ® ÅÛÇÃ¸´")]
    [TextArea(20, 100)]
    public string template;
}