namespace zzz;

using MassTransit;

public class CustomerAuditConsumer : IConsumer<CustomerDataReceived>
{
    private readonly ILogger<CustomerAuditConsumer> _logger;
    public CustomerAuditConsumer(ILogger<CustomerAuditConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<CustomerDataReceived> context)
    {
        _logger.LogInformation("CustomerDataReceived#{id}", context.Message.Id);

        return Task.CompletedTask;
    }
}

public class CustomerAuditConsumerDefinition : ConsumerDefinition<CustomerAuditConsumer>
{
    public CustomerAuditConsumerDefinition()
    {
        EndpointName = "audit-queue";
    }
}
