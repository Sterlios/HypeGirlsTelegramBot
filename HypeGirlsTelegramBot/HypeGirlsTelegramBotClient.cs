using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

public class HypeGirlsTelegramBotClient : IMessanger
{
    private const string _startCommand = "/start";

    private ITelegramBotClient _bot;

    public event Action<Message> Send;
    public event Action<User> AddedMember;

    public HypeGirlsTelegramBotClient(string token)
    {
        _bot = new TelegramBotClient(token);
    }

    public void Run()
    {
        Console.WriteLine("Запущен бот " + _bot.GetMeAsync().Result.FirstName);
        Console.WriteLine("Login " + _bot.GetMeAsync().Result.Username);

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { },
        };

        _bot.StartReceiving(
            HandleUpdateAsync,
            HandleError,
            receiverOptions,
            cancellationToken
        );

        Console.ReadLine();
    }

    private async Task HandleError(ITelegramBotClient bot, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        Console.WriteLine();
    }

    private async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));
        Console.WriteLine();

        switch (update.Type)
        {
            case UpdateType.Message:
                if (update.Message.Text != null)
                    await HandleMessageAsync(update);
                break;

            default:
                break;
        }
    }

    private async Task HandleMessageAsync(Update update)
    {
        switch (update.Message.Text?.ToLower())
        {
            case _startCommand:
                await Task.Run(() => AddedMember?.Invoke(update.Message.From));
                break;

            default:
                await Task.Run(() => Send?.Invoke(update.Message));
                break;
        }
    }

    public void Recieve(User user, string message)
    {
        _bot.SendTextMessageAsync(user.Id, message);
    }

    public void Recieve(User user, string message, FileInfo picture)
    {
        _bot.SendPhotoAsync(user.Id, new InputOnlineFile(picture.OpenRead()), message);
    }
}
