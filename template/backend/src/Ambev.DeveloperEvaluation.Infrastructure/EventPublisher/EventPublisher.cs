using Ambev.DeveloperEvaluation.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Infrastructure.EventPublisher
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EventPublisher> _logger;

        public EventPublisher(IConfiguration configuration, ILogger<EventPublisher> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task PublishAsync<T>(T @event) where T : class
        {
            _logger.LogInformation("Publishing event: {@Event}", @event);
        }
    }
}
