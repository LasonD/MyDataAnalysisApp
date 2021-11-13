using System;
using System.Threading.Tasks;
using DAL.Data;
using log4net;
using Telegram.Bot;

namespace ConsoleUI
{
    public class UsersNotifier
    {
        private readonly ITelegramBotClient botClient;
        private readonly TelegramDbContext context;
        private readonly ILog log = LogManager.GetLogger(typeof(UsersNotifier));

        public UsersNotifier(ITelegramBotClient botClient, string connectionString)
        {
            this.botClient = botClient;
            context = new TelegramDbContext(connectionString);
        }

        public async Task NotifyAllAsync(string notification)
        {
            log.Debug($"Starting notification of users with message '{notification}'");

            foreach (var user in context.TelegramUsers)
            {
                try
                {
                    await botClient.SendTextMessageAsync(user.UserId, notification);

                    log.Info($"Successfully notified user {user.Username} {user.UserId} with message '{notification}'");
                }
                catch (Exception e)
                {
                   log.Error($"Failed to send notification '{notification}' to user {user.Username} " +
                             $"with id {user.UserId}, error message: {e.Message}");
                }
            }
        }
    }
}
