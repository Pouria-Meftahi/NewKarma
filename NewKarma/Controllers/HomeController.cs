using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using NewKarma.Models;
using NewKarma.Models.Domain;
using NewKarma.Models.View;
using NewKarma.Repository.UOW;
using ReflectionIT.Mvc.Paging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unit;
        private string _title = "";
        public HomeController(IUnitOfWork unit, AppDbContext context)
        {
            _context = context;
            _unit = unit;
        }
        public async Task<IActionResult> Index(int page = 1, int row = 5, string title = "")
        {
            if (title == string.Empty)
            {
                return View();
            }
            else
            {
                var Products = _unit.BaseRepo<Product>().FindByConditionAsync(
                    filter: s => s.Situation == true
                    && s.Title.Contains(title.TrimStart().TrimEnd())
                    || s.Category.Title.Contains(title.TrimStart().TrimEnd())
                    || s.Brand.Title.Contains(title.TrimStart().TrimEnd())
                    || s.RlCarModelProduct.FirstOrDefault().Car.CarModel.Contains(title.TrimStart().TrimEnd())
                    || s.RlCarModelProduct.FirstOrDefault().Car.CarTitle.Contains(title.TrimStart().TrimEnd()), includes: a => a.Brand);
                var PagingModel = PagingList.Create(await Products, row, page);
                PagingModel.Action = "Index";
                PagingModel.RouteValue = new RouteValueDictionary
                {
                    {"row",row },
                    {"title",title }
                };
                ViewData["Search"] = title;
                if (Products.Result.Count() == 0)
                {
                    ViewBag.Message = "نتیجه ای برای جستجوی شما پیدا نشد";
                }
                return PartialView("_SearchResult", PagingModel);
            }
        }

        public async Task<ActionResult> GetLatestProd(int Offset, int LimitInList)
        {
            return Json(await _unit.ProductRepo.LatestProduct(Offset, LimitInList));

        }
   
        #region Category
        public async Task<IActionResult> GetProductCategory(int catId, int offset) =>
        Json(await _unit.ProductRepo.GetProdByCategory(catId, offset));
        public async Task<ActionResult> ProductByCategory(int catId)
        {
            VmCategory category = _context.Categories.Where(a => a.CatId == catId)
                .Select(s => new VmCategory {CatID = s.CatId, Title = s.Title, Description = s.Description }).FirstOrDefault();
            return View(category);
        }
        #endregion

        #region Brand
        public async Task<ActionResult> GetProductBrand(int offset, int brandId) =>
        Json(await _unit.ProductRepo.GetProdByBrand(offset, brandId));
        public async Task<IActionResult> ProductByBrand(int brandId)
        {
            VmBrand vmBrand = _context.Brands.Where(a => a.BrandId == brandId)
            .Select(a => new VmBrand {brandId = a.BrandId, Title = a.Title, Description = a.Description, Logo = a.Logo }).FirstOrDefault();
            return View(vmBrand);
        }
        #endregion

        #region Car
        public async Task<IActionResult> GetProductCar(int carId, int offset) =>
        Json(await _unit.ProductRepo.GetProdByCar(carId, offset));

        public async Task<ActionResult> ProductByCar(int carId)
        {
            VmCar vmCar = _context.Cars.Where(a => a.CarId == carId)
                .Select(s => new VmCar {CarId = s.CarId, CarTitle = s.CarTitle, CarModel = s.CarModel }).AsNoTracking().FirstOrDefault();
            return View(vmCar);
            //var cars = _unit.BaseRepo<RlCarModelProduct>().FindAllAsync().Result.Select(a => a.CarId).ToList();
            //var productByCar = _unit.BaseRepo<Product>().FindByConditionAsync(filter: s => s.Situation == true && s.Title.Contains(title.TrimStart().TrimEnd()) && s.RlCarModelProduct.Select(a => a.CarId).First() == carId, includes: b => b.Category);
            //var PaginModel = PagingList.Create(await productByCar, row, page);
            //PaginModel.Action = "ProductByCar";
            //PaginModel.RouteValue = new RouteValueDictionary
            //{
            //    {"row",row },
            //    {"title",title }
            //};
            //ViewData["Search"] = title;
            //if (productByCar.Result.Count() == 0)
            //{
            //    ViewBag.Message = "نتیجه ای برای جستجوی شما پیدا نشد";
            //}
            //return View(PaginModel ?? null);
        }
        #endregion


        //public IActionResult ProductById(int? ProductId)
        //{
        //    var prodById = _unit.BaseRepo<Product>().FindByConditionAsync(a => a.ProductId == ProductId && a.Situation == true, includes: b => b.Brand).Result.FirstOrDefault();
        //    var carId = _context.RlCarModelProducts.Where(a => a.ProductId == prodById.ProductId).Select(a => a.CarId);
        //    ViewBag.cars = _context.Cars.Where(a => carId.Contains(a.CarId));
        //    ViewBag.Brand = _context.Brands.Where(b => b.BrandId == prodById.BrandIDFK).SingleOrDefault().Title;
        //    return View(prodById);
        //}

        //public IActionResult About()
        //{
        //    ViewData["Message"] = "Your application description page.";

        //    return View();
        //}

        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";

        //    return View();
        //}


        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        public IActionResult NullProduct()
        {
            return View();
        }
        public IActionResult NullProductByCat()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
