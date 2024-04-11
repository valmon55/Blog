using ASP.Blog.BLL.ViewModels;
using ASP.Blog.DAL.Entities;
using ASP.Blog.DAL.UoW;
using ASP.Blog.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.BLL.Controllers
{
    public class UserController : Controller
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(UserManager<User> userManager,
                SignInManager<User> signInManager, 
                IUnitOfWork unitOfWork, IMapper mapper, RoleManager<UserRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            //return View("Register");
            return View(new RegisterViewModel());
        }

        [Route("Register")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var user = _mapper.Map<User>(model);

                var result = await _userManager.CreateAsync(user, model.PasswordReg);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    
                    //var userRole = new UserRole() { Name = "User", Description = "Пользователь" };
                    //var userRole = new UserRole() { Name = "Admin", Description = "Администратор" };
                    var userRole = new UserRole() { Name = "Moderator", Description = "Модератор" };

                    if (_roleManager.GetRoleNameAsync(userRole).Result != userRole.Name)
                    {
                        await _roleManager.CreateAsync(userRole);
                    }

                    var currentUser = await _userManager.FindByIdAsync(Convert.ToString(user.Id));

                    ///добавляет в таблицу [AspNetUserRoles] соответствие между ролью и пользователем
                    await _userManager.AddToRoleAsync(currentUser, userRole.Name);

                    await _signInManager.RefreshSignInAsync(currentUser);

                    return RedirectToAction("Index","Home");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    
                }
            }
            return View(model);
            //return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [Route("Test")]
        [HttpGet]
        public async Task<IActionResult> Test()
        {
            return View();
            //return RedirectToAction("Test");
        }

        [Route("Login")]
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var user = _mapper.Map<User>(model);
                //User signedUser = _userManager.FindByEmailAsync(user.Email).Result;
                User signedUser = _userManager.Users.Include(x => x.userRole).FirstOrDefault(u => u.Email == model.Email);
                var userRole = _userManager.GetRolesAsync(signedUser).Result.FirstOrDefault();
                if (signedUser is null)
                    ModelState.AddModelError("", "Неверный логин!");
                //В модели не хранится пароль -> Нужно сравнивать в хешированным model.Password
                //if(signedUser.PasswordHash != 
                //    _userManager.PasswordHasher.HashPassword(signedUser, model.Password))
                //    ModelState.AddModelError("", "Неверный пароль!");

                if (signedUser != null)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                        //new Claim(ClaimsIdentity.DefaultRoleClaimType, user.userRole.Name)
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole)
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                        claims,
                        "AppCookie",
                        ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType
                        );

                    await _signInManager.SignInWithClaimsAsync(signedUser, isPersistent:false, claims);

                    //await _signInManager.Context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    //    new ClaimsPrincipal(claimsIdentity)
                    //    );
                    //var result = await _signInManager.PasswordSignInAsync(signedUser.UserName, model.Password, false, false);
                    //if (result.Succeeded)
                    //{
                    //    return RedirectToAction("Index", "Home");
                    //}
                    //else
                    //{
                    //    ModelState.AddModelError("", "Неверный логин или пароль");
                    //}
                }
                else
                {
                    ModelState.AddModelError("", $"Логин {user.Email} не найден");
                }
            }
            return RedirectToAction("Index", "Home");
        }
        [Route("Logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [Authorize(Roles = "Admin")]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(UserEditViewModel model)
        { 
            if (ModelState.IsValid) 
            { 
            
            } 
            return View("Update", model);
        }
        [Authorize(Roles = "Admin")]
        [Route("Remove")]
        [HttpPost]
        public async Task<IActionResult> Remove(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return View("Update", model);
        }

    }
}
