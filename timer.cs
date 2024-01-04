using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace sbtest
{
    public class timer
    {
        private readonly ILogger _logger;
        private readonly string waitTime = Environment.GetEnvironmentVariable("waitTime") ?? "9";

        public timer(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<timer>();
        }

        [Function("timer")]
        public void Run([TimerTrigger("* * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            int tempInt = Convert.ToInt32(waitTime);

            Task.Factory.StartNew(async () =>
            {
                await Task.Delay(1000 * 60 * tempInt);
            });
        }
    }
}
