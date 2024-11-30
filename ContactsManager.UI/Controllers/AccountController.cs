using Azure;
using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTO;
using ContactsManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace ContactsManager.UI.Controllers
{
    [Route("[controller]/[action]")]
    
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRegisterService _registerService;
        private readonly ILoginService _loginService;
        private readonly ILogoutService _logoutService;

        public AccountController(UserManager<ApplicationUser> userManager, IRegisterService registerService, ILoginService loginService, ILogoutService logoutService)
        {
            _userManager = userManager;
            _registerService = registerService;
            _loginService = loginService;
            _logoutService = logoutService;
        }

        [Authorize("NotAuth")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuth")]
        public async Task<IActionResult> Register(RegisterDTO register)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                return View(register);
            }
            RegisterResponseDto response = await _registerService.RegisterUser(register);

            if (response != null && response.IsSucceeded)
            { 
                
                return RedirectToAction(nameof(PersonController.Index), "Person");
            }
            else
            {
                if (response != null && response.ErrorMessage != null)
                {
                    foreach (IdentityError error in response.ErrorMessage)
                    {
                        ModelState.AddModelError("Register", error.Description);
                    }
                }
                return View(register);
            }
        }
        [Authorize("NotAuth")]
        public IActionResult Login()
        {
            return View();
        }
        [Authorize("NotAuth")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO login, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
                return View(login);
            }

            LoginResponseDto response = await _loginService.LoginAsync(login);

            if (response != null && response.IsSucceeded)
            {
                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
                return RedirectToAction(nameof(PersonController.Index), "Person");
            }
            else
            {
                if (response != null && response.ErrorMessage != null)
                {
                    ModelState.AddModelError("Login", response.ErrorMessage); 
                }
                return View(login);
            }
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _logoutService.LogoutAsync();
            return RedirectToAction(nameof(PersonController.Index), "Person");
        }
    }
}
