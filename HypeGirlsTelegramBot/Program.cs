using Newtonsoft.Json;
using System.IO;

class Program
{
    static void Main()
    {
        Configuration config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("config.json")); // HypeGirlsBot

        HypeGirlsTelegramBotClient client = new HypeGirlsTelegramBotClient(config.TelegramBotToken);
        FileManager fileManager = FileManager.GetInstance(config.WelcomePicturesPath, config.PicturesPath);
        MessageHandler messageHandler = new MessageHandler(client, config.TargetWord, config.PictureMessage, fileManager);
        NewUserHandler newUserHandler = new NewUserHandler(client, fileManager, config.WelcomeMessage, config.InstructionMessage);

        client.Run();
    }
}
