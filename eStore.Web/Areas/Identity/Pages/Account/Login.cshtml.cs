using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using eStore.Application.Features.Identity.Commands;
using eStore.Infrastructure.Identity.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Logging;
using eStore.Domain.Settings;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components;
using NToastNotify;

namespace eStore.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private IMediator _mediator;
        private readonly JwtSecurityTokenSettings _jwt;
        private readonly IToastNotification _toastNotification;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            IMediator mediator,
            IOptions<JwtSecurityTokenSettings> jwt,
            IToastNotification toastNotification
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _mediator = mediator;
            _jwt = jwt.Value;
            _toastNotification = toastNotification;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }
        public  ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _jwt.Audience;
            validationParameters.ValidIssuer = _jwt.Issuer;
            validationParameters.IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);


            return principal;
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {

                var command = new LoginUserCommand() { Email = Input.Email, Password = Input.Password };
                var reponse =  await Mediator.Send(command);
                if(!reponse.Succeeded)
                {
                    _toastNotification.AddErrorToastMessage(reponse.Messages[0]);
                    return Page();
                }
                //// This doesn't count login failures towards account lockout
                //// To enable password failures to trigger account lockout, set lockoutOnFailure: true
                ////_userManager.Login
                //var userIdentity = new ClaimsIdentity("Custom");
                var claims = ValidateToken(reponse.Data.Token);
                var user = await _userManager.GetUserAsync(claims);
                //await _signInManager.SignInAsync(user, false);
                //you could firstly get the user name and password from the database then you could create the application user and use SignInAsync to login the user.
                //var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, Id = "f1143ba9-4d45-4948-a186-700a033f3abf" };


                await _signInManager.SignInAsync(user, isPersistent: true);
                _toastNotification.AddSuccessToastMessage(string.Format("Logged in as {0}",user.UserName));
                return LocalRedirect(returnUrl);

                //ApplicationUser signedUser = await _userManager.FindByEmailAsync(Input.Email);
                //var result = await _signInManager.PasswordSignInAsync(signedUser.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                ////var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                //if (result.Succeeded)
                //{
                //    _logger.LogInformation("User logged in.");
                //    return LocalRedirect(returnUrl);
                //}
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                //}
                //if (result.IsLockedOut)
                //{
                //    _logger.LogWarning("User account locked out.");
                //    return RedirectToPage("./Lockout");
                //}
                //else
                //{
                //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                //    return Page();
                //}
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
