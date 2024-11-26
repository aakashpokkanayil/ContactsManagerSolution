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
    public class LoginService : ILoginService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginService(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDTO loginDTO)
        {
            // lockoutOnFailure if true , multiple false attempt result in block login for a while.
            SignInResult result= await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false,lockoutOnFailure:false);
            if (result.Succeeded)
            {
                return new LoginResponseDto
                {
                    IsSucceeded = true
                };
            }
            else
            {
                return new LoginResponseDto
                {
                    IsSucceeded = false,
                    ErrorMessage = "InValid UserName or Password."
                };

            }
        }
    }
}
