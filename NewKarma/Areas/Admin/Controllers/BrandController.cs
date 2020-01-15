using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NewKarma.Areas.Identity.Data;
using NewKarma.Models.Domain;
using NewKarma.Repository.UOW;
using ReflectionIT.Mvc.Paging;

namespace NewKarma.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unit;
        public BrandController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpGet, DisplayName("مدیریت برند"), Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> Index(int page = 1, int row = 5)
        {
            var brand = _unit.BaseRepo<Brand>().FindAllAsync();
            var PagingModel = PagingList.Create(await brand, row, page);
            PagingModel.RouteValue = new RouteValueDictionary
            {
                {"row",row},
            };
            return View(PagingModel ?? null);
        }

        [HttpGet, DisplayName("افزودن برند"), Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public IActionResult Create() { return View(); }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand model)
        {
            if (ModelState.IsValid)
            {
                await _unit.BaseRepo<Brand>().Create(model);
                await _unit.Commit();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet, DisplayName("ویرایش برند"), Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var brand = await _unit.BaseRepo<Brand>().FindByIdAsync(id);
            if (brand == null)
            {
                return null;
            }
            ViewBag.BrandId = new SelectList(_unit.BaseRepo<Brand>().FindAll());
            return View(brand);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Brand model)
        {
            if (id != model.BrandId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _unit.BaseRepo<Brand>().Update(model);
                    await _unit.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExist(model.BrandId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet, DisplayName("حذف برند"), Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var brand = await _unit.BaseRepo<Brand>().FindByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmred(int id)
        {
            var brand = await _unit.BaseRepo<Brand>().FindByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            else
            {
                _unit.BaseRepo<Brand>().Delete(brand);
                await _unit.Commit();
                return RedirectToAction(nameof(Index));
            }
        }



        public bool BrandExist(int id)
        {
            return _unit._context.Brands.Any(a => a.BrandId == id);
        }
    }
}