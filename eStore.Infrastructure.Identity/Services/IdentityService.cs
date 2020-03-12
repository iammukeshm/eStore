using eStore.Application.DTOs;
using eStore.Application.Exceptions;
using eStore.Application.Features.Identity.ViewModels;
using eStore.Application.Interfaces;
using eStore.Domain.Settings;
using eStore.Infrastructure.Identity.Extensions;
using eStore.Infrastructure.Identity.Helpers;
using eStore.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace eStore.Infrastructure.Identity.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly ClientAppSettings _client;
        private readonly JwtSecurityTokenSettings _jwt;

        public IdentityService(UserManager<ApplicationUser> userManager, IEmailService emailService, IOptions<ClientAppSettings> client, IOptions<JwtSecurityTokenSettings> jwt, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _client = client.Value;
            _jwt = jwt.Value;
        }
        public async Task<Result<string>> RegisterAsync(string userName, string password, string email)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = email,
            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = $"{_client.Url}{_client.EmailConfirmationPath}?uid={user.Id}&code={System.Net.WebUtility.UrlEncode(code)}";

                await _emailService.SendEmailConfirmationAsync(email, callbackUrl);

                return result.ToApplicationResult("", user.Id);
            }
            return result.ToApplicationResult("", user.Id);
        }

        public async Task<Result<LoginViewModel>> LoginAsync(string email, string password)
        {
            //if (await _userManager.FindByNameAsync(userName) == null)
            //{
            //    throw new NotFoundException(nameof(ApplicationUser), userName, nameof(userName));
            //}
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                NotFoundException ex =  new NotFoundException(nameof(ApplicationUser), email, nameof(email));
                return new Result<LoginViewModel>(false, new string[] { ex.Message }, null);
            }
            //if (user == null)
            //    return BadRequest(new string[] { "Invalid credentials." });

            var loginModel = new LoginViewModel()
            {
                HasVerifiedEmail = false
            };

            // Only allow login if email is confirmed
            if (!user.EmailConfirmed)
            {
                throw new Exception(string.Format("Email not confirmed for user {0}.", user.Email));
            }

            //// Used as user lock
            //if (user.LockoutEnabled)
            //    return BadRequest(new string[] { "This account has been locked." });

            if (await _userManager.CheckPasswordAsync(user, password))
            {
                loginModel.HasVerifiedEmail = true;

                if (user.TwoFactorEnabled)
                {
                    loginModel.TFAEnabled = true;
                    return new Result<LoginViewModel>(false, new string[] { "2 Factor Authentication" }, null);
                }
                else
                {
                    JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);
                    loginModel.TFAEnabled = false;
                    loginModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                    return new Result<LoginViewModel>(true, new string[] { "" }, loginModel);
                }
            }
            return new Result<LoginViewModel>(false, new string[] { string.Format("Incorrect Credentials for user {0}.", user.Email) }, null);
           // throw new Exception();
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            string ipAddress = IpHelper.GetIpAddress();
            var claims = new[]
            {
                  new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                  new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                  new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                  new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }.Union(userClaims);
            //var claims = new[]
            //{
            //    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    new Claim(JwtRegisteredClaimNames.Email, user.Email),
            //    new Claim("uid", user.Id),
            //    new Claim("ip", ipAddress)
            //}
            //.Union(userClaims)
            //.Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
