using eStore.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eStore.Infrastructure.Identity.Extensions
{
    public static class IdentityResultExtensions
    {
        public static Result<string> ToApplicationResult(this IdentityResult result, string message, string data)
        {
            return result.Succeeded
                ? Result<string>.Success(message, data)
                : Result<string>.Failure(result.Errors.Select(e => e.Description));
        }
    }
}
