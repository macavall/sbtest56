using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace sbtest
{
    public class sbtrigger
    {
        private readonly ILogger<sbtrigger> _logger;
        private readonly int SBOutputCount = Convert.ToInt32(Environment.GetEnvironmentVariable("SBOutputCount"));

        public sbtrigger(ILogger<sbtrigger> logger)
        {
            _logger = logger;
        }

        [Function(nameof(sbtrigger))]
        [ServiceBusOutput("%queueName%", Connection = "sbconnstring")]
        public string[] Run([ServiceBusTrigger("%queueName%", Connection = "sbconnstring")] ServiceBusReceivedMessage message)
        {
            string[] tempList = new string[SBOutputCount];

            _logger.LogInformation("Message ID: {id}", message.MessageId);
            _logger.LogInformation("Message Body: {body}", message.Body);
            _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

            int loopCount = Convert.ToInt32(Environment.GetEnvironmentVariable("SBOutputCount"));

            _logger.LogInformation($"LoopCount: {loopCount}");

            // For each Function Invocation, we will create ten more Service Bus Output Messages 
            // The increases Service Bus Message Queue count will initiate further scale out
            for (var i = 0; i < SBOutputCount; i++)
            {
                tempList[i] = $"Output message created at {DateTime.Now}";
            }

            return tempList;
        }
    }
}
