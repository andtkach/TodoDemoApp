using AutoMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Tasks.Common.Message;
using Todo.Process.Database;

namespace Todo.Process.Consumers
{
    public sealed class MessageConsumer : IConsumer<MessageTask>
    {
        private readonly ILogger<MessageConsumer> _logger;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public MessageConsumer(ILogger<MessageConsumer> logger, IMapper mapper, ApplicationDbContext context, 
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _mapper = mapper;
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task Consume(ConsumeContext<MessageTask> context)
        {
            var id = context.Message.Id;
            _logger.LogInformation("Message received: {Id}", id);
            try
            {
                await UpdateDatabase(id);
                _logger.LogInformation("Message processed: {Id}", id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while processing message: {Id}. {Message}", id, e.Message);
            }
        }

        private async Task UpdateDatabase(int id)
        {
            var task = await _context.TodoItems.FirstOrDefaultAsync(t => t.Id == id);
            if (task != null)
            {
                task.Audio = await GetAudio(new InputModel()
                {
                    Input = task.Name
                });
                await _context.SaveChangesAsync();
                _logger.LogInformation("Task updated: {Id}", id);
            }
            else
            {
                _logger.LogWarning("Task not found: {Id}", id);
            }
        }

        private async Task<string> GetAudio(InputModel input)
        {
            var httpClient = _httpClientFactory.CreateClient("OpenAI");
            var httpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = JsonContent.Create(input)
            };

            var response = await httpClient.SendAsync(httpRequestMessage);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("Error while getting audio: {StatusCode}", response.StatusCode);
                var rawResponse =  await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Error raw response: {Response}", rawResponse);
                return string.Empty;
            }

            _logger.LogInformation("Audio received");

            var byteArray = await response.Content.ReadAsByteArrayAsync();
            return Convert.ToBase64String(byteArray);
        }
    }
}
