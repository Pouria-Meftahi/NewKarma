using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NewKarma.Models;
using NewKarma.Models.Domain;
using NewKarma.Models.View;
using NewKarma.Repository.UOW;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NewKarma.Controllers
{
    public class HomeController : Controller
    {
        public int CurrentPage { get; set; }
        public string Link { get; set; }
        public int Next { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }

        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unit;

        public HomeController(IUnitOfWork unit, AppDbContext context)
        {
            _context = context;
            _unit = unit;
        }

        public IActionResult Index(string id)
        {
            if (id != null)
            {
                ViewBag.ConfirmEmailAlert = "لینک فعال سازی حساب کاربری به ایمیل شما ارسال شد لطفا با کلیک روی این لینک حساب خود را فعال کنید.";
            }
            return View();
        }

        [ActionName("Products")]
        public async Task<IActionResult> Products(int page = 1, int row = 4)
        {
            var Products = _unit.BaseRepo<Product>().FindByConditionAsync(includes: a => a.Brand);
            var PagingModel = PagingList.Create(await Products, row, page);
            PagingModel.Action = "Products";
            PagingModel.RouteValue = new RouteValueDictionary
            {
                {"row",row }
            };
            return View(PagingModel ?? null);
        }
        [ActionName("ProductByCategory")]
        public async Task<IActionResult> ProductByCategory(int? catId, int page = 1, int row = 4)
        {
            var prodByCat = (_unit.BaseRepo<Product>().FindByConditionAsync(a => a.CatIDFK == catId, includes: b => b.Brand));
            var PaginfModel = PagingList.Create(await prodByCat, row, page);
            PaginfModel.Action = "ProductByCategory";
            PaginfModel.RouteValue = new RouteValueDictionary
            {
                {"row",row }
            };
            return View(PaginfModel ?? null);
        }

        public IActionResult ProductById(int? ProductId)
        {
            var prodById = _unit.BaseRepo<Product>().FindByConditionAsync(a => a.ProductId == ProductId, includes: b => b.Brand).Result.FirstOrDefault();
            var carId = _context.RlCarModelProducts.Where(a => a.ProductId == prodById.ProductId).Select(a=>a.CarId);
            ViewBag.cars = _context.Cars.Where(a => carId.Contains(a.CarId));
            ViewBag.Brand = _context.Brands.Where(b => b.BrandId == prodById.BrandIDFK).SingleOrDefault().Title;
            return View(prodById);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

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
