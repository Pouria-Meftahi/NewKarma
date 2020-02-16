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
        private readonly IHostingEnvironment _env;
        public CategoryController(IUnitOfWork unit,IHostingEnvironment env)
        {
            _unit = unit;
            _env = env;
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
                    //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Category",fileName);
                    //using (var fileStream = new FileStream(filePath, FileMode.Create))
                    //{
                    //    await image.CopyToAsync(fileStream);
                    //}

                    //< init >
                        //string sImage_Folder = "User_Images";
                        //string sTarget_Filename = "User_Image_" + IDUser + ".jpg";
                    //</ init >
                    //< get Path >
                    string sPath_WebRoot = _env.WebRootPath;
                    string sPath_of_Target_Folder = sPath_WebRoot + "\\img\\imgUpload\\Category";
                    string sFile_Target_Original = sPath_of_Target_Folder + "\\Original\\" + fileName;
                    //string sImage_Filename_Original = sPath_of_Target_Folder + uploaded_File.FileName;
                    //</ get Path >
                    //< Copy File to Target >
                    using (var stream = new FileStream(sFile_Target_Original, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    //
                    Image_resize(sFile_Target_Original, sPath_of_Target_Folder + fileName, 50);
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
                //Todo:Delete Image Of This Category From DataBase Of File Server
                _unit.BaseRepo<Category>().Delete(category);
                await _unit.Commit();
                return RedirectToAction(nameof(Index));
            }
        }




        private void Image_resize(string inputImagePath, string outputImagePath, int newWidth)

        {

            //---------------< Image_resize() >---------------

            //*Resizes an Image in Asp.Net MVC Core 2

            //*Using Nuget CoreCompat.System.Drawing

            //using System.IO

            //using System.Drawing;             //CoreCompat

            //using System.Drawing.Drawing2D;   //CoreCompat

            //using System.Drawing.Imaging;     //CoreCompat



            const long quality = 50L;

            Bitmap source_Bitmap = new Bitmap(inputImagePath);



            double dblWidth_origial = source_Bitmap.Width;

            double dblHeigth_origial = source_Bitmap.Height;

            double relation_heigth_width = dblHeigth_origial / dblWidth_origial;

            int new_Height = (int)(newWidth * relation_heigth_width);



            //< create Empty Drawarea >

            var new_DrawArea = new Bitmap(newWidth, new_Height);

            //</ create Empty Drawarea >



            using (var graphic_of_DrawArea = Graphics.FromImage(new_DrawArea))

            {

                //< setup >

                graphic_of_DrawArea.CompositingQuality = CompositingQuality.HighSpeed;

                graphic_of_DrawArea.InterpolationMode = InterpolationMode.HighQualityBicubic;

                graphic_of_DrawArea.CompositingMode = CompositingMode.SourceCopy;

                //</ setup >



                //< draw into placeholder >

                //*imports the image into the drawarea

                graphic_of_DrawArea.DrawImage(source_Bitmap, 0, 0, newWidth, new_Height);

                //</ draw into placeholder >



                //--< Output as .Jpg >--

                using (var output = System.IO.File.Open(outputImagePath, FileMode.Create))

                {

                    //< setup jpg >

                    var qualityParamId = Encoder.Quality;

                    var encoderParameters = new EncoderParameters(1);

                    encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);

                    //</ setup jpg >



                    //< save Bitmap as Jpg >

                    var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == ImageFormat.Jpeg.Guid);

                    new_DrawArea.Save(output, codec, encoderParameters);

                    //resized_Bitmap.Dispose();

                    output.Close();

                    //</ save Bitmap as Jpg >

                }

                //--</ Output as .Jpg >--

                graphic_of_DrawArea.Dispose();

            }

            source_Bitmap.Dispose();

            //---------------</ Image_resize() >---------------

        }
    }
}