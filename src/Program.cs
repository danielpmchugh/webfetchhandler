using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

var host = new HostBuilder()
        .ConfigureFunctionsWorkerDefaults()
        .ConfigureAppConfiguration((hostContext, config) =>
        {
            config.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true);
            //NLog.Extensions.Logging.ConfigSettingLayoutRenderer.DefaultConfiguration = config;
        })
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            logging.AddConsole();
            logging.AddNLog();
        })
        .ConfigureServices((hostContext, services) =>
        {
            var config = hostContext.Configuration;
            services.AddLogging(
            builder =>
            {
                builder.AddConsole();
                builder.AddNLog();
            });
            services.Configure<AppConfig>(config);
            IServiceCollection serviceCollection = services.Configure<WebFetchHandlerConfig>(config.GetSection(WebFetchHandlerConfig.WebFetchHandlerConfigName));
            services.AddSingleton<IWebFetchHandlerService, WebFetchHandlerService>();
            services.AddHttpClient<WebFetchHandlerService>("webhandlerClient")
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))  //Set lifetime to five minutes
                .AddPolicyHandler(GetRetryPolicy());
        })
        .Build();
    await host.RunAsync();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Forbidden)
        .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
