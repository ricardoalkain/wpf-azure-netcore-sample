using System.Threading.Tasks;
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
        public static async Task ProcessTravelerMessagesAsync(
            [ServiceBusTrigger("ttms_write", Connection = "ServiceBusConnection")]string queueMessage,
            [Table("traveler")]CloudTable table,
            ILogger logger)
        {
            logger.LogDebug("Message received. Deserializing...");
            var msg = JsonConvert.DeserializeObject<TravelerMessage>(queueMessage);
            logger.LogInformation("Traveler Message TYPE: '{type}' ID: {Key}", msg.Type, msg.Key);

            if (msg.Content == null)
            {
                logger.LogError("Message ID {Key} has no content", msg.Key);
                return;
            }

            var entity = msg.Content.ToEntity();
            TableResult result;

            switch (msg.Type)
            {
                case MessageType.Create:
                    result = await TableStorageHelper.CreateTravelerAsync(table, entity, logger);
                    break;
                case MessageType.Update:
                    result = await TableStorageHelper.UpdateTravelerAsync(table, entity, logger);
                    break;
                case MessageType.Delete:
                    result = await TableStorageHelper.DeleteTravelerAsync(table, msg.Content.Id.ToString(), logger);
                    break;
                default:
                    logger.LogError("Error routing Traveler message ID {Key}: No action implemented for message type {Type}", msg.Key, msg.Type);
                    return;
            }

            if (result.HttpStatusCode >= 200 && result.HttpStatusCode < 300)
            {
                logger.LogInformation("Message ID {Key} successfully processed!", msg.Key);
            }
            else
            {
                logger.LogError("Message ID {Key} process returned HTTP {HttpStatusCode}: Message => {queueMessage}", msg.Key, result.HttpStatusCode, queueMessage);
            }
        }
    }
}
