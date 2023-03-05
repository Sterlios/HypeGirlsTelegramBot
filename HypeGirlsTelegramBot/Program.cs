class Program
{
    static void Main()
    {
        string token = "";

        HypeGirlsTelegramBotClient client = new HypeGirlsTelegramBotClient(token);
        _ = new MessageHandler(client);
        client.Run();
    }
}
