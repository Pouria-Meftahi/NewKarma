using NewKarma.Models.Domain;
using NewKarma.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Repository
{
    public interface ICatRepo
    {
        //Task<bool> CreateCategory(VmCategory model);
        Task<Category> Create(VmCategory model);
        Task<Category> Update(VmCategory model, int CatId);
        Task<Category> Update(int? CatId);
        Task<Category> Remove(int? CatId);
        Task<Category> RemoveConfirm(int CatId);
        List<VmCategory> GetAllArticleCat();
    }
}
