using HotelierMVC.Areas.admin.ViewModels;
using HotelierMVC.DAL;
using HotelierMVC.Models;
using HotelierMVC.Utilities.Enums;
using HotelierMVC.Utilities.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace HotelierMVC.Areas.admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly string roots=Path.Combine("assets","img");


        public EmployeeController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var employeeVM = await _context.Employees
                .Include(e => e.Major)
                .Where(e => !e.IsDeleted)
                .Select(e => new GetEmployeeVM
                {
                    Id = e.Id,
                    Name = e.Name,
                    Surname = e.Surname,
                    Image = e.Image,
                    MajorName = e.Major.Name
                }).ToListAsync();
            return View(employeeVM);
        }

        public async Task<IActionResult> Create()
        {
            CreateEmployeeVM employeeVM = new CreateEmployeeVM()
            {
                Majors = await _context.Majors.ToListAsync()
            };
            return View(employeeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize("Admin")]

        public async Task<IActionResult> Create(CreateEmployeeVM employeeVM)
        {
            if (!ModelState.IsValid)
            {
                return View(employeeVM);
            }

            employeeVM.Majors=await _context.Majors.ToListAsync();

            if (!employeeVM.Image.ValidateSize(FileSize.MB, 2))
            {
                ModelState.AddModelError("Image", "Image size must be less than 2mb");
                return View(employeeVM);
            }

            if (!employeeVM.Image.ValidateType("image/"))
            {
                ModelState.AddModelError("Image", "Image type is incorrect");
                return View(employeeVM);
            }

            bool result = await _context.Majors.AnyAsync(e => e.Id == employeeVM.MajorId);
            if (!result)
            {
                ModelState.AddModelError(nameof(CreateEmployeeVM.MajorId), "Major does not exists");
                return View(employeeVM);
            }

            string imagepath =await employeeVM.Image.CreateFile(_env.WebRootPath, roots);
            Employee employee = new Employee()
            {
                Name = employeeVM.Name,
                Surname = employeeVM.Surname,
                Image = imagepath,
                MajorId = employeeVM.MajorId,
                FbLink = employeeVM.FbLink,
                TwitterLink = employeeVM.TwitterLink,
                InstagramLink = employeeVM.InstagramLink,
            };
            await _context.AddAsync(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult>Update(int? id)
        {
            if (id < 1 || id == null) return BadRequest();
            Employee? employee=await _context.Employees.Include(e=>e.Major).FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null) return NotFound();

            UpdateEmployeeVM employeeVM = new UpdateEmployeeVM()
            {
                Id = employee.Id,
                Name = employee.Name,
                Surname = employee.Surname,
                ExistingImage = employee.Image,
                MajorId = employee.MajorId,
                Majors = await _context.Majors.ToListAsync(),
                FbLink = employee.FbLink,
                TwitterLink = employee.TwitterLink,
                InstagramLink = employee.InstagramLink
            };
            return View(employeeVM);
        }

        [HttpPost]
        [Authorize("Admin")]
        public async Task<IActionResult> Update(int? id,UpdateEmployeeVM employeeVM)
        {
            if (id < 1 || id == null) return BadRequest();
            Employee? existed = await _context.Employees.Include(e=>e.Major).FirstOrDefaultAsync(e => e.Id == id);
            if (existed == null) return NotFound();
            employeeVM.Majors = await _context.Majors.ToListAsync();

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if(employeeVM.Image!=null)
            {
                if (!employeeVM.Image.ValidateSize(FileSize.MB, 2))
                {
                    ModelState.AddModelError("Image", "Image size must be less than 2mb");
                    return View(employeeVM);
                }

                if (!employeeVM.Image.ValidateType("image/"))
                {
                    ModelState.AddModelError("Image", "Image type is incorrect");
                    return View(employeeVM);
                }

                existed.Image.DeleteFile(_env.WebRootPath, roots);
                existed.Image=await employeeVM.Image.CreateFile(_env.WebRootPath,roots);
            }

            else
            {
             
                    existed.Image = employeeVM.ExistingImage;
                
            }
            if(existed.Id!=employeeVM.MajorId)
            {
                bool result=await _context.Majors.AnyAsync(c=>c.Id==employeeVM.MajorId);
                if(!result)
                {

                    ModelState.AddModelError(nameof(CreateEmployeeVM.MajorId), "Major does not exists");
                    return View(employeeVM);
                }
            }

            existed.Name = employeeVM.Name;
            existed.Surname = employeeVM.Surname;
            existed.MajorId = employeeVM.MajorId;
            existed.FbLink = employeeVM.FbLink;
            existed.InstagramLink = employeeVM.InstagramLink;
            existed.TwitterLink = employeeVM.TwitterLink;

            _context.Employees.Update(existed);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1 || id == null) return BadRequest();
            Employee? employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null) return NotFound();
            if(!string.IsNullOrEmpty(employee.Image))
            {
                employee.Image.DeleteFile(_env.WebRootPath,roots);
            }
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
