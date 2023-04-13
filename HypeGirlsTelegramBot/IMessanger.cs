using System;
using System.IO;
using Telegram.Bot.Types;

public interface IMessanger
{
    public event Action<Message> Send;
    public event Action<User> AddedMember;
    void Recieve(User user, string message);
    void Recieve(User user, string message, FileInfo picture);
}
