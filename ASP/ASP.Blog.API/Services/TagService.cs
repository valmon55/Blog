﻿using ASP.Blog.API.DAL.Entities;
using ASP.Blog.API.DAL.Repositories;
using ASP.Blog.API.DAL.UoW;
using ASP.Blog.API.Data.Entities;
using ASP.Blog.BLL.Extentions;
using ASP.Blog.BLL.ViewModels.Tag;
using ASP.Blog.Controllers;
using ASP.Blog.DAL.Entities;
using ASP.Blog.DAL.Repositories;
using ASP.Blog.DAL.UoW;
using ASP.Blog.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ASP.Blog.API.Services.IServices
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

        public List<TagViewModel> AllTags()
        {
            _logger.LogInformation($"Вывод списка всех тегов.");
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tags = repo.GetAll();
            var tagsView = new List<TagViewModel>();
            foreach (var tag in tags)
            {
                tagsView.Add(_mapper.Map<TagViewModel>(tag));
            }
            return tagsView;
        }

        public void DeleteTag(int id)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            repo.DeleteTag(repo.GetTagById(id));
            _logger.LogInformation($"Удален тег с ID = {id}");
        }

        public TagViewModel UpdateTag(int id)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tag = repo.GetTagById(id);
            var tagView = _mapper.Map<TagViewModel>(tag);
            _logger.LogInformation($"Тег для обновления: {tagView.Tag_Name}");
            
            return tagView;
        }

        public void UpdateTag(TagViewModel model)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tag = repo.GetTagById(model.Id);
            tag.Convert(model);

            repo.UpdateTag(tag);
            _logger.LogInformation($"Тег {tag.Tag_Name} обновлен.");
        }
    }
}
