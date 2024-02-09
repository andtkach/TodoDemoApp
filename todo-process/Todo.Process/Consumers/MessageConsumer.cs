using AutoMapper;
using MassTransit;
using Tasks.Common.Message;

namespace Todo.Process.Consumers
{
    public sealed class MessageConsumer : IConsumer<MessageTask>
    {
        private readonly ILogger<MessageConsumer> _logger;
        private readonly IMapper _mapper;

        public MessageConsumer(ILogger<MessageConsumer> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<MessageTask> context)
        {
            _logger.LogInformation("Message received: {Id}", context.Message.Id);
            
        }
    }
}
