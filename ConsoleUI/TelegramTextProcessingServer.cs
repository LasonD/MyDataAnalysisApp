using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DataAnalysisLib.TextAnalysisVisualization;
using DataAnalysisLib.TextAnalyzer;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace ConsoleUI
{
    public class TelegramTextProcessingServer : IUpdateHandler
    {
        private ExcelTextAnalysisVisualizer visualizer;
        private TextAnalyzer analyzer;

        public async Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                await HandleInternal(botClient, update, cancellationToken);
            }
            catch (Exception e)
            {
                await botClient.SendTextMessageAsync(update.Message.From.Id, e.StackTrace, cancellationToken: cancellationToken);
            }
        }

        private async Task HandleInternal(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type is UpdateType.Message && update.Message.Type is MessageType.Document)
            {
                var message = update.Message;

                var sentFileStream = new MemoryStream();
                var resultFileStream = new MemoryStream();

                await botClient.GetInfoAndDownloadFileAsync(message.Document.FileId, sentFileStream, cancellationToken);

                sentFileStream.Position = 0;

                SetupAnalysisInfrastructure(sentFileStream, resultFileStream);

                await visualizer.PlotAsync();

                resultFileStream.Position = 0;

                await botClient.SendDocumentAsync(message.From.Id, new InputOnlineFile(resultFileStream, "Report.xlsx"), cancellationToken: cancellationToken);
            }
        }

        public Task HandleError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public UpdateType[] AllowedUpdates { get; } = {
            UpdateType.Message
        };

        private void SetupAnalysisInfrastructure(Stream source, Stream destination)
        {
            analyzer = new TextAnalyzer(source);
            visualizer = new ExcelTextAnalysisVisualizer(destination, analyzer);
        }
    }
}
