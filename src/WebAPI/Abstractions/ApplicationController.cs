using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.UseCases;
using MassTransit;
using MassTransit.Mediator;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Abstractions
{
    [ApiController, Route("api/[controller]/[action]")]
    public abstract class ApplicationController : ControllerBase
    {
        private readonly IMediator _mediator;

        protected ApplicationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected async Task<IActionResult> SendCommandAsync(ICommand command, CancellationToken cancellationToken)
        {
            await SendMessage(command, cancellationToken);
            return Accepted();
        }

        protected async Task<IActionResult> GetQueryResponseAsync<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken)
            where TQuery : class, IQuery
            where TResponse : class
        {
            var response = await GetResponseAsync<TQuery, TResponse>(query, cancellationToken);
            return Ok(response.Message);
        }

        private Task<Response<TResponse>> GetResponseAsync<TMessage, TResponse>(IQuery query, CancellationToken cancellationToken)
            where TMessage : class
            where TResponse : class
            => _mediator.CreateRequestClient<TMessage>().GetResponse<TResponse>(query, cancellationToken);

        private Task SendMessage<TMessage>(TMessage message, CancellationToken cancellationToken)
            => _mediator.Send(message, cancellationToken);
    }
}