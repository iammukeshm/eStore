using AutoMapper;
using eStore.Application.DTOs;
using eStore.Application.Interfaces;
using eStore.Application.Specifications.CatalogSpecification;
using eStore.Domain.Entities.Catalog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eStore.Application.Features.Catalog.Queries
{
    public class GetCatalogItemsQuery : IRequest<Result<List<CatalogItemViewModel>>>
    {

        public int pageIndex { get; set; }
        public int itemsPage { get; set; }
        public int? brandId { get; set; }
        public int? typeId { get; set; }
        public class GetCatalogItemsQueryHandler : IRequestHandler<GetCatalogItemsQuery, Result<List<CatalogItemViewModel>>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IAsyncRepository<CatalogItem> _itemRepository;
            public GetCatalogItemsQueryHandler(ICurrentUserService currentUser, IApplicationDbContext context, IMapper mapper, IAsyncRepository<CatalogItem> itemRepository)
            {
                _context = context;
                _mapper = mapper;
                _itemRepository = itemRepository;
            }
            public async Task<Result<List<CatalogItemViewModel>>> Handle(GetCatalogItemsQuery request, CancellationToken cancellationToken)
            {
                var filterSpecification = new CatalogFilterSpecification(request.brandId, request.typeId);
                var filterPaginatedSpecification = new CatalogFilterPaginatedSpecification(request.itemsPage * request.pageIndex, request.itemsPage, request.brandId, request.typeId);

                // the implementation below using ForEach and Count. We need a List.
                var itemsOnPage = await _itemRepository.ListAsync(filterPaginatedSpecification);
                var totalItems = await _itemRepository.CountAsync(filterSpecification);
                return default;
            }
        }
    }
}
