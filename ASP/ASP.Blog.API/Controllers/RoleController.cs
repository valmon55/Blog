using ASP.Blog.API.DAL.Entities;
using ASP.Blog.API.DAL.UoW;
using ASP.Blog.API.Data.Entities;
using ASP.Blog.API.Services.IServices;
using ASP.Blog.API.ViewModels.Role;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ASP.Blog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController : Controller
    {
        private IMapper _mapper;
        private ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleService _roleService;

        public RoleController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, 
                IMapper mapper,
                ILogger<UserController> logger,
                RoleManager<UserRole> roleManager,
                IRoleService roleService)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _roleService = roleService;
        }

        [Route("Role/AddRole")]
        [HttpGet]
        public IActionResult AddRole()
        {
            //_logger.LogInformation("Выполняется переход на страницу добавления роли.");
            //return View(new RoleViewModel());
            return View(_roleService.AddRole());
        }
        [Route("Role/AddRole")]
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                ////Инициализируем так, чтобы заполнить ID
                //var role = new UserRole();

                //var roleData = _mapper.Map<UserRole>(model);
                //role.Name = roleData.Name;
                //role.Description = roleData.Description;

                //await _roleManager.CreateAsync(role);
                //_logger.LogInformation($"Создана роль {role.Name}");
                await _roleService.AddRole(model);
            }
            else
            {
                _logger.LogError("Модель RoleViewModel не прошла проверку!");
                ModelState.AddModelError("", "Ошибка в модели!");
            }
            _logger.LogInformation($"Выполняется переход на страницу просмотра всех ролей");
            return RedirectToAction("AllRoles", "Role");
        }


        [Route("AllRoles")]
        [HttpGet]
        public IActionResult AllRoles()
        {
            //var repo = _unitOfWork.GetRepository<UserRole>() as UserRoleRepository;
            //var roles = repo.GetUserRoles();
            //var rolesView = new List<RoleViewModel>();
            //foreach (var role in roles)
            //{
            //    rolesView.Add(_mapper.Map<RoleViewModel>(role));
            //}
            //_logger.LogInformation($"Просмотр всех ролей");
            //return View(rolesView);
            return View(_roleService.AllRoles());
        }

        [Authorize(Roles = "Admin")]
        [Route("Role/Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateAsync(string roleId)
        {
            //var role = await _roleManager.FindByIdAsync(roleId);
            //var roleView = _mapper.Map<RoleViewModel>(role);
            //_logger.LogInformation($"Роль для обновления: {roleView.Name}");

            var roleView = await _roleService.UpdateRole(roleId);
            return View("EditRole", roleView);
            //return View("EditRole", _roleService.UpdateRole(roleId));
        }

        [Authorize(Roles = "Admin")]
        [Route("Role/Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var role = await _roleManager.FindByIdAsync(model.ID);
                //role.Convert(model);

                //await _roleManager.UpdateAsync(role);
                //_logger.LogInformation($"Роль {role.Name} обновлена");
                await _roleService.UpdateRole(model);
            }
            else
            {
                _logger.LogError("Модель RoleViewModel не прошла проверку!");
                ModelState.AddModelError("", "Некорректные данные");
            }
            _logger.LogInformation($"Перенаправление на страницу просмотра всех ролей");

            return RedirectToAction("AllRoles");
        }
        [Authorize(Roles = "Admin")]
        [Route("Role/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(string roleId)
        {
            //var role = await _roleManager.FindByIdAsync(roleId);
            //if (role != null)
            //{
            //    await _roleManager.DeleteAsync(role);
            //    _logger.LogInformation($"Роль с ID = {roleId} удалена.");
            //}
            await _roleService.DeleteRole(roleId);

            return RedirectToAction("AllRoles");
        }

    }
}
