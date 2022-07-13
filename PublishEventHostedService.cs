using MassTransit;

namespace zzz;

public class PublishEventHostedService : IHostedService, IDisposable
{
    private Timer? _timer = null;
    private readonly Random _random = new();

    private readonly ILogger<PublishEventHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IBusControl _busControl;

    public PublishEventHostedService(ILogger<PublishEventHostedService> logger,
                                     IServiceProvider serviceProvider,
                                     IBusControl busControl)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _busControl = busControl;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        _logger.LogInformation("try send event...");
        var result = _busControl.CheckHealth();

        switch (result.Status)
        {
            case BusHealthStatus.Healthy:
                SendEvent();
                break;
            default:
                _logger.LogInformation("bus is not ready, waiting...");
                break;
        }
    }

    private void SendEvent()
    {
        using var scope = _serviceProvider.CreateScope();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        publishEndpoint.Publish(new FileReceivedEvent(_random.Next(1, 999))).Wait();
        _logger.LogInformation("FileReceivedEvent published");
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
