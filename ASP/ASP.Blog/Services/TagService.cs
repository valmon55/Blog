using ASP.Blog.BLL.ViewModels.Tag;
using ASP.Blog.Controllers;
using ASP.Blog.DAL.Entities;
using ASP.Blog.DAL.Repositories;
using ASP.Blog.DAL.UoW;
using ASP.Blog.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ASP.Blog.Services.IServices
{
    public class TagService : ITagService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TagController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public TagService(UserManager<User> userManager,
                SignInManager<User> signInManager,
                IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<TagController> logger,
                RoleManager<UserRole> roleManager)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }
        public TagViewModel AddTag()
        {
            return new TagViewModel();
        }

        public void AddTag(TagViewModel model)
        {
            var tag = _mapper.Map<Tag>(model);
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            repo.Create(tag);
            _logger.LogInformation($"Создан тег {tag.Tag_Name}");
        }
    }
}
