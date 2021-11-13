using System;
using System.Threading.Tasks;
using DAL.Data;
using Telegram.Bot;

namespace ConsoleUI
{
    public class UsersNotifier
    {
        private readonly ITelegramBotClient botClient;
        private readonly TelegramDbContext context;

        public UsersNotifier(ITelegramBotClient botClient, string connectionString)
        {
            this.botClient = botClient;
            context = new TelegramDbContext(connectionString);
        }

        public async Task NotifyAllAsync(string notification)
        {
            foreach (var user in context.TelegramUsers)
            {
                try
                {
                    await botClient.SendTextMessageAsync(user.UserId, notification);
                }
                catch (Exception e)
                {
                   
                }
            }
        }
    }
}
