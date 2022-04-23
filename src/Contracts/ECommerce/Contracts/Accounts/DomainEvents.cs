using ECommerce.Abstractions.Messages.Events;
using ECommerce.Contracts.Common;

namespace ECommerce.Contracts.Accounts;

public static class DomainEvents
{
    public record AccountDeleted(Guid AccountId) : Event;

    public record AccountCreated(Guid AccountId, Guid UserId, string Email, string FirstName) : Event;

    public record ProfileUpdated(Guid AccountId, DateOnly Birthdate, string Email, string FirstName, string LastName) : Event;

    public record ResidenceAddressDefined(Guid AccountId, Models.Address Address) : Event;

    public record ProfessionalAddressDefined(Guid AccountId, Models.Address Address) : Event;
}