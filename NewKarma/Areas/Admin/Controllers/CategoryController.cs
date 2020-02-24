using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using NewKarma.Models.Domain;
using NewKarma.Models.View;
using NewKarma.Repository.UOW;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly IUnitOfWork _unit;
        public CategoryController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpGet, DisplayName("مدیریت دسته بندی"), Authorize]
        public async Task<IActionResult> Index(int page = 1, int row = 5)
        {
            var category = _unit.BaseRepo<Category>().FindAllAsync();
            var PagingModel = PagingList.Create(await category, row, page);
            List<int> Rows = new List<int>
            {
                5,10,15,20,50,100
            };

            ViewBag.RowID = new SelectList(Rows, row);
            ViewBag.NumOfRow = (page - 1) * row + 1;
            PagingModel.RouteValue = new RouteValueDictionary
            {
                {"row",row},
            };
            return View(PagingModel ?? null);
        }

        [HttpGet, DisplayName("افزودن دسته بندی"), Authorize]
        public IActionResult Create() { return View(); }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VmCategory model)
        {
            if (ModelState.IsValid)
            {
                //if (image != null && image.Length > 0)
                //{

                //    var fileName = Path.GetFileName(image.FileName);
                //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Category\\");
                //    string TempImage = filePath + "Original\\" + fileName;
                //    using (var fileStream = new FileStream(TempImage, FileMode.Create))
                //    {
                //        await image.CopyToAsync(fileStream);
                //    }
                //    Image_resize(TempImage, filePath + fileName, 50);
                //    string destination = filePath + "_" + fileName;
                //    System.IO.File.Move(TempImage, destination);
                //    if (System.IO.File.Exists(destination))
                //        System.IO.File.Delete(destination);
                //}
                Category category = new Category
                {
                    Title = model.Title,
                    UserIDFK = model.UserIDFK,
                };
                await _unit.BaseRepo<Category>().Create(category);
                await _unit.Commit();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet, DisplayName("ویرایش دسته بندی"), Authorize]
        public async Task<IActionResult> Edit(int? catId, Category model)
        {
            if (catId == null)
            {
                return NotFound();
            }
            var category = await _unit.BaseRepo<Category>().FindByIdAsync(catId);
            if (category == null)
            {
                return null;
            }
            ViewBag.CatId = new SelectList(_unit.BaseRepo<Category>().FindAll());
            return View(category);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int catId, Category model)
        {
            if (catId != model.CatId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    Category categoryOld = _unit.BaseRepo<Category>().FindByIdAsync(model.CatId).Result;
                    _unit.BaseRepo<Category>().Update(categoryOld);
                    await _unit.Commit();
                    ViewBag.MsgConfirm = "ذخیره تغییرات با موفقیت انجام شد";
                    return View(model);
                }     
                catch (Exception ex)
                {
                    ViewBag.MsgFailed = "در ذخیره تغییرات خطایی رخ داده است.";
                    return View(model);
                }
            }
            else
            {
                ViewBag.MsgFaild = "اطلاعات فرم نا معتبر است.";
                return View(model);
            }
        }
        [HttpGet, Authorize]
        public async Task<IActionResult> Delete(int? catId)
        {
            if (catId == null)
            {
                return NotFound();
            }
            var category = await _unit.BaseRepo<Category>().FindByIdAsync(catId);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmred(int catId)
        {
            var category = await _unit.BaseRepo<Category>().FindByIdAsync(catId);
            if (category == null)
            {
                return NotFound();
            }
            else

                _unit.BaseRepo<Category>().Delete(category);
            await _unit.Commit();
            return RedirectToAction(nameof(Index));
        }



    }
}