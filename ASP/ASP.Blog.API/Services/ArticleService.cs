using ASP.Blog.API.Controllers;
using ASP.Blog.API.DAL.Entities;
using ASP.Blog.API.DAL.Repositories;
using ASP.Blog.API.DAL.UoW;
using ASP.Blog.API.Data.Entities;
using ASP.Blog.API.Extentions;
using ASP.Blog.API.Services.IServices;
using ASP.Blog.API.ViewModels.Article;
using ASP.Blog.API.ViewModels.Tag;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Article> _articleRepository;
        private readonly IRepository<Tag> _tagRepository;

        public ArticleService(IUnitOfWork unitOfWork,
                IMapper mapper,
                ILogger<TagController> logger,
                IRepository<Article> articleRepository,
                IRepository<Tag> tagRepository
                )
        {
            _mapper = mapper;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _articleRepository = articleRepository;
            _tagRepository = tagRepository;
        }
        //public ArticleViewModel AddArticle(User user)
        //{
        //    var repo = _unitOfWork.GetRepository<Tag>() as TagRepository;
        //    var allTags = repo.GetTags();
        //    _logger.LogInformation("Выполняется переход на страницу добавления статьи.");

        //    return new ArticleViewModel(user) { Tags = allTags, ArticleDate = DateTime.Now };
        //}

        public void AddArticle(ArticleAddRequest model, User user)
        {
            _logger.LogInformation($"Создаёт статью пользователь {user.UserName} : {user.First_Name} {user.Last_Name}");

            var dbTags = new List<Tag>();

            if(model.Tags != null)
            {
                var tagsId = model.Tags.Select(t => t.Id).ToList();
                dbTags = _tagRepository.GetAll().Where(t => tagsId.Contains(t.ID)).ToList();
            }

            var article = new Article()
            {
                ArticleDate = DateTime.Now,
                Title = model.Title,
                Content = model.Content,
                User = user,
                Tags = dbTags
            };

            _logger.LogInformation("Выполняется добавление новой статьи статьи.");
            _articleRepository.Create(article);
            //_logger.LogInformation($"Выполняется переход на страницу просмотра статей пользователя {user.UserName} : {user.First_Name} {user.Last_Name}.");
        }

        public List<ArticleViewRequest> AllArticles(User user = null)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var articles = new List<Article>();
            var articlesView = new List<ArticleViewRequest>();

            if (user is not null)
            {
                articles = repo.GetArticlesByUserId(user.Id);
            }
            else
            {
                articles = repo.GetArticles();
            }

            articlesView = articles.Select(p => new ArticleViewRequest()
            {
                Id = p.ID,
                ArticleDate = p.ArticleDate,
                AuthorId = p.UserId,
                Title = p.Title,
                Content = p.Content,
                Tags = p.Tags.Select(t => new TagRequest() { Id = t.ID, Tag_Name = t.Tag_Name }).ToList()
            }).ToList();

            return articlesView;
        }

        public List<ArticleViewRequest> AllUserArticles()
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

        public void UpdateArticle(ArticleEditRequest model, User user)
        {
            var repo = _unitOfWork.GetRepository<Article>() as ArticleRepository;
            var article = repo.GetArticleById(model.Id);

            var tagRepo = _unitOfWork.GetRepository<Tag>() as TagRepository;

            var dbTags = tagRepo.GetAll().ToList();

            var checkedModelTagsId = model.Tags.Select(x => x.Id).Intersect(dbTags.Select(x => x.ID)).ToList();

            var addTagsId = checkedModelTagsId.Except(article.Tags.Select(x => x.ID)).ToList();
            var delTagsId = article.Tags.Select(x => x.ID).Except(checkedModelTagsId).ToList();

            // Очищаем
            //article.Tags.Clear();
            //Добавляем
            foreach (var dbTag in dbTags)
            {
                if(addTagsId.Contains(dbTag.ID))
                {
                    article.Tags.Add(dbTag);
                }
                if(delTagsId.Contains(dbTag.ID))
                {
                    article.Tags.Remove(dbTag);
                }
            }
        
            article.User = user;
            article.ArticleDate = model.ArticleDate;
            article.Title = model.Title;
            article.Content = model.Content;

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
