using AutoMapper;
using eStore.Application.DTOs;
using eStore.Application.Features.Cart.ViewModels;
using eStore.Application.Interfaces;
using eStore.Application.Interfaces.Repository;
using eStore.Application.Specifications.CartSpecification;
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
        public class GetUserCartQueryHandler : IRequestHandler<GetUserCartQuery, Result<UserCartViewModel>>
        {
            private readonly ICurrentUserService _currentUser;
            private readonly IApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ICartRepository _cartRepository;
            public GetUserCartQueryHandler(ICurrentUserService currentUser, IApplicationDbContext context, IMapper mapper, ICartRepository cartRepository)
            {
                _currentUser = currentUser;
                _context = context;
                _mapper = mapper;
                _cartRepository = cartRepository;
            }
            public async Task<Result<UserCartViewModel>> Handle(GetUserCartQuery request, CancellationToken cancellationToken)
            {
                var specification = new CartWithItemsSpecification(_currentUser.UserId);
                var userCart = await _cartRepository.FirstOrDefaultAsync(specification);
                if(userCart == null)
                {
                    var message = new List<string> { "" };
                    return Result<UserCartViewModel>.Failure(message);
                }
                var result = _mapper.Map<UserCartViewModel>(userCart);
                return Result<UserCartViewModel>.Success(result);                
            }
        }
    }
}
