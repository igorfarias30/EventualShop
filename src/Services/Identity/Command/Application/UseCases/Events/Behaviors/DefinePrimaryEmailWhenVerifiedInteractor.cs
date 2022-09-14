﻿using Application.Abstractions;
using Application.Abstractions.Gateways;
using Application.Abstractions.Interactors;
using Contracts.Services.Identity;
using Domain.Aggregates;

namespace Application.UseCases.Events.Behaviors;

public class DefinePrimaryEmailWhenVerifiedInteractor : EventInteractor<User, DomainEvent.EmailVerified>
{
    public DefinePrimaryEmailWhenVerifiedInteractor(IEventStoreGateway eventStoreGateway, IEventBusGateway eventBusGateway, IUnitOfWork unitOfWork)
        : base(eventStoreGateway, eventBusGateway, unitOfWork) { }

    public override Task InteractAsync(DomainEvent.EmailVerified @event, CancellationToken cancellationToken)
        => OnInteractAsync(@event.Id, user => new Command.DefinePrimaryEmail(user.Id, @event.Email), cancellationToken);
}