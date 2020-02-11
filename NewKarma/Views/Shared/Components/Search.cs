using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using NewKarma.Models.Domain;
using NewKarma.Repository.UOW;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewKarma.Views.Shared.Components
{
    public class Search : ViewComponent
    {
        private readonly IUnitOfWork _unit;
        public Search(IUnitOfWork unit)
        {
            _unit = unit;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(string title = "", int row = 4, int page = 1)
        {
            var Products = _unit.BaseRepo<Product>().FindByConditionAsync(filter: s => s.Title.Contains(title.TrimStart().TrimEnd()), includes: a => a.Brand);
            var PagingModel = PagingList.Create(await Products, row, page);
            PagingModel.Action = "Products";
            PagingModel.RouteValue = new RouteValueDictionary
            {
                {"row",row },
                {"title",title }
            };

            ViewBag.Search = title;
            if (Products.Result.Count() == 0)
            {
                ViewBag.Message = "نتیجه ای برای جستجوی شما پیدا نشد";
            }
            return await Task.FromResult(View(PagingModel ?? null));
        }
    }
}
