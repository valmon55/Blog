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
using System.Collections.Generic;
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

        [Route("AddRole")]
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleAddRequest model)
        {
            if (ModelState.IsValid)
            {
                await _roleService.AddRole(model);
                return StatusCode(201);
            }
            else
            {
                _logger.LogError("Модель RoleViewModel не прошла проверку!");
                return StatusCode(403);
            }
        }

        [Route("AllRoles")]
        [HttpGet]
        public List<RoleRequest> AllRoles()
        {
            return _roleService.AllRoles();
        }

        [Authorize(Roles = "Admin")]
        [Route("Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(RoleRequest model)
        {
            if (ModelState.IsValid)
            {
                await _roleService.UpdateRole(model);
                return StatusCode(201);
            }
            else
            {
                _logger.LogError("Модель RoleViewModel не прошла проверку!");
                return StatusCode(403);
            }
        }
        [Authorize(Roles = "Admin")]
        [Route("Delete")]
        [HttpDelete]
        public async Task<IActionResult> Delete(string roleId)
        {
            try
            {
                await _roleService.DeleteRole(roleId);
                return StatusCode(201);
            }
            catch
            {
                return StatusCode(403);
            }
        }
    }
}
