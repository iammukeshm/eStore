using AutoMapper;
using eStore.Application.DTOs;
using eStore.Application.Features.Cart.ViewModels;
using eStore.Application.Features.Catalog;
using eStore.Application.Interfaces;
using eStore.Application.Interfaces.Repository;
using eStore.Domain.Entities.CartAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eStore.Application.Features.Cart.Commands
{
    public class AddItemToCartCommand : IRequest<Result<long>>
    {
        public int cartId { get; set; }
        public CatalogItemViewModel itemModel { get; set; }
        public class AddItemToCartCommandHandler : IRequestHandler<AddItemToCartCommand, Result<long>>
        {
            private readonly ICurrentUserService _currentUser;
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ICartRepository _cartRepository;
            public AddItemToCartCommandHandler(ICurrentUserService currentUser, IApplicationDbContext context, IMapper mapper, ICartRepository cartRepository)
            {
                _currentUser = currentUser;
                _context = context;
                _mapper = mapper;
                _cartRepository = cartRepository;
            }
            public async Task<Result<long>> Handle(AddItemToCartCommand command, CancellationToken cancellationToken)
            {
                var cart = await _cartRepository.GetByIdAsync(command.cartId);

                cart.AddItem(command.itemModel.Id, command.itemModel.Price, 1);

                await _cartRepository.UpdateAsync(cart);

                return Result<long>.Success(1);
                
            }
        }
    }
}
