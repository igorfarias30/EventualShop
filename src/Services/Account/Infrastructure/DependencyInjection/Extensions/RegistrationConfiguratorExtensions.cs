﻿using System;
using Application.UseCases.CommandHandlers;
using Application.UseCases.EventHandlers;
using Application.UseCases.EventHandlers.Projections;
using Application.UseCases.QueriesHandlers;
using MassTransit;
using Messages.Abstractions;
using Messages.Accounts;

namespace Infrastructure.DependencyInjection.Extensions
{
    public static class RegistrationConfiguratorExtensions
    {
        public static void AddCommandConsumers(this IRegistrationConfigurator cfg)
        {
            cfg.AddCommandConsumer<CreateAccountConsumer, Commands.CreateAccount>();
            cfg.AddCommandConsumer<DefineProfessionalAddressConsumer, Commands.DefineProfessionalAddress>();
            cfg.AddCommandConsumer<DefineResidenceAddressConsumer, Commands.DefineResidenceAddress>();
            cfg.AddCommandConsumer<DeleteAccountConsumer, Commands.DeleteAccount>();
            cfg.AddCommandConsumer<UpdateProfileConsumer, Commands.UpdateProfile>();
        }

        public static void AddEventConsumers(this IRegistrationConfigurator cfg)
        {
            cfg.AddConsumer<AccountCreatedConsumer>();
            cfg.AddConsumer<AccountDeletedConsumer>();
            cfg.AddConsumer<ProfessionalAddressDefinedConsumer>();
            cfg.AddConsumer<ProfileUpdatedConsumer>();
            cfg.AddConsumer<ResidenceAddressDefinedConsumer>();
            cfg.AddConsumer<UserRegisteredConsumer>();
        }

        public static void AddQueryConsumers(this IRegistrationConfigurator cfg)
        {
            cfg.AddConsumer<GetAccountDetailsConsumer>();
            cfg.AddConsumer<GetAccountsDetailsWithPaginationConsumer>();
        }

        private static void AddCommandConsumer<TConsumer, TMessage>(this IRegistrationConfigurator configurator)
            where TConsumer : class, IConsumer
            where TMessage : class, IMessage
        {
            configurator
                .AddConsumer<TConsumer>()
                .Endpoint(e => e.ConfigureConsumeTopology = false);

            EndpointConvention.Map<TMessage>(new Uri($"exchange:{typeof(TMessage).ToKebabCaseString()}"));
        }
    }
}