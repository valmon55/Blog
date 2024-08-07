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
        public void AddTag(TagAddRequest model)
        {
            var tag = new Tag() { Tag_Name = model.Tag_Name };
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            repo.Create(tag);
            _logger.LogInformation($"Создан тег {tag.Tag_Name}");
        }
        public List<TagRequest> AllTags()
        {
            _logger.LogInformation($"Вывод списка всех тегов.");
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tags = repo.GetAll();
            var tagsView = new List<TagRequest>();
            foreach (var tag in tags)
            {
                //tagsView.Add(_mapper.Map<TagViewModel>(tag));
                tagsView.Add(new TagRequest() { Id = tag.ID, Tag_Name = tag.Tag_Name});
            }
            return tagsView;
        }
        public void DeleteTag(int id)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            repo.DeleteTag(repo.GetTagById(id));
            _logger.LogInformation($"Удален тег с ID = {id}");
        }
        public void UpdateTag(TagRequest model)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var tag = repo.GetTagById(model.Id);
            //tag.Convert(model);
            tag.Tag_Name = model.Tag_Name;

            repo.UpdateTag(tag);
            _logger.LogInformation($"Тег {tag.Tag_Name} обновлен.");
        }
    }
}
