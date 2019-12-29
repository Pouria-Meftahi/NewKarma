using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using NewKarma.Areas.Admin.Models;
using NewKarma.Areas.Identity.Data;
using NewKarma.Tools;
using ReflectionIT.Mvc.Paging;

namespace NewKarma.Areas.Admin.Controllers
{
    [Area("Admin"), DisplayName("مدیریت کاربران")]
    public class UserManagerController : Controller
    {
        private readonly IApplicationUserManager _userManager;
        private readonly IApplicationRoleManager _roleManager;
        private readonly IConvertDate _convertDate;
        private readonly IEmailSender _emailSender;
        public UserManagerController(IApplicationUserManager userManager, IApplicationRoleManager roleManager, IConvertDate convertDate, IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _convertDate = convertDate;
            _emailSender = emailSender;
        }
        [DisplayName("مشاهده کاربران"), Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> Index(string Msg, int page = 1, int row = 5)
        {
            if (Msg == "Success")
            {
                ViewBag.Alert = "ثبت نام با موفقیت انجام شد";
            }
            if (Msg == "SendEmailSuccess")
            {
                ViewBag.Alert = "ارسال ایمیل به کاربران با موفقیت انجام شد.";
            }
            List<int> Rows = new List<int>
            {
                5,10,15,20,50,100
            };
            ViewBag.RowId = new SelectList(Rows, row);
            ViewBag.NumOfPage = (page - 1) * row + 1;
            var PagingModel = PagingList.Create(await _userManager.GetAllUserWithRoleAsync(), row, page);
            PagingModel.RouteValue = new RouteValueDictionary
            {
                {"row",row }
            };
            return View(PagingModel);
        }
        [DisplayName("مشاهده جزیئات"), Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
                return NotFound();
            else
            {
                var UserDet = await _userManager.FindUserWithRolesByIdAsync(id);
                if (UserDet == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(UserDet);
                }
            }
        }
        [HttpGet, DisplayName("ویرایش"), Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
                return NotFound();
            var User = await _userManager.FindUserWithRolesByIdAsync(id);
            if (User == null)
                return NotFound();
            else
            {
                ViewBag.AllRoles = _roleManager.GetAllRole();
                //if (User.BirthDate != null)
                //    User.PersianBirthDate = _convertDate.ConvertMiladiToShamsi((DateTime)User.BirthDate, "yyyy/MM/dd");
                //User.BirthDate = _convertDate.ConvertShamsiToMiladi(User.BirthDate.ToLongDateString());
                return View(User);
            }
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VmUsersManager ViewModel)
        {
            if (ModelState.IsValid)
            {
                var User = await _userManager.FindByIdAsync(ViewModel.Id);
                if (User == null)
                    return NotFound();
                else
                {
                    IdentityResult Result;
                    var RecentRoles = await _userManager.GetRolesAsync(User);
                    if (ViewModel.Roles == null)
                        ViewModel.Roles = new string[] { };
                    var DeleteRoles = RecentRoles.Except(ViewModel.Roles);
                    var AddRoles = ViewModel.Roles.Except(RecentRoles);

                    Result = await _userManager.RemoveFromRolesAsync(User, DeleteRoles);
                    if (Result.Succeeded)
                    {
                        Result = await _userManager.AddToRolesAsync(User, AddRoles);
                        if (Result.Succeeded)
                        {
                            User.FirstName = ViewModel.Name;
                            User.LastName = ViewModel.Family;
                            User.Email = ViewModel.Email;
                            //User.PhoneNumber = ViewModel.PhoneNumber;
                            //User.Avatar = ViewModel.Avatar;//TODO:Impelement For Upload Image
                            //User.Bio = ViewModel.Bio;
                            User.UserName = ViewModel.UserName;
                            //User.BirthDate = _convertDate.ConvertShamsiToMiladi(ViewModel.PersianBirthDate);
                            Result = await _userManager.UpdateAsync(User);
                            if (Result.Succeeded)
                            {
                                ViewBag.AlertSuccess = "ذخیره تغییرات با موفقیت انجام شد.";
                            }
                        }
                    }

                    if (Result != null)
                    {
                        foreach (var item in Result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
            }

            ViewBag.AllRoles = _roleManager.GetAllRole();
            return View(ViewModel);
        }
        [HttpGet, DisplayName("حذف"), Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
                return NotFound();
            var User = await _userManager.FindByIdAsync(id);
            if (User == null)
                return NotFound();
            else
                return View(User);
        }
        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<IActionResult> Deleted(string id)
        {
            if (id == null)
                return NotFound();
            var User = await _userManager.FindByIdAsync(id);
            if (User == null)
                return NotFound();
            else
            {
                var Result = await _userManager.DeleteAsync(User);
                if (Result.Succeeded)
                    return RedirectToAction("Index");
                else
                    ViewBag.AlertError = "در حذف اطلاعات خطایی رخ داده است.";

                return View(User);
            }
        }
        public async Task<IActionResult> SendEmail(string[] emails, string subject, string message)
        {
            if (emails != null)
            {
                for (int i = 0; i < emails.Length; i++)
                {
                    await _emailSender.SendEmailAsync(emails[i], subject, message);
                }
            }

            return RedirectToAction("Index", new { Msg = "SendEmailSuccess" });
        }
        public async Task<IActionResult> GetUserInRole(string id, int page = 1, int row = 10)
        {
            var PagingModel = PagingList.Create(await _roleManager.GetUsersInRoleAsync(id), row, page);
            return View("Index", PagingModel);
        }
    }
}