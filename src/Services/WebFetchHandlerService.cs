using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

using NLog;

public class WebFetchHandlerService : IWebFetchHandlerService
{
    private readonly ILogger<WebFetchHandlerService> _logger;
    private readonly IOptions<WebFetchHandlerConfig> _config;
    private readonly HttpClient _httpClient;

    public WebFetchHandlerService(
        ILogger<WebFetchHandlerService> logger, 
        IOptions<WebFetchHandlerConfig> config, 
        IHttpClientFactory factory
        )
    {
        _logger = logger;
        _config = config;
        _httpClient = factory.CreateClient("webhandlerClient");
    }

    public async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogTrace("WebFetchHandlerService is starting.");
        _logger.LogTrace("WebFetchHandlerService config: {Config}", _config.Value);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogTrace("WebFetchHandlerService is running.");

            var tasks = _config.Value.Symbols.Select(async symbol =>
            {
                var url = _config.Value.URL.Replace("{symbol}", symbol);
                _logger.LogTrace("WebFetchHandlerService calling: {url}", url);
                try
                {
                    var result = await _httpClient.GetStringAsync(url, stoppingToken);

                    _logger.LogInformation("@{event}", new
                    {
                        Symbol = symbol,
                        TimeOfEvent = DateTime.UtcNow,
                        Result = result
                    });                    
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "WebFetchHandlerService encountered an error for symbol {symbol}", symbol);
                }
            });

            await Task.WhenAll(tasks);

            // TODO: Use the HttpClient instance to make HTTP requests.
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }

        _logger.LogTrace("WebFetchHandlerService is stopping.");
    }
}