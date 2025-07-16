using E_ticket.Models;
using E_ticket.Models.viewmodel;
using E_ticket.Repostoris.IRepository;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_ticket.Areas.identity.Controllers
{
    [Area("identity")]
    public class accountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRuserotprepostoity _ruserotprepostoity;

        public accountController(UserManager<ApplicationUser> userManager, IEmailSender emailSender,
            SignInManager<ApplicationUser> signInManager , IRuserotprepostoity ruserotprepostoity)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _ruserotprepostoity = ruserotprepostoity;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Registervm registervm)
        {
            ApplicationUser applicationUser = new()
            {
                FirstName = registervm.FirstName,
                LastName = registervm.LastName,
                Email = registervm.Email,
                Address = registervm.Address,
                UserName = registervm.UserName

            };
            //ApplicationUser applicationUser1 = registervm.Adapt<ApplicationUser>();
            var resualt = await _userManager.CreateAsync(applicationUser, registervm.Password);
            if (resualt.Succeeded)
            {
                //send email
                var token = _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                var link = Url.Action(nameof(ConfirmEmail), "account", new { area = "identity"
                    , token = token, userid = applicationUser.Id }, Request.Scheme);
                await _emailSender.SendEmailAsync(registervm.Email,
                "Confirm Your Account", $"<h1>Confirm Your Account By Clicking <a href={link}>Here</a></h1>");

                //send massge
                TempData["successfull notification"] = "succssefull add create account ,Confirm Your Account!";
                return RedirectToAction(nameof(Index), "Home", new { area = "movies" });
            }
            foreach (var item in resualt.Errors)
            {
                ModelState.AddModelError(string.Empty, item.Description);
                return View(registervm);
            }
            //TempData["successfull notification"] = string.Join(" ", resualt.Errors.Select(e => e.Description));
            return View(registervm);


        }
        public async Task<IActionResult> ConfirmEmail(string userid, string token)
        {
            var user = await _userManager.FindByIdAsync(userid);
            if (user is not null)
            {
                var resault = await _userManager.ConfirmEmailAsync(user, token);
                if (resault.Succeeded)
                {
                    return View();
                }

                TempData["successfull notification"] = string.Join(" ", resault.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Index), "Home", new { area = "movies" });
            }
            return NotFound();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Loginvm loginvm)
        {
            var user = await _userManager.FindByEmailAsync(loginvm.NameOrEmail) ??
                 await _userManager.FindByNameAsync(loginvm.NameOrEmail);
            if (user is not null)
            {
                var resualt = await _signInManager.PasswordSignInAsync(user.UserName !, loginvm.Password, loginvm.Remmeberme, lockoutOnFailure: true);
                if (resualt.Succeeded)
                {
                    TempData["successfull notification"] = "succsefull login ";
                    return RedirectToAction(nameof(Index), "Home", new { area = "movies" });
                }
                if (resualt.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "the lockout ");
                    return View(loginvm);
                }
                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "confirm your accont ");
                    return View(loginvm);
                }
            }
            ModelState.AddModelError(string.Empty, "Invalid User Name Or Password");
            return View(loginvm);

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["successfull notification"] = "succsefull logout ";
            return RedirectToAction(nameof(Index), "Home", new { area = "movies" });

        }
        public IActionResult ResendEmailConfrimation()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendEmailConfrimation(resendEmailConftimtioanvm resendEmailConftimtioanvm)
        {
            var user = await _userManager.FindByEmailAsync(resendEmailConftimtioanvm.EmailOrName) ??
                await _userManager.FindByNameAsync(resendEmailConftimtioanvm.EmailOrName);

            if (user is not null)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action(nameof(ConfirmEmail), "Account", new { area = "Identity", token = token, userId = user.Id }, Request.Scheme);
                await _emailSender.SendEmailAsync(user.Email !,
                "Confirm Your Account", $"<h1>Confirm Your Account By Clicking <a href={link}>Here</a></h1>");

            }

            // Send msg
            TempData["success-notification"] = "Confirm Your Account Again!";
            return RedirectToAction(nameof(Index), "Home", new { ara = "movies" });
         }
        public IActionResult ForgerPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgerPassword(ForgetPasswordvm forgetPasswordvm)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordvm.EmailOrName) ??
                await _userManager.FindByNameAsync(forgetPasswordvm.EmailOrName);
            var userotp = await _ruserotprepostoity.GetAsync(e => e.ApplicationUserId == user.Id);
            if (userotp.Count(e => (e.Date.Day == DateTime.UtcNow.Day &&
            e.Date.Month == DateTime.UtcNow.Month &&
            e.Date.Year == DateTime.UtcNow.Year)) < 10)
            {
                if (user is not null)
                {
                    var otpunmber = new Random().Next(1000, 9999);
                    await _emailSender.SendEmailAsync(user.Email , "Reset Password", $"<h1>Reset Password Using OTP Number {otpunmber}</h1>");

                    await _ruserotprepostoity.CreateAsync(new()
                    {
                        Code = otpunmber.ToString(),
                        Date = DateTime.UtcNow,
                        ExprationDate = DateTime.UtcNow.AddHours(1),
                        ApplicationUserId = user.Id

                    });
                    await _ruserotprepostoity.CommitAsync();

                    TempData["redirectoaction"] = Guid.NewGuid().ToString();
                    return RedirectToAction(nameof(ResandCode), "account", new { area = "identity", userid = user.Id });
                }
            }
          
            TempData["success-notification"] = "Too Many Request, Please try again Later";
            return View(forgetPasswordvm);
            
        }
        public IActionResult ResandCode(string userid)
        {
            if (TempData["redirectoaction"] is not null)
            {
                if (userid is not null)
                {
                    return View( new ResandCodevm()
                    {
                        Userid = userid
                    });
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ResandCode(ResandCodevm resandCodevm)
        {
         var userotp = (await _ruserotprepostoity.GetAsync(e => e.ApplicationUserId == resandCodevm.Userid))
                .OrderBy(e => e.id).LastOrDefault();
            if (userotp is not null)
            {

                if (userotp.Code == resandCodevm.Code && DateTime.UtcNow < userotp.ExprationDate)
                {
                    TempData["redirectoaction"] = Guid.NewGuid().ToString();
                    return RedirectToAction(nameof(ChangePassword), "account", new { area = "identity", userid = userotp.id });
                }
            }
            TempData["success-notification"] = "errors in the code";
            return View(resandCodevm);
        }
        public IActionResult ChangePassword(string userid)
        {
            if (TempData["redirectoaction"] is not null)
            {
                if (userid is not null)
                {
                    return View(new ChangePasswordvm()
                    {
                        Userid = userid
                    });
                }
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordvm changePasswordvm)
        {

            var user = await _userManager.FindByIdAsync(changePasswordvm.Userid);
            if (user is not null)
            {
                var token =await _userManager.GeneratePasswordResetTokenAsync(user);
              await  _userManager.ResetPasswordAsync(user, token, changePasswordvm.Password);
                TempData["success-notification"] = "succsefull resand password";
                RedirectToAction(nameof(Index), "Home", new { area = "identity" });
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return RedirectToAction(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Try signing in with an external login
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl ?? "/");
            }

            // If the user cannot log in, try finding them by email
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var name = info.Principal.FindFirstValue(ClaimTypes.Name);
            if (email != null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    // Create a new user if they do not exist
                    user = new ApplicationUser
                    {
                        UserName = name.Replace(" ", ""),
                        Email = email,
                        FirstName = name,
                        LastName = String.Empty
                    };
                    var createUserResult = await _userManager.CreateAsync(user);
                    if (!createUserResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Error creating user.");
                        return RedirectToAction(nameof(Login));
                    }
                }

                // Ensure the external login is linked
                var existingLogins = await _userManager.GetLoginsAsync(user);
                var hasGoogleLogin = existingLogins.Any(l => l.LoginProvider == info.LoginProvider);

                if (!hasGoogleLogin)
                {
                    var addLoginResult = await _userManager.AddLoginAsync(user, info);
                    if (!addLoginResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, "Error linking external login.");
                        return RedirectToAction(nameof(Login));
                    }
                }

                // Sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl ?? "/");
            }

            return RedirectToAction(nameof(Login));
        }



    }
}
