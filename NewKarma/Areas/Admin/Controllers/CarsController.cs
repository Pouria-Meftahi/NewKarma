using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NewKarma.Models.Domain;
using NewKarma.Models.View;
using NewKarma.Repository.UOW;
using ReflectionIT.Mvc.Paging;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Areas.Admin.Controllers
{
    public class CarsController : BaseController
    {
        private readonly IUnitOfWork _unit;
        public CarsController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpGet, DisplayName("مدیریت خوردو"), Authorize]
        public async Task<IActionResult> Index(int page = 1, int row = 5)
        {
            var car = _unit.BaseRepo<Car>().FindAllAsync();
            var PagingModel = PagingList.Create(await car, row, page);
            PagingModel.RouteValue = new RouteValueDictionary
            {
                {"row",row},
            };
            return View(PagingModel ?? null);
        }

        [HttpGet, DisplayName("افزودن خودرو"), Authorize]
        public IActionResult Create() { return View(); }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car model)
        {
            if (ModelState.IsValid)
            {
                await _unit.BaseRepo<Car>().Create(model);
                await _unit.Commit();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet, DisplayName("ویرایش خودرو"), Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var car = await _unit.BaseRepo<Car>().FindByIdAsync(id);
            if (car == null)
            {
                return null;
            }
            ViewBag.CarId = new SelectList(_unit.BaseRepo<Car>().FindAll());
            return View(car);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car model)
        {
            if (id != model.CarId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _unit.BaseRepo<Car>().Update(model);
                    await _unit.Commit();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExist(model.CarId))
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

        [HttpGet, DisplayName("حذف خودرو"), Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var car = await _unit.BaseRepo<Car>().FindByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmred(int id)
        {
            var car = await _unit.BaseRepo<Car>().FindByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            else
            {
                _unit.BaseRepo<Car>().Delete(car);
                await _unit.Commit();
                return RedirectToAction(nameof(Index));
            }
        }

        public bool CarExist(int id)
        {
            return _unit._context.Cars.Any(a => a.CarId == id);
        }
    }
}