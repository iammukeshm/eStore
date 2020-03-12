using eStore.Application.DTOs;
using eStore.Application.Features.Identity.ViewModels;
using eStore.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eStore.Application.Features.Identity.Commands
{
    public class LoginUserCommand : IRequest<Result<LoginViewModel>>
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginViewModel>>
        {
            private readonly IIdentityService _context;
            public LoginUserCommandHandler(IIdentityService context)
            {
                _context = context;
            }

            public async Task<Result<LoginViewModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
            {
                return await _context.LoginAsync(request.Email, request.Password);
            }
        }
    }
}
