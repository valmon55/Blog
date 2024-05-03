using ASP.Blog.BLL.Extentions;
using ASP.Blog.BLL.ViewModels.Tag;
using ASP.Blog.DAL.Entities;
using ASP.Blog.DAL.Repositories;
using ASP.Blog.DAL.UoW;
using ASP.Blog.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASP.Blog.Controllers
{
    public class TagController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public TagController(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork, IMapper mapper, RoleManager<UserRole> roleManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }
        [Route("AddTag")]
        [HttpGet]
        public async Task<IActionResult> AddTag()
        {
            return View(new TagViewModel());
        }
        [Route("AddTag")]
        [HttpPost]
        public async Task<IActionResult> AddTag(TagViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tag = _mapper.Map<Tag>(model);
                var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
                repo.Create(tag);
            }
            else
            {
                ModelState.AddModelError("", "Ошибка в модели!");
            }
            return RedirectToAction("AllTags","Tag");
        }
        [Route("AllTags")]
        [HttpGet]
        public async Task<IActionResult> AllTags()
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tags = repo.GetAll();
            var tagsView = new List<TagViewModel>();
            foreach(var tag in tags)
            {
                tagsView.Add(_mapper.Map<TagViewModel>(tag));
            }

            return View(tagsView);
        }
        [Route("Tag")]
        [HttpGet]
        public async Task<IActionResult> TagById(int id)
        {
            return View();
        }
        [Route("DeleteTag")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            repo.DeleteTag(repo.GetTagById(id));

            return RedirectToAction("AllTags","Tag");
        }
        [Route("Tag/Update")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tag = repo.GetTagById(id);
            var tagView = _mapper.Map<TagViewModel>(tag);            

            return View("EditTag",tagView);
        }
        [Route("Tag/Update")]
        [HttpPost]
        public async Task<IActionResult> Update(TagViewModel model)
        {
            if(ModelState.IsValid) 
            {
                var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
                var tag = repo.GetTagById(model.Id);
                tag.Convert(model);

                repo.UpdateTag(tag);
            }
            else
            {
                ModelState.AddModelError("", "Ошибка в модели!");
            }
            return RedirectToAction("AllTags", "Tag");
        }
    }
}
