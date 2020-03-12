using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eStore.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendAsync(string EmailDisplayName, string Subject, string Body, string From, string To);

        Task SendEmailConfirmationAsync(string Email, string CallbackUrl);

        Task SendPasswordResetAsync(string Email, string CallbackUrl);

        Task SendException(Exception ex);
    }
}
