using ASP.Blog.API.DAL.Entities;
using ASP.Blog.API.DAL.Repositories;
using ASP.Blog.API.DAL.UoW;
using ASP.Blog.API.Data.Entities;
using ASP.Blog.API.Services.IServices;
using ASP.Blog.API.ViewModels.Tag;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ASP.Blog.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TagController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITagService _tagService;
        public TagController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, 
                IMapper mapper,
                ILogger<TagController> logger,
                RoleManager<UserRole> roleManager,
                ITagService tagService)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _tagService = tagService;
        }
        [Route("AddTag")]
        [HttpPost]
        public IActionResult AddTag(TagViewModel model)
        {
            if (ModelState.IsValid)
            {
                _tagService.AddTag(model);
                return StatusCode(201);
            }
            else
            {
                _logger.LogError($"Ошибка в модели TagViewModel");
                ModelState.AddModelError("", "Ошибка в модели!");
                return StatusCode(403);
            }
        }
        [Route("AllTags")]
        [HttpGet]
        public List<TagViewModel> AllTags()
        {
            return _tagService.AllTags();
        }
        [Route("DeleteTag")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            try
            {
                _tagService.DeleteTag(id);
                return StatusCode(201);
            }
            catch
            {
                return StatusCode(403);
            }
        }
        [Route("Update")]
        [HttpPost]
        public IActionResult Update(TagViewModel model)
        {
            if(ModelState.IsValid) 
            {
                _tagService.UpdateTag(model);
                return StatusCode(201);
            }
            else
            {
                _logger.LogError("Модель TagViewModel не прошла проверку!");
                ModelState.AddModelError("", "Ошибка в модели!");
                return StatusCode(500);
            }
        }
    }
}
