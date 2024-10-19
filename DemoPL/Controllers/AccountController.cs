using Demo.DAL.Models;
using DemoPL.Helpers;
using DemoPL.Helpers.Interfaces;
using DemoPL.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IMailSettings _mailSettings;
		private readonly ISmsService _smsService;

		public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager , 
            IMailSettings mailSettings, ISmsService smsService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
			_mailSettings = mailSettings;
			_smsService = smsService;
		}
        #region Register
        // Register
        public IActionResult Register()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel Model)
        {

            if (ModelState.IsValid)
            {
                var User = new ApplicationUser()
                {
                    UserName = Model.Email.Split('@')[0],
                    Email = Model.Email,
                    FName = Model.FName,
                    LName = Model.LName,
                    IsAgree = Model.IsAgree


                };

                var Result = await _userManager.CreateAsync(User, Model.Password);

                if (Result.Succeeded)
                    return RedirectToAction("Login");
                else
                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);


            }
            return View(Model);


        }

        #endregion
        #region Login
        // Login 
        public IActionResult Login()
        {

            return View();

        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user is not null)
                {
                    bool Flag = await _userManager.CheckPasswordAsync(user, model.Password);

                    if (Flag)
                    {

                        var Result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RemeberMe, false);

                        if (Result.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }


                    }
                    else
                        ModelState.AddModelError(string.Empty, "Incorrect Password");


                }

                else
                    ModelState.AddModelError(string.Empty, "Email is not Exsits");



            }

            return View(model);
        }

        #endregion


        #region Sign out
        // Sign out 

        public new async Task<IActionResult> SignOut()
        {

            //  await HttpContext.SignOutAsync(GoogleDefaults.AuthenticationScheme);

            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region AccessDenied

        //  AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion


        #region ForgetPassword
        public IActionResult ForgetPassword()
        {
            return View();
        }
         public IActionResult ForgetPasswordSMS()
        {
            return View();
        }

        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }

        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(User);
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = User.Email, Token = token }, Request.Scheme);


                    var Email = new Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = ResetPasswordLink
                    };

                    _mailSettings.SendEmail(Email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }

                else
                {
                    ModelState.AddModelError(string.Empty, "Email is not Exists");
                }


            }

            return View("ForgetPassword", model);
        }
        public async Task<IActionResult> SendSMS(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(model.Email);
                if (User is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(User);
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = User.Email, Token = token }, Request.Scheme);


                    var sms = new SmsMessage()
                    {
                       PhoneNumber = User.PhoneNumber,
                       Body = ResetPasswordLink

                    };

                    _smsService.SendSms(sms);
                    return RedirectToAction(nameof(CheckYourPhone));
                }

               
                    ModelState.AddModelError(string.Empty, "Email is not Exists");
                


            }

            return View("ForgetPassword", model);
        }

        public IActionResult CheckYourInbox()
        {
            return View();
        } 
        public IActionResult CheckYourPhone()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            //P@ssw0rd
            //Pa$$w0rd
            if (ModelState.IsValid)
            {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;

                var User = await _userManager.FindByEmailAsync(email);
                var Result = await _userManager.ResetPasswordAsync(User, token, model.NewPassword);

                if (Result.Succeeded)
                    return RedirectToAction(nameof(Login));

                else
                    foreach (var error in Result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);

        }

        #endregion



        [HttpGet]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // GET: /Account/GoogleResponse
        [HttpGet]
		public async Task<IActionResult> GoogleResponse()
		{
			var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

			if (result?.Succeeded != true) 
			{
				return RedirectToAction("Login");
			}

			// Extract user's email from Google authentication
			var email = result.Principal.FindFirstValue(ClaimTypes.Email);
			var name = result.Principal.FindFirstValue(ClaimTypes.Name); // Get the name

			// Case-sensitivity check 
			if (!string.IsNullOrEmpty(email))
			{
				email = email.ToLowerInvariant(); // Normalize email for case-insensitive comparison
			}

			// Check if the user exists 
			var user = await _userManager.FindByEmailAsync(email);

			if (user == null)
			{
                // If user does not exist, create a new user in your system
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true, // Optionally, set email as confirmed since it's from a trusted provider
                                           
                    FName = name.Split(' ').FirstOrDefault(),
					 LName = name.Split(' ').LastOrDefault() 




				};  

				// Create the new user with a random password (or store it if needed)
				var createResult = await _userManager.CreateAsync(user);

				if (!createResult.Succeeded)
				{
					// Handle case when user creation fails
					foreach (var error in createResult.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
					// Redirect to an error page if you want
					return RedirectToAction("UserCreationFailed");
				}

			}

			try
			{
				await _signInManager.SignInAsync(user, isPersistent: false);
			}
			catch (Exception ex)
			{
				
					ModelState.AddModelError(string.Empty, ex.Message);
				
				return RedirectToAction(nameof(Login));
			}

			return RedirectToAction("Index", "Home");
		}
	}
}
