using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataAnalysisLib.TextAnalysisVisualization;
using DataAnalysisLib.TextAnalyzer;
using log4net;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace ConsoleUI
{
    public class TelegramTextProcessingServer : IUpdateHandler
    {
        private const string ReportFileName = "Report.xlsx";

        private ExcelTextAnalysisVisualizer visualizer;
        private TextAnalyzer analyzer;
        private readonly ILog log = LogManager.GetLogger(typeof(TelegramTextProcessingServer));

        public async Task HandleUpdate(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            try
            {
                await HandleInternal(botClient, update, cancellationToken);
            }
            catch (Exception e)
            {
                await botClient.SendTextMessageAsync(update.Message.From.Id, e.StackTrace, cancellationToken: cancellationToken);

                log.Error($"Exception while processing update from " +
                          $"{update.Message.From.Username} {update.Message.From.Id}");
            }
        }

        private async Task HandleInternal(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            LogUpdate(update);

            if (update.Type is UpdateType.Message && update.Message.Type is MessageType.Document)
            {
                await HandleDocumentProcessingRequest(botClient, update, cancellationToken);
            }
        }

        private async Task HandleDocumentProcessingRequest(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;

            var sentFileStream = new MemoryStream();
            var resultFileStream = new MemoryStream();

            await botClient.GetInfoAndDownloadFileAsync(message.Document.FileId, sentFileStream, cancellationToken);

            sentFileStream.Position = 0;

            SetupAnalysisInfrastructure(sentFileStream, resultFileStream);

            await visualizer.PlotAsync();

            resultFileStream.Position = 0;

            await botClient.SendDocumentAsync(message.From.Id,
                new InputOnlineFile(resultFileStream, ReportFileName), cancellationToken: cancellationToken);
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

        private void LogUpdate(Update update)
        {
            var msg = update.Message;

            var logBuilder = new StringBuilder($"Received update from user {msg.From.Username} {update.Message.From.Id}. ");

            logBuilder.AppendLine($"Message type: {msg.Type}, ");

            switch (update.Message.Type)
            {
                case MessageType.Text:
                    logBuilder.AppendLine($"Message text: {msg.Text}, ");
                    break;
                case MessageType.Document:
                    logBuilder.AppendLine($"Document: {msg.Document.FileName}, size {msg.Document.FileSize}");
                    break;
            }

            log.Info(logBuilder.ToString());
        }
    }
}
