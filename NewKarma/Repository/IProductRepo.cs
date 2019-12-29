using NewKarma.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Repository
{
    public interface IProductRepo
    {
        Task<bool> CreateProductAsync(VmProduct model);
        //List<VmProduct> GetAllProduct(string title, string ISBN, string Language, string Publisher, string Author, string Translator, string Category);
        List<VmProductAdmin> GetAllProduct(string title,string Car, string Brand, string Category);
    }
}
