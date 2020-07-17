using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NewKarma.Models;
using NewKarma.Models.Domain;
using NewKarma.Models.View;
using NewKarma.Repository.UOW;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace NewKarma.Repository
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;

        public ProductRepo(AppDbContext context)
        {
            _context = context;
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
        public async Task<List<VmProduct>> GetProdByCar(int carId, int offset)
        {
            //var icarId = await _context.RlCarModelProducts.FindAsync(carId);
            var prodByCar = await _context.Products.Where(a => a.Situation == true && a.RlCarModelProduct.FirstOrDefault().CarId == carId)
                .Select(s => new VmProduct
                {
                    Title = s.Title,
                    Description = s.Description,
                    Image =s.Img,
                }).Skip(offset*3).Take(3).AsNoTracking().ToListAsync();
            return prodByCar;
        }

        public async Task<List<VmProduct>> GetProdByBrand(int offset, int brandId)
        {
            var prodByBrand = await _context.Products.Where(a => a.Situation == true && a.BrandIDFK == brandId)
                .Include(b => b.Brand)
                //Hack:Using ProjectTo or Mapper
                .Select(s => new VmProduct
                {
                    Title = s.Title,
                    Description = s.Description,
                    Image = s.Img,
                    BrandLogo = s.Brand.Logo,
                    BrandName = s.Brand.Title
                }).Skip(offset * 3).Take(3).AsNoTracking().ToListAsync();
            return prodByBrand;
        }


        public async Task<List<VmProduct>> GetProdByCategory(int catId, int offset)
        {
            var prodbyCategory = await _context.Products.Where(a => a.Situation == true && a.CatIDFK == catId)
                .Include(b => b.Category)
                .Select(s => new VmProduct
                {
                    Title = s.Title,
                    Description = s.Description,
                    Image = s.Img,
                }).Skip(offset * 3).Take(3).AsNoTracking().ToListAsync();
            return prodbyCategory;
        }

        public async Task<List<VmProduct>> LatestProduct(int offset, int limit)
        {
            List<VmProduct> ViewModelList = new List<VmProduct>();
            var Prod = await (from row in _context.Products
                              join b in _context.Brands on row.BrandIDFK equals b.BrandId into br
                              from brn in br.DefaultIfEmpty()

                              where (row.Situation == true)
                              select new
                              {
                                  row.ProductId,
                                  row.Title,
                                  row.Situation,
                                  row.CreatedDate,
                                  row.Description,
                                  row.Img,
                                  Brand = brn != null ? brn.Title : " ",
                                  BramdImage = brn != null ? brn.Logo : " ",
                              }).GroupBy(b => b.ProductId).Skip(offset * limit).Take(limit)
                        .Select(s => new { ProductId = s.Key, ProductGroup = s }).AsNoTracking().ToListAsync();
            foreach (var item in Prod)
            {
                VmProduct vm = new VmProduct()
                {
                    CreatedDate = item.ProductGroup.First().CreatedDate,
                    ProductId = item.ProductId,
                    BrandName = item.ProductGroup.First().Brand,
                    BrandLogo = item.ProductGroup.First().BramdImage,
                    Title = item.ProductGroup.First().Title,
                    Description = item.ProductGroup.First().Description,
                    Image = item.ProductGroup.First().Img
                };
                ViewModelList.Add(vm);
            }
            return ViewModelList;
        }
    }
}
