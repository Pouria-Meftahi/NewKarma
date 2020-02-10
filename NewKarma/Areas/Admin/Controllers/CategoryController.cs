using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using NewKarma.Models.Domain;
using NewKarma.Models.View;
using NewKarma.Repository.UOW;
using ReflectionIT.Mvc.Paging;
using System;
using System.ComponentModel;
using System.IO;
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
            PagingModel.RouteValue = new RouteValueDictionary
            {
                {"row",row},
            };
            return View(PagingModel ?? null);
        }

        [HttpGet, DisplayName("افزودن دسته بندی"), Authorize]
        public IActionResult Create() { return View(); }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VmCategory model, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {

                    //Todo:Manage Size Image
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Category", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    Category category = new Category
                    {
                        Description = model.Description,
                        Icon = fileName,
                        Title = model.Title,
                        UserIDFK = model.UserIDFK,
                    };
                    await _unit.BaseRepo<Category>().Create(category);
                    await _unit.Commit();
                }
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
        public async Task<IActionResult> Edit(int catId, Category model, IFormFile image)
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
                    if (categoryOld != null)
                    {
                        categoryOld.Title = model.Title;
                        categoryOld.Description = model.Description;
                        categoryOld.UserIDFK = model.UserIDFK;
                        var oldImage = _unit.BaseRepo<Category>().FindByIdAsync(model.CatId).Result.Icon;
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Category", oldImage);
                        if (System.IO.File.Exists(oldPath))
                        {
                            if (image != null && image.Length > 0)
                            {
                                System.IO.File.Delete(oldPath);
                                //Todo:Resize Image And Even Set 
                                var newImage = Path.GetFileName(image.FileName);
                                var newPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Category", newImage);
                                using (var fileStream = new FileStream(newPath, FileMode.Create))
                                {
                                    await image.CopyToAsync(fileStream);
                                }
                                categoryOld.Icon = newImage;
                            }
                        }
                        else
                        {
                            if (image != null && image.Length > 0)
                            {
                                //Todo:Resize Image
                                var fileName = Path.GetFileName(image.FileName);
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Category", fileName);
                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await image.CopyToAsync(fileStream);
                                }
                                categoryOld.Icon = fileName;
                            }
                        }
                    }
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
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _unit.BaseRepo<Category>().FindByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmred(int id)
        {
            var category = await _unit.BaseRepo<Category>().FindByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            else
            {
                //Todo:Delete Image Of This Category From DataBase Of File Server
                _unit.BaseRepo<Category>().Delete(category);
                await _unit.Commit();
                return RedirectToAction(nameof(Index));
            }
        }
    }
}