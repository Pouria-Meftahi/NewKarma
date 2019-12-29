using NewKarma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Repository.UOW
{
    public interface IUnitOfWork
    {
        AppDbContext _context { get; }
        IProductRepo ProductRepo { get; }
        IBaseRepo<T> BaseRepo<T>() where T : class;
        Task Commit();
    }
}
