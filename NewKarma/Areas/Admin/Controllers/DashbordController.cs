using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewKarma.Areas.Identity.Data;
using NewKarma.Models;
using NewKarma.Models.Domain;
using NewKarma.Repository.UOW;

namespace NewKarma.Areas.Admin.Controllers
{
    [Area("Admin"), DisplayName("پنل مدیریت ")]
    public class DashbordController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UnitOfWork _unit;
        public DashbordController(AppDbContext context,UnitOfWork unit)
        {
            _context = context;
            _unit = unit;
        }
        [DisplayName("داشبرد"), Authorize(Policy = ConstantPolicies.DynamicPermission)]
        public IActionResult Index()
        {
            var catitem = _unit.BaseRepo<Category>().FindAll();
            var branditem = _unit.BaseRepo<Brand>().FindAll();
            var proditem = _unit.BaseRepo<Product>().FindAll();
            ViewData["CatCount"] = catitem.Count();
            ViewData["BrandCount"] = branditem.Count().ToString("n",CultureInfo.GetCultureInfo("fa-IR"));
            ViewData["ProductCount"] = proditem.Count();
            ViewData["LastBrandAdded"]= branditem.OrderByDescending(a=>a.BrandId).Take(1).FirstOrDefault().Title;
            Chart lineChart = GenerateLineChart();
            Chart polarChart = GeneratePolarChart();
            Chart pieChart = GeneratePieChart();
            ViewData["LineChart"] = lineChart;
            ViewData["PolarChart"] = polarChart;
            ViewData["PieChart"] = pieChart;
            return View();
        }
        private Chart GeneratePolarChart()
        {
            Chart chart = new Chart()
            {
                Type = Enums.ChartType.PolarArea
            };
            Data data = new Data()
            {
                Labels = _context.Cars.Select(a => a.CarTitle + " " + (!string.IsNullOrEmpty(a.CarModel) ? a.CarModel : string.Empty)).ToList()
            };
            PolarDataset dataset = new PolarDataset()
            {
                Label = "My dataset",
                BackgroundColor = new List<ChartColor>() {
                    ChartColor.FromHexString("#FF6384"),
                    ChartColor.FromHexString("#4BC0C0"),
                    ChartColor.FromHexString("#FFCE56"),
                    ChartColor.FromHexString("#E7E9ED"),
                    ChartColor.FromHexString("#36A2EB")
                },
                Data = _context.Cars.Select(a => (double)a.RlCarModelProduct.Count()).ToList()
            };
            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            return chart;
        }
        private Chart GeneratePieChart()
        {
            Chart chart = new Chart()
            {
                Type = Enums.ChartType.Pie
            };
            Data data = new Data()
            {
                Labels = _context.Brands.Select(a => a.Title).ToList()
            };

            PieDataset dataset = new PieDataset()
            {

                Label = "My dataset",
                BackgroundColor = new List<ChartColor>()
                {
                    ChartColor.FromHexString("#FF6384"),
                    ChartColor.FromHexString("#36A2EB"),
                    ChartColor.FromHexString("#FFCE56"),
                    ChartColor.CreateRandomChartColor(true)
                },
                HoverBackgroundColor = new List<ChartColor>() {
                    ChartColor.FromHexString("#FF6384"),
                    ChartColor.FromHexString("#36A2EB"),
                    ChartColor.FromHexString("#FFCE56"),
                    ChartColor.CreateRandomChartColor(true)
                },
                Data = _context.Brands.Include(s => s.Products).Select(a => (double)a.Products.Count()).ToList() 
            };
            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            chart.Data = data;

            return chart;
        }
        private Chart GenerateLineChart()
        {
            Chart chart = new Chart
            {
                Type = Enums.ChartType.Line
            };
            Data data = new Data
            {
                Labels = _context.Categories.Select(a => a.Title).ToList()
            };
            List<double> dataList = _context.Categories.Include(c => c.Products).Select(a => (double)a.Products.Count()).ToList();
            LineDataset dataset = new LineDataset()
            {
                Label = "تعداد قطعات موجود",
                Data = dataList,
                Fill = "false",
                LineTension = 0.1,
                BackgroundColor = ChartColor.FromRgba(75, 192, 192, 0.4),
                BorderColor = ChartColor.FromRgb(75, 192, 192),
                BorderCapStyle = "butt",
                BorderDash = new List<int> { },
                BorderDashOffset = 0.0,
                BorderJoinStyle = "miter",
                PointBorderColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
                PointBackgroundColor = new List<ChartColor> { ChartColor.FromHexString("#ffffff") },
                PointBorderWidth = new List<int> { 1 },
                PointHoverRadius = new List<int> { 5 },
                PointHoverBackgroundColor = new List<ChartColor> { ChartColor.FromRgb(75, 192, 192) },
                PointHoverBorderColor = new List<ChartColor> { ChartColor.FromRgb(220, 220, 220) },
                PointHoverBorderWidth = new List<int> { 2 },
                PointRadius = new List<int> { 1 },
                PointHitRadius = new List<int> { 10 },
                SpanGaps = false
            };

            data.Datasets = new List<Dataset>();
            data.Datasets.Add(dataset);

            Options options = new Options()
            {
                Scales = new Scales()
            };

            Scales scales = new Scales()
            {
                YAxes = new List<Scale>()
                {
                    new CartesianScale()
                }
            };

            CartesianScale yAxes = new CartesianScale()
            {
                Ticks = new Tick()
            };

            Tick tick = new Tick()
            {
                Callback = "function(value, index, values) {return '$' + value;}"
            };

            yAxes.Ticks = tick;
            scales.YAxes = new List<Scale>() { yAxes };
            options.Scales = scales;
            chart.Options = options;

            chart.Data = data;

            return chart;
        }
    }
}