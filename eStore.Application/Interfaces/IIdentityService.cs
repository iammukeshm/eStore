using eStore.Application.DTOs;
using eStore.Application.Features.Identity.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eStore.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<Result<string>> RegisterAsync(string userName, string password, string email);
        Task<Result<LoginViewModel>> LoginAsync(string email, string password);
    }
}
