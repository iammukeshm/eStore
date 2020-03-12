using System;
using System.Collections.Generic;
using System.Text;

namespace eStore.Application.Features.Identity.ViewModels
{
    public class LoginViewModel
    {
        public bool? HasVerifiedEmail { get; set; }
        public bool? TFAEnabled { get; set; }
        public string Token { get; set; }
    }
}
