using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactsManager.Core.Services
{
    public class LogoutService : ILogoutService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LogoutService(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<LogoutResponseDto> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return new LogoutResponseDto() { IsSucceeded = true };
        }
    }
}
