using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewKarma.Areas.Identity.Data;
using NewKarma.Models;

namespace NewKarma.Areas.Admin.Controllers
{
    [Area("Admin"),DisplayName("پنل مدیریت ")]
    public class DashbordController : Controller
    {
        private readonly AppDbContext _context;
        public DashbordController(AppDbContext context)
        {
            _context = context;
        }
        [DisplayName("داشبرد"), Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public IActionResult Index()
        {
            return View();
        }
    }
}