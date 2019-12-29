using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewKarma.Models;
using NewKarma.Models.Domain;
using NewKarma.Models.View;
using NewKarma.Repository.UOW;

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


      


        public async Task<bool> CreateProductAsync(VmProduct model)
        {
            try
            {
                //Product product = new Product()
                //{
                //    ProductId = model.ProductId,
                //    BrandIDFK = model.BrandIDFK,
                //    CatIDFK = model.CatIDFK,
                //    UserIDFK = model.UserIdFK,
                //    Title = model.Title,
                //    Description = model.Description,
                //    CreatedDate = model.CreatedDate,
                //    Brand = model.Brand,
                //    Category = model.Category,
                //    ApplicationUser = model.ApplicationUser,
                //    Situation = true,
                //    Img = model.Image.ToString(),//Hack: Are u shore!?
                //};
                //if (model.Image != null)
                //{
                //    var fileName = Path.GetFileName(model.Image.FileName);
                //    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Product", fileName);
                //    //Hack:Using FileStream Or MemoryStream
                //    using (var fileStream = new FileStream(filePath, FileMode.Create))
                //    {
                //        await model.Image.CopyToAsync(fileStream);
                //    }
                //}
                //await _unit.BaseRepo<Product>().Create(product);
                //await _unit.Commit();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<VmProductAdmin> GetAllProduct(string title, string Car, string Brand, string Category)
        {
            string CategoryName = "";
            string BrandName = "";
            string CarName = "";
            List<VmProductAdmin> ViewModelList = new List<VmProductAdmin>();
            //ThenInclude(z => z.Brand).
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
                        ).GroupBy(b => b.ProductId).Select(s => new { ProductId = s.Key, ProductGroup = s }).ToList(); ;
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
                VmProductAdmin vm = new VmProductAdmin()
                {
                    Car = CarName,
                    ProductId = item.ProductId,
                    Category = CategoryName,
                    Brand = BrandName,
                    Title = item.ProductGroup.First().Title,
                    IsPublish = item.ProductGroup.First().Situation,
                    
                };
                ViewModelList.Add(vm);
            }
            return ViewModelList;
        }
    }
}
