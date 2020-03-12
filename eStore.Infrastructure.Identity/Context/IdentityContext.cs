using eStore.Application.Interfaces;
using eStore.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Infrastructure.Identity.Context
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>, IIdentityContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {

        }
    }
}
