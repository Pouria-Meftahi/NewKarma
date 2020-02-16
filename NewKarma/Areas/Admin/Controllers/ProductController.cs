using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
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
    public class ProductController : BaseController
    {
        private readonly IUnitOfWork _unit;
        private IMapper _mapper;
        public ProductController(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }

        [HttpGet, DisplayName("قطعات"), Authorize]
        public async Task<IActionResult> Index(int page = 1, int row = 10, string sortExpression = "Title", string title = "")
        {

            title = String.IsNullOrEmpty(title) ? "" : title;
            List<int> Rows = new List<int>
            {
                5,10,15,20,50,100
            };

            ViewBag.RowID = new SelectList(Rows, row);
            ViewBag.NumOfRow = (page - 1) * row + 1;
            ViewBag.Search = title;

            var PagingModel = PagingList.Create(_unit.ProductRepo.GetAllProduct(title, "", "", ""), row, page, sortExpression, "Title");
            PagingModel.RouteValue = new RouteValueDictionary
            {
                {"row",row},
                {"title",title }
            };
            ViewBag.BrandId = new SelectList(_unit.BaseRepo<Brand>().FindAll(), "BrandId", "Title");
            ViewBag.CatId = new SelectList(_unit.BaseRepo<Category>().FindAll(), "CatId", "Title");
            ViewBag.CarId = new SelectList(_unit.BaseRepo<Car>().FindAll().Select(a => new CarList { CarId = a.CarId, CarTitleModel = a.CarTitle + " " + a.CarModel }), "CarId", "CarTitleModel");
            return View(PagingModel) ?? null;
        }

        [HttpGet, DisplayName("جزئیات قطعات"), Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _unit.BaseRepo<VmProduct>().FindByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpGet, DisplayName("افزودن قطعات"), Authorize]
        public IActionResult Create()
        {
            ViewBag.BrandId = new SelectList(_unit.BaseRepo<Brand>().FindAll(), "BrandId", "Title");
            ViewBag.CatId = new SelectList(_unit.BaseRepo<Category>().FindAll(), "CatId", "Title");
            ViewBag.CarId = new SelectList(_unit.BaseRepo<Car>().FindAll().Select(a => new CarList { CarId = a.CarId, CarTitleModel = a.CarTitle + " " + a.CarModel }), "CarId", "CarTitleModel");
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VmProduct model, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.Length > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Product");
                    string sFile_Target_Original = filePath + "Original\\" + fileName;
                    using (var fileStream = new FileStream(sFile_Target_Original, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }
                    Image_resize(sFile_Target_Original, filePath + fileName, 360,203);
                    string destination = filePath + "_" + fileName;
                    System.IO.File.Move(sFile_Target_Original, destination);
                    if (System.IO.File.Exists(destination))
                        System.IO.File.Delete(destination);


                    Product product = new Product
                    {
                        Title = model.Title,
                        Description = model.Description,
                        Img = fileName,
                        CreatedDate = model.CreatedDate,
                        Situation = model.Situation,
                        UserIDFK = model.UserIdFK,
                        BrandIDFK = model.BrandIDFK,
                        CatIDFK = model.CatIDFK,
                        RlCarModelProduct = model.CarIDFK.Select(a => new RlCarModelProduct { CarId = a }).ToList(),
                    };
                    await _unit.BaseRepo<Product>().Create(product);
                    await _unit.Commit();
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.BrandId = new SelectList(_unit.BaseRepo<Brand>().FindAll(), "BrandId", "Title");
                ViewBag.CatId = new SelectList(_unit.BaseRepo<Category>().FindAll(), "CatId", "Title");
                ViewBag.CarId = new SelectList(_unit.BaseRepo<Car>().FindAll().Select(a => new CarList { CarId = a.CarId, CarTitleModel = a.CarTitle + " " + a.CarModel }), "CarId", "CarTitleModel");
                return View(model);
            }
        }


        [HttpGet, DisplayName("ویرایش قطعات"), Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                var product = await _unit.BaseRepo<Product>().FindByIdAsync(id);
                if (product == null)
                {
                    return NotFound();
                }
                else
                {
                    var viewModel = (from row in _unit._context.Products
                                     .Include(a => a.Category)
                                     .Include(a => a.Brand)
                                         //.Include(a => a.RlCarModelProduct)
                                     where (row.ProductId == id)
                                     select new VmProduct
                                     {
                                         ProductId = row.ProductId,
                                         Title = row.Title,
                                         Description = row.Description,
                                         CreatedDate = row.CreatedDate,
                                         Image = row.Img,
                                         Situation = row.Situation,
                                         BrandIDFK = row.BrandIDFK,
                                         CatIDFK = row.CatIDFK,
                                         UserIdFK = row.UserIDFK,
                                     }).FirstAsync();

                    int[] CarsArray = await _unit._context.RlCarModelProducts
                        .Where(a => a.ProductId == id)
                        .Select(a => a.CarId).ToArrayAsync();

                    viewModel.Result.CarIDFK = CarsArray;
                    ViewBag.CatId = new SelectList(_unit.BaseRepo<Category>().FindAll(), "CatId", "Title");
                    ViewBag.BrandId = new SelectList(_unit.BaseRepo<Brand>().FindAll(), "BrandId", "Title");
                    ViewBag.CarId = new SelectList(_unit.BaseRepo<Car>().FindAll().Select(a => new CarList { CarId = a.CarId, CarTitleModel = a.CarTitle + " " + a.CarModel }), "CarId", "CarTitleModel");
                    return View(viewModel.Result);
                }
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VmProduct model, IFormFile image)
        {
            ViewBag.CatId = new SelectList(_unit.BaseRepo<Category>().FindAll(), "CatId", "Title");
            ViewBag.BrandId = new SelectList(_unit.BaseRepo<Brand>().FindAll(), "BrandId", "Title");
            ViewBag.CarId = new SelectList(_unit.BaseRepo<Car>().FindAll().Select(a => new CarList { CarId = a.CarId, CarTitleModel = a.CarTitle + " " + a.CarModel }), "CarId", "CarTitleModel");
            if (ModelState.IsValid)
            {
                try
                {
                    Product productOld = _unit.BaseRepo<Product>().FindByIdAsync(model.ProductId).Result;
                    if (productOld != null)
                    {
                        productOld.Title = model.Title;
                        productOld.Description = model.Description;
                        productOld.Situation = model.Situation;
                        productOld.CreatedDate = model.CreatedDate;//Hack:Its Must be Check  For Is Published Or Not And set it recent time
                        productOld.BrandIDFK = model.BrandIDFK;
                        productOld.CatIDFK = model.CatIDFK;
                        productOld.UserIDFK = model.UserIdFK;
                        var oldImage = _unit.BaseRepo<Product>().FindByIdAsync(productOld.ProductId).Result.Img;
                        var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\imgUpload\\Product", oldImage);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                            if (image != null && image.Length > 0)
                            {
                                var fileName = Path.GetFileName(image.FileName);
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Product");
                                string TempImage = filePath + "Original\\" + fileName;
                                using (var fileStream = new FileStream(TempImage, FileMode.Create))
                                {
                                    await image.CopyToAsync(fileStream);
                                }
                                Image_resize(TempImage, TempImage + fileName, 360, 203);
                                string destination = filePath + "_" + fileName;
                                System.IO.File.Move(TempImage, destination);
                                if (System.IO.File.Exists(destination))
                                    System.IO.File.Delete(destination);

                                productOld.Img = fileName;
                            }
                        }
                        else
                        {
                            if (image != null && image.Length > 0)
                            {
                                var fileName = Path.GetFileName(image.FileName);
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Product");
                                string TempImage = filePath + "Original\\" + fileName;
                                using (var fileStream = new FileStream(TempImage, FileMode.Create))
                                {
                                    await image.CopyToAsync(fileStream);
                                }
                                Image_resize(TempImage, TempImage + fileName, 360, 203);
                                string destination = filePath + "_" + fileName;
                                System.IO.File.Move(TempImage, destination);
                                if (System.IO.File.Exists(destination))
                                    System.IO.File.Delete(destination);

                                productOld.Img = fileName;
                            }
                        }
                    }
                    _unit.BaseRepo<Product>().Update(productOld);

                    var recentCarModel = (from a in _unit._context.RlCarModelProducts
                                          where (a.ProductId == model.ProductId)
                                          select a.CarId).ToArray();
                    if (model.CarIDFK == null)
                        model.CarIDFK = new int[] { };//Hack:Whats Happening here?!?!?! ← ← ←
                    var deletedCarModel = recentCarModel.Except(model.CarIDFK);
                    var addedCarModel = model.CarIDFK.Except(recentCarModel);
                    if (deletedCarModel.Count() != 0)
                        _unit.BaseRepo<RlCarModelProduct>().DeleteRange(deletedCarModel.Select(a => new RlCarModelProduct { CarId = a, ProductId = model.ProductId }).ToList());
                    await _unit.Commit();
                    ViewBag.MsgConfirm = "ذخیره تغییرات با موفقیت انجام شد";
                    return View(model);
                }
                catch
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

        [HttpGet, DisplayName("حذف قطعات"), Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _unit.BaseRepo<Product>().FindByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _unit.BaseRepo<Product>().FindByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                _unit.BaseRepo<Product>().Delete(product);
                await _unit.Commit();
                return RedirectToAction(nameof(Index));
            }
        }

        private void Image_resize(string inputImagePath, string outputImagePath, int newWidth,int newHeight)
        {
            const long quality = 50L;
            Bitmap source_Bitmap = new Bitmap(inputImagePath);
            double dblWidth_origial = source_Bitmap.Width;
            double dblHeigth_origial = source_Bitmap.Height;
            var new_DrawArea = new Bitmap(newWidth, newHeight);
            using (var graphic_of_DrawArea = Graphics.FromImage(new_DrawArea))
            {
                graphic_of_DrawArea.CompositingQuality = CompositingQuality.HighSpeed;
                graphic_of_DrawArea.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic_of_DrawArea.CompositingMode = CompositingMode.SourceCopy;
                graphic_of_DrawArea.DrawImage(source_Bitmap, 0, 0, newWidth, newHeight);
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