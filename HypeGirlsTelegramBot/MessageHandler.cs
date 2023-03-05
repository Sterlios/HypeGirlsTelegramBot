using System;

public class MessageHandler : IMessanger
{
    private IMessanger _messanger;
    private string _triggerWord = "футурама";
    private char[] _seperators = new char[] { ' ', '.', ',' };
    private string _name;

    public event Action<string> Send;

    public MessageHandler(IMessanger messanger)
    {
        _messanger = messanger;
        _messanger.Send += OnSend;
    }

    ~MessageHandler()
    {
        _messanger.Send -= OnSend;
    }

    private void OnSend(string message)
    {
        string[] words = message.Split(_seperators, StringSplitOptions.RemoveEmptyEntries);

        if (words[0].ToLower() == _triggerWord.ToLower())
            for (int i = 1; i < words.Length; i++)
                _name += words[i];

        if (_name != string.Empty)
            Send?.Invoke(_name);
    }

    public void Recieve(string message)
    {
        _messanger.Recieve(message);
    }
}
