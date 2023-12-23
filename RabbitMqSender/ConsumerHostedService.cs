using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace RabbitMqConsumer;

public class ConsumerHostedService : IHostedService, IDisposable
{
    private readonly IMessageService _messageService;

    private int executionCount = 0;
    private Timer? _timer = null;
    public ConsumerHostedService(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Timed Hosted Service running.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(5));

        return Task.CompletedTask;
    }

    private void DoWork(object? state)
    {
        var count = Interlocked.Increment(ref executionCount);
        _messageService.ReadMessage();

        Console.WriteLine("Timed Hosted Service is working. Count: {Count}", count);
     
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        Console.WriteLine("Timed Hosted Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
