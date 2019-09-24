using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using TTMS.Common.Entities.Extensions;
using TTMS.Common.Messages;

namespace TTMS.Azure.Functions
{
    public static class TravelerServiceBusFunctions
    {
        [FunctionName(nameof(ProcessTravelerMessagesAsync))]
        public static async void ProcessTravelerMessagesAsync(
            [ServiceBusTrigger("ttms_write", Connection = "ServiceBusConnection")]string myQueueItem,
            [Table("traveler")]CloudTable table,
            ILogger logger)
        {
            logger.LogDebug("Message received. Deserializing...");
            var msg = JsonConvert.DeserializeObject<TravelerMessage>(myQueueItem);
            logger.LogInformation("Traveler Message type: {type}", msg.Type);

            var entity = msg.Content?.ToEntity() ?? throw new ArgumentNullException("Message.Content");

            switch (msg.Type)
            {
                case MessageType.Create:
                    await TableStorageHelper.CreateTravelerAsync(table, entity);
                    break;
                case MessageType.Update:
                    await TableStorageHelper.UpdateTravelerAsync(table, entity);
                    break;
                case MessageType.Delete:
                    await TableStorageHelper.DeleteTravelerAsync(table, msg.Content.Id);
                    break;
                default:
                    var ex = new NotImplementedException($"No action implemented for messages of type {msg.Type}");
                    logger.LogError(ex, "Error routing Traveler message");
                    throw ex;
            }
        }
    }
}
