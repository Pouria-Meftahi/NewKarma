using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NewKarma.Models;
using NewKarma.Models.Domain;
using NewKarma.Repository.UOW;
using NewKarma.Views.Shared.Components;
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
        public async Task<IActionResult> Products(int page = 1, int row = 6, string title = "")
        {
            var Products = _unit.BaseRepo<Product>().FindByConditionAsync(filter: s => s.Situation==true && s.Title.Contains(title.TrimStart().TrimEnd()), includes: a => a.Brand);
            var PagingModel = PagingList.Create(await Products, row, page);
            PagingModel.Action = "Products";
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
            return View(PagingModel ?? null);
        }

        public async Task<IActionResult> ProductByCategory(int? catId, int page = 1, int row = 4, string title = "")
        {
            var prodByCat = _unit.BaseRepo<Product>().FindByConditionAsync(filter: s => s.Situation == true && s.Title.Contains(title.TrimStart().TrimEnd()) && s.CatIDFK == catId, includes: b => b.Brand);
            var PaginfModel = PagingList.Create(await prodByCat, row, page);
            PaginfModel.Action = "ProductByCategory";
            PaginfModel.RouteValue = new RouteValueDictionary
            {
                {"row",row },
                {"title",title }
            };
            ViewData["Search"] = title;
            if (prodByCat.Result.Count() == 0)
            {
                ViewBag.Message = "نتیجه ای برای جستجوی شما پیدا نشد";
            }
            return View(PaginfModel ?? null);
            //return title != "" ? ViewComponent(typeof(Search)) : View(PagingModel ?? null);

        }
        
        public async Task<IActionResult> ProductByBrand(int? brandId, int page = 1, int row = 4,string title="")
        {
            var productByBrand = _unit.BaseRepo<Product>().FindByConditionAsync(filter: s => s.Situation == true && s.Title.Contains(title.TrimStart().TrimEnd())&&s.BrandIDFK == brandId, includes: b => b.Category);
            var PaginModel = PagingList.Create(await productByBrand, row, page);
            PaginModel.Action = "ProductByBrand";
            PaginModel.RouteValue = new RouteValueDictionary
            {
                {"row",row },
                {"title",title }
            };
            ViewData["Search"] = title;
            if (productByBrand.Result.Count() == 0)
            {
                ViewBag.Message = "نتیجه ای برای جستجوی شما پیدا نشد";
            }
            return View(PaginModel ?? null);
            //return title != "" ? ViewComponent(typeof(Search)) : View(PagingModel ?? null);
        }
        
        public async Task<IActionResult> ProductByCar (int? carId,int row = 4,int page = 1,string title = "")
        {
            var cars = _unit.BaseRepo<RlCarModelProduct>().FindAllAsync().Result.Select(a=>a.CarId).ToList();
            
            var productByCar = _unit.BaseRepo<Product>().FindByConditionAsync(filter: s => s.Situation == true && s.Title.Contains(title.TrimStart().TrimEnd()) && s.RlCarModelProduct.Select(a=> a.CarId).First() == carId, includes: b => b.Category);
            var PaginModel = PagingList.Create(await productByCar, row, page);
            PaginModel.Action = "ProductByCar";
            PaginModel.RouteValue = new RouteValueDictionary
            {
                {"row",row },
                {"title",title }
            };
            ViewData["Search"] = title;
            if (productByCar.Result.Count() == 0)
            {
                ViewBag.Message = "نتیجه ای برای جستجوی شما پیدا نشد";
            }
            return View(PaginModel ?? null);
        }
        
        public IActionResult ProductById(int? ProductId)
        {
            var prodById = _unit.BaseRepo<Product>().FindByConditionAsync(a => a.ProductId == ProductId && a.Situation ==true, includes: b => b.Brand).Result.FirstOrDefault();
            var carId = _context.RlCarModelProducts.Where(a => a.ProductId == prodById.ProductId).Select(a => a.CarId);
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
