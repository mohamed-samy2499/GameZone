﻿using GameZone.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameZone.Controllers
{
    public class AccountController : Controller
    {
        public UserManager<IdentityUser> UserManager { get; }
        public SignInManager<IdentityUser> SignInManager { get; }
        private readonly IMailingService _mailingService;
        #region Constructor
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IMailingService mailingService)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _mailingService = mailingService;
        }
        #endregion
        #region Sign up
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    
                    UserName = model.Name,
                    Email = model.Email
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        #endregion
        #region Sign in
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, true);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid Email");
            }
            return View(model);
        }
        #endregion
        #region Sign Out
        public async new Task<IActionResult> SignOut()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion
        #region Forgot password
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //send an email with the link to reset page
                var user = await UserManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var Token = await UserManager.GeneratePasswordResetTokenAsync(user);
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { Email = user.Email, Token = Token }, Request.Scheme);
                    var subject = "Reset Password";
                    var body = $@"<p>Click the button below to reset your password:</p>
                                <a href='{ResetPasswordLink}' 
                                style='display: inline-block; padding: 10px 20px; background-color: #007bff;
                                color: #fff; text-decoration: none; border-radius: 5px;'>Reset Password</a>
                                <p>If you did not request a password reset, please ignore this email.</p>";
                    await _mailingService.SendEmailAsync(model.Email, subject, body);
                    return RedirectToAction(nameof(CompleteForgotPassword));
                }
            }
            return View(model);
        }
        public IActionResult CompleteForgotPassword()
        {
            return View();
        }
        #endregion
        #region Reset Password
        public IActionResult ResetPassword()
        {
            string Email = Request.Query["Email"].ToString();
            string Token = Request.Query["Token"].ToString();
            ViewData["token"] = Token;
            ViewData["email"] = Email;

            return View(new ResetPasswordViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = null;
                if (model.Email != null && model.Token != null)
                {

                    user = await UserManager.FindByEmailAsync(model.Email);
                }
                if (user != null)
                {
                    var result = await UserManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(ResetPasswordDone));
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }
        public IActionResult ResetPasswordDone()
        {
            return View();
        }
        #endregion
    }
}
