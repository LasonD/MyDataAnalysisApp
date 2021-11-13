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
        private static readonly ITelegramBotClient Bot = new TelegramBotClient(Token);

        static async Task Main(string[] args)
        {
            XmlConfigurator.Configure();

            log.Debug("Starting the program");

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
    }
}
