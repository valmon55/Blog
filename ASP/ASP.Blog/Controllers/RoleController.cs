﻿using ASP.Blog.BLL.Extentions;
using ASP.Blog.BLL.ViewModels;
using ASP.Blog.BLL.ViewModels.Comment;
using ASP.Blog.BLL.ViewModels.Role;
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
using Microsoft.Extensions.Logging;
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
    public class RoleController : Controller
    {
        private IMapper _mapper;
        private ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public RoleController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, 
                IMapper mapper,
                ILogger<UserController> logger,
                RoleManager<UserRole> roleManager)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        [Route("Role/AddRole")]
        [HttpGet]
        public IActionResult AddRole()
        {
            return View(new RoleViewModel());
        }
        [Route("Role/AddRole")]
        [HttpPost]
        public async Task<IActionResult> AddRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Инициализируем так, чтобы заполнить ID
                var role = new UserRole();

                var roleData = _mapper.Map<UserRole>(model);
                role.Name = roleData.Name;
                role.Description = roleData.Description;
                
                await _roleManager.CreateAsync(role);
            }
            else
            {
                ModelState.AddModelError("", "Ошибка в модели!");
            }

            return RedirectToAction("AllRoles", "Role");
        }


        [Route("AllRoles")]
        [HttpGet]
        public IActionResult AllRoles()
        {
            var repo = _unitOfWork.GetRepository<UserRole>() as UserRoleRepository;
            var roles = repo.GetUserRoles();
            var rolesView = new List<RoleViewModel>();
            foreach (var role in roles)
            {
                rolesView.Add(_mapper.Map<RoleViewModel>(role));
            }

            return View(rolesView);
        }

        [Authorize(Roles = "Admin")]
        [Route("Role/Update")]
        [HttpGet]
        public async Task<IActionResult> UpdateAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var roleView = _mapper.Map<RoleViewModel>(role);

            return View("EditRole", roleView);
        }

        [Authorize(Roles = "Admin")]
        [Route("Role/Update")]
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.ID);
                role.Convert(model);
                
                await _roleManager.UpdateAsync(role);

                return RedirectToAction("AllRoles");
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("RoleEdit", model);
            }
        }
        [Authorize(Roles = "Admin")]
        [Route("Role/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }

            return RedirectToAction("AllRoles");
        }

    }
}
