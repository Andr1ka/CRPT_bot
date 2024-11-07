using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace CRPT_bot
{
    internal class UpdateHandler
    {
        internal static async Task UpdateH(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {  
          try
            {
               switch (update.Type) 
               {
                 case UpdateType.Message:
                 {
                   await MessageHandle(botClient, update.Message, cancellationToken);
                   break;
                 }
                 case UpdateType.CallbackQuery:
                 {
                   await CallbackQueryHandle(botClient, update.CallbackQuery, cancellationToken);
                   break;
                 }
               }
          }
          catch (Exception ex)
          {
            Console.WriteLine(ex.ToString());
          }
        }


        private static async Task MessageHandle(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            
            var user = message.From;
            
            var chat = message.Chat;
            
            Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

           
            switch (message.Type)
            {
                case MessageType.Text: 
                    await TextHandle(botClient, message, cancellationToken);
                    break;
                    
                default:
                    {
                      await botClient.SendTextMessageAsync(chat.Id,
                                                           "Используй только текст!"
                                                           );
                        break;
                    }
            }

            return;
        }

        private static async Task TextHandle(ITelegramBotClient botClient, Message message, CancellationToken cansellationToken)
        {
            var chat = message.Chat;
            switch (message.Text)
            {
                case "/start":
                    {
                        
                        var inlineKeyboard = new InlineKeyboardMarkup(
                        new List<InlineKeyboardButton[]>()
                        {

                                new InlineKeyboardButton[]
                                {
                                  InlineKeyboardButton.WithUrl(" подписочка ", "https://t.me/CRPTtg"),
                                  InlineKeyboardButton.WithCallbackData(" кнопочка", "button1"),
                                },
                                new InlineKeyboardButton[]
                                {
                                 InlineKeyboardButton.WithCallbackData(" баги, пожелания и предложения ", "button2"),
                                 },
                        });

                        await botClient.SendTextMessageAsync(chat.Id,
                                                             "выберите опцию",
                                                             replyMarkup: inlineKeyboard); 
                        break;
                    }
                default:
                    await botClient.SendTextMessageAsync(chat.Id, "не существует такой комманды");
                    break;
            }
            return;
        }

        private static async Task CallbackQueryHandle(ITelegramBotClient botClient, CallbackQuery callbackQuery, CancellationToken cancellationToken)
        {
            {

                var user = callbackQuery.From; 
                Console.WriteLine($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");//логгирование

                
                var chat = callbackQuery.Message.Chat;

                switch (callbackQuery.Data)
                {
                   
                    case "button1":
                        {
                           
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Другая кнопочка", showAlert: true);

                            break;
                        }
                    case "button2":
                        {
                            await botClient.AnswerCallbackQueryAsync(callbackQuery.Id, "Загрузгга");

                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                "@l3t_m3");
                            break;
                        }
                }

                return;
            }

        }
    }
}
