using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace src
{
    public class GetDataTrigger
    {
        private readonly ILogger _logger;
        private readonly IWebFetchHandlerService webFetchHandlerService;

        public GetDataTrigger(ILoggerFactory loggerFactory, IWebFetchHandlerService webFetchHandlerService)
        {
            _logger = loggerFactory.CreateLogger<GetDataTrigger>();
            this.webFetchHandlerService = webFetchHandlerService;
        }

        [Function("GetDataTrigger")]
        public async Task RunAsync([TimerTrigger("0/30 * * * * *")] MyInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            await this.webFetchHandlerService.ExecuteAsync(default);
            _logger.LogInformation($"C# Timer trigger function finished at: {DateTime.Now}");
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
