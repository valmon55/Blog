using ASP.Blog.API.DAL.Entities;
using ASP.Blog.API.DAL.UoW;
using ASP.Blog.API.Data.Entities;
using ASP.Blog.API.Services.IServices;
using ASP.Blog.API.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASP.Blog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private IMapper _mapper;
        private ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public UserController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, 
                IMapper mapper,
                ILogger<UserController> logger,
                RoleManager<UserRole> roleManager,
                IUserService userService)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _userService = userService;
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

                    _logger.LogInformation($"Пользователь {user.Last_Name} {user.First_Name} зарегистрирован.");

                    return StatusCode(201);
                }
                else
                {
                    _logger.LogError("Возникли ошибки при регистрации:");
                    foreach (var error in result.Errors)
                    {
                        _logger.LogError($"Код ошибки: {error.Code}{Environment.NewLine}Описание: {error.Description}");
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return StatusCode(403);
                }
            }
            else
            {
                _logger.LogError("Возникли ошибки в данных для регистрации пользователя.");
                return StatusCode(500);
            }
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
                    _logger.LogError($"Логин {user.Email} не найден");
                    return StatusCode(404);
                }
                /// Если ролей почему-то нет, то устанавливаем:
                /// для пользователя Admin - роль Admin
                /// для остальных - User
                if(userRole is null) 
                {
                    _logger.LogError($"У пользователя {signedUser.UserName} нет роли!");
                    if(signedUser.UserName == "Admin")
                    {
                        await _userManager.AddToRoleAsync(signedUser, "Admin");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(signedUser, "User");
                    }
                    userRole = _userManager.GetRolesAsync(signedUser).Result.FirstOrDefault();
                    _logger.LogWarning($"Пользователю {signedUser.userRole} присвоили роль {userRole}");
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
                    _logger.LogError($"Логин {user.Email} не найден");
                    return StatusCode(404);
                }
                return StatusCode(201);
            }
            else
            {
                return StatusCode(500);
            }
        }
        [Route("Logout")]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation($"Выполнен Logout.");
                return StatusCode(201);
            }
            catch (Exception ex) 
            {
                return StatusCode(403);
            }
        }
        [Authorize(Roles="Admin")]
        [Route("AllUsers")]
        [HttpGet]
        public async Task<List<UserViewModel>> AllUsersAsync()
        {
            return await _userService.AllUsers();
        }

        [Authorize(Roles = "Admin")]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _userService.UpdateUser(model, model.SelectedRoles);
                return StatusCode(201);
            }
            else
            {
                _logger.LogError("Модель UserViewModel не прошла проверку!");
                return StatusCode(500);
            }
        }
        [Authorize(Roles = "Admin")]
        [Route("Delete")]
        [HttpPost]
        public IActionResult Delete(string userId)
        {
            try
            {
                _userService.DeleteUser(userId);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
 