using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NewKarma.Areas.Admin.Models;
using NewKarma.Areas.Identity.Data;
using ReflectionIT.Mvc.Paging;

namespace NewKarma.Areas.Admin.Controllers
{
    [Area("Admin"), DisplayName("مدیریت نقش ها")]
    public class RolesManagerController : Controller
    {
        private readonly IApplicationRoleManager _roleManager;
        public RolesManagerController(IApplicationRoleManager roleManager)
        {
            _roleManager = roleManager;
        }
        [HttpGet, DisplayName("نثش ها"), Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public IActionResult Index(int page = 1, int row = 10)
        {
            var Roles = _roleManager.GetAllRolesUserCount();
            //ViewBag.Style = "warning";
            //if (Roles.FirstOrDefault().RoleName=="")
            //{
            //}
            //TODO:impelemnt Coler Taker In Panel
            var PaginationModel = PagingList.Create(Roles, row, page);
            PaginationModel.RouteValue = new RouteValueDictionary
            {
                {"row",row }
            };
            return View(PaginationModel);
        }
        [HttpGet, DisplayName("ایجاد نقش"), Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public IActionResult AddRole()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(VmRolesManager model)
        {
            if (ModelState.IsValid)
            {
                if (await _roleManager.RoleExistsAsync(model.RoleName))
                {
                    ViewBag.Error = " خطا!!! این نقش تکراری است";
                }
                else
                {
                    var Result = await _roleManager.CreateAsync(new ApplicationRole(model.RoleName, model.RoleDescription));
                    if (Result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    ViewBag.Error = "در ذخیره اطلاعات خطایی رخ داده است.";
                }
            }
            return View(model);
        }
        [HttpGet, DisplayName("ویرایش نثش"), Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> EditRole(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            else
            {
                var Role = await _roleManager.FindByIdAsync(Id);
                if (Role == null)
                {
                    return NotFound();
                }
                else
                {
                    VmRolesManager oRoleManager = new VmRolesManager()
                    {
                        RoleId = Role.Id,
                        RoleName = Role.Name,
                        RoleDescription = Role.Description,
                        RecentRoleName = Role.Name
                    };
                    return View(oRoleManager);
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(VmRolesManager model)
        {
            if (ModelState.IsValid)
            {
                var Role = await _roleManager.FindByIdAsync(model.RoleId);
                if (Role == null)
                {
                    return NotFound();
                }
                if (await _roleManager.RoleExistsAsync(model.RoleName) && model.RecentRoleName != model.RoleName)
                {
                    ViewBag.Error = "خطا!!! این نقش تکراری است";
                }
                else
                {
                    Role.Name = model.RoleName;
                    var Result = await _roleManager.UpdateAsync(Role);
                    if (Result.Succeeded)
                    {
                        ViewBag.Success = "alert-sucsess";
                        return View(model);
                    }
                    ViewBag.Error = "alert-danger";
                    return View(model);
                }
            }
            return View(model);
        }
        [HttpGet, DisplayName("حذف نثش"), Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> DeleteRole(string RoleId)
        {
            if (RoleId == null)
            {
                return NotFound();
            }
            var Role = await _roleManager.FindByIdAsync(RoleId);
            if (Role == null)
            {
                return NotFound();
            }
            VmRolesManager oRoleManager = new VmRolesManager()
            {
                RoleId = Role.Id,
                RoleName = Role.Name
            };
            return View(oRoleManager);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteRole")]
        public async Task<IActionResult> DeletedRole(string RoleId)
        {
            if (RoleId == null)
            {
                return NotFound();
            }
            var Role = await _roleManager.FindByIdAsync(RoleId);
            if (Role == null)
            {
                return NotFound();
            }
            var Result = await _roleManager.DeleteAsync(Role);
            if (Result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Error = "در حذف اطلاعات خطایی رخ داده است.";
            VmRolesManager oRoleManager = new VmRolesManager()
            {
                RoleId = Role.Id,
                RoleName = Role.Name
            };
            return View(oRoleManager);
        }
    }
}