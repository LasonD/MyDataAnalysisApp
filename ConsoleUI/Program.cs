using System;
using System.IO;
using System.Threading.Tasks;
using DataAnalysisLib.TextAnalysisVisualization;
using DataAnalysisLib.TextAnalyzer;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;

namespace ConsoleUI
{
    internal class Program
    {
        private static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        private static readonly string Token = Configuration.GetSection("TelegramBotToken").Value;
        private static readonly ITelegramBotClient Bot = new TelegramBotClient(Token);

        static async Task Main(string[] args)
        {
            try
            {
                await Bot.ReceiveAsync(new TelegramTextProcessingServer());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
