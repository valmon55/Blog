using ASP.Blog.BLL.Extentions;
using ASP.Blog.BLL.ViewModels;
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
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public RoleController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, IMapper mapper, RoleManager<UserRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
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
        public IActionResult Update(string userId)
        {
            var repo = _unitOfWork.GetRepository<User>() as UserRepository;
            var user = repo.GetUserById(userId);
            var userView = _mapper.Map<UserViewModel>(user);
            userView.BirthDate = user.BirthDate;

            return View("EditUser", userView);
        }

        [Authorize(Roles = "Admin")]
        [Route("User/Update")]
        [HttpPost]
        public IActionResult Update(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var repo = _unitOfWork.GetRepository<User>() as UserRepository;

                var user = repo.GetUserById(model.Id);
                user.Convert(model);
                repo.UpdateUser(user);

                return RedirectToAction("AllUsers");
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("UserEdit", model);
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
