using System;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace ConsoleUI
{
    internal class Program
    {
        private static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        private static readonly string Token = Configuration.GetSection("TelegramBotToken").Value;
        private static readonly string DbConnectionString = Configuration.GetConnectionString("TelegramBotDatabase");
        private static readonly ITelegramBotClient Bot = new TelegramBotClient(Token);

        static async Task Main(string[] args)
        {
            XmlConfigurator.Configure();

            log.Debug("Starting the program");

            if (args.Length > 0)
            {
                await NotifyUsers(string.Join(' ', args));
            }

            try
            {
                await Bot.ReceiveAsync(new TelegramTextProcessingServer());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                log.Error(e.Message);
            }
        }

        private static async Task NotifyUsers(string notification)
        {
            var notifier = new UsersNotifier(Bot, DbConnectionString);
            await notifier.NotifyAllAsync(notification);
        }
    }
}
