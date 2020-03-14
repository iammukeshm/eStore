using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Application.Interfaces
{
    public interface ICurrentUserService
    {
        string UserId { get; }
    }
}
