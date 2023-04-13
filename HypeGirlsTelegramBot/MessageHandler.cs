using System;
using System.IO;
using System.Threading;
using Telegram.Bot.Types;

public class MessageHandler : MessageCreator
{
    private IMessanger _messanger;
    private char[] _seperators = new char[] { ' ', '.', ',', '!', '?' };
    private FileManager _fileManager;
    private string _pictureMessage;
    private string[] _triggerWords;

    public MessageHandler(IMessanger messanger, string triggerWord, string[] pictureMessage, FileManager fileManager)
    {
        _messanger = messanger;
        _messanger.Send += OnSend;
        _fileManager = fileManager;
        _pictureMessage = Create(pictureMessage);
        _triggerWords = triggerWord.Split(_seperators, StringSplitOptions.RemoveEmptyEntries);
    }

    ~MessageHandler()
    {
        _messanger.Send -= OnSend;
    }

    private void OnSend(Message message)
    {
        string[] words = message.Text.Split(_seperators, StringSplitOptions.RemoveEmptyEntries);

        if (IsCorrectTrigger(message, words))
        {
            string name = string.Empty;

            for (int i = _triggerWords.Length; i < words.Length; i++)
                name += words[i] + " ";

            if (name != string.Empty)
                Recieve(message.From, name);
        }
    }

    private void Recieve(User user, string name)
    {
        name = name.Trim();

        if (!_fileManager.TryGetPicture(name, out FileInfo fileInfo))
        {
            _messanger.Recieve(user, "Excuse me, ma'am, I'm new here and may not be fully aware of customs. " +
                "Could you kindly provide your full name instead of a shortened version? " +
                "For instance, Maria instead of Masha or Alevtina instead of Alia?");
            return;
        }

        _messanger.Recieve(user, _pictureMessage, fileInfo);
    }

    private bool IsCorrectTrigger(Message message, string[] words)
    {
        bool isCorrect = true; 

        if (words.Length > _triggerWords.Length)
        {
            for (int i = 0; i < _triggerWords.Length; i++)
                if (!words[i].ToLower().Equals(_triggerWords[i].ToLower()))
                    isCorrect = false;
        }
        else
        {
            isCorrect = false;
        }
             
        if (!isCorrect)
            _messanger.Recieve(message.From, "Could you please repeat the spell so it works? I didn't hear it clearly.");

        return isCorrect;
    }
}
