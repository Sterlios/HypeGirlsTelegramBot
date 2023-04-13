public abstract class MessageCreator
{
    protected string Create(string[] texts)
    {
        string text = string.Empty;

        foreach (var part in texts)
            text += $"{part}\n\n";

        return text;
    }
}
