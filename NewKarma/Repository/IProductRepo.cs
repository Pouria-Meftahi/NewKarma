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
    }
}
