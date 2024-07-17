using ASP.Blog.API.Controllers;
using ASP.Blog.API.DAL.Entities;
using ASP.Blog.API.DAL.Repositories;
using ASP.Blog.API.DAL.UoW;
using ASP.Blog.API.Data.Entities;
using ASP.Blog.API.Extentions;
using ASP.Blog.API.ViewModels.Tag;
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
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<TagController> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
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
