﻿using System.Linq;
using System.Threading.Tasks;
using Application.EventSourcing.Projections;
using ECommerce.Abstractions.Messages.Queries.Responses;
using ECommerce.Contracts.Common;
using ECommerce.Contracts.ShoppingCart;
using MassTransit;
using GetShoppingCartQuery = ECommerce.Contracts.ShoppingCart.Queries.GetShoppingCart;
using GetCustomerShoppingCartQuery = ECommerce.Contracts.ShoppingCart.Queries.GetCustomerShoppingCart;

namespace Application.UseCases.Queries;

public class GetShoppingCartDetailsConsumer :
    IConsumer<GetShoppingCartQuery>,
    IConsumer<GetCustomerShoppingCartQuery>
{
    private readonly IShoppingCartProjectionsService _projectionsService;

    public GetShoppingCartDetailsConsumer(IShoppingCartProjectionsService projectionsService)
    {
        _projectionsService = projectionsService;
    }

    public async Task Consume(ConsumeContext<GetShoppingCartQuery> context)
    {
        var cartDetails = await _projectionsService.GetCartAsync(context.Message.CartId, context.CancellationToken);
        await RespondAsync(cartDetails, context);
    }

    public async Task Consume(ConsumeContext<GetCustomerShoppingCartQuery> context)
    {
        var cartDetails = await _projectionsService.GetCartByCustomerIdAsync(context.Message.CustomerId, context.CancellationToken);
        await RespondAsync(cartDetails, context);
    }

    private static Task RespondAsync(CartProjection projection, ConsumeContext context)
        => projection is null
            ? context.RespondAsync<NotFound>(new())
            : context.RespondAsync(MapToCartDetailsResponse(projection));

    private static Responses.Cart MapToCartDetailsResponse(CartProjection cart)
        => new()
        {
            Id = cart.Id,
            Total = cart.Total,
            Items = cart.Items.Select(projection
                => new Models.Item
                {
                    Id = projection.Id,
                    ProductId = projection.ProductId,
                    Quantity = projection.Quantity,
                    PictureUrl = projection.PictureUrl,
                    ProductName = projection.ProductName,
                    UnitPrice = projection.UnitPrice
                }),
            IsDeleted = cart.IsDeleted,
            PaymentMethods = cart.PaymentMethods.Select<IPaymentMethodProjection, Models.IPaymentMethod>(method
                => method switch
                {
                    CreditCardPaymentMethodProjection creditCard
                        => new Models.CreditCard
                        {
                            Amount = creditCard.Amount,
                            Expiration = creditCard.Expiration,
                            Number = creditCard.Number,
                            HolderName = creditCard.HolderName,
                            SecurityNumber = creditCard.SecurityNumber
                        },
                    DebitCardPaymentMethodProjection debitCard
                        => new Models.DebitCard
                        {
                            Amount = debitCard.Amount,
                            Expiration = debitCard.Expiration,
                            Number = debitCard.Number,
                            HolderName = debitCard.HolderName,
                            SecurityNumber = debitCard.SecurityNumber
                        },
                    PayPalPaymentMethodProjection payPal
                        => new Models.PayPal
                        {
                            Amount = payPal.Amount,
                            Password = payPal.Password,
                            UserName = payPal.UserName
                        },
                    _ => default
                }),
            CustomerId = cart.CustomerId
        };
}