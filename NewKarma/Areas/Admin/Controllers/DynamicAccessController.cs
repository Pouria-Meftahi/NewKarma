using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NewKarma.Areas.Admin.Models;
using NewKarma.Areas.Identity.Data;
using NewKarma.Areas.Identity.Services;

namespace NewKarma.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DynamicAccessController : Controller
    {
        public readonly IApplicationRoleManager _roleManager;
        public readonly IMvcActionsDiscoveryService _mvcActions;

        public DynamicAccessController(IApplicationRoleManager roleManager, IMvcActionsDiscoveryService mvcActions)
        {
            _roleManager = roleManager;
            _mvcActions = mvcActions;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
                return NotFound();
            var Role = await _roleManager.FindClaimInRole(id);
            if (Role == null)
                return NotFound();
            var SecuredControllerAction = _mvcActions.GetAllSecuredControllerActionsWithPolicy(ConstantPolicies.DynamicPermission);
            return View(new VmDynamicAccessIndex
            {
                RoleIncludeRoleClaims = Role,
                SecuredControllerActinos = SecuredControllerAction
            });
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(VmDynamicAccessIndex model)
        {
            var Result = await _roleManager.AddOrUpdateClaimsAsync(model.RoleId, ConstantPolicies.DynamicPermissionClaimType, model.ActionId);
            if (!Result.Succeeded)
                ModelState.AddModelError(string.Empty, "در حین انجام عملیات خطایی رخ داده است");
            return RedirectToAction("Index", new { id = model.RoleId });
        }
    }
}