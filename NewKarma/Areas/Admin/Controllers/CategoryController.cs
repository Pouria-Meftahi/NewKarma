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

                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Category\\");
                    string TempImage = filePath + "Original\\" + fileName;
                    using (var fileStream = new FileStream(TempImage, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    Image_resize(TempImage, filePath + fileName, 50);
                    string destination = filePath + "_" + fileName;
                    System.IO.File.Move(TempImage, destination);
                    if (System.IO.File.Exists(destination))
                        System.IO.File.Delete(destination);

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
                                ///http://localhost:5000/img/imgUpload/Category/C:/Users/Pouria%20Meftahi/Documents/Visual%20Studio%202017/Projects/WEB/2019/Julay/NewKarma/NewKarma/wwwroot/img/imgUpload/Category/Original/Pouria.jpg
                                var fileName = Path.GetFileName(image.FileName);
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Category\\");
                                string TempImage = filePath + "Original\\" + fileName;
                                using (var fileStream = new FileStream(TempImage, FileMode.Create))
                                {
                                    await image.CopyToAsync(fileStream);
                                }
                                categoryOld.Icon = TempImage;
                                Image_resize(TempImage, filePath + fileName, 50);
                                string destination = filePath + "_" + fileName;
                                System.IO.File.Move(TempImage, destination);
                                if (System.IO.File.Exists(destination))
                                    System.IO.File.Delete(destination);
                            }
                        }
                        else
                        {
                            if (image != null && image.Length > 0)
                            {

                                var fileName = Path.GetFileName(image.FileName);
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Category\\");
                                string TempImage = filePath + "Original\\" + fileName;
                                using (var fileStream = new FileStream(TempImage, FileMode.Create))
                                {
                                    await image.CopyToAsync(fileStream);
                                }
                                categoryOld.Icon = fileName;
                                Image_resize(TempImage, filePath + fileName, 50);
                                string destination = filePath + "_" + fileName;
                                System.IO.File.Move(TempImage, destination);
                                if (System.IO.File.Exists(destination))
                                    System.IO.File.Delete(destination);
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
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Category\\", category.Icon);
                System.IO.File.Delete(filePath);
                _unit.BaseRepo<Category>().Delete(category);
                await _unit.Commit();
                return RedirectToAction(nameof(Index));
            }
        }




        private void Image_resize(string inputImagePath, string outputImagePath, int newWidth)
        {
            const long quality = 50L;
            Bitmap source_Bitmap = new Bitmap(inputImagePath);
            double dblWidth_origial = source_Bitmap.Width;
            double dblHeigth_origial = source_Bitmap.Height;
            int new_Height = newWidth;
            //double relation_heigth_width = dblHeigth_origial / dblWidth_origial;
            //int new_Height = (int)(newWidth * relation_heigth_width);
            var new_DrawArea = new Bitmap(newWidth, new_Height);
            using (var graphic_of_DrawArea = Graphics.FromImage(new_DrawArea))
            {
                graphic_of_DrawArea.CompositingQuality = CompositingQuality.HighSpeed;
                graphic_of_DrawArea.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic_of_DrawArea.CompositingMode = CompositingMode.SourceCopy;
                graphic_of_DrawArea.DrawImage(source_Bitmap, 0, 0, newWidth, new_Height);
                using (var output = System.IO.File.Open(outputImagePath, FileMode.Create))
                {
                    var qualityParamId = Encoder.Quality;
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                    var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);
                    new_DrawArea.Save(output, codec, encoderParameters);
                    output.Close();
                }
                graphic_of_DrawArea.Dispose();
            }
            source_Bitmap.Dispose();
        }
    }
}