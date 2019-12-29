using NewKarma.Models;
using System.Threading.Tasks;

namespace NewKarma.Repository.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext _context { get; }
        private IProductRepo _ProductRepo;

        public IProductRepo ProductRepo 
        {
            get
            {
                if (_ProductRepo == null)
                {
                    _ProductRepo = new ProductRepo(_context);
                }
                return _ProductRepo;
            }
        }
        
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        
        public IBaseRepo<T> BaseRepo<T>() where T : class
        {
            IBaseRepo<T> repository = new BaseRepo<T, AppDbContext>(_context);
            return repository;
        }
        
        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}