using HotelierMVC.Models;
using HotelierMVC.Utilities.Enums;
using HotelierMVC.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelierMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM userVM)
        {
            if(!ModelState.IsValid)
            {
                return View(userVM);
            }


            AppUser user = new AppUser()
            {
                Name = userVM.Name,
                Email = userVM.Email,
                Surname = userVM.Surname,
                UserName = userVM.UserName
            };
            IdentityResult result=await _userManager.CreateAsync(user,userVM.Password);
            if(!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    return View(userVM);
                }
            }

            await _userManager.AddToRoleAsync(user,UserRole.Member.ToString());
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM userVM,string? returnUrl)
        {
            if(!ModelState.IsValid)
            {
                return View(userVM);
            }

            AppUser user =await _userManager.Users.FirstOrDefaultAsync(u=>u.UserName==userVM.UserNameOrEmail||u.Email==userVM.UserNameOrEmail);
            if(user==null)
            {
                ModelState.AddModelError(string.Empty, "Your email,username or password is incorrect");
                return View(userVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user, userVM.Password, userVM.IsPersistent, false);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your account is locked please try later");
                return View(userVM);
            }
            if(!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Your email,username or password is incorrect");
                return View(userVM);
            }
            if(returnUrl==null)
            {
                return RedirectToAction("Index", "Home");
            }

            return Redirect(returnUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //public async Task<IActionResult> CreateRoles()
        //{
        //    foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
        //    {
        //        if(!await _roleManager.RoleExistsAsync(role.ToString()))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name= role.ToString() }); 
        //        }
        //    }

        //    return RedirectToAction(nameof(HomeController.Index), "Home");  
        //}
    }
}
