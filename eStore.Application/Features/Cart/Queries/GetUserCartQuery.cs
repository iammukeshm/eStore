using AutoMapper;
using eStore.Application.DTOs;
using eStore.Application.Features.Cart.ViewModels;
using eStore.Application.Interfaces;
using eStore.Application.Interfaces.Repository;
using eStore.Application.Specifications.CartSpecification;
using eStore.Domain.Entities.Catalog;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eStore.Application.Features.Cart.Queries
{
    public class GetUserCartQuery : IRequest<Result<UserCartViewModel>>
    {
        public string userId { get; set; }
        public class GetUserCartQueryHandler : IRequestHandler<GetUserCartQuery, Result<UserCartViewModel>>
        {
         
            private readonly IMapper _mapper;
            private readonly ICartRepository _cartRepository;
            private readonly IUriComposer _uriComposer;
            private readonly IAsyncRepository<CatalogItem> _itemRepository;
            public GetUserCartQueryHandler(IMapper mapper, ICartRepository cartRepository, IUriComposer uriComposer, IAsyncRepository<CatalogItem> itemRepository)
            {
                _mapper = mapper;
                _cartRepository = cartRepository;
                _itemRepository = itemRepository;
                _uriComposer = uriComposer;
            }
            public async Task<Result<UserCartViewModel>> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
            {
                var specification = new CartWithItemsSpecification(request.userId);
                var userCart = (await _cartRepository.ListAsync(specification)).FirstOrDefault();
                if (userCart == null)
                {
                    var message = new List<string> { "" };
                    return Result<UserCartViewModel>.Failure(message);
                }
                var data = await CreateViewModelFromCart(userCart);
                return Result<UserCartViewModel>.Success(data);                
            }
            private async Task<UserCartViewModel> CreateViewModelFromCart(eStore.Domain.Entities.CartAggregate.Cart cart)
            {
                var viewModel = new UserCartViewModel();
                viewModel.Id = cart.Id;
                viewModel.BuyerId = cart.BuyerId;
                viewModel.Items = await GetCartItems(cart.Items); ;
                return viewModel;
            }
            private async Task<List<CartItemViewModel>> GetCartItems(IReadOnlyCollection<eStore.Domain.Entities.CartAggregate.CartItem> cartItems)
            {
                var items = new List<CartItemViewModel>();
                foreach (var item in cartItems)
                {
                    var itemModel = new CartItemViewModel
                    {
                        Id = item.Id,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity,
                        CatalogItemId = item.CatalogItemId
                    };
                    var catalogItem = await _itemRepository.GetByIdAsync(item.CatalogItemId);
                    itemModel.PictureUrl = _uriComposer.ComposePicUri(catalogItem.PictureUri);
                    itemModel.ProductName = catalogItem.Name;
                    items.Add(itemModel);
                }

                return items;
            }
        }
    }
}
