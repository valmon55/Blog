using ASP.Blog.API.Controllers;
using ASP.Blog.API.DAL.Entities;
using ASP.Blog.API.DAL.Repositories;
using ASP.Blog.API.DAL.UoW;
using ASP.Blog.API.Data.Entities;
using ASP.Blog.API.Extentions;
using ASP.Blog.API.Services.IServices;
using ASP.Blog.API.ViewModels.Article;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ASP.Blog.API.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<TagController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<UserRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public ArticleService(UserManager<User> userManager,
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
        public ArticleViewModel AddArticle(User user)
        {
            var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var allTags = repo.GetTags();
            _logger.LogInformation("Выполняется переход на страницу добавления статьи.");

            return new ArticleViewModel(user) { Tags = allTags, ArticleDate = DateTime.Now };
        }

        public void AddArticle(ArticleViewModel model, List<int> SelectedTags, User user)
        {
            var tagsId = new List<int>();
            var tagRepo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            SelectedTags.ForEach(id => tagsId.Add(tagRepo.GetTagById(id).ID));
            var tags = new List<Tag>();
            foreach (var tag in tagsId)
            {
                tags.Add(tagRepo.GetTagById((int)tag));
                _logger.LogInformation($"Выбран тег {tagRepo.GetTagById((int)tag).Tag_Name}");
            }

            //var user = await _userManager.FindByNameAsync(User.Identity.Name);
            _logger.LogInformation($"Создаёт статью пользователь {user.UserName} : {user.First_Name} {user.Last_Name}");
            model.User = user;
            model.Tags = tags;
            model.ArticleDate = DateTime.Now;

            var article = _mapper.Map<Article>(model);
            var repo = _unitOfWork.GetRepository<Article>();

            _logger.LogInformation("Выполняется добавление новой статьи статьи.");
            repo.Create(article);
            _logger.LogInformation($"Выполняется переход на страницу просмотра статей пользователя {user.UserName} : {user.First_Name} {user.Last_Name}.");

        }

        public List<ArticleViewModel> AllArticles(User user = null)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var articles = new List<Article>();
            var articlesView = new List<ArticleViewModel>();

            if (user is not null)
            {
                _logger.LogInformation($"Выполняется переход на страницу просмотра всех статей пользователя {user.UserName} : {user.First_Name} {user.Last_Name}.");
                articles = repo.GetArticlesByUserId(user.Id);
                _logger.LogInformation($"Статьи пользователя {user.UserName} : {user.First_Name} {user.Last_Name}:");
            }
            else
            {
                _logger.LogInformation("Выполняется переход на страницу просмотра всех статей.");
                articles = repo.GetArticles();
                _logger.LogInformation("Все статьи:");
            }
                
            foreach (var article in articles)
            {
                articlesView.Add(_mapper.Map<ArticleViewModel>(article));
                _logger.LogInformation($"Дата: {article.ArticleDate.ToShortDateString()} {article.ArticleDate.ToShortTimeString()} \n" +
                    $"заголовок: {article.Title}");
            }
            return articlesView;
        }

        public List<ArticleViewModel> AllUserArticles()
        {
            throw new NotImplementedException();
        }

        public void DeleteArticle(int id)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            _logger.LogInformation($"Удаление статьи, заголовок: {repo.Get(id).Title}");
            repo.DeleteArticle(repo.Get(id));
        }

        public ArticleViewModel UpdateArticle(int id, User user)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var article = repo.GetArticleById(id);
            _logger.LogInformation($"Статья для обновления:\n" + $"дата {article.ArticleDate.ToShortDateString()} {article.ArticleDate.ToShortTimeString()} \n" +
                    $"заголовок {article.Title} \n" + $"текст {article.Content}");
            // TODO: менять автора статьи? возможно не потребуется
            article.User = user;
            var articleView = _mapper.Map<ArticleViewModel>(article);

            var tagRepo = _unitOfWork.GetRepository<Tag>() as TagRepository;
            var allTags = tagRepo.GetTags();

            var checkedTags = article.Tags;

            var checkedTagsDic = new Dictionary<Tag, bool>();

            foreach (var tag in allTags)
            {
                checkedTagsDic.Add(tag, false);
                foreach (var checkedTag in checkedTags)
                {
                    if (tag.Tag_Name == checkedTag.Tag_Name)
                    {
                        checkedTagsDic[tag] = true;
                    }
                }
            }

            return new ArticleViewModel(user)
            {
                Tags = allTags,
                CheckedTagsDic = checkedTagsDic,
                ArticleDate = articleView.ArticleDate,
                Title = articleView.Title,
                Content = articleView.Content
            };
        }

        public void UpdateArticle(ArticleViewModel model, List<int> SelectedTags, User user)
        {
            var tagRepo = _unitOfWork.GetRepository<Tag>() as TagRepository;

            var tagsId = new List<int>();
            SelectedTags.ForEach(id => tagsId.Add(tagRepo.GetTagById(id).ID));
            var tags = new List<Tag>();
            foreach (var tag in tagsId)
            {
                tags.Add(tagRepo.GetTagById((int)tag));
            }
            model.CheckedTagsDic = SelectedTags
                .Select(tagId => tagRepo.Get(tagId))
                .ToDictionary(tag => tag, tag => true);
            model.User = user;
            model.Tags = tags;
            model.ArticleDate = DateTime.Now;

            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var article = repo.GetArticleById(model.Id);
            article.Convert(model);

            _logger.LogInformation($"Обновление статьи:\n" + $"дата {article.ArticleDate.ToShortDateString()} {article.ArticleDate.ToShortTimeString()} \n" +
                    $"заголовок {article.Title} \n" + $"текст {article.Content}");

            repo.Update(article);
        }

        public ArticleViewModel ViewArticle(int id)
        {
            _logger.LogInformation($"Выполняется переход на страницу просмотра статьи.");
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var article = repo.GetArticleById(id);
            _logger.LogInformation($"Статья: \n" + $"дата: {article.ArticleDate.ToShortDateString()} {article.ArticleDate.ToShortTimeString()} \n" +
                    $"заголовок: {article.Title} \n" + $"текст: {article.Content}");
            var commentRepo = _unitOfWork.GetRepository<Comment>() as CommentRepository;
            var comments = commentRepo.GetCommentsByArticleId(id);
            _logger.LogInformation($"Количество комментариев: {comments.Count}");
            var articleView = _mapper.Map<ArticleViewModel>(article);
            articleView.Comments = comments;

            return articleView;
        }
    }
}
