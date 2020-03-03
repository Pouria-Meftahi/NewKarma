using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NewKarma.Models.Domain;
using NewKarma.Repository.UOW;
using ReflectionIT.Mvc.Paging;
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
    public class BrandController : BaseController
    {
        private readonly IUnitOfWork _unit;
        public BrandController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpGet, DisplayName("مدیریت برند"), Authorize]
        public async Task<IActionResult> Index(int page = 1, int row = 15)
        {
            var brand = _unit.BaseRepo<Brand>().FindAllAsync();
            var PagingModel = PagingList.Create(await brand, row, page);
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
        [HttpGet, DisplayName("افزودن برند"), Authorize]
        public IActionResult Create() { return View(); }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand model,IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Brand\\");
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

                    Brand brand = new Brand
                    {
                        Logo = fileName,
                        Title = model.Title,
                        UserIDFK = model.UserIDFK,
                    };
                    await _unit.BaseRepo<Brand>().Create(brand);
                    await _unit.Commit();
                }
                else
                {
                    await _unit.BaseRepo<Brand>().Create(model);
                    await _unit.Commit();
                }
                return RedirectToAction(nameof(Index));//Hack:Is It Working I Mean Redirect to aciton
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet, DisplayName("ویرایش برند"), Authorize]
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
        public async Task<IActionResult> Edit(int id, Brand model ,IFormFile image)
        {
            if (id != model.BrandId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    Brand brandOld = _unit.BaseRepo<Brand>().FindByIdAsync(model.BrandId).Result;
                    if (brandOld != null)
                    {
                        brandOld.Title = model.Title;
                        brandOld.UserIDFK = model.UserIDFK;
                        var oldImage = _unit.BaseRepo<Brand>().FindByIdAsync(model.BrandId).Result.Logo;
                        if (oldImage==null)
                        {
                            if (image != null && image.Length > 0)
                            {
                                var fileName = Path.GetFileName(image.FileName);
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Brand\\");
                                string TempImage = filePath + "Original\\" + fileName;
                                using (var fileStream = new FileStream(TempImage, FileMode.Create))
                                {
                                    await image.CopyToAsync(fileStream);
                                }
                                brandOld.Logo = TempImage;
                                Image_resize(TempImage, filePath + fileName, 50);
                                string destination = filePath + "_" + fileName;
                                System.IO.File.Move(TempImage, destination);
                                if (System.IO.File.Exists(destination))
                                    System.IO.File.Delete(destination);
                            }
                        }
                        else
                        {
                            var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Brand", oldImage);
                            if (System.IO.File.Exists(oldPath))
                            {
                                if (image != null && image.Length > 0)
                                {
                                    System.IO.File.Delete(oldPath);
                                    var fileName = Path.GetFileName(image.FileName);
                                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Brand\\");
                                    string TempImage = filePath + "Original\\" + fileName;
                                    using (var fileStream = new FileStream(TempImage, FileMode.Create))
                                    {
                                        await image.CopyToAsync(fileStream);
                                    }
                                    brandOld.Logo = TempImage;
                                    Image_resize(TempImage, filePath + fileName, 50);
                                    string destination = filePath + "_" + fileName;
                                    System.IO.File.Move(TempImage, destination);
                                    if (System.IO.File.Exists(destination))
                                        System.IO.File.Delete(destination);
                                }
                            }
                        }
                    }
                    _unit.BaseRepo<Brand>().Update(brandOld);
                    await _unit.Commit();
                    ViewBag.MsgConfirm = "ذخیره تغییرات با موفقیت انجام شد";
                    return View(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExist(model.BrandId))
                    {
                        ViewBag.MsgFailed = "خطا برند مورد نظر پیدا نشد";
                        return NotFound();
                    }
                    else
                    {
                        ViewBag.MsgFailed = "در ذخیره تغییرات خطایی رخ داده است.";
                        return View(model);
                    }
                }
            }
            else
            {
                ViewBag.MsgFaild = "اطلاعات فرم نا معتبر است.";
                return View(model);
            }
        }

        [HttpGet, DisplayName("حذف برند"), Authorize]
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
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\brand\\", brand.Logo);
                System.IO.File.Delete(filePath);
                _unit.BaseRepo<Brand>().Delete(brand);
                await _unit.Commit();
                return RedirectToAction(nameof(Index));
            }
        }


        //Hack:CHeck And Remove If Its Not Zaruri!
        public bool BrandExist(int id)
        {
            return _unit._context.Brands.Any(a => a.BrandId == id);
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