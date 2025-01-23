
using HotelierMVC.DAL;
using HotelierMVC.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HotelierMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Employees = await _context.Employees.OrderByDescending(x => x.Name).ToListAsync()
            };
            return View(homeVM);
        }
    }
}
