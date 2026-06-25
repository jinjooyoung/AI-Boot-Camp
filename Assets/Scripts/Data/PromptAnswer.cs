using System.Collections.Generic;

public class PromptAnswer
{
    public QuestionData question;
    public string input;
    public List<string> toggleAnswers = new();

    public string GetValue()
    {
        List<string> values = new();

        if (!string.IsNullOrWhiteSpace(input))
            values.Add(input);

        values.AddRange(toggleAnswers);

        return string.Join(", ", values);
    }
}