using System.IO;
using System.Threading;
using Telegram.Bot.Types;

public class NewUserHandler : MessageCreator
{
    private IMessanger _messanger;
    private FileManager _fileManager;
    private string _welcomeMessage;
    private string _instructionMessage;

    public NewUserHandler(IMessanger messanger, FileManager fileManager, string[] welcomeMessage, string[] instructionMessage)
    {
        _messanger = messanger;
        _messanger.AddedMember += OnAddedMember;
        _fileManager = fileManager;
        _welcomeMessage = Create(welcomeMessage);
        _instructionMessage = Create(instructionMessage);
    }

    ~NewUserHandler()
    {
        _messanger.AddedMember -= OnAddedMember;
    }

    private void OnAddedMember(User user)
    {
        Recieve(user);
    }

    private void Recieve(User user)
    {
        FileInfo fileInfo = _fileManager.GetWelcomePicture();

        _messanger.Recieve(user, _welcomeMessage, fileInfo);
        Thread.Sleep(2000);
        _messanger.Recieve(user, _instructionMessage);
    }
}
