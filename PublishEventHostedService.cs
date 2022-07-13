using MassTransit;

namespace zzz;

public class PublishEventHostedService : IHostedService, IDisposable
{
    private Timer? _timer = null;
    private readonly Random _random = new();

    private readonly ILogger<PublishEventHostedService> _logger;
    private readonly IBusControl _busControl;
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishEventHostedService(ILogger<PublishEventHostedService> logger,
                              IBusControl busControl,
                              IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _busControl = busControl;
        _publishEndpoint = publishEndpoint;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(TrySendEvent, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
        return Task.CompletedTask;
    }

    private void TrySendEvent(object? state)
    {
        _logger.LogInformation("try send event...");
        var result = _busControl.CheckHealth();

        switch (result.Status)
        {
            case BusHealthStatus.Healthy:
                _publishEndpoint.Publish(new FileReceivedEvent(_random.Next(1, 999))).Wait();
                _logger.LogInformation("FileReceivedEvent published");
                break;
            default:
                _logger.LogInformation("bus is not ready, waiting...");
                break;
        }
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
