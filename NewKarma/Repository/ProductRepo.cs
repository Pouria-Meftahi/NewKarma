using Microsoft.EntityFrameworkCore;
using NewKarma.Models;
using NewKarma.Models.View;
using NewKarma.Repository.UOW;
using System.Collections.Generic;
using System.Linq;

namespace NewKarma.Repository
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;
        public ProductRepo(AppDbContext context)
        {
            _context = context;
        }

        private readonly IUnitOfWork _unit;
        public ProductRepo(IUnitOfWork unit)
        {
            _unit = unit;
        }
        public List<VmProduct> GetAllProduct(string title, string Car, string Brand, string Category)
        {
            string CategoryName = "";
            string BrandName = "";
            string CarName = "";
            List<VmProduct> ViewModelList = new List<VmProduct>();
            var Prod = (from row in _context.RlCarModelProducts.Include(a => a.Product).Include(x => x.Car)

                        join c in _context.Categories on row.Product.CatIDFK equals c.CatId into cg
                        from cog in cg.DefaultIfEmpty()

                        join b in _context.Brands on row.Product.BrandIDFK equals b.BrandId into br
                        from brn in br.DefaultIfEmpty()

                        where (row.Product.Title.Contains(title.TrimStart().TrimEnd()))
                        select new
                        {
                            row.Product.ProductId,
                            row.Product.Title,
                            row.Product.Situation,
                            row.Product.CreatedDate,

                            row.Product.Description,
                            Car = row.Car.CarTitle + " " + row.Car.CarModel,
                            Category = cog != null ? cog.Title : " ",
                            Brand = brn != null ? brn.Title : " ",
                        }).Where(a => a.Car.Contains(Car.TrimStart().TrimEnd())
                        && a.Category.Contains(Category.TrimStart().TrimEnd())
                        && a.Brand.Contains(Brand.TrimStart().TrimEnd())
                        ).GroupBy(b => b.ProductId).Select(s => new { ProductId = s.Key, ProductGroup = s }).ToList();
            foreach (var item in Prod)
            {
                CategoryName = "";
                BrandName = "";
                CarName = "";
                foreach (var a in item.ProductGroup.Select(a => a.Car).Distinct())
                {
                    if (CarName == "")
                        CarName = a;
                    else
                        CarName = CarName + " - " + a;
                }
                foreach (var a in item.ProductGroup.Select(a => a.Category).Distinct())
                {
                    if (CategoryName == "")
                        CategoryName = a;
                    else
                        CategoryName = CategoryName + " - " + a;
                }
                foreach (var a in item.ProductGroup.Select(a => a.Brand).Distinct())
                {
                    if (BrandName == "")
                        BrandName = a;
                    else
                        BrandName = BrandName + " - " + a;
                }
                VmProduct vm = new VmProduct()
                {
                    CreatedDate = item.ProductGroup.First().CreatedDate,
                    CarName = CarName,
                    ProductId = item.ProductId,
                    CategoryName = CategoryName,
                    BrandName = BrandName,
                    Title = item.ProductGroup.First().Title,
                    Situation = item.ProductGroup.First().Situation,
                };
                ViewModelList.Add(vm);
            }
            return ViewModelList;
        }
    }
}
