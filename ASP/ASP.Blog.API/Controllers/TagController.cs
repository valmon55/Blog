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

namespace ASP.Blog.API.Controllers
{
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
        [HttpGet]
        public IActionResult AddTag()
        {
            return View(_tagService.AddTag());
        }
        [Route("AddTag")]
        [HttpPost]
        public IActionResult AddTag(TagViewModel model)
        {
            if (ModelState.IsValid)
            {
                _tagService.AddTag(model);
            }
            else
            {
                _logger.LogError($"Ошибка в модели TagViewModel");
                ModelState.AddModelError("", "Ошибка в модели!");
            }
            return RedirectToAction("AllTags","Tag");
        }
        [Route("AllTags")]
        [HttpGet]
        public IActionResult AllTags()
        {
            return View(_tagService.AllTags());
        }
        [Route("Tag")]
        [HttpGet]
        public IActionResult TagById(int id)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tag = repo.GetTagById(id);
            var tagView = _mapper.Map<TagViewModel>(tag);

            //пока неизвестно где буду использовать
            return RedirectToAction("AllTags", "Tag");
        }
        [Route("DeleteTag")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _tagService.DeleteTag(id);

            return RedirectToAction("AllTags","Tag");
        }
        [Route("Tag/Update")]
        [HttpGet]
        public IActionResult Update(int id)
        {
            return View("EditTag", _tagService.UpdateTag(id));
        }
        [Route("Tag/Update")]
        [HttpPost]
        public IActionResult Update(TagViewModel model)
        {
            if(ModelState.IsValid) 
            {
                _tagService.UpdateTag(model);
            }
            else
            {
                _logger.LogError("Модель TagViewModel не прошла проверку!");
                ModelState.AddModelError("", "Ошибка в модели!");
            }

            _logger.LogInformation($"Перенаправление на страницу просмотра всех тегов.");
            return RedirectToAction("AllTags", "Tag");
        }
    }
}
