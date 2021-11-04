using System.Threading.Tasks;
using Application.Abstractions.EventSourcing.Projections.Pagination;
using Application.EventSourcing.Projections;
using MassTransit;
using Messages.Services.Catalogs;
using GetCatalogItemsDetailsWithPaginationQuery = Messages.Services.Catalogs.Queries.GetCatalogItemsDetailsWithPagination;

namespace Application.UseCases.Queries;

public class GetCatalogItemsDetailsConsumer : IConsumer<GetCatalogItemsDetailsWithPaginationQuery>
{
    private readonly ICatalogProjectionsService _projectionsService;

    public GetCatalogItemsDetailsConsumer(ICatalogProjectionsService projectionsService)
    {
        _projectionsService = projectionsService;
    }

    public async Task Consume(ConsumeContext<GetCatalogItemsDetailsWithPaginationQuery> context)
    {
        var catalogItems = await _projectionsService.GetCatalogItemsWithPaginationAsync(
            paging: new Paging { Limit = context.Message.Limit, Offset = context.Message.Offset },
            predicate: catalog => catalog.Id == context.Message.CatalogId
                                  && catalog.IsActive
                                  && catalog.IsDeleted == false,
            selector: catalog => catalog.Items,
            cancellationToken: context.CancellationToken);

        await context.RespondAsync<Responses.CatalogItemsDetailsPagedResult>(catalogItems);
    }
}