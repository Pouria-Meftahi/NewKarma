using NewKarma.Models;
using NewKarma.Models.Domain;
using NewKarma.Models.View;
using NewKarma.Repository.UOW;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Repository
{
    public class CatRepo : ICatRepo
    {
        private readonly IUnitOfWork _unit;
        public CatRepo(IUnitOfWork unit)
        {
            _unit = unit;
        }
        //public async Task<bool> CreateCategory(VmCategory model)
        //{
        //    try
        //    {
        //        Category category = new Category()
        //        {
        //            Title = model.Title,
        //            Icon = model.Icon.ToString(),//Hack: Are u shore!?
        //            Description = model.Description,
        //            UserIDFK = model.UserIDFK,
        //        };
        //        if (model.Icon != null)
        //        {
        //            var filName = Path.GetFileName(model.Icon.FileName);
        //            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img\\imgUpload\\Category", filName);
        //            //Hack:Using FileStream Or MemoryStream
        //            using (var fileStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await model.Icon.CopyToAsync(fileStream);
        //            }
        //        }
        //        await _unit.BaseRepo<Category>().Create(category);
        //        await _unit.Commit();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        public async Task<Category> Create(VmCategory model)
        {
            Category prodCategory = new Category
            {
                CatId = model.CatID,
                Icon = model.Icon,
                Description = model.Description,
                Title =model.Title,
            };
            await _unit.BaseRepo<Category>().Create(prodCategory);
            await _unit.Commit();
            return prodCategory;
        }
        public List<VmCategory> GetAllArticleCat()
        {
            //var qArticle = _unit.ArticleCategories;
            var allCategory = _unit.BaseRepo<VmCategory>().FindAll();
            List<VmCategory> lst = new List<VmCategory>();
            foreach (var item in allCategory)
            {
                VmCategory vmCategory = new VmCategory
                {
                    CatID = item.CatID,
                    Title = item.Title,
                    Description = item.Description,
                    Icon = item.Icon
                };
                lst.Add(vmCategory);
            }
            return lst ?? null;
        }

        public async Task<Category> Remove(int? CatId)
        {
            var category = _unit.BaseRepo<Category>().FindByIdAsync(CatId);
            //await _unit.BaseRepo<Category>().Delete(category);
            await _unit.Commit();
            return null;
            //return await _unit.ArticleCategories.FirstOrDefaultAsync(a => a.CatId == CatId);
        }

        public async Task<Category> RemoveConfirm(int CatId)
        {
            //var remove = _unit.ArticleCategories.Include(a => a.Articles).Include(a => a.Language)
            //    .Include(a => a.ApplicationUser).FirstOrDefault(a => a.CatId == CatId);
            //_unit.ArticleCategories.Remove(remove);
            //await _unit.SaveChangesAsync();
            //return remove;
            return null;    
        }

        public async Task<Category> Update(VmCategory model, int CatId)
        {

            //var old = await _unit.BaseRepo<Category>().Update()
            //.Where(a => a.CatId == CatId).FirstOrDefaultAsync();
            //old.CatId = model.CatId;
            //old.ArticleCatName = model.ArticleCatName;
            //old.LangId_FK = model.LangId_FK;
            //old.UserId_FK = model.UserId_FK;
            //_unit.ArticleCategories.Update(old);
            //return old;
            return null;

        }

        public async Task<Category> Update(int? CatId)
        {
            //return await _unit.ArticleCategories.SingleOrDefaultAsync(a => a.CatId == CatId);
            return null;
        }
    }
}
