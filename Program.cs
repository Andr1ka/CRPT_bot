using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace CRPT_bot
{
    class Program
    {
        private static TelegramBotClient _botClient;
        private static ReceiverOptions _receiverOptions;

        static async Task Main()
        {
            
            string botToken = ReadBotTokenFromJson("config.json");

            _botClient = new TelegramBotClient(botToken);
            _receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new[]
                {
                    UpdateType.Message,
                    UpdateType.CallbackQuery
                }
            };

            using var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            _botClient.StartReceiving(UpdateHandler.UpdateH, ErrorHandler.ErrorH, _receiverOptions, token);

            var me = await _botClient.GetMeAsync();
            Console.WriteLine($"{me.FirstName} запущен!");

            Console.WriteLine("Нажмите Enter для выхода...");
            Console.ReadLine();
            cts.Cancel();
        }

        private static string ReadBotTokenFromJson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Файл конфигурации {filePath} не найден.");
            }

            string jsonString = File.ReadAllText(filePath);
            using JsonDocument document = JsonDocument.Parse(jsonString);
            JsonElement root = document.RootElement;
            return root.GetProperty("BotToken").GetString();
        }
    }
}
