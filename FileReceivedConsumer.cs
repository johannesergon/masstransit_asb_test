namespace zzz;

using MassTransit;

public class FileReceivedConsumer : IConsumer<FileReceived>
{
    private readonly ILogger<FileReceivedConsumer> _logger;
    public FileReceivedConsumer(ILogger<FileReceivedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<FileReceived> context)
    {
        _logger.LogInformation("FileReceived#{id}", context.Message.Id);

        return Task.CompletedTask;
    }
}

public class FileReceivedConsumerDefinition : ConsumerDefinition<FileReceivedConsumer>
{
    public FileReceivedConsumerDefinition()
    {
        EndpointName = "input-queue";
    }
}
