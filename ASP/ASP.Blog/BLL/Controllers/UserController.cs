using ASP.Blog.BLL.ViewModels;
using ASP.Blog.DAL.UoW;
using ASP.Blog.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASP.Blog.BLL.Controllers
{
    public class UserController : Controller
    {
        private IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(UserManager<User> userManager,
                SignInManager<User> signInManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
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
                //почему-то не заполняется, но оно нужно для регистрации
                //user.NormalizedEmail = user.Email.ToUpper();

                var result = await _userManager.CreateAsync(user, model.PasswordReg);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
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
                IdentityUser signedUser = _userManager.FindByEmailAsync(user.Email).Result;
                if (signedUser != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(signedUser.UserName, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Неверный логин или пароль");
                    }
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

        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> Update(UserEditViewModel model)
        { 
            if (ModelState.IsValid) 
            { 
            
            } 
            return View("Update", model);
        }
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
