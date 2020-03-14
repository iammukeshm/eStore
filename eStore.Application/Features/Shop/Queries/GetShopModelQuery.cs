using AutoMapper;
using eStore.Application.DTOs;
using eStore.Application.Features.Catalog;
using eStore.Application.Features.Common.ViewModels;
using eStore.Application.Features.Shop.ViewModels;
using eStore.Application.Interfaces;
using eStore.Application.Specifications.CatalogSpecification;
using eStore.Domain.Entities.Catalog;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eStore.Application.Features.Shop.Queries
{
    public class GetShopModelQuery : IRequest<Result<ShopViewModel>>
    {

        public int pageIndex { get; set; }
        public int itemsPage { get; set; }
        public int? brandId { get; set; }
        public int? typeId { get; set; }
        public class GetShopModelQueryHandler : IRequestHandler<GetShopModelQuery, Result<ShopViewModel>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IAsyncRepository<CatalogItem> _itemRepository;
            private readonly IAsyncRepository<CatalogBrand> _brandRepository;
            private readonly IAsyncRepository<CatalogType> _typeRepository;
            private readonly IUriComposer _uriComposer;
            public GetShopModelQueryHandler
            (
                IApplicationDbContext context,
                IMapper mapper, 
                IAsyncRepository<CatalogItem> itemRepository,
                IAsyncRepository<CatalogBrand> brandRepository,
                IAsyncRepository<CatalogType> typeRepository,
                IUriComposer uriComposer
            )
            {
                _context = context;
                _mapper = mapper;
                _itemRepository = itemRepository;
                _brandRepository = brandRepository;
                _typeRepository = typeRepository;
                _uriComposer = uriComposer;
            }
            public async Task<Result<ShopViewModel>> Handle(GetShopModelQuery request, CancellationToken cancellationToken)
            {
                var filterSpecification = new CatalogFilterSpecification(request.brandId, request.typeId);
                var filterPaginatedSpecification = new CatalogFilterPaginatedSpecification(request.itemsPage * request.pageIndex, request.itemsPage, request.brandId, request.typeId);

                // the implementation below using ForEach and Count. We need a List.
                var itemsOnPage = await _itemRepository.ListAsync(filterPaginatedSpecification);
                var totalItems = await _itemRepository.CountAsync(filterSpecification);

                var vm = new ShopViewModel()
                {
                    CatalogItems = itemsOnPage.Select(i => new CatalogItemViewModel()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        PictureUri = _uriComposer.ComposePicUri(i.PictureUri),
                        Price = i.Price
                    }),
                    Brands = await GetBrands(),
                    Types = await GetTypes(),
                    BrandFilterApplied = request.brandId ?? 0,
                    TypesFilterApplied = request.typeId ?? 0,
                    PaginationInfo = new PaginationInfoViewModel()
                    {
                        ActualPage = request.pageIndex,
                        ItemsPerPage = itemsOnPage.Count,
                        TotalItems = totalItems,
                        TotalPages = int.Parse(Math.Ceiling(((decimal)totalItems / request.itemsPage)).ToString())
                    }
                };

                vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
                vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

                return Result<ShopViewModel>.Success(vm);
            }
            public async Task<IEnumerable<SelectListItem>> GetBrands()
            {
                var brands = await _brandRepository.ListAllAsync();

                var items = new List<SelectListItem>
            {
                new SelectListItem() { Value = null, Text = "All", Selected = true }
            };
                foreach (CatalogBrand brand in brands)
                {
                    items.Add(new SelectListItem() { Value = brand.Id.ToString(), Text = brand.Brand });
                }

                return items;
            }

            public async Task<IEnumerable<SelectListItem>> GetTypes()
            {
                var types = await _typeRepository.ListAllAsync();
                var items = new List<SelectListItem>
            {
                new SelectListItem() { Value = null, Text = "All", Selected = true }
            };
                foreach (CatalogType type in types)
                {
                    items.Add(new SelectListItem() { Value = type.Id.ToString(), Text = type.Type });
                }

                return items;
            }
        }
    }
}
