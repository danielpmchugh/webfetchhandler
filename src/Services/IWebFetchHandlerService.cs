public interface IWebFetchHandlerService
{
    Task ExecuteAsync(CancellationToken stoppingToken);
}