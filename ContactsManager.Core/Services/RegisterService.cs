﻿using AutoMapper;
using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.Enum;
using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace ContactsManager.Core.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RegisterService(IMapper mapper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<RegisterResponseDto> RegisterUser(RegisterDTO register)
        {
            ApplicationUser user = _mapper.Map<ApplicationUser>(register);
            IdentityResult result= await _userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
            {
                if (register.UserType == UserTypeOptions.Admin)
                {
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                        await _roleManager.CreateAsync(new ApplicationRole() 
                        {
                            Name=UserTypeOptions.Admin.ToString() 
                        });
                    }
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                }
                else
                {
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) is null)
                    {
                        await _roleManager.CreateAsync(new ApplicationRole()
                        {
                            Name = UserTypeOptions.User.ToString()
                        });
                    }
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                // This makes usersiged in and isPersistent make user sigedin even after browser closed until logout like google.
                // We can set this like remember me checkbox option
                // _signInManager.SignInAsync will also create a cookie of current session and send it to client.
                return new RegisterResponseDto
                {
                    IsSucceeded = true
                };
            }
            else
            {
                return new RegisterResponseDto
                {
                    IsSucceeded = false,
                    ErrorMessage = result.Errors
                };
                
            }

        }
    }
}
