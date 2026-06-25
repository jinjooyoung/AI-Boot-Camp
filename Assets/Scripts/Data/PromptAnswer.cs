using System.Collections.Generic;

public class PromptAnswer
{
    public QuestionData question;
    public string input;
    public List<string> toggles = new();

    public string GetValue()
    {
        List<string> values = new();

        if (!string.IsNullOrWhiteSpace(input))
            values.Add(input);

        values.AddRange(toggles);

        return string.Join(", ", values);
    }
}