using NewKarma.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Repository
{
    public interface IProductRepo
    {
        List<VmProduct> GetAllProduct(string title,string Car, string Brand, string Category);
        Task<List<VmProduct>> LatestProduct(int offset, int limit);
        Task<List<VmProduct>> GetProdByCategory(int catId,int offset);
        Task<List<VmProduct>> GetProdByBrand(int brandId, int offset);
        Task<List<VmProduct>> GetProdByCar(int carId,int offset);
    }
}
