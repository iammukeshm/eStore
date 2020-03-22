using AutoMapper;
using eStore.Application.DTOs;
using eStore.Application.Features.Cart.ViewModels;
using eStore.Application.Interfaces;
using eStore.Application.Interfaces.Repository;
using eStore.Domain.Entities.CartAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using eStore.Domain.Entities.CartAggregate;

namespace eStore.Application.Features.Cart.Commands
{
    public class CreateCartForUserCommand : IRequest<Result<UserCartViewModel>>
    {
        public string userId { get; set; }
        public class CreateCartForUserCommandHandler : IRequestHandler<CreateCartForUserCommand, Result<UserCartViewModel>>
        {
            private readonly ICurrentUserService _currentUser;
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ICartRepository _cartRepository;
            public CreateCartForUserCommandHandler(ICurrentUserService currentUser, IApplicationDbContext context, IMapper mapper, ICartRepository cartRepository)
            {
                _currentUser = currentUser;
                _context = context;
                _mapper = mapper;
                _cartRepository = cartRepository;
            }
            public async Task<Result<UserCartViewModel>> Handle(CreateCartForUserCommand command, CancellationToken cancellationToken)
            {
                var cart = new eStore.Domain.Entities.CartAggregate.Cart(command.userId);
                await _cartRepository.AddAsync(cart);

                var data = new UserCartViewModel()
                {
                    BuyerId = cart.BuyerId,
                    Id = cart.Id,
                    Items = new List<CartItemViewModel>()
                };
                return Result<UserCartViewModel>.Success(data);
            }
        }
    }
}

