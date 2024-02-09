using System.Threading.Channels;
using AutoMapper;
using MassTransit;
using Tasks.Common.Message;

namespace Tasks.Api.Processors
{
    public class TaskProcessor : IHostedService
    {
        private readonly Channel<MessageTask> _channel;
        private readonly ILogger<TaskProcessor> _logger;
        private IServiceScopeFactory _serviceScopeFactory;
        private IMapper _mapper;

        public TaskProcessor(Channel<MessageTask> channel, ILogger<TaskProcessor> logger, 
            IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            _channel = channel;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            IPublishEndpoint publishEndpoint =
                scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                
            return Task.Factory.StartNew(async () =>
            {
                while (!_channel.Reader.Completion.IsCanceled)
                {
                    var message = await _channel.Reader.ReadAsync(cancellationToken);
                    var personCreatedEvent = _mapper.Map<MessageTask>(message);
                    await publishEndpoint.Publish(personCreatedEvent, cancellationToken);
                    _logger.LogInformation($"Republished message task: {message.Id}");
                }
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
