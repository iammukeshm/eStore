using AutoMapper;
using eStore.Application.DTOs;
using eStore.Application.Features.Catalog.ViewModels;
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
    public class GetCatalogDetailsQuery : IRequest<Result<CatalogDetailsViewModel>>
    {

        public int? Id { get; set; }
        public class GetCatalogDetailsQueryHandler : IRequestHandler<GetCatalogDetailsQuery, Result<CatalogDetailsViewModel>>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly IAsyncRepository<CatalogItem> _itemRepository;
            private readonly IUriComposer _uriComposer;
            public GetCatalogDetailsQueryHandler(ICurrentUserService currentUser, IApplicationDbContext context, IMapper mapper, IAsyncRepository<CatalogItem> itemRepository, IUriComposer uriComposer)
            {
                _context = context;
                _mapper = mapper;
                _itemRepository = itemRepository;
                _uriComposer = uriComposer;
            }
            public async Task<Result<CatalogDetailsViewModel>> Handle(GetCatalogDetailsQuery request, CancellationToken cancellationToken)
            {
                var data = await _itemRepository.GetByIdAsync(request.Id.Value);
                var vm = new CatalogDetailsViewModel()
                {
                    CatalogDetail = new CatalogItemViewModel()
                    {
                        Id = data.Id,
                        Name = data.Name,
                        PictureUri = _uriComposer.ComposePicUri(data.PictureUri),
                        Price = data.Price
                    }

                };
                return Result<CatalogDetailsViewModel>.Success(vm);
            }
        }
    }
}
