using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

public class HypeGirlsTelegramBotClient : IMessanger
{
    private ITelegramBotClient _bot;
    private Chat _channelChat;

    public event Action<string> Send;

    public HypeGirlsTelegramBotClient(string token)
    {
        _bot = new TelegramBotClient(token);
        _channelChat = _bot.GetChatAsync("@testgroupOlolo").Result;
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
                await HandleMessageAsync(update);
                return;

            default:
                break;
        }
    }

    private async Task HandleMessageAsync(Update update)
    {
        string message = update.Message.Text;
        await Task.Run(() => Send?.Invoke(message));
    }

    public void Recieve(string message)
    {
        _bot.SendTextMessageAsync(_channelChat, message);
    }
}
