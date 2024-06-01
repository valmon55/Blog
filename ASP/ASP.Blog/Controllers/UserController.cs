using ASP.Blog.BLL.Extentions;
using ASP.Blog.BLL.ViewModels;
using ASP.Blog.DAL.Entities;
using ASP.Blog.DAL.Repositories;
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

namespace ASP.Blog.Controllers
{
    public class UserController : Controller
    {
        private IMapper _mapper;
        private ILogger _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, 
                IMapper mapper, 
                ILogger logger,
                RoleManager<UserRole> roleManager)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        [Route("Register")]
        [HttpGet]
        public IActionResult Register()
        {
            _logger.WriteEvent("Регистрация нового пользователя.");
            return View(new RegisterViewModel());
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ///Создание пользователей с 3 разными ролями
                var userRole = new UserRole() { Name = "User", Description = "Пользователь" };
                //var userRole = new UserRole() { Name = "Admin", Description = "Администратор" };
                //var userRole = new UserRole() { Name = "Moderator", Description = "Модератор" };

                //var roles = _roleManager.Roles.ToList();
                if (!_roleManager.RoleExistsAsync(userRole.Name).Result)
                {
                    await _roleManager.CreateAsync(userRole);
                }

                var user = _mapper.Map<User>(model);

                var result = await _userManager.CreateAsync(user, model.PasswordReg);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    var currentUser = await _userManager.FindByIdAsync(Convert.ToString(user.Id));

                    ///добавляет в таблицу [AspNetUserRoles] соответствие между ролью и пользователем
                    await _userManager.AddToRoleAsync(currentUser, userRole.Name);

                    await _signInManager.RefreshSignInAsync(currentUser);

                    _logger.WriteEvent($"Пользователь {user.Last_Name} {user.First_Name} зарегистрирован.");

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.WriteError("Возникли ошибки при регистрации:");
                    foreach (var error in result.Errors)
                    {
                        _logger.WriteError($"Код ошибки: {error.Code}{Environment.NewLine}Описание: {error.Description}");
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                }
            }
            return View(model);
        }

        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
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
                User signedUser = _userManager.Users.Include(x => x.userRole).FirstOrDefault(u => u.Email == model.Email);
                var userRole = _userManager.GetRolesAsync(signedUser).Result.FirstOrDefault();
                if (signedUser is null)
                {
                    _logger.WriteError($"Логин {user.Email} не найден");
                    ModelState.AddModelError("", "Неверный логин!");
                }

                if (signedUser != null)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole)
                    };

                    await _signInManager.SignInWithClaimsAsync(signedUser, isPersistent: false, claims);
                }
                else
                {
                    _logger.WriteError($"Логин {user.Email} не найден");
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

        [Route("AllUsers")]
        [HttpGet]
        public async Task<IActionResult> AllUsersAsync()
        {
            var repo = _unitOfWork.GetRepository<User>() as UserRepository;
            var users = repo.GetUsers();

            var usersView = new List<UserViewModel>();
            
            foreach (var user in users)
            {
                var userView = _mapper.Map<UserViewModel>(user);
                
                var userRoleNames = await _userManager.GetRolesAsync(user);
                var allRoles = await _roleManager.Roles.ToListAsync();
                var userRoles = new List<UserRole>();

                foreach(var role in allRoles)
                {
                    if(userRoleNames.Contains(role.Name))
                    {
                        userRoles.Add(role);
                    }
                }
                userView.UserRoles = userRoles;

                usersView.Add(userView);
            }

            return View(usersView);
        }

        [Authorize(Roles = "Admin")]
        [Route("User/Update")]
        [HttpGet]
        public IActionResult Update(string userId)
        {
            var repo = _unitOfWork.GetRepository<User>() as UserRepository;
            var user = repo.GetUserById(userId);
            var userView = _mapper.Map<UserViewModel>(user);
            //userView.BirthDate = user.BirthDate;


            var allRoles = _roleManager.Roles.ToList();
            var checkedRolesDic = new Dictionary<UserRole, bool>();
            foreach(var role in allRoles)
            {
                if(_userManager.IsInRoleAsync(user, role.Name).Result) 
                {
                    checkedRolesDic.Add(role, true);
                }
                else
                {
                    checkedRolesDic.Add(role, false);
                }
            }

            userView.CheckedRolesDic = checkedRolesDic;

            return View("EditUser", userView);
        }

        [Authorize(Roles = "Admin")]
        [Route("User/Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(UserViewModel model, List<string> selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);

                user.Convert(model);
                
                var roles = await _roleManager.Roles.ToListAsync();

                foreach(var role in roles)
                {
                    //определяем есть ли роль у пользователя
                    var IsInRole = await _userManager.IsInRoleAsync(user, role.Name);

                    //добавляем роль
                    if (selectedRoles.Contains(role.Id) && !IsInRole)
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    //убираем роль
                    if (!selectedRoles.Contains(role.Id) && IsInRole)
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }

                }
                await _userManager.UpdateAsync(user);

                return RedirectToAction("AllUsers");
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("EditUser", model);
            }
        }
        [Authorize(Roles = "Admin")]
        [Route("User/Delete")]
        [HttpPost]
        public IActionResult Delete(string userId)
        {
            var repo = _unitOfWork.GetRepository<User>() as UserRepository;
            var user = repo.GetUserById(userId);
            repo.DeleteUser(user);

            return RedirectToAction("AllUsers");
        }

    }
}
