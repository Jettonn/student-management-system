using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SMS.Application.Dtos;
using SMS.Application.GenericRepository;
using SMS.Application.Interfaces;
using SMS.Application.Models;
using SMS.Application.ViewModels.Account;

namespace SMS.Application.Controllers
{
    public class AccountController : Controller
    {
        private readonly IToastNotification toastNotification;
        private readonly IGenericRepository<Professor> professorRepository;
        private readonly IUserService userService;
        public AccountController(IToastNotification toastNotification,
                                IGenericRepository<Professor> professorRepository,
                                IUserService userService)
        {
            this.toastNotification = toastNotification;
            this.professorRepository = professorRepository;
            this.userService = userService;
        }
        public IActionResult LoginOrRegister(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (String.IsNullOrEmpty(returnUrl))
            {
                ViewData["ReturnUrl"] = "/Home/Index";
            }
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Register(AccountViewModel model)
        {
            try
            {
                if (model.registerViewModel.ConfirmPassword != model.registerViewModel.Password)
                {
                    toastNotification.AddErrorToastMessage("Confirm password and Password should be the same to register!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                    return View("LoginOrRegister");
                }
                var professors = professorRepository.GetAll();
                if (professors.Where(x => x.Username == model.registerViewModel.Username).Any())
                {
                    toastNotification.AddWarningToastMessage("The username you are trying to register is already taken, try another one please!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                    return View("LoginOrRegister");
                }
                if (!ModelState.IsValid)
                {
                    toastNotification.AddErrorToastMessage("An error occured during registration!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                    return View("LoginOrRegister");
                }
                userService.AddNewUser(model);
                toastNotification.AddSuccessToastMessage("Success", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return View("LoginOrRegister");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AccountViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                toastNotification.AddErrorToastMessage("An error occured!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return View("LoginOrRegister");
            }
            if (String.IsNullOrEmpty(model.loginViewModel.Password) || String.IsNullOrWhiteSpace(model.loginViewModel.Password))
            {
                toastNotification.AddErrorToastMessage("Username or Password aren't correct!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                return View("LoginOrRegister");
            }
            var user = userService.Authenticate(model.loginViewModel.Username, model.loginViewModel.Password);
            ViewBag.returnUrl = returnUrl;
            string returnTo = "/Account/LoginOrRegister";

            var userDto = new User();

            try
            {
                if (user != null)
                {
                    userDto = new User
                    {
                        Firstname = user.Firstname,
                        Lastname = user.Lastname,
                        Password = user.Password,
                        Username = user.Username,
                        Id = user.ProfessorId
                    };
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        returnTo = returnUrl;
                    }
                    else
                    {
                        returnTo = "/Home/Index";
                    }
                }
                else
                {
                    toastNotification.AddErrorToastMessage("Username or Password aren't correct!", new ToastrOptions() { CloseButton = true, ProgressBar = true, PositionClass = "toast-bottom-right", PreventDuplicates = true });
                    return View("LoginOrRegister");
                }

                //cookies authentication
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.Username)
                };
                var identity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();
                return Redirect(returnTo);
            }
            catch (UnauthorizedAccessException ue)
            {
                ModelState.AddModelError(String.Empty, ue.Message);
                return View("LoginOrRegister", model);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(String.Empty, e.Message);
                return View("LoginOrRegister", model);
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("LoginOrRegister");
        }
    }
}
