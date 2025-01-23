using HotelierMVC.Areas.admin.ViewModels;
using HotelierMVC.DAL;
using HotelierMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelierMVC.Areas.admin.Controllers
{
    [Area("Admin")]
    public class MajorController : Controller
    {
        private readonly AppDbContext _context;

        public MajorController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var majorVM = await _context.Majors
                .Where(m => !m.IsDeleted)
                .Include(m => m.Employees)
                .Select(m => new GetMajorVM
                {
                    Id = m.Id,
                    Name = m.Name,
                    EmployeeCount=m.Employees.Count()
                }).ToListAsync();
            return View(majorVM);
        }
        public async Task<IActionResult> Create()
        {
            CreateMajorVM majorVM = new CreateMajorVM();
            return View(majorVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
      [Authorize("Admin")]
        public async Task<IActionResult> Create(CreateMajorVM majorVM)
        {
            if (!ModelState.IsValid)
            {
                return View(majorVM);
            }

            bool result = await _context.Majors.AnyAsync(m => m.Name.Trim() == majorVM.Name.Trim());
            if (result)
            {
                ModelState.AddModelError("Name", "Name already exists");
                return View(majorVM);
            }

            Major major = new Major()
            {
                Name = majorVM.Name
            };
            await _context.Majors.AddAsync(major);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id < 1 || id == null) return BadRequest();
            Major? major=await _context.Majors.FirstOrDefaultAsync(m => m.Id == id);
            if (major == null) return NotFound();
            UpdateMajorVM updateMajorVM = new UpdateMajorVM()
            {
                Id = major.Id,
                Name = major.Name,
            };
            return View(updateMajorVM);
        }

        [HttpPost]
      [Authorize("Admin")]

        public async Task<IActionResult> Update(int? id,UpdateMajorVM majorVM)
        {
            if (id < 1 || id == null) return BadRequest();
            Major? existed = await _context.Majors.FirstOrDefaultAsync(m => m.Id == id);
            if (existed == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            bool result = await _context.Majors.AnyAsync(m => m.Name.Trim() == majorVM.Name.Trim()&& m.Id!=majorVM.Id);
            if (result)
            {
                ModelState.AddModelError("Name", "Name already exists");
                return View(majorVM);
            }

            existed.Name = majorVM.Name;
            existed.Id = majorVM.Id;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        public async Task<IActionResult> Delete(int id)
        {

            if (id < 1 || id == null) return BadRequest();
            Major? major = await _context.Majors.FirstOrDefaultAsync(m => m.Id == id);
            if (major == null) return NotFound();

            major.IsDeleted=true;

            _context.Majors.Remove(major);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }






    }
}
