using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using NewKarma.Areas.Identity.Data;
using NewKarma.Models;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using static NewKarma.Models.View.VmAccount;

namespace NewKarma.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHostingEnvironment _env;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly AppDbContext _context;
        public AccountController(UserManager<ApplicationUser> userManager, IHostingEnvironment env, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _context = context;
            _env = env;
        }
        //#region Register
        //[HttpGet]
        //public IActionResult Register()
        //{
        //    return View();
        //}
        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = model.UserName,
        //            Email = model.Email,
        //            RegisterDate = DateTime.Now,
        //            Status = true
        //        };
        //        IdentityResult result = await _userManger.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            var role = _roleManager.FindByNameAsync("کاربر");
        //            if (role == null)
        //                await _roleManager.CreateAsync(new ApplicationRole("کاربر"));
        //            result = await _userManger.AddToRoleAsync(user, "کاربر");
        //            if (result.Succeeded)
        //            {
        //                var code = await _userManger.GenerateEmailConfirmationTokenAsync(user);
        //                var callbackUrl = Url.Action("ConfirmEmail", "Account", values: new { userId = user.Id, code = code }, protocol: Request.Scheme);

        //                await _emailSender.SendEmailAsync(model.Email,
        //                "تایید ایمیل حساب کاربری - استودیو پویا نمایی نقطه",
        //                   $"<div dir='rtl' style='font-family:tahoma;font-size:14px'>لطفا با کلیک روی لینک ایمیل خود را تایید کنید." +
        //                  $"  <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>کلیک کنید</a></div>");
        //                return RedirectToAction("Index", "Home", new { id = "ConfirmEmail" });
        //            }
        //        }
        //        foreach (var item in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, item.Description);
        //        }
        //    }
        //    return View();
        //}
        //#endregion
        #region Login
        [HttpGet]
        public IActionResult Login(string ReturnUrl = null)
        {
            return View();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByNameAsync(model.UserName);
                if (User != null)
                {
                    if (User.Status)
                    {
                        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index", "Dashbord", new { area = "Admin" });
                        }
                        else if (result.IsLockedOut)
                        {
                            ModelState.AddModelError(string.Empty, "حساب کاربری شما به مدت 20 دقیقه به دلیل تلاش های ناموفق قفل شد.");
                            return View(model);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "حساب کاربری شما فعال نمی باشد به ایمیل خود مراجعه کنید.");
                            return View(model);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "نام کاربری یا کلمه عبور اشتباه می باشد");
                    return View(model);
                }
            }
            return View(model);
        }
        #endregion
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #region ForgetPassword
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel ViewModel)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByEmailAsync(ViewModel.Email);
                if (User == null)
                {
                    ModelState.AddModelError(string.Empty, "ایمیل شما صحیح نمی باشد.");
                }

                if (!await _userManager.IsEmailConfirmedAsync(User))
                {
                    ModelState.AddModelError(string.Empty, "لطفا با تایید ایمیل حساب کاربری خود را فعال کنید.");
                }

                var Code = await _userManager.GeneratePasswordResetTokenAsync(User);
                var CallbackUrl = Url.Action("ResetPassword", "Account", values: new { Code }, protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(ViewModel.Email, "بازیابی کلمه عبور", $"<p style='font-family:tahoma;font-size:14px'>برای بازنشانی کلمه عبور خود <a href='{HtmlEncoder.Default.Encode(CallbackUrl)}'>اینجا کلیک کنید</a></p>");

                return RedirectToAction("ForgetPasswordConfirmation");
            }

            return View();
        }

        [HttpGet]
        public IActionResult ForgetPasswordConfirmation()
        {
            return View();
        }
        #endregion
        #region ResetPassword
        [HttpGet]
        public IActionResult ResetPassword(string Code = null)
        {
            if (Code == null)
                return NotFound();
            else
            {
                var ViewModel = new ResetPasswordViewModel { Code = Code };
                return View(ViewModel);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel ViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                var User = await _userManager.FindByEmailAsync(ViewModel.Email);
                if (User == null)
                {
                    ModelState.AddModelError(string.Empty, "ایمیل شما صحیح نمی باشد.");
                    return View();
                }
                var Result = await _userManager.ResetPasswordAsync(User, ViewModel.Code, ViewModel.Password);
                if (Result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }

                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }
        }
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion
        public IActionResult AccessDenied(string ReturnUrl = null)
        {
            return View();
        }
    }
}